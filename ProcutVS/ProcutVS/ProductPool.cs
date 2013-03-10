using System;
using System.Collections.Generic;
using System.Text;

namespace ProcutVS
{
	class ProductPool
	{
		private static Dictionary<string, ProductWraper> upcProductDic = new Dictionary<string, ProductWraper>();
		private static Dictionary<string, ProductWraper> amazonAsinProductDic = new Dictionary<string, ProductWraper>();
		private static Dictionary<string, ProductWraper> bestBuySkuProductDic = new Dictionary<string, ProductWraper>();


		public static void Cache(Product product, CacheType cacheType)
		{
			if (!string.IsNullOrEmpty(product.UPC)
				&& !upcProductDic.ContainsKey(product.UPC))
			{
				upcProductDic.Add(product.UPC, new ProductWraper()
												{
													CacheType = cacheType,
													Product = product,
													LastUsedTime = DateTime.Now
												});
			}

			if (!string.IsNullOrEmpty(product.AmazonASIN)
				&& !amazonAsinProductDic.ContainsKey(product.AmazonASIN))
			{
				amazonAsinProductDic.Add(product.AmazonASIN, new ProductWraper()
																{
																	CacheType = cacheType,
																	Product = product,
																	LastUsedTime = DateTime.Now
																});
			}

			if (!string.IsNullOrEmpty(product.BestBuySKU)
				&& !bestBuySkuProductDic.ContainsKey(product.BestBuySKU))
			{
				bestBuySkuProductDic.Add(product.BestBuySKU, new ProductWraper()
																{
																	CacheType = cacheType,
																	Product = product,
																	LastUsedTime = DateTime.Now
																});
			}
		}


		public static Product GetByUPC(string UPC, CacheType cacheType)
		{
			Product product = null;

			if (!upcProductDic.ContainsKey(UPC)
				|| (cacheType == CacheType.Full && upcProductDic[UPC].CacheType == CacheType.Simple))
				product = GetAFilledProductByUPC(UPC, cacheType);
			else
				product = upcProductDic[UPC].Product;

			return product;
		}

		public static Product GetByAmazonASIN(string ASIN, CacheType cacheType)
		{
			Product product = null;

			if (!amazonAsinProductDic.ContainsKey(ASIN)
				|| (cacheType == CacheType.Full && amazonAsinProductDic[ASIN].CacheType == CacheType.Simple))
				product = GetAFilledProductByAmazonASIN(ASIN, cacheType);
			else
				product = amazonAsinProductDic[ASIN].Product;

			return product;
		}

		public static Product GetByBestBuySKU(string SKU, CacheType cacheType)
		{
			Product product = null;

			if (!bestBuySkuProductDic.ContainsKey(SKU)
				|| (cacheType == CacheType.Full && upcProductDic[SKU].CacheType == CacheType.Simple))
				product = GetAFilledProductBySKU(SKU, cacheType);
			else
				product = bestBuySkuProductDic[SKU].Product;

			return product;
		}


		#region GetFromSource
		private static Product GetAFilledProductByUPC(string UPC, CacheType cacheType)
		{
			Product product = new Product();
			product.UPC = UPC;

			BestBuyFiller.Do(product);

			if (cacheType == CacheType.Full)
			{
				AmazonFiller.Do(product);
				BestBuySpecFiller.Do(product);
				BestBuyReviewsFiller.Do(product);
			}
			return product;
		}

		//
		private static Product GetAFilledProductBySKU(string SKU, CacheType cacheType)
		{
			Product product = new Product();
			product.BestBuySKU = SKU;

			BestBuyFiller.Do(product);
			if (cacheType == CacheType.Full)
			{
				AmazonFiller.Do(product);
				BestBuySpecFiller.Do(product);
				BestBuyReviewsFiller.Do(product);
			}
			return product;
		}

		//
		private static Product GetAFilledProductByAmazonASIN(string ASIN, CacheType cacheType)
		{
			Product product = new Product();
			product.AmazonASIN = ASIN;

			AmazonFiller.Do(product);
			BestBuyFiller.Do(product);
			if (cacheType == CacheType.Full)
			{
				BestBuySpecFiller.Do(product);
				BestBuyReviewsFiller.Do(product);
			}
			return product;
		}


		#endregion
	}

	internal class ProductWraper
	{
		public Product Product;
		public CacheType CacheType;
		public DateTime LastUsedTime;
	}

	internal enum CacheType
	{
		Full,
		Simple
	}
}
