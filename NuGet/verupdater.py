#!/usr/bin/env python
# -*- coding: utf-8 -*-
#
# Copyright (c) 2017 - cologler <skyoflw@gmail.com>
# ----------
#
# ----------

import os
import sys
import traceback
import xml.etree.ElementTree as ET
import re


NS = { 'ns': 'http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd' }
ET.register_namespace('', 'http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd')


def detect_proj(proj_id):
    return proj_id.lower().startswith('jasily')


class VersionUpdater:
    def __init__(self, script_path):
        script_dir = os.path.dirname(script_path)
        self._verfile = os.path.join(script_dir, 'version.txt')
        self._sln_root = os.path.dirname(script_dir)
        self._version = self.read_version()

    def read_version(self):
        with open(self._verfile) as fp:
            ver = fp.read()
        m = re.match(r'^(\d)+\.(\d)+\.(\d)+\.(\d)+$', ver)
        vs = list(m.groups())
        vs[-1] = str(int(vs[-1]) + 1)
        ver = '.'.join(vs)
        with open(self._verfile, 'w') as fp:
            fp.write(ver)
        return ver

    def resolve_version(self, path):
        if os.path.splitext(path)[1].lower() != '.nuspec':
            raise Exception
        self._internal_incr_version(path)

    def _internal_incr_version(self, path):
        xml_model = ET.parse(path)
        package = xml_model.getroot()
        metadata = package.find('ns:metadata', NS)
        version = metadata.find('ns:version', NS)
        version.text = self._version
        dependencies = metadata.find('ns:dependencies', NS)
        if dependencies:
            def visit_dependency(dependency_root):
                for dependency in dependency_root.findall('ns:dependency', NS):
                    subproj = dependency.attrib['id']
                    if detect_proj(subproj):
                        subproj_nuspec = os.path.join(self._sln_root, subproj, subproj + '.nuspec')
                        self._internal_incr_version(subproj_nuspec)
                        dependency.attrib['version'] = self._version
                for group in dependency_root.findall('ns:group', NS):
                    visit_dependency(group)
            visit_dependency(dependencies)
        xml_model.write(path, xml_declaration=True, encoding='utf-8')


def main(argv=None):
    if argv is None:
        argv = sys.argv
    try:
        updater = VersionUpdater(argv[0])
        updater.resolve_version(argv[1])
    except Exception:
        traceback.print_exc()
        input()

if __name__ == '__main__':
    main()
