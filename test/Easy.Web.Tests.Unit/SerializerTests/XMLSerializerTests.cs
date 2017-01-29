namespace Easy.Web.Tests.Unit.SerializerTests
{
    using System.IO;
    using System.Text;
    using Easy.Web.Serialization.XML;
    using Easy.Web.Tests.Unit.Models;
    using NUnit.Framework;
    using Shouldly;

    public class XMLSerializerTests
    {
        [Test]
        public void When_checking_if_serializer_supports_media_type()
        {
            var serializer = new XMLSerializer();
            serializer.CanSerialize("application/xml").ShouldBeTrue();
            serializer.CanSerialize("application/json").ShouldBeFalse();
        }

        [Test]
        public void When_checking_if_deserializer_supports_media_type()
        {
            var serializer = new XMLDeserializer();
            serializer.CanDeserialize("application/xml").ShouldBeTrue();
            serializer.CanDeserialize("application/json").ShouldBeFalse();
        }

        [Test]
        public void When_serializing_model_with_default_settings()
        {
            var model = new Person { Age = 10, Name = "Joe" };
            var serializer = new XMLSerializer();

            using (var mem = new MemoryStream())
            {
                serializer.Serialize(model, mem, Encoding.UTF8);
                mem.Position = 0;
                var bytes = mem.ToArray();
                var xml = Encoding.UTF8.GetString(bytes);

                xml.ShouldNotBeNullOrWhiteSpace();
                xml.ShouldContain("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xml.ShouldContain("<Person");
                xml.ShouldContain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"");
                xml.ShouldContain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                xml.ShouldContain("<Age>10</Age>");
                xml.ShouldContain("<Name>Joe</Name>");
                xml.ShouldContain("</Person>");
            }
        }

        [Test]
        public void When_deserializing_model_with_default_settings()
        {
            const string XML = @"﻿<?xml version=""1.0"" encoding=""utf-8""?>
<Person xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Age>10</Age>
  <Name>Joe</Name>
</Person>";
            var bytes = Encoding.UTF8.GetBytes(XML);

            using (var mem = new MemoryStream(bytes))
            {
                var deserializer = new XMLDeserializer();
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
            var serializer = new XMLSerializer();
            var deserializer = new XMLDeserializer();

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