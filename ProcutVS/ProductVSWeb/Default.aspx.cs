using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ProcutVS;

public partial class _Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		StringBuilder sb = new StringBuilder();

		PrintHotProductVS(sb);

		Remix.Category category = CategoryPool.GetById(Remix.Server.ROOT_CATEGORY_ID);

		sb.Append("<div>");
		if (category.SubCategories.Count > 0)
			sb.Append("<h2>View By Category</h2>");

		HTMLGenerator.PrintSubCategoryNav(sb, category);

		sb.Append("</div>");

		sb.Append(@"

");

		Response.Write(HTMLGenerator.GetHeaher(category.Path[category.Path.Count - 1].Name, "Product Comparison, Shopping Comparison, Product VS, Product Rating, Product Reviews", "To list detailed comparison report of products features and factors to help consumers choose the right products they need. "));
		Response.Write(sb.ToString());
		Response.Write(HTMLGenerator.GetFooter());

	}

	private void PrintHotProductVS(StringBuilder sb)
	{
		KeyValuePair<ProductPair, int>[] pairs = HotProductPairManager.GetHostProductPairs();

		// warm up
		List<string> Upcs = new List<string>();
		foreach (KeyValuePair<ProductPair, int> pair in pairs)
		{
			Upcs.Add(pair.Key.UPC1);
			Upcs.Add(pair.Key.UPC2);
		}
		ProductPool.WarmUpProductsByUpcs(Upcs);

		sb.Append("<div>");
		sb.Append("<h2>Hot Product Comparisons</h2>");
		sb.Append("<ul class='vs-ul'>");
		int i = 1;
		foreach (KeyValuePair<ProductPair, int> keyPair in pairs)
		{
			if (keyPair.Key == null)
				continue;

			Product product1 = ProductPool.GetByUPC(keyPair.Key.UPC1, CacheType.Simple);
			Product product2 = ProductPool.GetByUPC(keyPair.Key.UPC2, CacheType.Simple);

			sb.Append(string.Format("<li {8}><a href='/VS.aspx?upc1={0}&upc2={1}&p1={6}&p2={7}' title='{2} VS {3}'><span><img src='{4}' /> {2}  </span><span class='vs'>VS</span><span><img src='{5}' /> {3} </span></a></li>",
				product1.UPC, product2.UPC, product1.Name, product2.Name,
				string.IsNullOrEmpty(product1.LargeImageUrl) ? product1.ThumbnailImageUrl : product1.LargeImageUrl,
				string.IsNullOrEmpty(product2.LargeImageUrl) ? product2.ThumbnailImageUrl : product2.LargeImageUrl,
				HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product1.Name)),
				HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product2.Name)),
				(i+1 % 2 == 0) ? "style='margin-right:45px;'" : ""));

			if (i+1 % 2 == 0)
				sb.Append("<div style='clear:both;'></div>");

			i++;
		}
		sb.Append("<div style='clear:both;'></div>");

		sb.Append("</ul></div>");

	}
}