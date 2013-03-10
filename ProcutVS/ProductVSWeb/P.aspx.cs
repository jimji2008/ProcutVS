using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.Text.RegularExpressions;
using ProcutVS;
using System.Text;
using System.Configuration;
using ProcutVS.Appinions;

public partial class P : System.Web.UI.Page
{
	string UPC = "";
	static readonly string[] IgnoreQANames;

	static P()
	{
		IgnoreQANames = ConfigurationManager.AppSettings["IGNORE_QA"].ToLower().Split(new[] { ',' });
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		UPC = Request["upc"];

		StringBuilder sb = new StringBuilder();

		try
		{
			Product product = ProductPool.GetByUPC(UPC, CacheType.Full);

			//
			sb.Append(HTMLGenerator.GetCategoryNavHtml(product.BBYCategoryPath));

			if (string.IsNullOrEmpty(product.Name))
			{
				sb.Append("<h1 class='product-name'>The product (UPC:" + UPC + ") does not exist.</h1>");
			}
			else
			{
				//
				sb.Append("<h1 class='product-name'>" + product.Name + "</h1>");
				//
				PrintProduct(product, sb);
			}

			PrintOpinions(product, sb);

			Response.Write(HTMLGenerator.GetHeaher(product.Name,
				product.Name + "Product Details,Product Prices, Product Reviews,Product Pros and Cons",
				"Product " + product.Name + " Product Details, Prices, Reviews and Pros & Cons."));
			Response.Write(sb.ToString());
			Response.Write(HTMLGenerator.GetFooter());
		}
		catch (Exception ex)
		{
			Response.Write(ex);
		}
	}

	private void PrintOpinions(Product product, StringBuilder sb)
	{
		List<Opinion> opinions = ProcutVS.Appinions.Server.GetOpinions(product.Name);
		if (opinions.Count == 0)
			return;

		sb.Append("<h2>Related Articles</h2>");

		sb.Append("<ol class='related_article'>");
		foreach (var opinion in opinions)
		{
			sb.Append(string.Format("<li><a href='{0}'>{1}</a> <span class='time'>{3}</span><br/>{2} <a href='{0}' class='more'>more ...</a></li>",
				opinion.doc_link, opinion.doc_title, 
				opinion.pre_sent + opinion.sent + opinion.post_sent,
				opinion.publish_date));
		}
		sb.Append("</ol>");
	}


	private void PrintProduct(Product product, StringBuilder sb)
	{
		sb.Append(@"
	<table class='p-table'>
	<tr>
	<td style='width:330px;text-align:center;'><a href='" +
				  product.LargeImageUrl + @"' target='_blank' title='Click to see big picture'><img src='" +
				  product.LargeImageUrl +
				  @"' style='max-width:300px;max-height:300px' /></a></td>
	<td ><p>");

		if (product.AmazonPrice > 0)
		{
			sb.Append(@"
Amazon Price: <span class='price'>$" +
					  product.AmazonPrice + @"</span> <a href='" + product.AmazonDetailsUrl +
					  "' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a>");
		}

		if (product.BBYSalePrice > 0)
		{

			sb.Append(@"
Best Buy Price: <span class='price'>$" +
					  product.BBYSalePrice + @"</span> <a href='" + product.BBYCJAffiliateUrl +
					  @"' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a><p>");
		}

		sb.Append(@"</p>
	<p class='desc' style='font-size:115%;margin:5px 0;'>" +
			  product.Desc + @"<p>");

		string html = HTMLGenerator.GetFeatursBlock(product);
		if (!string.IsNullOrEmpty(html))
		{
			sb.Append(@"
		<div class='feature' style='margin-top:8px;'>
		<p class='bold'>Features:</p><p>" + html + @"</p></div>");
		}

		sb.Append(@"
	</td>
	</tr>
	</table>
	");

		html = HTMLGenerator.GetBetterRanked(product, 7, 7);
		if (!string.IsNullOrEmpty(html))
		{
			sb.Append(
				@"<h2>Higher Better Ranked Similar Products - They might be better choices. Click to compare with it.</h2>");
			sb.Append(html);
		}

		html = GetProsConsBlock(product);
		if (!string.IsNullOrEmpty(html))
		{
			sb.Append(
				@"<h2>Pros & Cons<span style='float:right;font-size:80%;font-weight:normal;color:#999'>Provided by <a href='http://www.productwiki.com' target='_blank' style='color:#666;text-decoration: none;'>productwiki.com</a>, the free resource for UNBIASED product reviews and information</span></h2>");
			sb.Append(html);
		}

		html = HTMLGenerator.GetReviewsBlock(product);
		if (!string.IsNullOrEmpty(html))
		{
			sb.Append(@"<h2>Customers Reviews Reviews</h2>");
			sb.Append(html);
		}

		html = HTMLGenerator.GetAccessories(product);
		if (!string.IsNullOrEmpty(html))
		{
			sb.Append(@"<h2>Accessories</h2>");
			sb.Append(html);
		}
	}

	private string GetProsConsBlock(Product product)
	{
		if (product.ProductWikiProsCons.ProsList.Count == 0)
			return "";

		StringBuilder sb = new StringBuilder();

		sb.Append(@"
<div style='margin:10px 0'>
<div style='float:left;margin-right:20px;width:400px;'>
<span class=pros-t>Pros</span><ul class='pros'>");
		foreach (var pros in product.ProductWikiProsCons.ProsList)
		{
			sb.Append(@"
<li><span title='Number of Vote = Agreement - Disagreement'>" + pros.Score + @"</span> " + pros.Text + @"</li>
");
		}
		sb.Append("</ul>");

		sb.Append(@"
</div>
<div style='float:left;margin-right:10px;width:400px;'>
<span class=cons-t>Cons</span><ul class='cons'>");
		foreach (var cons in product.ProductWikiProsCons.ConsList)
		{
			sb.Append(@"
<li><span title='Number of Vote = Agreement - Disagreement'>" + cons.Score + @"</span> " + cons.Text + @"</li>
");
		}
		sb.Append(@"</ul>
</div><div>
<div class='clear'></div>
");

		return sb.ToString();
	}

}



