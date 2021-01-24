using BlitzFramework.Constants;

namespace BlitzFramework.SearchArgs
{
    public class BaseSearchArgs
    {
        public int PageSize { get; set; } = FrameworkConstants.DefaultPageSize;
        public int PageNumber { get; set; } = FrameworkConstants.DefaultPageNumber;
    }
}
