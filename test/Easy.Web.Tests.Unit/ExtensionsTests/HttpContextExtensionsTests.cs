namespace Easy.Web.Tests.Unit.ExtensionsTests
{
    using System;
    using System.Collections.Generic;
    using Easy.Web.Core.Extensions;
    using Easy.Web.Core.Helpers;
    using Easy.Web.Core.Interfaces;
    using Easy.Web.Serialization.JSONNet;
    using Easy.Web.Tests.Unit.Models;
    using NSubstitute;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class HttpContextExtensionsTests
    {
        [Test]
        public void When_getting_serializer_when_no_serializer_has_been_registered()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var ctx = new DummyContext(serviceProvider);

            ISerializer someSerializer;
            ctx.TryGetSerializer(MediaTypes.XML, out someSerializer).ShouldBeFalse();
            someSerializer.ShouldBeNull();
        }

        [Test]
        public void When_getting_serializer_when_incorrect_serializer_has_been_registered()
        {
            const string MediaType = "application/foo";

            var serviceProvider = new DummyServiceProvider();
            serviceProvider.Register<ISerializer>(new JSONNetSerializer());
            var ctx = new DummyContext(serviceProvider);

            ISerializer someSerializer;
            ctx.TryGetSerializer(MediaType, out someSerializer).ShouldBeFalse();
            someSerializer.ShouldBeNull();
        }

        [Test]
        public void When_getting_serializer_when_correct_serializer_has_been_registered()
        {
            const string MediaType = MediaTypes.JSON;

            var serviceProvider = new DummyServiceProvider();
            serviceProvider.Register<ISerializer>(new JSONNetSerializer());
            var ctx = new DummyContext(serviceProvider);

            ISerializer someSerializer;
            ctx.TryGetSerializer(MediaType, out someSerializer).ShouldBeTrue();
            someSerializer.ShouldNotBeNull();
            someSerializer.ShouldBeOfType<JSONNetSerializer>();
        }

        private sealed class DummyServiceProvider : IServiceProvider
        {
            private readonly IDictionary<Type, object> _registrations = new Dictionary<Type, object>();

            public void Register<T>(object implementation)
            {
                _registrations.Add(typeof(T), implementation);
            }

            public object GetService(Type serviceType)
            {
                if (serviceType.IsGenericType && serviceType.GenericTypeArguments[0] == typeof(ISerializer))
                {
                    var result = new List<ISerializer>();
                    foreach (var pair in _registrations)
                    {
                        if (pair.Key == typeof(ISerializer))
                        {
                            result.Add((ISerializer)pair.Value);
                        }
                    }

                    return result;
                }

                return _registrations[serviceType];
            }
        }
    }
}