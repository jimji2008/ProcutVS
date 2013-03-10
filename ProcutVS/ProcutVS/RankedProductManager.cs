using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace ProcutVS
{
	/// <summary>
	/// start:
	/// I/O -> Dic
	/// Dic -> List
	/// Add new:
	/// Dic -> I/O
	/// Dic -> List
	/// 
	/// to serialize as a list may reduce the size of file.
	/// </summary>
	public class RankedProductManager
	{
		private const int REFRESH_THRESHOLD = 10;

		private static Dictionary<string, Dictionary<string, RankedProduct>> ClassRankedProductsDic =
			new Dictionary<string, Dictionary<string, RankedProduct>>();

		private static Dictionary<string, List<RankedProduct>> ClassRankedProductListDic =
			new Dictionary<string, List<RankedProduct>>();

		static readonly object snycLock = new object();

		private static int updateCounter = 0;

		static RankedProductManager()
		{
			IO2Dic();
			Dic2List();
		}

		public static void AddRandedProduct(Product product)
		{
			AddRandedProduct(new RankedProduct()
								{
									ClassId = product.BBYSubClassId,
									Rank_Amazon = product.AmazonSaleRank > 0 ? product.AmazonSaleRank : int.MaxValue,
									Rank_BBYLong = product.BBYSalesRankLongTerm > 0 ? product.BBYSalesRankLongTerm : int.MaxValue,
									Rank_BBYMedium = product.BBYSalesRankMediumTerm > 0 ? product.BBYSalesRankMediumTerm : int.MaxValue,
									Rank_BBYShort = product.BBYSalesRankShortTerm > 0 ? product.BBYSalesRankShortTerm : int.MaxValue,
									UPC = product.UPC
								});
		}

		public static void AddRandedProduct(RankedProduct rankedProduct)
		{
			if (rankedProduct == null || string.IsNullOrEmpty(rankedProduct.ClassId))
				return;

			lock (snycLock)
			{
				Dictionary<string, RankedProduct> upcRankedProduct = null;
				if (ClassRankedProductsDic.ContainsKey(rankedProduct.ClassId))
				{
					upcRankedProduct = ClassRankedProductsDic[rankedProduct.ClassId];
				}
				else
				{
					upcRankedProduct = new Dictionary<string, RankedProduct>();
				}

				upcRankedProduct[rankedProduct.UPC] = rankedProduct;
				ClassRankedProductsDic[rankedProduct.ClassId] = upcRankedProduct;


				// for performace reason, use REFRESH_THRESHOLD
				if (updateCounter++ > REFRESH_THRESHOLD)
				{
					updateCounter = 0;

					Dic2IO();
					Dic2List();
				}
			}
		}

		public static RankInfo GetRankInfo(string classId, string UPC)
		{
			if (string.IsNullOrEmpty(classId) || !ClassRankedProductsDic.ContainsKey(classId))
				return new RankInfo();
			if (string.IsNullOrEmpty(UPC) || !ClassRankedProductsDic[classId].ContainsKey(UPC))
				return new RankInfo();
			if (!ClassRankedProductListDic.ContainsKey(classId))
				return new RankInfo();

			return GetRankInfo(ClassRankedProductsDic[classId][UPC]);
		}

		private static RankInfo GetRankInfo(RankedProduct rankedProduct)
		{
			List<RankedProduct> rankedProducts = ClassRankedProductListDic[rankedProduct.ClassId];

			int ix = -1;
			// search 
			for (int i = 0; i < rankedProducts.Count; i++)
			{
				if (rankedProducts[i].UPC == rankedProduct.UPC)
				{
					ix = i;
					break;
				}
			}

			RankedProduct[] betterRankedProducts = null;

			if (ix > 0 && ++ix < rankedProducts.Count)
				betterRankedProducts = rankedProducts.GetRange(ix, rankedProducts.Count - ix).ToArray();
			else
				betterRankedProducts = new RankedProduct[] { };

			return new RankInfo()
			{
				Index = ix,
				Total = rankedProducts.Count,
				BetterRankedProducts = betterRankedProducts
			};
		}


		private static void Dic2List()
		{
			ClassRankedProductListDic.Clear();
			foreach (var classId in ClassRankedProductsDic.Keys)
			{
				List<RankedProduct> rankedProductlist = new List<RankedProduct>();
				foreach (var rankedProduct in ClassRankedProductsDic[classId].Values)
				{
					rankedProductlist.Add(rankedProduct);
				}
				rankedProductlist.Sort();
				ClassRankedProductListDic[classId] = rankedProductlist;
			}
		}

		private static void IO2Dic()
		{
			List<RankedProduct> rankedProductList = DataAccess.ReadRankedProductList();

			foreach (var rankedProduct in rankedProductList)
			{
				if (!ClassRankedProductsDic.ContainsKey(rankedProduct.ClassId))
					ClassRankedProductsDic[rankedProduct.ClassId] = new Dictionary<string, RankedProduct>();

				ClassRankedProductsDic[rankedProduct.ClassId][rankedProduct.UPC] = rankedProduct;
			}
		}

		private static void Dic2IO()
		{
			List<RankedProduct> rankedProductList = new List<RankedProduct>();

			foreach (var upcRankedProduct in ClassRankedProductsDic.Values)
			{
				foreach (var rankedProduct in upcRankedProduct.Values)
				{
					rankedProductList.Add(rankedProduct);
				}
			}

			DataAccess.WriteProductWikiByUPC(rankedProductList);
		}
	}

	public class RankedProduct : IComparable<RankedProduct>
	{
		public string UPC;
		public int Rank_Amazon;
		public int Rank_BBYShort;
		public int Rank_BBYMedium;
		public int Rank_BBYLong;
		public string ClassId;

		#region IComparable<RankedProduct> Members

		public int CompareTo(RankedProduct other)
		{
			int r = 0;
			if (Rank_Amazon > other.Rank_Amazon)
				r++;
			else if (Rank_Amazon < other.Rank_Amazon)
				r--;

			if (Rank_BBYShort > other.Rank_BBYShort)
				r++;
			else if (Rank_BBYShort < other.Rank_BBYShort)
				r--;

			if (Rank_BBYMedium > other.Rank_BBYMedium)
				r++;
			else if (Rank_BBYMedium < other.Rank_BBYMedium)
				r--;

			if (Rank_BBYLong > other.Rank_BBYLong)
				r++;
			else if (Rank_BBYLong < other.Rank_BBYLong)
				r--;

			return r;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("UPC:{0},Rank:{1},ClassId:{2}", UPC, Rank_Amazon, ClassId);
		}
	}

	public class RankInfo
	{
		static readonly RankedProduct[] EmptyBetterRankedProducts = new RankedProduct[] { };

		public int Index;
		public int Total;
		public RankedProduct[] BetterRankedProducts = EmptyBetterRankedProducts;
	}
}
