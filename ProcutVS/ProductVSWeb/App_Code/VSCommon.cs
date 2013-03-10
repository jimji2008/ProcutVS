using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DiffPlex.DiffBuilder.Model;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for VSCommon
/// </summary>
public class VSCommon
{
	static Differ differ = new Differ();
	static ISideBySideDiffBuilder diffBuilder = new SideBySideDiffBuilder2(differ);
	internal static readonly bool IsLiveEnv = HttpContext.Current.Request.Url.Host.ToLower().Contains("productvs.net");
	internal static readonly Regex numberReg = new Regex(@"\s*\d+\s*", RegexOptions.Compiled| RegexOptions.IgnoreCase);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value1"></param>
	/// <param name="value2"></param>
	/// <returns>html formatted vs pair</returns>
	public static VSPair PrintDiff2Values(string value1, string value2)
	{
		VSPair valuePair = new VSPair();
		var result = diffBuilder.BuildDiffModel(HttpUtility.HtmlDecode(value1), HttpUtility.HtmlDecode(value2));

		valuePair.ChangeType = result.NewText.Lines[0].Type;

		StringBuilder sb = new StringBuilder();

		foreach (var line in result.OldText.Lines)
		{
			valuePair.Value1 = PrintDiffLine(line);
		}
		foreach (var line in result.NewText.Lines)
		{
			valuePair.Value2 = PrintDiffLine(line);
		}

		return valuePair;
	}

	private static string PrintDiffLine(DiffPiece line)
	{
		StringBuilder sb = new StringBuilder();
		if (line.Type == ChangeType.Unchanged)
		{
			sb.Append(line.Text);
		}
		else
		{
			foreach (var piece in line.SubPieces)
			{
				if (string.IsNullOrEmpty(piece.Text))
					continue;

				if (piece.Type == ChangeType.Unchanged)
				{
					sb.Append(string.Format("{0}", HttpUtility.HtmlEncode(piece.Text)));
				}
				else if (piece.Type == ChangeType.Deleted || piece.Type == ChangeType.Inserted)
				{
					sb.Append(string.Format("<span style='color:red'>{0}</span>", HttpUtility.HtmlEncode(piece.Text)));
				}
				else if (!IsNumber(piece.Text))
				{
					sb.Append(string.Format("<span style='color:red'>{0}</span>", HttpUtility.HtmlEncode(piece.Text)));
				}
				else
				{
					foreach (var character in piece.SubPieces)
					{
						if (character.Type == ChangeType.Unchanged)
						{
							sb.Append(string.Format("{0}", HttpUtility.HtmlEncode(character.Text)));
						}
						else
						{
							sb.Append(string.Format("<span style='color:red'>{0}</span>", HttpUtility.HtmlEncode(character.Text)));
						}
					}
				}
			}
		}

		return sb.ToString();
	}

	private static bool IsNumber(string str)
	{
		return numberReg.IsMatch(str);
	}
}

public class VSPair
{
	public string Value1;
	public string Value2;
	public ChangeType ChangeType;
}


class Judgement
{
	public string Name;
	public bool BiggerIsBetter;
	public string ProsStatement;
	public string ConsStatement;
}