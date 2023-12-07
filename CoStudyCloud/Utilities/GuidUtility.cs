namespace CoStudyCloud.Utilities
{
    /// <summary>
    /// Represents GUID utility
    /// </summary>
    public static class GuidUtility
    {
        /// <summary>
        /// Generate a URL friendly GUID
        /// </summary>
        /// <returns></returns>
        public static string GenerateUrlFriendlyGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "_");
        }
    }
}
