namespace Easy.Web.Tests.Unit.SerializerTests
{
    using System.IO;
    using System.Text;
    using Easy.Web.Serialization.JSONNet;
    using Easy.Web.Tests.Unit.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NUnit.Framework;
    using Shouldly;

    public class JSONNetSerializerTests
    {
        [Test]
        public void When_checking_if_serializer_supports_media_type()
        {
            var serializer = new JSONNetSerializer();
            serializer.CanSerialize("application/json").ShouldBeTrue();
            serializer.CanSerialize("application/xml").ShouldBeFalse();
        }

        [Test]
        public void When_checking_if_deserializer_supports_media_type()
        {
            var serializer = new JSONNetDeserializer();
            serializer.CanDeserialize("application/json").ShouldBeTrue();
            serializer.CanDeserialize("application/xml").ShouldBeFalse();
        }

        [Test]
        public void When_serializing_model_with_default_settings()
        {
            var model = new Person { Age = 10, Name = "Joe" };
            var serializer = new JSONNetSerializer();

            using (var mem = new MemoryStream())
            {
                serializer.Serialize(model, mem, Encoding.UTF8);
                mem.Position = 0;
                var bytes = mem.ToArray();
                var json = Encoding.UTF8.GetString(bytes);

                json.ShouldNotBeNullOrWhiteSpace();
                json.ShouldBe("﻿{\"age\":10,\"name\":\"Joe\"}");
            }
        }

        [Test]
        public void When_serializing_model_with_custom_settings()
        {
            var model = new Person { Age = 10, Name = "Joe" };

            var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            var serializer = new JSONNetSerializer(settings);

            using (var mem = new MemoryStream())
            {
                serializer.Serialize(model, mem, Encoding.UTF8);
                mem.Position = 0;
                var bytes = mem.ToArray();
                var json = Encoding.UTF8.GetString(bytes);

                json.ShouldNotBeNullOrWhiteSpace();
                json.ShouldBe("﻿{\"Age\":10,\"Name\":\"Joe\"}");
            }
        }

        [Test]
        public void When_deserializing_model_with_default_settings()
        {
            const string JSON = "﻿{\"age\":10,\"name\":\"Joe\"}";
            var bytes = Encoding.UTF8.GetBytes(JSON);

            using (var mem = new MemoryStream(bytes))
            {
                var deserializer = new JSONNetDeserializer();
                var model = deserializer.Deserialize<Person>(mem, Encoding.UTF8);
                
                model.ShouldNotBeNull();
                model.Age.ShouldBe(10);
                model.Name.ShouldBe("Joe");
            }
        }

        [Test]
        public void When_deserializing_model_with_custom_settings()
        {
            const string JSON = "﻿{\"Age\":10,\"Name\":\"Joe\"}";
            var bytes = Encoding.UTF8.GetBytes(JSON);

            using (var mem = new MemoryStream(bytes))
            {
                var settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
                var deserializer = new JSONNetDeserializer(settings);
                var model = deserializer.Deserialize<Person>(mem, Encoding.UTF8);

                model.ShouldNotBeNull();
                model.Age.ShouldBe(10);
                model.Name.ShouldBe("Joe");
            }
        }

        [Test]
        public void When_serializing_and_deserializing_model()
        {
            var model = new Person { Age = 10, Name = "Joe" };
            var serializer = new JSONNetSerializer();
            var deserializer = new JSONNetDeserializer();

            using (var mem = new MemoryStream())
            {
                serializer.Serialize(model, mem, Encoding.UTF8);
                var result = deserializer.Deserialize<Person>(mem, Encoding.UTF8);

                result.ShouldNotBeNull();
                result.Age.ShouldBe(10);
                result.Name.ShouldBe("Joe");
            }
        }
    }
}