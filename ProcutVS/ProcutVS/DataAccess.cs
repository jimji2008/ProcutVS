using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using ProductWiki;
using Remix;

namespace ProcutVS
{
	public class DataAccess
	{
		private static readonly string VAR_FILE_PATH = ConfigurationManager.AppSettings["VAR_FILE_PATH"];
		private static readonly string HOT_PRODUCT_PAIR_FILE = Path.Combine(VAR_FILE_PATH, "HotProductPairs.json");
		private static readonly string VAR_PRODUCTWIKI_FILE_PATH = Path.Combine(VAR_FILE_PATH, "ProductWiki");
		private static readonly string RANKED_PRODUCT_FILE = Path.Combine(VAR_FILE_PATH, "RankedProducts_1.1.json");
		private static readonly string CATEGORIES_ARCHIVE_FILE = Path.Combine(VAR_FILE_PATH, "CategoriesArchive.xml");
		private static readonly string PRODUCT_SPEC_PATH = Path.Combine(VAR_FILE_PATH, "spec");

		static DataAccess()
		{
			if (!Directory.Exists(VAR_FILE_PATH))
				Directory.CreateDirectory(VAR_FILE_PATH);
			if (!Directory.Exists(VAR_FILE_PATH))
				Directory.CreateDirectory(VAR_FILE_PATH);
			if (!Directory.Exists(VAR_PRODUCTWIKI_FILE_PATH))
				Directory.CreateDirectory(VAR_PRODUCTWIKI_FILE_PATH);
			if (!Directory.Exists(VAR_FILE_PATH))
				Directory.CreateDirectory(VAR_FILE_PATH);
			if (!Directory.Exists(PRODUCT_SPEC_PATH))
				Directory.CreateDirectory(PRODUCT_SPEC_PATH);

		}

		public static void SaveProduct(Product product)
		{
			string str = JavaScriptConvert.SerializeObject(product);
			string path = "data";
			
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			File.WriteAllText(Path.Combine(path,"product_"+product.UPC+".json"),str);
		}


		public static Queue<ProductPair> ReadHotProductVisitQueue()
		{
			Queue<ProductPair> ProductVisitQueue;
			if (File.Exists(HOT_PRODUCT_PAIR_FILE))
			{
				ProductVisitQueue = new Queue<ProductPair>(JavaScriptConvert.DeserializeObject<ProductPair[]>(
						File.ReadAllText(HOT_PRODUCT_PAIR_FILE)));
			}
			else
			{
				ProductVisitQueue = new Queue<ProductPair>();
			}

			return ProductVisitQueue;
		}

		public static void WriteHotProductVisitQueue(Queue<ProductPair> queue)
		{
			File.WriteAllText(HOT_PRODUCT_PAIR_FILE,
				  JavaScriptConvert.SerializeObject(queue.ToArray()));
		}


		public static pw_api_results ReadProductWikiByUPC(string UPC)
		{
			string file = Path.Combine(VAR_PRODUCTWIKI_FILE_PATH, UPC + ".json");
			if (File.Exists(file))
			{
				return JavaScriptConvert.DeserializeObject<pw_api_results>(File.ReadAllText(file));
			}

			return null;
		}

		public static void WriteProductWikiByUPC(string upc,pw_api_results results)
		{
			string file = Path.Combine(VAR_PRODUCTWIKI_FILE_PATH, upc + ".json");
			File.WriteAllText(file, JavaScriptConvert.SerializeObject(results));
		}

		public static List<RankedProduct> ReadRankedProductList()
		{
			List<RankedProduct> rankedProductList = null;
			if (File.Exists(RANKED_PRODUCT_FILE))
			{
				try
				{
					rankedProductList =
					   JavaScriptConvert.DeserializeObject<List<RankedProduct>>(
						   File.ReadAllText(RANKED_PRODUCT_FILE));
				}
				catch (Exception ex)
				{
					rankedProductList = new List<RankedProduct>();
					Logger.Error("ReadRankedProductList() ", ex);
				}
			}
			else
			{
				rankedProductList = new List<RankedProduct>();
			}
			return rankedProductList;
		}

		public static void WriteProductWikiByUPC(List<RankedProduct> list)
		{
			File.WriteAllText(RANKED_PRODUCT_FILE,
							  JavaScriptConvert.SerializeObject(list));
		}

		//
		public static Categories ReadCategories()
		{
			Categories categories = null;
			try
			{
				categories = UTF8XmlSerializer.Deserialize<Categories>(File.ReadAllText(CATEGORIES_ARCHIVE_FILE));
			}
			catch (Exception ex)
			{
				categories = new Categories();
				Logger.Error("DataAccess ReadCategories Error", ex);
			}
			return categories;
		}

		//

		public static Dictionary<string, Specification> ReadProductSpecByUPC(string UPC)
		{
			try
			{
				string file = Path.Combine(PRODUCT_SPEC_PATH, UPC + ".json");
				if (File.Exists(file))
				{
					return JavaScriptConvert.DeserializeObject<Dictionary<string, Specification>>(File.ReadAllText(file));
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public static void WriteProductSpecByUPC(string upc, Dictionary<string, Specification> spctDic)
		{
			string file = Path.Combine(PRODUCT_SPEC_PATH, upc + ".json");
			File.WriteAllText(file,
							  JavaScriptConvert.SerializeObject(spctDic));
		}

	}
}
