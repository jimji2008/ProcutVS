using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using ProcutVS;
using Remix;

namespace ProductVSConsole
{
	class SiteMapGenerator
	{
		internal static void Do()
		{
			SiteMapUrlSet urlSet = new SiteMapUrlSet();

			//homepage
			Console.WriteLine("Gen homepage.");
			urlSet.Add(new SiteMapUrl()
			{
				Loc = "http://www.productvs.net/",
				Lastmod = DateTime.Now,
				Changefreq = "daily",
				Priority = "1.0"
			});

			//categories
			string categoryId = Remix.Server.ROOT_CATEGORY_ID;
			//categoryId = "abcat0208006";
			GenCategoryUrls(urlSet, categoryId);

			//
			string xml = UTF8XmlSerializer.Serialize(urlSet);
			File.WriteAllText("sitemap.xml", xml);
		}

		private static void GenCategoryUrls(SiteMapUrlSet urlSet, string categoryId)
		{
			Console.WriteLine("Gen Category, categoryId: " + categoryId);

			Remix.Category category = CategoryPool.GetById(categoryId);

			urlSet.Add(new SiteMapUrl()
						{
							Loc = string.Format(@"http://www.productvs.net/Category.aspx?name={1}&id={0}'", category.Id, HttpUtility.UrlEncode(category.Name)),
							Lastmod = DateTime.Now,
							Changefreq = "weekly",
							Priority = "0.8"
						});

			foreach (var subCategory in category.SubCategories)
			{
				GenCategoryUrls(urlSet, subCategory.Id);
			}
		}
	}


	[XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
	public class SiteMapUrlSet : List<SiteMapUrl>
	{

	}

	[XmlType("url")]
	public class SiteMapUrl
	{
		[XmlElement("loc")]
		public string Loc;
		[XmlElement("lastmod")]
		public DateTime Lastmod;
		[XmlElement("changefreq")]
		public string Changefreq;
		[XmlElement("priority")]
		public string Priority;
	}
}
