using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GameStore.Web.Pagination
{
    public static class PagingsHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, int currentPage, int totalPages, string currentUrl, int range = 3)
        {
            var resultHtml = new StringBuilder();

            var listTag = new TagBuilder("ul");
            listTag.AddCssClass("pagination");

            var listItems = new StringBuilder();

            var leftSide = 1;
            var rightSide = currentPage + range;

            if (currentPage > range)
            {
                leftSide = currentPage - range;
            }

            if (totalPages - currentPage <= range)
            {
                rightSide = totalPages;
            }

            for (var pageNumber = leftSide; pageNumber <= rightSide; pageNumber++)
            {
                var listItem = new TagBuilder("li");

                var linkTag = new TagBuilder("a");
                linkTag.SetInnerText(pageNumber.ToString());
                linkTag.MergeAttribute("href", ConcatUrlWithPageNumber(currentUrl, pageNumber).ToString());

                listItem.AddCssClass("page-button");
                listItem.InnerHtml = linkTag.ToString();

                if (pageNumber == currentPage)
                {
                    listItem.AddCssClass("active");
                }

                listItems.AppendLine(listItem.ToString());
            }

            listTag.InnerHtml = listItems.ToString();
            resultHtml.Append(listTag);

            return MvcHtmlString.Create(resultHtml.ToString());
        }

        private static StringBuilder ConcatUrlWithPageNumber(string currentUrl, int pageNumber)
        {
            if (currentUrl.Contains("page="))
            {
                currentUrl = new Regex(@"((\&|\?)page=(\d)*)").Replace(currentUrl, string.Empty);
            }

            var fullUrl = new StringBuilder(currentUrl);
            fullUrl.Append(currentUrl.Contains("&") ? "&" : "?");
            fullUrl.Append($"page={pageNumber}");

            return fullUrl;
        }
    }
}