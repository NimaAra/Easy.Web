namespace Easy.Web.Tests.Unit.SerializerTests
{
    using System.IO;
    using System.Text;
    using Easy.Web.Serialization.Jil;
    using Easy.Web.Tests.Unit.Models;
    using Jil;
    using NUnit.Framework;
    using Shouldly;

    public class JilSerializerTests
    {
        [Test]
        public void When_checking_if_serializer_supports_media_type()
        {
            var serializer = new JilSerializer();
            serializer.CanSerialize("application/json").ShouldBeTrue();
            serializer.CanSerialize("application/xml").ShouldBeFalse();
        }

        [Test]
        public void When_checking_if_deserializer_supports_media_type()
        {
            var serializer = new JilDeserializer();
            serializer.CanDeserialize("application/json").ShouldBeTrue();
            serializer.CanDeserialize("application/xml").ShouldBeFalse();
        }

        [Test]
        public void When_serializing_model_with_default_settings()
        {
            var model = new Person { Age = 10, Name = "Joe" };
            var serializer = new JilSerializer();

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

            var settings = new Options(serializationNameFormat: SerializationNameFormat.CamelCase);
            var serializer = new JilSerializer(settings);

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
        public void When_deserializing_model_with_default_settings()
        {
            const string JSON = "﻿{\"age\":10,\"name\":\"Joe\"}";
            var bytes = Encoding.UTF8.GetBytes(JSON);

            using (var mem = new MemoryStream(bytes))
            {
                var deserializer = new JilDeserializer();
                var model = deserializer.Deserialize<Person>(mem, Encoding.UTF8);
                
                model.ShouldNotBeNull();
                model.Age.ShouldBe(10);
                model.Name.ShouldBe("Joe");
            }
        }

        [Test]
        public void When_deserializing_model_with_custom_settings()
        {
            const string JSON = "﻿{\"age\":10,\"name\":\"Joe\"}";
            var bytes = Encoding.UTF8.GetBytes(JSON);

            using (var mem = new MemoryStream(bytes))
            {
                var settings = new Options(serializationNameFormat: SerializationNameFormat.CamelCase);
                
                var deserializer = new JilDeserializer(settings);
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
            var serializer = new JilSerializer();
            var deserializer = new JilDeserializer();

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