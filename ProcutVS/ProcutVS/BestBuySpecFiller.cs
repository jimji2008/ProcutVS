using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcutVS
{
	public class BestBuySpecFiller
	{
		static readonly Regex doctypeReg = new Regex(@"<.{4}pt35.{3}>(.*?)<.{4}pt36.{3}>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		/// <summary>
		/// by BestBuyUrl
		/// </summary>
		/// <param name="product">BestBuyUrl</param>
		public static void Do(Product product)
		{
			product.SpecDic = DataAccess.ReadProductSpecByUPC(product.UPC);
			if (product.SpecDic != null)
			{
				return;
			}

			//
			if (string.IsNullOrEmpty(product.BBYUrl))
				return;

			product.SpecDic = new Dictionary<string, Specification>();

			try
			{
				WebClient client = GetWebClient();
				string html = client.DownloadString(product.BBYUrl);

				XmlDocument doc = Html2Xml.Convert(html);

				XmlNodeList nodes = doc.SelectNodes("//div[@id='tabbed-specifications']//li");
				if (nodes != null)
				{
					foreach (XmlNode node in nodes)
					{
						string name = node.SelectSingleNode("div[@class='label']").InnerText.Trim();
						string value = node.SelectSingleNode("div[@class='data']").InnerText.Trim();
						product.SpecDic.Add(name, new Specification() { Name = name, Value = value, CanDiff = true, NameForDisplay = name });
					}
				}

				if(product.SpecDic.Count > 0)
					DataAccess.WriteProductSpecByUPC(product.UPC, product.SpecDic);
			}
			catch (Exception ex)
			{
				//throw;
			}
		}

		internal static WebClient GetWebClient()
		{
			WebClient webClient;
			webClient = new WebClient();
			webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3");
			webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
			webClient.Headers.Add("Accept-Language", "en-us,zh-cn;q=0.7,zh;q=0.3");
			webClient.Headers.Add("Accept-Charset", "UTF-8,*");
			webClient.Encoding = Encoding.UTF8;

			return webClient;
		}
	}
}
