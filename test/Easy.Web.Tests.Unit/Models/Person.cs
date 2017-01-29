namespace Easy.Web.Tests.Unit.Models
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;

    [Serializable, XmlRoot("Person")]
    [ProtoContract]
    public sealed class Person
    {
        [ProtoMember(1)]
        public int Age { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}