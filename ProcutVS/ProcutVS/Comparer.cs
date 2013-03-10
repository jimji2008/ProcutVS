using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Newtonsoft.Json;
using System.IO;

namespace ProcutVS
{
	class Comparer
	{
		public static void Do(string upc1, string upc2)
		{
			Product product1 = DataAccess.GetProduct("794552240051");
			Product product2 = DataAccess.GetProduct("794552240358");

			Differ differ = new Differ();

			ISideBySideDiffBuilder diffBuilder = new SideBySideDiffBuilder2(differ);

			PrintToConsole(product1, diffBuilder, product2);

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


		static readonly Regex numberReg = new Regex(@"^(\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		private static void PrintToConsole(Product product1, ISideBySideDiffBuilder diffBuilder, Product product2)
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
					string old= result.OldText.Lines[0].Text;
					int oldNum;
					Match match = numberReg.Match(old);
					if (int.TryParse(match.Groups[0].Value, out oldNum))
					{
						int newNum;
						if (int.TryParse(numberReg.Match(result.NewText.Lines[0].Text).Groups[0].Value, out newNum))
						{
							int diff = newNum - oldNum;
							string lastStr = old.Substring(match.Groups[0].Index + match.Groups[0].Length);
							Console.Write(" - {0} {1}{2}",diff>0?"<":">",Math.Abs(diff),lastStr);
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
	}
}
