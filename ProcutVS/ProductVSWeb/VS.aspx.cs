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

public partial class VS : System.Web.UI.Page
{
	string UPC1 = "";
	string UPC2 = "";
	static readonly string[] IgnoreQANames;

	static VS()
	{
		IgnoreQANames = ConfigurationManager.AppSettings["IGNORE_QA"].ToLower().Split(new[] { ',' });
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		UPC1 = Request["upc1"];
		UPC2 = Request["upc2"];

		StringBuilder sb = new StringBuilder();

		try
		{
			Product product1 = ProductPool.GetByUPC(UPC1, CacheType.Full);
			Product product2 = ProductPool.GetByUPC(UPC2, CacheType.Full);

			HotProductPairManager.AddVisit(UPC1, UPC2);

			//
			sb.Append(HTMLGenerator.GetCategoryNavHtml(product1.BBYCategoryPath));

			//
			sb.Append("<h1 class='product-name'><a href='P.aspx?UPC=" + product1.UPC + "&name=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product1.Name)) + "'>" + product1.Name
				+ "</a> <span class='vs'>VS</span> <a href='P.aspx?UPC=" + product2.UPC + "&name=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product2.Name)) + "'>" +
				product2.Name + "</a></h1>");

			//
			PrintProductVS(product1, product2, sb);


			Response.Write(HTMLGenerator.GetHeaher(product1.Name + " VS " + product2.Name,
				"Product Comparison," + product1.Name + " VS " + product2.Name + " Better/Difference",
				"List diference between " + product1.Name + " and " + product2.Name + ", and reviews to help consumers choose better products."));
			Response.Write(sb.ToString());
			Response.Write(HTMLGenerator.GetFooter());
		}
		catch (Exception ex)
		{
			Logger.Error("vs Page", ex);

			Response.Write(ex);
		}
	}


