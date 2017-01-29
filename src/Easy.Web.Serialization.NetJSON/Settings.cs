namespace Easy.Web.Serialization.JSONNet
{
    using NetJSON;

    /// <summary>
    /// Provides access to the default settings for the serialization.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the default <see cref="NetJSONSettings"/>.
        /// </summary>
        public static NetJSONSettings Default = new NetJSONSettings
        {
            CamelCase = true,
            Format = NetJSONFormat.Default,
            UseEnumString = true,
            CaseSensitive = false,
            DateFormat = NetJSONDateFormat.ISO,
            TimeZoneFormat = NetJSONTimeZoneFormat.Utc,
            SkipDefaultValue = true
        };
    }
}