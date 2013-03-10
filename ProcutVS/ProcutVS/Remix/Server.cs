/*    Remix.NET
 * 
 * Remix.NET is licensed under the MIT license:
 * http://www.opensource.org/licenses/mit-license.html
 * 
 * Copyright (c) 2009 Omar Abdelwahed
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;
using System.Web;
using ProcutVS;

namespace Remix
{
	public class Server
	{
		// Version 1 Product interface. Change with new versions!
		private static string PRODUCT = "http://api.remix.bestbuy.com/v1/products";
		private static string REVIEW = "http://api.remix.bestbuy.com/v1/reviews";
		private static string CATEGORY = "http://api.remix.bestbuy.com/v1/categories";

		// Your Remix API key goes here.
		private static string APIKEY = "khzzhe8e7gmcehzn9egx3rqs";
		// CJ Publisher Id
		private static string PID = "5324381";

		public readonly static string ROOT_CATEGORY_ID = "cat00000";

		private string user = "";
		private string pass = "";

		public Server(string u, string p)
		{
			this.user = u;
			this.pass = p;
		}

		public string Get(string url)
		{
			try
			{
				// Create the web request   
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(this.user + ":" + this.pass)));

				// HACK: Fixes "417 Expectation Failed".
				System.Net.ServicePointManager.Expect100Continue = false;

				// Get response   
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				// check return codes
				//200 OK: everything went awesome.
				if (response.StatusCode == HttpStatusCode.OK)
					return new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();
				else
					return null; // and log!
			}
			catch (Exception e)
			{
				Logger.Error("Remix Error",e);
				return null;
			}
		}

		public string Post(string url, string status)
		{
			try
			{
				// Create the web request   
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(this.user + ":" + this.pass)));

				// Set type to POST   
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";

				// Create the data we want to send   
				StringBuilder data = new StringBuilder();
				data.Append("status=" + HttpUtility.UrlEncode(status));

				// Create a byte array of the data we want to send   
				byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

				// Set the content length in the request headers   
				request.ContentLength = byteData.Length;

				// HACK: Fixes "417 Expectation Failed" error from Twitter.
				System.Net.ServicePointManager.Expect100Continue = false;

				// Write data   
				using (Stream postStream = request.GetRequestStream())
				{
					postStream.Write(byteData, 0, byteData.Length);
				}

				// Get response   
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				// check return codes
				if (response.StatusCode == HttpStatusCode.OK)
					return new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();
				else
					return null; // and log!
			}
			catch (WebException we)
			{
				return null;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public Products GetProduct(string skuid, bool bTerse)
		{
			return this.GetProduct(skuid, bTerse, null, null);
		}

		public Products GetProduct(string skuid, bool bTerse, string postalcode, string radius)
		{
			string search = "sku='" + skuid + "'";
			string show = "";
			string area = "";

			if (bTerse) show = "&show=sku,productId,name,type,regularPrice,salePrice,url,inStoreAvailability,onlineAvailability,thumbnailImage";
			if (postalcode != null && radius != null)
			{
				area = "+stores(area(" + postalcode + "," + radius + "))";
				if (bTerse) show += ",stores";
			}

			Products p = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")" + area + "?apiKey=" + APIKEY + "&PID=" + PID + show));
			return p;
		}

		public Products GetProduct(string UPC)
		{
			return GetProductByUpcs(new string[] { UPC });
		}

		public Products GetProductByUpcs(string[] UPCs)
		{
			if (UPCs == null || UPCs.Length == 0)
			{
				return new Products();
			}

			string search = "upc in(" + string.Join(",", UPCs) + ")";

			Products p = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")?apiKey=" + APIKEY + "&PID=" + PID));
			return p;
		}


		public Products GetProductByKeywords(string[] keywords, int ipage)
		{
			if (keywords == null || keywords.Length == 0)
			{
				return new Products();
			}

			string search = "search=" + string.Join(" ", keywords);

			string spage = "";
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			string show = "&show=upc,sku,name,regularPrice,salePrice,url,thumbnailImage,largeImage,image,categoryPath,shortDescription,relatedProducts,subclass,subclassId";
			string sort = "&sort=salesRankShortTerm";

			Products p = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")?apiKey=" + APIKEY + "&PID=" + PID + spage + show + sort));
			return p;
		}

		public Products GetProductBySkus(string[] SKUs)
		{
			if (SKUs == null || SKUs.Length == 0)
			{
				return new Products();
			}

			string search = "sku in(" + string.Join(",", SKUs) + ")";

			Products p = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")?apiKey=" + APIKEY + "&PID=" + PID));
			return p;
		}

		public Products GetProduct(Category category, int ipage)
		{
			string search = "categoryPath.Id='" + category.Id + "'";
			string spage = "";
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			string show = "&show=upc,sku,name,regularPrice,salePrice,url,thumbnailImage,largeImage,image,categoryPath,shortDescription,relatedProducts,subclass,subclassId";
			string sort = "&sort=salesRankShortTerm";

			Products p = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")?apiKey=" + APIKEY + "&PID=" + PID + spage + show + sort));
			return p;
		}

		public Products GetGames(string args, bool bTerse)
		{
			return this.GetGames(args, bTerse, 0, null, null);
		}

		public Products GetGames(string args, bool bTerse, int ipage)
		{
			return this.GetGames(args, bTerse, ipage, null, null);
		}

		public Products GetGames(string args, bool bTerse, int ipage, string postalcode, string radius)
		{
			args = args.Replace(" ", "%20");
			string search = "type='Game'"
				+ "&name='" + args + "*'";
			string sort = "&sort=regularPrice.desc";
			string show = "";
			string spage = "";
			string area = "";
			if (bTerse) show = "&show=sku,productId,name,type,regularPrice,salePrice,url,inStoreAvailability,onlineAvailability,thumbnailImage";
			if (postalcode != null && radius != null)
			{
				area = "+stores(area(" + postalcode + "," + radius + "))";
				if (bTerse) show += ",stores";
			}
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Products list = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")" + area + "?apiKey=" + APIKEY + "&PID=" + PID + sort + show + spage));

			/*
			int totalpages = 0;
			int next = 0;
			try
			{
				totalpages = Convert.ToInt32(list.TotalPages);
				next = Convert.ToInt32(list.CurrentPage) + 1;
			}
			catch (Exception ex) { }
            
			list.RemoveAll(delegate(Product p) { return !p.Type.Contains("Game"); });
            
			if (list.Count == 0 && totalpages > 0 && next <= totalpages)
			{
				list.AddRange(this.GetGames(args, bTerse, next));
			}

			foreach (Product p in list) { p.generateImageUrl(); };
			 */

			return list;
		}

		public Products GetMusic(string args, bool bTerse)
		{
			return this.GetMusic(args, bTerse, 0, null, null);
		}

		public Products GetMusic(string args, bool bTerse, int ipage)
		{
			return this.GetMusic(args, bTerse, ipage, null, null);
		}

		public Products GetMusic(string args, bool bTerse, int ipage, string postalcode, string radius)
		{
			args = args.Replace(" ", "%20");
			string search = "type='Music'"
				+ "&artistName='" + args + "*'";
			string sort = "&sort=sku.desc";
			string show = "";
			string spage = "";
			string area = "";
			if (bTerse) show = "&show=sku,productId,name,artistName,type,regularPrice,salePrice,url,inStoreAvailability,onlineAvailability,thumbnailImage";
			if (postalcode != null && radius != null)
			{
				area = "+stores(area(" + postalcode + "," + radius + "))";
				if (bTerse) show += ",stores";
			}
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Products list = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")" + area + "?apiKey=" + APIKEY + "&PID=" + PID + sort + show + spage));

			/*
			int totalpages = 0;
			int next = 0;
			try
			{
				totalpages = Convert.ToInt32(list.TotalPages);
				next = Convert.ToInt32(list.CurrentPage) + 1;
			}
			catch (Exception ex) { }

			list.RemoveAll(delegate(Product p) { return !p.Type.Contains("Music"); });

			if (list.Count == 0 && totalpages > 0 && next <= totalpages)
			{
				list.AddRange(this.GetMusic(args, bTerse, next));
			}

			foreach (Product p in list) { p.generateImageUrl(); };
			*/

			return list;
		}

		public Products GetMovies(string args, bool bTerse)
		{
			return this.GetMovies(args, bTerse, 0, null, null);
		}

		public Products GetMovies(string args, bool bTerse, int ipage)
		{
			return this.GetMovies(args, bTerse, ipage, null, null);
		}

		public Products GetMovies(string args, bool bTerse, int ipage, string postalcode, string radius)
		{
			args = args.Replace(" ", "%20");
			string search = "type='Movie'"
				+ "&name='" + args + "*'";
			string sort = "&sort=sku.desc";
			string show = "";
			string spage = "";
			string area = "";
			if (bTerse) show = "&show=sku,productId,name,type,regularPrice,salePrice,url,inStoreAvailability,onlineAvailability,thumbnailImage";
			if (postalcode != null && radius != null)
			{
				area = "+stores(area(" + postalcode + "," + radius + "))";
				if (bTerse) show += ",stores";
			}
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Products list = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")" + area + "?apiKey=" + APIKEY + "&PID=" + PID + sort + show + spage));

			/*
			int totalpages = 0;
			int next = 0;
			try
			{
				totalpages = Convert.ToInt32(list.TotalPages);
				next = Convert.ToInt32(list.CurrentPage) + 1;
			}
			catch (Exception ex) { }

			list.RemoveAll(delegate(Product p) { return !p.Type.Contains("Movie"); });

			if (list.Count == 0 && totalpages > 0 && next <= totalpages)
			{
				list.AddRange(this.GetMovies(args, bTerse, next));
			}

			foreach (Product p in list) { p.generateImageUrl(); };
			*/

			return list;
		}

		public Products GetHardGoods(string args, bool bTerse)
		{
			return this.GetHardGoods(args, bTerse, 0, null, null);
		}

		public Products GetHardGoods(string args, bool bTerse, int ipage)
		{
			return this.GetHardGoods(args, bTerse, ipage, null, null);
		}

		public Products GetHardGoods(string args, bool bTerse, int ipage, string postalcode, string radius)
		{
			// Note: Filter out accessories in favor of actual hard goods (ie: ipod, not ipod earbuds)??

			args = args.Replace(" ", "%20");
			string search = "type='HardGood'"
				+ "&name='" + args + "*'";
			string sort = "&sort=regularPrice.desc";
			string show = "";
			string spage = "";
			string area = "";
			if (bTerse) show = "&show=sku,productId,name,type,regularPrice,salePrice,url,inStoreAvailability,onlineAvailability,thumbnailImage";
			if (postalcode != null && radius != null)
			{
				area = "+stores(area(" + postalcode + "," + radius + "))";
				if (bTerse) show += ",stores";
			}
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Products list = UTF8XmlSerializer.Deserialize<Products>(Get(PRODUCT + "(" + search + ")" + area + "?apiKey=" + APIKEY + "&PID=" + PID + sort + show + spage));

			/*
			int totalpages = 0;
			int next = 0;
			try
			{
				totalpages = Convert.ToInt32(list.TotalPages);
				next = Convert.ToInt32(list.CurrentPage) + 1;
			}
			catch (Exception ex) { }

			list.RemoveAll(delegate(Product p) { return !p.Type.Contains("HardGood"); });

			if (list.Count == 0 && totalpages > 0 && next <= totalpages)
			{
				list.AddRange(this.GetHardGoods(args, bTerse, next));
			}

			foreach (Product p in list) { p.generateImageUrl(); };
			*/

			return list;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="skuid"></param>
		/// <param name="ipage"></param>
		/// <returns></returns>
		public Reviews GetReview(string skuid, int ipage)
		{
			string search = "sku='" + skuid + "'";
			string sort = "&sort=rating.desc";
			string spage = "";
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Reviews list = UTF8XmlSerializer.Deserialize<Reviews>(Get(REVIEW + "(" + search + ")?apiKey=" + APIKEY + sort + spage));

			return list;
		}


		public Categories GetCategory(string id, int ipage)
		{
			Categories list = GetCategory(new string[] { id }, ipage);

			return list;
		}


		/*
		 * Category
		 */
		private static readonly Dictionary<string, Category> IdCateDic = new Dictionary<string, Category>();
		public Categories GetCategory(string[] ids, int ipage)
		{
			if (ids == null || ids.Length == 0)
				return new Categories();

			if (IdCateDic.Count == 0)
			{
				TryToLoadCategoriesDicFromFile();
			}

			if (IdCateDic.ContainsKey(ids[0]))
			{
				Categories categories = new Categories();
				foreach (var id in ids)
				{
					categories.Add(IdCateDic[id]);
				}
				return categories;
			}
			else
			{
				return GetCategoryFromNetwork(ids, ipage);
			}
		}

		private void TryToLoadCategoriesDicFromFile()
		{
			Categories categories = DataAccess.ReadCategories();
			foreach (var category in categories)
			{
				IdCateDic[category.Id] = category;
			}
		}

		private Categories GetCategoryFromNetwork(string[] ids, int ipage)
		{
			if (ids == null || ids.Length == 0)
			{
				return new Categories();
			}

			string search = "id in(" + string.Join(",", ids) + ")";
			string spage = "";
			if (ipage > 0) spage = "&page=" + ipage.ToString();

			Categories list = UTF8XmlSerializer.Deserialize<Categories>(Get(CATEGORY + "(" + search + ")?apiKey=" + APIKEY + spage));

			return list;
		}

	}
}
