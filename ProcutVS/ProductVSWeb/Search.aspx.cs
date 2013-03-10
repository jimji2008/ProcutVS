using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ProcutVS;
using System.Text.RegularExpressions;

public partial class Search : System.Web.UI.Page
{
	static readonly Regex numberReg = new Regex(@"^\d{12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	protected void Page_Load(object sender, EventArgs e)
	{
		string keyword = Request["keyword"] ?? "";

		if (numberReg.IsMatch(keyword))
			Response.Redirect("/P.aspx?UPC=" + keyword, true);


		StringBuilder sb = new StringBuilder();

		sb.Append("<h1 class='product-name'>Search Results For '" + keyword + "'</h1>");

		PrintProducts(keyword, sb);

		sb.Append(@"

");
		Response.Write(HTMLGenerator.GetHeaher(HttpUtility.HtmlEncode(keyword), "Product Comparison," + HttpUtility.HtmlEncode(keyword), "List prodcts of keywords " + HttpUtility.HtmlEncode(keyword) + " for users to compare them and then choose the right one."));
		Response.Write(sb.ToString());
		Response.Write(HTMLGenerator.GetFooter());
	}



	void PrintProducts(string keyword, StringBuilder sb)
	{
		Products products = new Products();
		int.TryParse(Request["page"], out products.CurrentPage);
		products.CurrentPage = products.CurrentPage == 0 ? 1 : products.CurrentPage;
		BestBuySearchProductsFiller.Do(products, keyword);

		sb.Append("<div>");
		sb.Append("<h2>Product VS Pair List - Total " + products.Total + " items in " + products.TotalPages + " pages</h2>");

		sb.Append("<ul class='better-ranked'>");

		foreach (var product in products)
		{
			ProductPool.Cache(product, CacheType.Simple);

			sb.Append(@"
<li>
<a href='/P.aspx?upc=" + product.UPC + @"&title=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product.Name)) + @"' title='" + product.Name + (product.BBYSalePrice > 0 ? " $" + product.BBYSalePrice : "") + @"'>
<img src='" + (string.IsNullOrEmpty(product.LargeImageUrl) ? product.ThumbnailImageUrl : product.LargeImageUrl) + @"' /><br />
" + product.Name + @"
" + (product.BBYSalePrice > 0 ? "<br /><span style='color:#000'>$" + product.BBYSalePrice + "</span>" : "") + @"
</a>
</li>
");
		}
		sb.Append("</ul>");
		sb.Append("<div class='clear' style='width:100%;'></div>");

		sb.Append("</div>");

		//page

		string baseUrl = "/Search.aspx?keyword=" + HttpUtility.UrlEncode(keyword);
		HTMLGenerator.PrintPageNav(products.CurrentPage, products.TotalPages, baseUrl, sb);
	}
}