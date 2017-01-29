namespace Easy.Web.Tests.Integration.Models
{
    using System;
    using System.Xml.Serialization;
    using ProtoBuf;
    using System.ComponentModel.DataAnnotations;

    [Serializable, XmlRoot("SampleModel")]
    [ProtoContract]
    public sealed class SampleModel
    {
        [ProtoMember(1)]
        [Required]
        public int? Id { get; set; }

        [ProtoMember(2)]
        [MinLength(3, ErrorMessage = "Minimum length failed.")]
        [MaxLength(6, ErrorMessage = "Maximum length failed.")]
        public string Category { get; set; }
    }
}