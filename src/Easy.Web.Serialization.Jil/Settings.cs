namespace Easy.Web.Serialization.Jil
{
    using global::Jil;

    /// <summary>
    /// Provides access to the default settings for the serialization.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the default <see cref="Options"/>.
        /// </summary>
        public static Options Default = Options.MillisecondsSinceUnixEpochExcludeNullsIncludeInheritedUtcCamelCase;
    }
}