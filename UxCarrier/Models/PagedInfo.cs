namespace UxCarrier.Models
{
    public class PageInfo
    {
        /// <summary>
        /// start from 1
        /// </summary>
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public Sorting Sort { get; private set; }

        public string SortText { get; private set; }

        public PageInfo() : this(null)
        {
        }

        public PageInfo(PageInfoDto dto)
        {
            if (dto == null)
            {
                PageSize = 10;
                PageIndex = 1;
                SortText = "";
                Sort = new Sorting(SortText);
            }
            else
            {
                PageSize = dto.PageSize;
                PageIndex = dto.PageIndex < 1 ? 1 : dto.PageIndex;
                SortText = dto.SortText;
                Sort = new Sorting(SortText);
            }
        }

        public int Offset => PageSize * (PageIndex - 1);
        public int Limit => PageSize;

        // rowNo start from 1, used by SQL BETWEEN {startRowNo} AND {endRowNo}
        public int StartRowNo => Offset + 1;
        public int EndRowNo => Offset + PageSize;
    }

    public class PageInfoDto
    {
        /// <summary>
        /// start from 1
        /// </summary>
        public readonly int PageIndex;

        public readonly int PageSize;

        public readonly string SortText;

        public PageInfoDto(int pageIndex, int pageSize, string sortText = "")
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortText = sortText;
        }
    }

    public class PagedResult
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalRows { get; private set; }
        public int LastPage => (int)Math.Ceiling((double)TotalRows / PageSize);
        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < LastPage;

        public PagedResult(PageInfo pageInfo, int totalRows)
        {
            var pageInfoNonNull = pageInfo ?? new PageInfo();

            PageIndex = pageInfoNonNull.PageIndex;
            PageSize = pageInfoNonNull.PageSize;
            TotalRows = totalRows;
        }
    }
}