﻿using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.DependencyInjection.MemberInjection.Internal
{
    internal abstract class MemberInjector
    {
        protected const int CompileThreshold = 4;
    }

    internal abstract class MemberInjector<T, TMember> : MemberInjector, IMemberInjector<T>
    {
        private readonly bool isInstanceValueType;
        private readonly bool isMemberValueType;
        private readonly MemberInfo member;
        protected Action<T, TMember> injectAction;

        public MemberInjector(IInternalMemberInjectorFactory factory, MemberInfo member)
        {
            this.member = member;
            this.isInstanceValueType = factory.IsValueType;
            this.isMemberValueType = typeof(TMember).GetTypeInfo().IsValueType;
        }

        public void Inject(IServiceProvider serviceProvider, T instance, bool isRequired)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (this.isInstanceValueType && Equals(null, instance)) throw new ArgumentNullException(nameof(instance));

            if (this.isMemberValueType)
            {
                var value = serviceProvider.GetService(typeof(TMember));
                if (value != null)
                {
                    this.injectAction(instance, (TMember) value);
                }
                else if (isRequired)
                {
                    throw new MemberResolveException(this.member);
                }
            }
            else
            {
                TMember value;
                if (isRequired)
                {
                    try
                    {
                        value = serviceProvider.GetRequiredService<TMember>();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new MemberResolveException(this.member);
                    }
                }
                else
                {
                    value = serviceProvider.GetService<TMember>();
                }
                this.injectAction(instance, value);
            }
        }

        protected Action<T, TMember> InitializeAction()
        {
            if (CompileThreshold <= 0) return this.CompileInjectAction();

            var count = 0;
            return (i, v) =>
            {
                if (Interlocked.Increment(ref count) == CompileThreshold)
                {
                    Task.Run(() => Interlocked.Exchange(ref this.injectAction, this.CompileInjectAction()));
                }

                this.ReflectInjectAction(i, v);
            };
        }

        protected abstract void ReflectInjectAction(T instance, TMember value);

        protected abstract Action<T, TMember> CompileInjectAction();
    }
}
