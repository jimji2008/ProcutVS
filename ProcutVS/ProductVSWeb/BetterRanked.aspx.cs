using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ProcutVS;

public partial class BetterRanked : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string UPC = Request["upc"];

		StringBuilder sb = new StringBuilder();
		Product product = ProductPool.GetByUPC(UPC, CacheType.Simple);

		sb.Append("<h1 class='product-name'>Better Ranked Products Compared With <a href='P.aspx?UPC=" + product.UPC + "&name=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product.Name)) + "'>" + product.Name + "</a></h1>");

		PrintProducts(product, sb);

		sb.Append(@"

");
		Response.Write(HTMLGenerator.GetHeaher("Better Ranked Products", "Product Comparison,Better Ranked Products", "List prodcts of Better Ranked Products products, let users to compare them and then choose the right one."));
		Response.Write(sb.ToString());
		Response.Write(HTMLGenerator.GetFooter());
	}

	private const int PAGE_SIZE = 10;
	void PrintProducts(Product product, StringBuilder sb)
	{
		RankInfo renkInfo = RankedProductManager.GetRankInfo(product.BBYSubClassId, product.UPC);
		int page;
		if (!int.TryParse(Request["page"], out page))
			page = page == 0 ? 1 : page;

		// warm up
		List<string> upcList = new List<string>();
		for (int i = (page - 1) * PAGE_SIZE; i < page * PAGE_SIZE && i < renkInfo.BetterRankedProducts.Length; i++)
		{
			upcList.Add(renkInfo.BetterRankedProducts[i].UPC);
		}
		ProductPool.WarmUpProductsByUpcs(upcList);

		//
		int totalPage = renkInfo.BetterRankedProducts.Length / PAGE_SIZE;
		totalPage += (renkInfo.BetterRankedProducts.Length % PAGE_SIZE == 0) ? 0 : 1;

		sb.Append("<div>");
		sb.Append("<h2>Better Ranked Product List - Total " + totalPage + " Pages</h2>");

		sb.Append("<ul class='better-ranked'>");


		foreach (var betterProductUpc in upcList)
		{
			Product betterProduct = ProductPool.GetByUPC(betterProductUpc, CacheType.Simple);

			sb.Append(@"
<li>
<a href='/VS.aspx?upc1=" + product.UPC + "&UPC2=" + betterProduct.UPC + @"&title=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product.Name)) + @" VS " + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(betterProduct.Name)) + @"' title='" + betterProduct.Name + @"'>
<img src='" + (string.IsNullOrEmpty(betterProduct.LargeImageUrl) ? betterProduct.ThumbnailImageUrl : betterProduct.LargeImageUrl) + @"' />
" + betterProduct.Name + @"
</a>
</li>
");
		}
		sb.Append("</ul>");
		sb.Append("<div class='clear' style='width:100%;'></div>");

		sb.Append("</div>");

		//page

		string baseUrl = "/BetterRanked.aspx?upc=" + product.UPC;
		HTMLGenerator.PrintPageNav(page, totalPage, baseUrl, sb);
	}
}