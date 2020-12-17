using BlitzFramework.Constants;

namespace BlitzFramework.SearchArgs
{
    public class BaseSearchArgs
    {
        public int PageSize { get; set; } = AppConstants.DefaultPageSize;
        public int PageNumber { get; set; } = AppConstants.DefaultPageNumber;
    }
}
