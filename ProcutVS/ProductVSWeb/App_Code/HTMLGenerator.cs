using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using ProcutVS;
using System.Configuration;

/// <summary>
/// Summary description for HTMLGenerator
/// </summary>
public class HTMLGenerator
{
	private static readonly string StatCountCode;
	private static readonly string[] IgnoreIPS;
	static HTMLGenerator()
	{
		IgnoreIPS = ConfigurationManager.AppSettings["IGNORE_IPS"].Split(new[] { ',' });

		StatCountCode = @"
<script type='text/javascript'>
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-24901873-1']);
  _gaq.push(['_trackPageview']);
  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>

<!-- Start of StatCounter Code for Default Guide -->
<script type='text/javascript'>
var sc_project=6980349; 
var sc_invisible=1; 
var sc_security='a3e93aa1'; 
</script>
<script type='text/javascript'
src='http://www.statcounter.com/counter/counter.js'></script>
<noscript><div class='statcounter'><a title='tumblr visit
counter' href='http://statcounter.com/tumblr/'
target='_blank'><img class='statcounter'
src='http://c.statcounter.com/6980349/0/a3e93aa1/1/'
alt='tumblr visit counter'></a></div></noscript>
<!-- End of StatCounter Code for Default Guide -->

<!-- Begin Motigo Webstats counter code -->
<a id='mws4851939' href='http://webstats.motigo.com/'>
<img width='80' height='15' border='0' alt='Free counter and web stats' src='http://m1.webstats.motigo.com/n80x15.gif?id=AEoI4w3/3i/uep48KArM9sArUUEg' /></a>
<script src='http://m1.webstats.motigo.com/c.js?id=4851939&amp;lang=EN&amp;i=3' type='text/javascript'></script>
<!-- End Motigo Webstats counter code -->
";
	}

	public static string GetHeaher(string title, string keywords, string desc)
	{
		string html = @"
<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head runat='server'>
	<link type='text/css' rel='stylesheet' href='/css/vs.css'> 
	<script type='text/javascript' src='/scripts/jquery-1.4.1.min.js'></script>
	<script type='text/javascript' src='/scripts/watermark.js'></script>
	<script>
		jQuery(function ($) {
			$('#search_box').Watermark('keywords / upc code');
		});
	</script>
    <title>Product VS - " + title + @"</title>
	<meta name='keywords' content='" + keywords + @"' />
	<meta name='description' content='" + desc + @"' />
</head>
<body>
    <div id='body-center'>
		<div id='vs-header'>
		<table><tr><td id='logo'><a href='/' title='site home ProductVS.net'>ProductVS.net</a></td>
		<td id='sitedesc'>We list the facts, You choose the right products.</td>
		<td id='search'><form method='get' action='Search.aspx'><input id='search_box' name='keyword' style='width:200px;' /><input type='submit' value='Search' /></form></td></tr></table>	
		</div>
";

		return html;
	}

	public static string GetFooter()
	{
		string statCountCode = StatCountCode;
		if (VSCommon.IsLiveEnv == false)
		{
			statCountCode = "";
		}
		else
		{
			foreach (string ip in IgnoreIPS)
			{
				if (ip == HttpContext.Current.Request.UserHostAddress)
				{
					statCountCode = "";
					break;
				}
			}
		}

		string html = @"
<div id='vs-footer'>
<span>&copy;2011 ProductVS.net(Beta). All Rights Reserved. Contact us by email productvs at Yahoo dot com.</span>
</div>
</div>
" + statCountCode + @"
</body>
</html>
";

		return html;
	}

	public static string GetCategoryNavHtml(IEnumerable<Remix.Category> catePaths)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append("<div class='cate-path'>");
		sb.Append(string.Format(@"<a href='/Category.aspx?id={0}'>Home</a>", Remix.Server.ROOT_CATEGORY_ID));
		if (catePaths != null)
		{
			foreach (var categoryPath in catePaths)
			{
				if (categoryPath.Id != Remix.Server.ROOT_CATEGORY_ID)
					sb.Append(
						string.Format(@" <span class='connector'>-&gt;</span> <a href='/Category.aspx?id={0}' title='{1}'>{1}</a>",
						              categoryPath.Id, categoryPath.Name));
			}
		}
		sb.Append("</div>");

