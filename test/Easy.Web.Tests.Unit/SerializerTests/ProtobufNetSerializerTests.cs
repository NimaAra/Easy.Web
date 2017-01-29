namespace Easy.Web.Tests.Unit.SerializerTests
{
    using System.IO;
    using System.Text;
    using Easy.Web.Serialization.ProtobufNet;
    using Easy.Web.Tests.Unit.Models;
    using NUnit.Framework;
    using Shouldly;

    public class ProtobufNetSerializerTests
    {
        [Test]
        public void When_checking_if_serializer_supports_media_type()
        {
            var serializer = new ProtobufNetSerializer();
            serializer.CanSerialize("application/octet-stream").ShouldBeTrue();
            serializer.CanSerialize("application/xml").ShouldBeFalse();
            serializer.CanSerialize("application/json").ShouldBeFalse();
        }

        [Test]
        public void When_checking_if_deserializer_supports_media_type()
        {
            var serializer = new ProtobufNetDeserializer();
            serializer.CanDeserialize("application/octet-stream").ShouldBeTrue();
            serializer.CanDeserialize("application/xml").ShouldBeFalse();
            serializer.CanDeserialize("application/json").ShouldBeFalse();
        }   

        [Test]
        public void When_serializing_model_with_default_settings()
        {
            var model = new Person { Age = 10, Name = "Joe" };
            var serializer = new ProtobufNetSerializer();

            using (var mem = new MemoryStream())
            {
                serializer.Serialize(model, mem, Encoding.UTF8);
                mem.Position = 0;
                var bytes = mem.ToArray();
                bytes.ShouldNotBeNull();
                bytes.Length.ShouldBe(7);
                bytes.ShouldBe(new byte[] { 8, 10, 18, 3, 74, 111, 101 });
            }
        }

        [Test]
        public void When_deserializing_model_with_default_settings()
        {
            var bytes = new byte[] {8, 10, 18, 3, 74, 111, 101};

            using (var mem = new MemoryStream(bytes))
            {
                var deserializer = new ProtobufNetDeserializer();
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
            var serializer = new ProtobufNetSerializer();
            var deserializer = new ProtobufNetDeserializer();

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