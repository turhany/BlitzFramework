using System.Collections.Generic;
using System.Globalization;

namespace BlitzFramework.Constants
{
    public static class FrameworkConstants
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 25;
        public const int MaxPageSize = int.MaxValue;
        public const int DefaultCacheDuration = 10;

        public const string RedLockHostAddress = "RedLockHostAddress";
        public const string RedLockHostPassword = "RedLockHostPassword";
        public const string RedLockHostPort = "RedLockHostPort";

        public const string JsonContentType = "application/json";
        public static CultureInfo DefaultLanguage = new CultureInfo("en-US");
        public static List<CultureInfo> SupportedLanguages =>
            new List<CultureInfo>
            {
                new CultureInfo("en-US")
            };

        public const string ServerErrorMessage = "Internal Server Error.";
        public const string DeleteItemNotFound = "Delete item not found. Id: {0}";

        public const string DeletedOnFieldName = "DeletedOn";
        public const string IsDeletedFieldName = "IsDeleted";
        public const string CreatedOnFieldName = "CreatedOn";
        public const string CreatedByFieldName = "CreatedBy";
        public const string UpdatedOnFieldName = "UpdatedOn";
        public const string UpdatedByFieldName = "UpdatedBy";
    }
}