		return sb.ToString();
	}


	public static void PrintSubCategoryNav(StringBuilder sb, Remix.Category category)
	{
		sb.Append("<ul class='sub-cate'>");

		// warm up
		List<string> categoryIds = new List<string>();
		foreach (var subCategory in category.SubCategories)
		{
			categoryIds.Add(subCategory.Id);
		}
		CategoryPool.WarmUpCategories(categoryIds);


		// sub
		foreach (var subCategory in category.SubCategories)
		{
			sb.Append(string.Format(@"<li><a href='/Category.aspx?id={0}' title='{1}'>{1}</a>", subCategory.Id, subCategory.Name));
			Remix.Category fullSubCategory = CategoryPool.GetById(subCategory.Id);

			// sub-sub
			if (fullSubCategory.SubCategories.Count > 0)
			{
				int i = 0;
				sb.Append(" - ");
				foreach (var subSubCategory in fullSubCategory.SubCategories)
				{
					sb.Append(string.Format(@"<a href='/Category.aspx?id={0}' title='{1}' class='sub-sub-cate'>{1}</a>, ",
											subSubCategory.Id, subSubCategory.Name));

					if (i++ == 3)
						break;
				}
				sb.Append(
					string.Format(@"<a href='/Category.aspx?id={0}' title='more subcategories' class='sub-sub-cate'>more...</a></li>",
								  subCategory.Id));
			}
		}
		sb.Append("</ul>");
	}

	static Random random = new Random();
	public static string GetBetterRanked(Product product, int count, int cols)
	{
		StringBuilder sb = new StringBuilder();

		RankedProduct[] betterRankedProducts = RankedProductManager.GetRankInfo(product.BBYSubClassId, product.UPC).BetterRankedProducts;

		if (betterRankedProducts.Length == 0)
			return "";


		// init index list
		List<int> inxList = new List<int>();
		if (betterRankedProducts.Length > count + count / 2)
		{
			while (inxList.Count < count)
			{
				int i = random.Next(betterRankedProducts.Length);
				if (!inxList.Contains(i))
					inxList.Add(i);
			}
		}
		else
		{
			for (int i = 0; i < count && i < betterRankedProducts.Length; i++)
			{
				inxList.Add(i);
			}
		}

		// warm up
		List<string> Upcs = new List<string>();
		foreach (var i in inxList)
		{
			Upcs.Add(betterRankedProducts[i].UPC);
		}
		ProductPool.WarmUpProductsByUpcs(Upcs);

		//
		sb.Append("<ul class='better-ranked'>");
		for (int i = 0; i < inxList.Count; i++)
		{
			Product p = ProductPool.GetByUPC(betterRankedProducts[inxList[i]].UPC, CacheType.Simple);

			sb.Append(@"
<li>
<a href='/VS.aspx?upc1=" + product.UPC + @"&upc2=" + p.UPC + @"&title=" + HttpUtility.HtmlEncode(product.Name + " VS " + p.Name) + @"' title='Click to compare with it. " + p.Name + @"'>
<img src='" + (string.IsNullOrEmpty(p.LargeImageUrl) ? p.ThumbnailImageUrl : p.LargeImageUrl) + @"' />
" + p.Name + @"
</a>
</li>
");

			if ((i + 1) % cols == 0)
				sb.Append("<div class='clear' style='width:100%;'></div>");

		}
		sb.Append("</ul>");
		sb.Append("<div class='clear'></div>");

		if (betterRankedProducts.Length > count + count / 2)
		{
			sb.Append("<div style='margin-bottom:15px;'><span class='connector'>-&gt;</span> <a href='BetterRanked.aspx?UPC=" + product.UPC + "&title=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product.Name)) + "' title='Better ranked products compared to " + product.Name + "'>See all " + betterRankedProducts.Length + " better ranked products</a></div>");
		}

		return sb.ToString();
	}


	public static string GetAccessories(Product product)
	{
		StringBuilder sb = new StringBuilder();

		// warm up
		List<string> Skus = new List<string>();
		foreach (var p in product.Accessories)
		{
			Skus.Add(p.BBYSKU);
		}
		ProductPool.WarmUpProductsBySkus(Skus);

		if (product.Accessories.Count == 0)
			return string.Empty;

		//
		sb.Append("<ul class='better-ranked'>");
		foreach (var p in product.Accessories)
		{
			Product refershedP = ProductPool.GetByBestBuySKU(p.BBYSKU, CacheType.Simple);
			sb.Append(@"
<li>
<a href='/P.aspx?upc=" + refershedP.UPC + @"&title=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product.Name)) + @"' title='" + refershedP.Name + @"'>
<img src='" + (string.IsNullOrEmpty(refershedP.LargeImageUrl) ? refershedP.ThumbnailImageUrl : refershedP.LargeImageUrl) + @"' />
" + refershedP.Name + @"
</a>
</li>
");
		}
		sb.Append("</ul>");
		sb.Append("<div class='clear' style='width:100%;'></div>");

		return sb.ToString();
	}


	public static string GetReviewsBlock(Product product)
	{
		StringBuilder sb = new StringBuilder();

		sb.Append(@"<div style='margin-bottom:10px;'>");
		foreach (var review in product.Reviews)
		{
			string starHtml = @"<div class='rating_bar' title='" + review.Rating.ToString("#.#") + @" Stars'><div style='width:" + review.Rating / 5 * 100 + "%'></div></div>";

			sb.Append(@"
<ul class='review'>
<li class='title'>" + review.Title + @"</li>
<li class='author'>" + review.Author + @"</li>
<li class='pubtime' title='pub time'>" + review.PubTime + @"</li>
<li class='rating' title='Rating'>" + starHtml + @"</li>
<li class='content'>" + review.Content + @"</li>
<li class='source'>source: <a href='" + product.BBYCJAffiliateUrl + @"' title='source url'>" + review.Source + @"</a></li>
</ul>
");
		}
		sb.Append(@"
<p style='margin:10px 0;'><span class='connector'>-&gt;</span> <a href='" + product.BBYCJAffiliateUrl + @"' target='_blank'>Read more reviews on Best Buy ...</a><br />
<span class='connector'>-&gt;</span> <a href='" + product.AmazonDetailsUrl + @"' target='_blank'>Read more reviews on Aamazon ...</a></p>
</div>
");

		return sb.ToString();

	}

	public static string GetFeatursBlock(Product product)
	{
		if (product.Features.Count == 0)
			return "";

		StringBuilder sb = new StringBuilder();

		sb.Append(@"<ol>");
		foreach (var feature in product.Features)
		{
			sb.Append(@"
<li>" + feature.Desc + @"</li>
");
		}
		sb.Append("</ol>");

		return sb.ToString();
	}


	public static void PrintPageNav(int current, int total, string baseUrl, StringBuilder sb)
	{
		sb.Append(@"
<div>
Page <b>" + current + "</b> <span class='connector'>|</span> Total <b>" + total + "</b> Pages");

		if (current > 1)
			sb.Append(" <span class='connector'>|</span> <a href='" + baseUrl + "&page=1'>First Page</a> <span class='connector'>|</span> <a href='" + baseUrl + "&page=" + (current - 1) + "'>Previous Page</a>");

		if (current < total)
			sb.Append(" <span class='connector'>|</span> <a href='" + baseUrl + "&page=" + (current + 1) + @"'>Next Page</a> <span class='connector'>|</span> <a href='" + baseUrl + "&page=" + (total) + @"'>Last Page</a>");

		sb.Append(@"
</div>
");
	}
}