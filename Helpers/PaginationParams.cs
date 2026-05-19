namespace School_Management.Helpers
{
    public class PaginationParams
    {
        private const int MaxPageSize = 10;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 5;

        public int PageSize { get => _pageSize;
            set => _pageSize =
                (value > MaxPageSize)
                ? MaxPageSize
                : value;
        }
        public string? Search { get; set;}
        public string? SortBy { get; set; }

    }
}
