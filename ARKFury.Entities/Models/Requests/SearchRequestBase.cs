using ArkFury.Common.Models;

namespace ArkFury.Entities.Models.Requests
{
    public abstract class SearchRequestBase : PagingRequest
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string SearchTerm { get; set; }

        public SearchRequestBase()
        {
            Type = string.Empty;
            SearchTerm = string.Empty;
        }

        public SearchRequestBase(bool all) : base(all)
        {
            Type = string.Empty;
            SearchTerm = string.Empty;
        }

        public SearchRequestBase(PagingRequest request)
        {
            Size = request.Size;
            From = request.From;
            Sort = request.Sort;
            Fields = request.Fields;

            Type = string.Empty;
            SearchTerm = string.Empty;
        }

        public SearchRequestBase(string id)
        {
            Id = id;
            Type = string.Empty;
            SearchTerm = string.Empty;
        }
    }
}