	private void PrintProductVS(Product product1, Product product2, StringBuilder sb)
	{
		string vsSummaryHtml = PrintVSSummary(product1, product2);

		StringBuilder sbNameHeader = new StringBuilder();
		PrintProductVSLine("Product Name", product1.Name, product2.Name, null, "", sbNameHeader);

		if (product1.AmazonPrice > 0 && product2.AmazonPrice > 0)
			PrintProductVSLine(
				new VSLine()
				{
					Name = "Amazon Price",
					Value1 = "$" + product1.AmazonPrice,
					Value2 = "$" + product2.AmazonPrice,
					Value1Comment = "<a href='" + product1.AmazonDetailsUrl + "' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a>",
					Value2Comment = "<a href='" + product2.AmazonDetailsUrl + "' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a>",
					SmartVsValues = new SmartVSValue[] { 
					new SmartVSValue(){Value = product1.AmazonPrice,  Unit = "$",PreposeUnit=true} ,
					new SmartVSValue(){Value = product2.AmazonPrice,  Unit = "$",PreposeUnit=true} 
					},
					CssStyle = "font-size:120%;font-weight:bold;"
				},
				sbNameHeader);

		if (product1.BBYSalePrice > 0 && product2.BBYSalePrice > 0)
			PrintProductVSLine(
				new VSLine()
				{
					Name = "Best Buy Price",
					Value1 = "$" + product1.BBYSalePrice,
					Value2 = "$" + product2.BBYSalePrice,
					Value1Comment = "<a href='" + product1.BBYCJAffiliateUrl + "' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a>",
					Value2Comment = "<a href='" + product2.BBYCJAffiliateUrl + "' target='_blank' style='font-size:80%;font-weight:normal'>Visit Store</a>",
					SmartVsValues = new SmartVSValue[] { 
						new SmartVSValue(){Value = product1.BBYSalePrice, Unit = "$",PreposeUnit=true} ,
						new SmartVSValue(){Value = product2.BBYSalePrice, Unit = "$",PreposeUnit=true} 
					},
					CssStyle = "font-size:120%;font-weight:bold;"
				},
				sbNameHeader);


		sb.Append(@"
<script type='text/javascript' src='scripts/vs.js'></script>
<h2>Compare Details/Features/Prices<span style='color:#999;font-size:90%;font-weight:normal;float:right;'>Last updated at " + product1.UpdatedTime + @" MST</span></h2>
<div id='headerTable' style='top: 0; width:1165px; position: fixed;display:none'>
<table class='vs-table' style='margin:0'>" + sbNameHeader.ToString() + @"
</table>
<div class='header-shadow'></div>
</div>
<table class='vs-table'><tr><td ></td>
<td style='vertical-align:bottom;'><a href='P.aspx?UPC=" + product1.UPC + "&name=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product1.Name)) + "' title='See the product details'><img src='" + product1.LargeImageUrl + @"' style='max-width:200px;max-height:300px' /></a></td>
<td style='vertical-align:bottom;'><a href='P.aspx?UPC=" + product2.UPC + "&name=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(product2.Name)) + "' title='See the product details'><img src='" + product2.LargeImageUrl + @"' style='max-width:200px;max-height:300px' /></a></td>
<td ></td>
</tr>
" + sbNameHeader.ToString().Replace("<tr ", "<tr id='nameTr' ") + @"</tr>
	");

		//general


		//
		sb.Append(vsSummaryHtml);

		//
		if (product1.AmazonSaleRank > 0 && product2.AmazonSaleRank > 0)
		{
			PrintProductVSLine("Amazon Sale Rank", product1.AmazonSaleRank.ToString(), product2.AmazonSaleRank.ToString(),
			new SmartVSValue[] { 
				new SmartVSValue(){Value = product1.AmazonSaleRank, Unit = ""} ,
				new SmartVSValue(){Value = product2.AmazonSaleRank, Unit = ""} 
			}, "The lower the number, the better the item has sold on Amazon.", sb);
		}
		else if (product1.AmazonSaleRank > 0)
		{
			PrintProductVSLine("Amazon Sale Rank", product1.AmazonSaleRank.ToString(), null, null, "", sb);
		}
		else if (product2.AmazonSaleRank > 0)
		{
			PrintProductVSLine("Amazon Sale Rank", null, product2.AmazonSaleRank.ToString(), null, "", sb);
		}


		if (product1.BBYSalesRankShortTerm > 0 && product2.BBYSalesRankShortTerm > 0)
		{
			if (product1.AmazonSaleRank > 0 && product2.AmazonSaleRank > 0)
				PrintProductVSLine("Best Buy Short Term Sale Rank", product1.BBYSalesRankShortTerm.ToString(), product2.BBYSalesRankShortTerm.ToString(),
				new SmartVSValue[] { 
					new SmartVSValue(){Value = product1.BBYSalesRankShortTerm, Unit = ""} ,
					new SmartVSValue(){Value = product2.BBYSalesRankShortTerm, Unit = ""} 
				}, "The lower the number, the better the item has sold on Best Buy.", sb);
		}
		else if (product1.BBYSalesRankMediumTerm > 0 && product2.BBYSalesRankMediumTerm > 0)
		{
			if (product1.AmazonSaleRank > 0 && product2.AmazonSaleRank > 0)
				PrintProductVSLine("Best Buy Medium Term Sale Rank", product1.BBYSalesRankMediumTerm.ToString(), product2.BBYSalesRankMediumTerm.ToString(),
				new SmartVSValue[] { 
					new SmartVSValue(){Value = product1.BBYSalesRankMediumTerm, Unit = ""} ,
					new SmartVSValue(){Value = product2.BBYSalesRankMediumTerm, Unit = ""} 
				}, "The lower the number, the better the item has sold on Best Buy.", sb);
		}
		else if (product1.BBYSalesRankLongTerm > 0 && product2.BBYSalesRankLongTerm > 0)
		{
			if (product1.AmazonSaleRank > 0 && product2.AmazonSaleRank > 0)
				PrintProductVSLine("Best Buy Long Term Sale Rank", product1.BBYSalesRankLongTerm.ToString(), product2.BBYSalesRankLongTerm.ToString(),
				new SmartVSValue[] { 
					new SmartVSValue(){Value = product1.BBYSalesRankLongTerm, Unit = ""} ,
					new SmartVSValue(){Value = product2.BBYSalesRankLongTerm, Unit = ""} 
				}, "The lower the number, the better the item has sold on Best Buy.", sb);
		}
		else if (product1.BBYSalesRankShortTerm > 0)
		{
			PrintProductVSLine("Best Buy Short Term Sale Rank", product1.BBYSalesRankShortTerm.ToString(), null, null, "", sb);
		}
		else if (product2.BBYSalesRankShortTerm > 0)
		{
			PrintProductVSLine("Best Buy Short Term Sale Rank", null, product2.BBYSalesRankShortTerm.ToString(), null, "", sb);
		}

		PrintProductNoneVSLine("Desc", product1.DescShort, product2.DescShort, "", sb);
		//PrintProductNoneVSLine("Long Desc", product1.Desc, product2.Desc, "", sb);

		//features
		PrintProductNoneVSLine("Features", HTMLGenerator.GetFeatursBlock(product1), HTMLGenerator.GetFeatursBlock(product2), "", sb);

		//spec
		if (product1.SpecDic != null && product2.SpecDic != null)
		{
			foreach (var key in product1.SpecDic.Keys)
			{
				if (product2.SpecDic.ContainsKey(key))
				{
					PrintProductVSLine(product1.SpecDic[key].NameForDisplay, product1.SpecDic[key].Value, product2.SpecDic[key].Value,
									   new SmartVSValue[]
					                   	{
					                   		new SmartVSValue(product1.SpecDic[key].Value),
					                   		new SmartVSValue(product2.SpecDic[key].Value)
					                   	}, "", sb);
				}
				else
				{
					PrintProductVSLine(product1.SpecDic[key].NameForDisplay, product1.SpecDic[key].Value, null, null, "", sb);
				}
			}
			foreach (var key in product2.SpecDic.Keys)
			{
				if (!product1.SpecDic.ContainsKey(key))
				{
					PrintProductVSLine(product2.SpecDic[key].NameForDisplay, null, product2.SpecDic[key].Value, null, "", sb);
				}
			}
		}

		//pros cons
		PrintProductNoneVSLine("Pros & Cons <br />From Consumers' Reviews", GetProsConsBlock(product1), GetProsConsBlock(product2), "These Pros & Cons are collected by expert product reviews site <a href='http://www.productwiki.com' target='_blank' style='color:#666'>productwiki.com</a>.", sb);

		//reviews
		PrintProductNoneVSLine("Reviews", HTMLGenerator.GetReviewsBlock(product1), HTMLGenerator.GetReviewsBlock(product2), "", sb);

		// higher ranked
		PrintProductNoneVSLine("Better Ranked Similar Items", HTMLGenerator.GetBetterRanked(product1, 4, 2), HTMLGenerator.GetBetterRanked(product2, 4, 2), "These products are better ranked depending on price, features, sales volume and other factors.", sb);

		sb.Append(@"</table>
<div>
<div class='share-left'>SHARE this page<br /> <span style='color:#999'>if you think <br />the comparison is helpful</span>
</div>
<div class='share-right'>
<ul>
<li>
<div id='fb-root'></div><script src='http://connect.facebook.net/en_US/all.js#xfbml=1'></script><fb:like href='" + Request.Url + @"' send='true' width='450' show_faces='true' font='arial'></fb:like></li>
<li>
<!-- Place this tag in your head or just before your close body tag -->
<script type='text/javascript' src='https://apis.google.com/js/plusone.js'></script>
<!-- Place this tag where you want the +1 button to render -->
<g:plusone></g:plusone>
</li>
<li>
<script src='http://platform.twitter.com/widgets.js' type='text/javascript'></script>
<a href='http://twitter.com/share' class='twitter-share-button'>Tweet</a>
</li>
<li>
<script src='http://www.stumbleupon.com/hostedbadge.php?s=2&r=http://ProductVS.net'></script>
</li>
<li>
<script src='http://platform.linkedin.com/in.js' type='text/javascript'></script>
<script type='IN/Share' data-counter='right'></script>
</li>
</ul>
</div>
</div>
<div class='clear'></div>
");
	}

	private string PrintVSSummary(Product product1, Product product2)
	{
		List<AttributePair> attributePairList = new List<AttributePair>();
		if (product1.SpecDic != null)
		{
			foreach (var key in product1.SpecDic.Keys)
			{
				if (product2.SpecDic == null || !product2.SpecDic.ContainsKey(key))
					continue;
				SmartVSValue v1 = new SmartVSValue(product1.SpecDic[key].Value);
				SmartVSValue v2 = new SmartVSValue(product2.SpecDic[key].Value);
				attributePairList.Add(new AttributePair()
										{
											Name = product1.SpecDic[key].Name,
											Value1 = v1,
											Value2 = v2
										});
			}
		}

		List<AttributeVSResult> productVSResultList1 = new List<AttributeVSResult>();
		List<AttributeVSResult> productVSResultList2 = new List<AttributeVSResult>();
		Dictionary<string, AttributeVSResult[]> vsResultDic = AttributeComparer.SmartCompare(attributePairList.ToArray());

		foreach (var key in vsResultDic.Keys)
		{
			productVSResultList1.Add(vsResultDic[key][0]);
			productVSResultList2.Add(vsResultDic[key][1]);
		}

		String surrmary1 = PrintOneProductSummary(productVSResultList1, 1);
		String surrmary2 = PrintOneProductSummary(productVSResultList2, 2);

		StringBuilder sb = new StringBuilder();
		PrintProductNoneVSLine("Comparison Summary", surrmary1, surrmary2, "", sb);

		return sb.ToString();
	}

	private static string PrintOneProductSummary(List<AttributeVSResult> productVSResultList, int position)
	{
		StringBuilder sb1 = new StringBuilder();
		sb1.Append("<ul class='summary_ul'>");
		productVSResultList.Sort();
		foreach (var AttributeVSResult in productVSResultList)
		{
			string diff = "";
			if (!string.IsNullOrEmpty(AttributeVSResult.Diff)
				&& AttributeVSResult.Diff != "0")
				diff = " (<span class='diff'>" + AttributeVSResult.Diff + "</span>)";

			string desc = "";
			if (!string.IsNullOrEmpty(AttributeVSResult.Description))
				desc = " <span class='desc'>" + AttributeVSResult.Description + "</span>";


			sb1.Append("<li id='" + AttributeVSResult.Name.GetHashCode() + "_" + position + "' onmouseover='highlightSummaryItem(this)' class='summay-" + AttributeVSResult.GoodBad + "'><span class='name'>" + AttributeVSResult.Name + ":</span> <span class='value'>" + AttributeVSResult.Value + "</span>" + desc + diff + "</li>");
		}
		sb1.Append("</ul>");
		return sb1.ToString();
	}


	private bool altLine = false;
	private void PrintProductVSLine(string name, string value1, string value2, SmartVSValue[] smartVsValues, string comment, StringBuilder sb)
	{
		PrintProductVSLine(name, value1, value2, smartVsValues, comment, null, sb);
	}

	private void PrintProductVSLine(string name, string value1, string value2, SmartVSValue[] smartVsValues, string comment, string cssStyle, StringBuilder sb)
	{
		PrintProductVSLine(new VSLine()
			{
				Name = name,
				Value1 = value1,
				Value2 = value2,
				SmartComment = comment,
				SmartVsValues = smartVsValues,
				CssStyle = cssStyle
			}
			, sb);
	}

	private void PrintProductVSLine(VSLine vsLine, StringBuilder sb)
	{

		if (IgnoreQANames.Contains(vsLine.Name.ToLower()))
		{
			sb.Append(string.Format("<tr class='{1}'><td class='name-col'>{0}</td>", vsLine.Name, altLine ? "vs-tr-alt" : "vs-tr"));
		}
		else
		{
			sb.Append(string.Format("<tr class='{1}'><td class='name-col'><a href='/Glossary.aspx?name={0}&v1={2}&v2={3}&upc={4}' target='_blank' title='click to see explanation'>{0}</a></td>", vsLine.Name, altLine ? "vs-tr-alt" : "vs-tr", HttpUtility.UrlEncode(vsLine.Value1), HttpUtility.UrlEncode(vsLine.Value2), UPC1));
		}

		altLine = !altLine;

		if (vsLine.Value2 == null)
		{
			sb.Append(string.Format("<td class='value-col'>{0}</td><td class='value-col'><span style='color:#999'>N/A</span></td><td></td>", vsLine.Value1));
		}
		else if (vsLine.Value1 == null)
		{
			sb.Append(string.Format("<td class='value-col'><span style='color:#999'>N/A</span></td><td class='value-col'>{0}</td><td></td>", vsLine.Value2));
		}
		else
		{
			VSPair vsPair = VSCommon.PrintDiff2Values(vsLine.Value1, vsLine.Value2);
			sb.Append(string.Format("<td class='value-col'{1}>{0} {2}</td>", vsPair.Value1,
				string.IsNullOrEmpty(vsLine.CssStyle) ? "" : " style='" + vsLine.CssStyle + "'",
				vsLine.Value1Comment));
			sb.Append(string.Format("<td class='value-col'{1}>{0} {2}</td>", vsPair.Value2,
				string.IsNullOrEmpty(vsLine.CssStyle) ? "" : " style='" + vsLine.CssStyle + "'",
				vsLine.Value2Comment));

			// smart vs and comment
			if (vsPair.ChangeType != ChangeType.Unchanged
				&& vsLine.SmartVsValues != null
				&& vsLine.SmartVsValues.Length == 2
				&& vsLine.SmartVsValues[0] != null
				&& vsLine.SmartVsValues[1] != null)
			{
				decimal diff = vsLine.SmartVsValues[0].Value - vsLine.SmartVsValues[1].Value;
				if (diff != 0 && vsLine.SmartVsValues[0].Unit == vsLine.SmartVsValues[1].Unit)
				{
					if (vsLine.SmartVsValues[0].PreposeUnit)
					{
						sb.Append(string.Format("<td class='comment-col'><span class='smartvs'>{0} {1}{2}</span> {3}</td>",
						diff > 0 ? ">" : "<", vsLine.SmartVsValues[0].Unit, Math.Abs(diff),
						vsLine.SmartComment));
					}
					else
					{
						sb.Append(string.Format("<td class='comment-col'><span class='smartvs'>{0} {1}{2}</span> {3}</td>",
						diff > 0 ? ">" : "<", Math.Abs(diff), vsLine.SmartVsValues[0].Unit,
						vsLine.SmartComment));
					}
				}
				else
				{
					sb.Append("<td class='comment-col'></td>");
				}
			}
			else
			{
				sb.Append("<td class='comment-col'></td>");
			}
		}
		sb.Append("</tr>");
	}

	void PrintProductNoneVSLine(string name, string value1, string value2, string comment, StringBuilder sb)
	{
		if (string.IsNullOrEmpty(value1) && string.IsNullOrEmpty(value2))
			return;

		if (name.ToLower() == "reviews" || name.ToLower() == "better ranked similar items")
			altLine = false;

		sb.Append(string.Format(@"
<tr class='{4}'>
<td class='name-col'>{0}</td>
<td class='value-col'>{1}</td>
<td class='value-col'>{2}</td>
<td class='comment-col'>{3}</td>
", name, value1, value2, comment, altLine ? "vs-tr-alt" : "vs-tr"));

		altLine = !altLine;
	}



	private string GetProsConsBlock(Product product)
	{
		if (product.ProductWikiProsCons.ProsList.Count == 0)
			return "";

		StringBuilder sb = new StringBuilder();

		sb.Append(@"<span class=pros-t>Pros</span><ul class='pros'>");
		foreach (var pros in product.ProductWikiProsCons.ProsList)
		{
			sb.Append(@"
<li><span title='Number of Vote = Agreement - Disagreement'>" + pros.Score + @"</span> " + pros.Text + @"</li>
");
		}
		sb.Append("</ul>");

		sb.Append(@"<span class=cons-t>Cons</span><ul class='cons'>");
		foreach (var cons in product.ProductWikiProsCons.ConsList)
		{
			sb.Append(@"
<li><span title='Number of Vote = Agreement - Disagreement'>" + cons.Score + @"</span> " + cons.Text + @"</li>
");
		}
		sb.Append("</ul>");

		return sb.ToString();
	}
}

class VSLine
{
	internal string Name;
	internal string Value1;
	internal string Value2;
	internal SmartVSValue[] SmartVsValues;
	internal string SmartComment;
	internal string CssStyle;
	internal string Value1Comment;
	internal string Value2Comment;
}

