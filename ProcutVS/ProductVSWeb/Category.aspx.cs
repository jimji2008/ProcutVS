using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ProcutVS;

public partial class Category : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string categoryId = Request["id"] ?? Remix.Server.ROOT_CATEGORY_ID;

		Remix.Category category = CategoryPool.GetById(categoryId);

		StringBuilder sb = new StringBuilder();

		sb.Append(HTMLGenerator.GetCategoryNavHtml(category.Path));

		sb.Append("<div>");
		if (category.SubCategories.Count > 0)
			sb.Append("<h2>Subcategories</h2>");

		HTMLGenerator.PrintSubCategoryNav(sb, category);

		sb.Append("</div>");

		PrintProductVSByCategory(category, sb);

		sb.Append(@"

");
		string categoryName = category.Path[category.Path.Count - 1].Name;
		Response.Write(HTMLGenerator.GetHeaher(categoryName, "Product Comparison," + categoryName, "List prodcts under category " + categoryName + " for users to compare them and then choose the right one."));
		Response.Write(sb.ToString());
		Response.Write(HTMLGenerator.GetFooter());
	}



	void PrintProductVSByCategory(Remix.Category category, StringBuilder sb)
	{
		Products products = new Products();
		int.TryParse(Request["page"], out products.CurrentPage);
		products.CurrentPage = products.CurrentPage == 0 ? 1 : products.CurrentPage;
		BestBuyCategoryProductsFiller.Do(products, category);

		sb.Append("<div>");
		sb.Append("<h2>Product VS Pair List - Total " + products.TotalPages + " Pages</h2>");

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
		string baseUrl = "/Category.aspx?id=" + HttpUtility.UrlEncode(category.Id);
		HTMLGenerator.PrintPageNav(products.CurrentPage, products.TotalPages, baseUrl, sb);
	}
}