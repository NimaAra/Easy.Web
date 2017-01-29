namespace Easy.Web.Serialization.JSONNet
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Provides access to the default settings for the serialization.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the default <see cref="JsonSerializerSettings"/>.
        /// </summary>
        public static JsonSerializerSettings Default = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = Formatting.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}