using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Newtonsoft.Json;
using System.IO;
using Remix;

namespace ProcutVS
{
	class Printer
	{
		static readonly Regex numberReg = new Regex(@"^(\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		public static void Do(string upc1, string upc2)
		{
			Product product1 = DataAccess.GetProduct("794552240051");
			Product product2 = DataAccess.GetProduct("794552240358");

			//PrintToHtml(product1, diffBuilder, product2);
		}

		private static void PrintToHtml(Product product1, ISideBySideDiffBuilder diffBuilder, Product product2)
		{
			foreach (var key in product1.SpecDic.Keys)
			{
				//var diffResult = differ.CreateCharacterDiffs(product1.SpecDic[key].Value, product2.SpecDic[key].Value,true);

				var result = diffBuilder.BuildDiffModel(product1.SpecDic[key].Value, product2.SpecDic[key].Value);

				foreach (var line in result.OldText.Lines)
				{
					if (line.Type == ChangeType.Unchanged)
					{
						Console.Write(line.Text);
					}
					else
					{

						foreach (var piece in line.SubPieces)
						{
							if (piece.Type == ChangeType.Unchanged)
							{
								Console.Write(piece.Text);
							}
							else
							{
								ConsoleColor color = Console.ForegroundColor;
								Console.ForegroundColor = ConsoleColor.Red;
								Console.Write(piece.Text);
								Console.ForegroundColor = color;
							}
						}
					}
				}
				Console.Write(" - ");
				foreach (var line in result.NewText.Lines)
				{
					if (line.Type == ChangeType.Unchanged)
					{
						Console.Write(line.Text);
					}
					else
					{
						foreach (var piece in line.SubPieces)
						{
							if (piece.Type == ChangeType.Unchanged)
							{
								Console.Write(piece.Text);
							}
							else
							{
								ConsoleColor color = Console.ForegroundColor;
								Console.ForegroundColor = ConsoleColor.Red;
								Console.Write(piece.Text);
								Console.ForegroundColor = color;
							}
						}
					}
				}

				Console.WriteLine();

				//foreach (var block in result.)
				//{
				//    diffResult.DiffBlocks[0].
				//}
			}
		}

		public static void PrintProductVSToConsole(Product product1, Product product2)
		{
			PrintProductVSLineToConsole(product1.Name, product2.Name);
			PrintProductVSLineToConsole(product1.AmazonPrice.ToString() + " USD", product2.AmazonPrice.ToString() + " USD");
			PrintProductVSLineToConsole(product1.BestBuySalePrice.ToString() + " USD", product2.BestBuySalePrice.ToString() + " USD");
			PrintProductVSLineToConsole(product1.AmazonPrice.ToString() + " USD", product2.AmazonPrice.ToString() + " USD");
			PrintProductVSLineToConsole(product1.AmazonSaleRank.ToString(), product2.AmazonSaleRank.ToString());

			foreach (var key in product1.SpecDic.Keys)
			{
				if (product2.SpecDic.ContainsKey(key))
				{
					PrintProductVSLineToConsole(product1.SpecDic[key].Value, product2.SpecDic[key].Value);
				}
				else
				{
					PrintProductVSLineToConsole(product1.SpecDic[key].Value, null);
				}
			}
		}

		static Differ differ = new Differ();
		static ISideBySideDiffBuilder diffBuilder = new SideBySideDiffBuilder2(differ);
		private static void PrintProductVSLineToConsole(string value1, string value2)
		{
			if (value2 == null)
			{
				Console.WriteLine(value1 + " - N/A");
				return;
			}

			var result = diffBuilder.BuildDiffModel(value1, value2);

			foreach (var line in result.OldText.Lines)
			{
				if (line.Type == ChangeType.Unchanged)
				{
					Console.Write(line.Text);
				}
				else
				{
					foreach (var piece in line.SubPieces)
					{
						if (piece.Type == ChangeType.Unchanged)
						{
							Console.Write(piece.Text);
						}
						else
						{
							foreach (var character in piece.SubPieces)
							{
								if (character.Type == ChangeType.Unchanged)
								{
									Console.Write(character.Text);
								}
								else
								{

									ConsoleColor color = Console.ForegroundColor;
									Console.ForegroundColor = ConsoleColor.Red;
									Console.Write(character.Text);
									Console.ForegroundColor = color;
								}
							}
						}
					}
				}
			}
			Console.Write(" - ");
			foreach (var line in result.NewText.Lines)
			{
				if (line.Type == ChangeType.Unchanged)
				{
					Console.Write(line.Text);
				}
				else
				{
					foreach (var piece in line.SubPieces)
					{
						if (piece.Type == ChangeType.Unchanged)
						{
							Console.Write(piece.Text);
						}
						else
						{
							foreach (var character in piece.SubPieces)
							{
								if (character.Type == ChangeType.Unchanged)
								{
									Console.Write(character.Text);
								}
								else
								{
									ConsoleColor color = Console.ForegroundColor;
									Console.ForegroundColor = ConsoleColor.Red;
									Console.Write(character.Text);
									Console.ForegroundColor = color;
								}
							}
						}
					}
				}
			}

			// line diff comment
			if (result.OldText.Lines[0].Type != ChangeType.Unchanged)
			{
				string old = result.OldText.Lines[0].Text;
				int oldNum;
				Match match = numberReg.Match(old);
				if (int.TryParse(match.Groups[0].Value, out oldNum))
				{
					int newNum;
					if (int.TryParse(numberReg.Match(result.NewText.Lines[0].Text).Groups[0].Value, out newNum))
					{
						int diff = newNum - oldNum;
						string lastStr = old.Substring(match.Groups[0].Index + match.Groups[0].Length);
						Console.Write(" - {0} {1}{2}", diff > 0 ? "<" : ">", Math.Abs(diff), lastStr);
					}
				}
			}

			Console.WriteLine();
		}

		public static void PrintProductVSByCategory(string categoryId)
		{
			Products products = new Products();
			BestBuyCategoryProductsFiller.Do(products, new Remix.Category() { Id = categoryId });

			foreach (var product in products)
			{
				ProductPool.Cache(product, CacheType.Simple);
			}

			Dictionary<string, string> VSDic = new Dictionary<string, string>();
			foreach (var product in products)
			{
				foreach (var similarProduct in product.SimilarProducts)
				{
					Product filledSimilarProduct = ProductPool.GetByBestBuySKU(similarProduct.BestBuySKU, CacheType.Simple);
					ProductPool.Cache(filledSimilarProduct, CacheType.Simple);

					if (filledSimilarProduct.BestBuyCategoryPath != null
						&& filledSimilarProduct.BestBuyCategoryPath[filledSimilarProduct.BestBuyCategoryPath.Length - 1].Id
						== product.BestBuyCategoryPath[product.BestBuyCategoryPath.Length - 1].Id)
					{
						VSDic[product.UPC] = filledSimilarProduct.UPC;
					}
				}
			}

			foreach (var key in VSDic.Keys)
			{
				Product product1 = ProductPool.GetByUPC(key, CacheType.Simple);
				Product product2 = ProductPool.GetByUPC(VSDic[key], CacheType.Simple);
				Console.WriteLine("{0},{1} -|- {2},{3}", product1.UPC, product1.Name, product2.UPC, product2.Name);
			}
		}


		public static void PrintCategorylayout(string categoryId)
		{
			Category category = CategoryPool.GetById(categoryId);

			Console.WriteLine("{0},{1}", category.Id, category.Name);
			Console.WriteLine("----------------");
			foreach (var subCategory in category.SubCategories)
			{
				Console.WriteLine("{0},{1}", subCategory.Id, subCategory.Name);
				Console.WriteLine("----------------");
				Category subCategory2 = CategoryPool.GetById(subCategory.Id);
				foreach (var subCategory3 in subCategory2.SubCategories)
				{
					Console.WriteLine("{0},{1}", subCategory3.Id, subCategory3.Name);
				}
			}
		}
	}
}
