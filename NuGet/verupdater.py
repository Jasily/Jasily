#!/usr/bin/env python
# -*- coding: utf-8 -*-
#
# Copyright (c) 2017 - cologler <skyoflw@gmail.com>
# ----------
# HOW TO USE: e.g:
# verupdater.py D:\Projects\jasily-csharp\Jasily.Standard\Jasily.Standard.nuspec
# ----------

import os
import sys
import traceback
import xml.etree.ElementTree as ET
import re


NS = { 'ns': 'http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd' }
ET.register_namespace('', 'http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd')


class AppVersion:
    def __init__(self, *args):
        if len(args) == 0:
            raise ValueError('miss args.')
        self._vers = []
        if len(args) == 1:
            if isinstance(args[0], str):
                self.parse_from_string(args[0])
            elif isinstance(args[0], AppVersion):
                self._vers = args[0]._vers.copy()
            else:
                raise ValueError
        else:
            self.parse_from_vers(args)

    def parse_from_string(self, s):
        fp = '(\d+)'
        op = r'(?:\.(\d+))?'
        m = re.match('^{fp}{op}{op}{op}$'.format(fp=fp, op=op), s)
        if not m:
            raise NotImplementedError
        for n in m.groups():
            if n != None:
                v = int(n)
                self._vers.append(v)

    def parse_from_vers(self, ns):
        for n in ns:
            if not isinstance(n, int):
                raise ValueError
            self._vers.append(n)

    def __str__(self):
        return '.'.join([str(x) for x in self._vers])

    def __gt__(self, other):
        return self.compare(other) > 0

    def __lt__(self, other):
        return self.compare(other) < 0

    def __eq__(self, other):
        return self.compare(other) == 0

    def compare(self, other):
        '''
        return - if self < other;
        return 0 if self == other;
        return + if self > other;
        '''
        if not isinstance(other, AppVersion):
            raise NotImplementedError
        c1 = min(len(self._vers), len(other._vers))
        for i in range(0, c1):
            l = self._vers[i]
            r = other._vers[i]
            if l != r:
                return l - r
        return len(self._vers) - len(other._vers)

    def incr(self):
        self._vers[-1] += 1
        return self


class PackageXml:
    def __init__(self, path):
        if not os.path.isfile(path):
            raise Exception('missing %s' % path)

        self._path = path

        self._xml_root = ET.parse(self._path)
        self._x_package = self._xml_root.getroot()
        self._x_metadata = self._x_package.find('ns:metadata', NS)
        self._x_version = self._x_metadata.find('ns:version', NS)

        self._version = AppVersion(self._x_version.text)

    def __str__(self):
        return os.path.split(self._path)[1]

    @property
    def package(self):
        return self._x_package

    @property
    def version(self):
        return AppVersion(self._version)

    @version.setter
    def version(self, value):
        nv = AppVersion(value)
        if self._version > nv:
            print('Error (%s): %s < %s' % (self, self._version, nv))
            raise ValueError
        self._version = nv
        self._x_version.text = str(value)

    def load(self):
        return self._xml_root

    def save(self):
        self._xml_root.write(self._path, xml_declaration=True, encoding='utf-8')


class VersionUpdater:
    def __init__(self, script_path):
        script_dir = os.path.dirname(script_path)
        self._verfile = os.path.join(script_dir, 'version.txt')
        self._sln_root = os.path.dirname(script_dir)
        self._version = self.read_version()
        self._xmls = []

    def on_detect_proj(self, proj_id):
        return False

    def read_version(self):
        with open(self._verfile) as fp:
            ver = AppVersion(fp.read())
        ver.incr()
        return ver

    def resolve_version(self, path):
        if os.path.splitext(path)[1].lower() != '.nuspec':
            raise Exception
        self._internal_incr_version(path)
        self.confirm()

    def _internal_incr_version(self, path):
        p = PackageXml(path)
        package = p.package
        metadata = package.find('ns:metadata', NS)
        p.version = self._version
        dependencies = metadata.find('ns:dependencies', NS)
        if dependencies:
            def visit_dependency(dependency_root):
                for dependency in dependency_root.findall('ns:dependency', NS):
                    subproj = dependency.attrib['id']
                    if self.on_detect_proj(subproj):
                        subproj_nuspec = os.path.join(self._sln_root, subproj, subproj + '.nuspec')
                        self._internal_incr_version(subproj_nuspec)
                        dependency.attrib['version'] = str(self._version)
                for group in dependency_root.findall('ns:group', NS):
                    visit_dependency(group)
            visit_dependency(dependencies)
        self._xmls.append(p)

    def confirm(self):
        print('press any key to update version to %s:' % str(self._version))
        for pkg in self._xmls:
            print('   ' + str(pkg))
        input()
        with open(self._verfile, 'w') as fp:
            fp.write(str(self._version))
        for item in self._xmls:
            item.save()


class JasilyVersionUpdater(VersionUpdater):
    def on_detect_proj(self, proj_id):
        return proj_id.lower().startswith('jasily')


def main(argv=None):
    if argv is None:
        argv = sys.argv
    try:
        updater = JasilyVersionUpdater(argv[0])
        updater.resolve_version(argv[1])
    except Exception:
        traceback.print_exc()
        input()

if __name__ == '__main__':
    main()
