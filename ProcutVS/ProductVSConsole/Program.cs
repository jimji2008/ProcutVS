using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProcutVS;
using ProcutVS.Appinions;
using ProductWiki;
using Remix;
using Yahoo.Answer;
using Product = ProcutVS.Product;

namespace ProductVSConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			//BestBuyFiller.Do();

			//Product product1 = ProductPool.GetByBestBuySKU("1831054", CacheType.Full);
			//Product product2 = ProductPool.GetByBestBuySKU("2260049", CacheType.Full);
			//Printer.PrintProductVSToConsole(product1, product2);

			//Printer.PrintCategorylayout(Remix.Server.ROOT_CATEGORY_ID);

			//FillOneProduct();

			//for (int i = 1; i < 10; i++)
			//    Printer.PrintCategorylayout("abcat0100000");

			//SiteMapGenerator.Do();


			//TestHotPairs();

			//TestRankedProduct();

			//TestRankedProduct2();

			//TestAnswer();

			//TestArchiveCategory();

			//TestProductWiki();

			//TestAttributeComparer();

			TestOpinion();
		}

		private static void TestOpinion()
		{
			List<Opinion> opinions = ProcutVS.Appinions.Server.GetOpinions("FRA073PU1 7000 BTU Portable Air Conditioner");

			foreach (var opinion in opinions)
			{
				Console.WriteLine("---------------------");
				Console.WriteLine(opinion.doc_title);
				Console.WriteLine(opinion.doc_link);
				Console.WriteLine(opinion.pre_sent);
				Console.WriteLine(opinion.sent);
				Console.WriteLine(opinion.post_sent);
				Console.WriteLine(opinion.publish_date);
			}

		}

		private static void TestAttributeComparer()
		{
			Product p1 = ProductPool.GetByUPC("027242809758", CacheType.Full);
			Product p2 = ProductPool.GetByUPC("885170029842", CacheType.Full);

			List<AttributePair> attributePairList = new List<AttributePair>();
			foreach (var key in p1.SpecDic.Keys)
			{
				if (!p2.SpecDic.ContainsKey(key))
					continue;
				SmartVSValue v1 = new SmartVSValue(p1.SpecDic[key].Value);
				SmartVSValue v2 = new SmartVSValue(p2.SpecDic[key].Value);
				attributePairList.Add(new AttributePair()
				{
					Name = p1.SpecDic[key].Name,
					Value1 = v1,
					Value2 = v2
				});
			}

			Dictionary<string, AttributeVSResult[]> vsResultDic = AttributeComparer.SmartCompare(attributePairList.ToArray());
			int j = 0;
			foreach (var key in vsResultDic.Keys)
			{
				int i = 0;
				Console.WriteLine(j++);
				foreach (var attributeVsResult in vsResultDic[key])
				{
					ConsoleColor color = Console.ForegroundColor;
					if (attributeVsResult.GoodBad == GoodBad.Good)
					{
						Console.ForegroundColor = ConsoleColor.Green;
					}
					else if (attributeVsResult.GoodBad == GoodBad.Bad)
					{
						Console.ForegroundColor = ConsoleColor.Red;
					}
					Console.WriteLine((i++) + ", " + attributeVsResult);
					Console.ForegroundColor = color;
				}
			}

		}

		private static void TestProductWiki()
		{
			pw_api_results result = ProductWiki.Server.GetProductByUPC("843163063013");
		}

		private static void TestArchiveCategory()
		{
			Dictionary<string, Category> idCateDic = new Dictionary<string, Category>();
			Categories categories = UTF8XmlSerializer.Deserialize<Categories>(File.ReadAllText("categories.xml"));
			foreach (var category in categories)
			{
				idCateDic[category.Id] = category;
			}

			foreach (Category category in idCateDic["cat00000"].SubCategories)
			{
				Console.WriteLine(category.Id + ", " + category.Name);
			}
		}

		private static void TestAnswer()
		{

			List<Yahoo.Answer.QuestionType> qList = new List<QuestionType>();
			Yahoo.Answer.ResultSet resultSet = Yahoo.Answer.Server.GetQuestions("processor better", 5);
			qList.AddRange(resultSet.Questions);
			resultSet = Yahoo.Answer.Server.GetQuestions("processor difference", 5);
			qList.AddRange(resultSet.Questions);
			resultSet = Yahoo.Answer.Server.GetQuestions("What is processor", 5);
			qList.AddRange(resultSet.Questions);
			resultSet = Yahoo.Answer.Server.GetQuestions("processor better", 5);
			qList.AddRange(resultSet.Questions);

			qList = qList.Distinct(new QuestionComparer()).ToList();



			Console.WriteLine(UTF8XmlSerializer.Serialize(qList));
		}


		class QuestionComparer : EqualityComparer<Yahoo.Answer.QuestionType>
		{
			public override bool Equals(QuestionType x, QuestionType y)
			{
				return x.id == y.id;
			}

			public override int GetHashCode(QuestionType obj)
			{
				return obj.id.GetHashCode();
			}
		}


		static Random random = new Random();
		private static void TestRankedProduct2()
		{
			for (int i = 0; i < 1; i++)
			{
				Console.WriteLine("======================");

				for (int classId = 0; classId < 10; classId++)
				{
					for (int upc = 0; upc < 1000; upc++)
					{
						RankedProductManager.AddRandedProduct(new RankedProduct()
																{
																	ClassId = classId.ToString(),
																	Rank_Amazon = random.Next(10000),
																	UPC = "UPC_" + upc
																});
					}
				}

				RankedProduct[] products = RankedProductManager.GetRankInfo("1", "UPC_1").BetterRankedProducts;
				foreach (var rankedProduct in products)
				{
					Console.WriteLine(rankedProduct);
				}
			}
		}

		private static void TestRankedProduct()
		{
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 5, Rank_BBYLong = 3, UPC = "001" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 6, Rank_BBYLong = 5, UPC = "002" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "2", Rank_Amazon = 12, Rank_BBYLong = 6, UPC = "003" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 11, Rank_BBYLong = 1, UPC = "004" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 1, Rank_BBYLong = 2, UPC = "005" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "2", Rank_Amazon = 15, Rank_BBYLong = 4, UPC = "006" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 21, Rank_BBYLong = 7, UPC = "007" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "3", Rank_Amazon = 22, Rank_BBYLong = 8, UPC = "008" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "3", Rank_Amazon = 16, Rank_BBYLong = 9, UPC = "009" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "3", Rank_Amazon = 56, Rank_BBYLong = 10, UPC = "010" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "3", Rank_Amazon = 2, Rank_BBYLong = 11, UPC = "011" });
			RankedProductManager.AddRandedProduct(new RankedProduct() { ClassId = "1", Rank_Amazon = 10, Rank_BBYLong = 12, UPC = "012" });

			RankedProduct[] products = RankedProductManager.GetRankInfo("1", "012").BetterRankedProducts;

			foreach (var rankedProduct in products)
			{
				Console.WriteLine(rankedProduct);
			}
		}

		private static void TestHotPairs()
		{
			HotProductPairManager.AddVisit("1", "2");
			HotProductPairManager.AddVisit("1", "2");
			HotProductPairManager.AddVisit("1", "3");
			HotProductPairManager.AddVisit("1", "4");
			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "2");
			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "6");
			HotProductPairManager.AddVisit("1", "7");
			HotProductPairManager.AddVisit("1", "5");

			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "5");
			HotProductPairManager.AddVisit("1", "5");

			KeyValuePair<ProductPair, int>[] pairs = HotProductPairManager.GetHostProductPairs();
			foreach (var keyValuePair in pairs)
			{
				Console.WriteLine(keyValuePair.Key.Key + ": " + keyValuePair.Value);
			}
		}

		private static void FillOneProduct()
		{
			Product product = new Product();
			product.UPC = "794552240358";
			//product.BestBuySKU = "9691888";

			//BestBuySpecFiller.Do("http://www.bestbuy.com/site/Anji+Mountain+Bamboo+Chairmat+&+Rug+Co.+-+44%22+x+52%22+Bamboo+Roll-Up+Chair+Mat+-+Natural/2715746.p?id=1218346644628&skuId=2715746&cmp=RMX&ky=2isnSVUvS8KgsXhGUincLJamwEoV5QK0b", product);

			BestBuyFiller.Do(product);
			BestBuySpecFiller.Do(product);
			AmazonFiller.Do(product);
			BestBuyReviewsFiller.Do(product);

			DataAccess.SaveProduct(product);
		}
	}
}
