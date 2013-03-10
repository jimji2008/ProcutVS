using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ProcutVS
{
	public class ProductPool
	{
		//private static Dictionary<string, ProductWraper> upcProductDic = new Dictionary<string, ProductWraper>();
		private static Dictionary<string, ProductWrapper> amazonAsinProductDic = new Dictionary<string, ProductWrapper>();
		private static Dictionary<string, ProductWrapper> bestBuySkuProductDic = new Dictionary<string, ProductWrapper>();

		const int CACHE_HOURS = 5;

		private static ProductWrapper Cache(string key, Product product, CacheType cacheType)
		{
			ProductWrapper cachedProductWrapper = new ProductWrapper()
			{
				CacheType = cacheType,
				Product = product,
			};

			HttpRuntime.Cache.Insert(key,
				cachedProductWrapper,
				null,
				DateTime.Now.AddHours(CACHE_HOURS),
				System.Web.Caching.Cache.NoSlidingExpiration,
				System.Web.Caching.CacheItemPriority.Normal,
				null);

			return cachedProductWrapper;
		}

		public static ProductWrapper Cache(Product product, CacheType cacheType)
		{
			if (product == null) return new ProductWrapper() { Product = null, CacheType = CacheType.Full };

			ProductWrapper cachedProductWrapper = null;
			if (!string.IsNullOrEmpty(product.UPC))
			{
				cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForUPC(product.UPC));
				if (cachedProductWrapper == null
					|| cacheType > cachedProductWrapper.CacheType)
				{
					cachedProductWrapper = Cache(GetCacheKeyForUPC(product.UPC), product, cacheType);
				}
			}

			//
			if (!string.IsNullOrEmpty(product.AmazonASIN))
			{
				cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForAmazonASIN(product.AmazonASIN));
				if (cachedProductWrapper == null
					|| cacheType > cachedProductWrapper.CacheType)
				{
					cachedProductWrapper = Cache(GetCacheKeyForAmazonASIN(product.AmazonASIN), product, cacheType);
				}
			}

			//
			if (!string.IsNullOrEmpty(product.BBYSKU))
			{
				cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForBBYSku(product.BBYSKU));
				if (cachedProductWrapper == null
					|| cacheType > cachedProductWrapper.CacheType)
				{
					cachedProductWrapper = Cache(GetCacheKeyForBBYSku(product.BBYSKU), product, cacheType);
				}
			}

			//
			RankedProductManager.AddRandedProduct(product);

			return cachedProductWrapper;
		}

		static string GetCacheKeyForUPC(string Id)
		{
			return "UPC_" + Id;
		}
		static string GetCacheKeyForBBYSku(string Id)
		{
			return "BBYSKU_" + Id;
		}
		static string GetCacheKeyForAmazonASIN(string Id)
		{
			return "ASIN_" + Id;
		}


		public static Product GetByUPC(string UPC, CacheType cacheType)
		{
			ProductWrapper cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForUPC(UPC));

			if (cachedProductWrapper == null
				 || cacheType > cachedProductWrapper.CacheType)
			{
				cachedProductWrapper = Cache(GetAFilledProductByUPC(UPC, cacheType), cacheType);
			}

			return cachedProductWrapper.Product;
		}

		public static Product GetByAmazonASIN(string ASIN, CacheType cacheType)
		{
			ProductWrapper cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForAmazonASIN(ASIN));

			if (cachedProductWrapper == null
				|| cacheType > cachedProductWrapper.CacheType)
			{
				cachedProductWrapper = Cache(GetAFilledProductByAmazonASIN(ASIN, cacheType), cacheType);
			}

			return cachedProductWrapper.Product;
		}

		public static Product GetByBestBuySKU(string SKU, CacheType cacheType)
		{
			ProductWrapper cachedProductWrapper = (ProductWrapper)HttpRuntime.Cache.Get(GetCacheKeyForBBYSku(SKU));

			if (cachedProductWrapper == null
				|| cacheType > cachedProductWrapper.CacheType)
			{
				cachedProductWrapper = Cache(GetAFilledProductBySKU(SKU, cacheType), cacheType);
			}

			return cachedProductWrapper.Product;
		}

		public static void WarmUpProductsByUpcs(IEnumerable<string> UPCs)
		{
			List<string> UPCList = new List<string>();
			foreach (string upc in UPCs)
			{
				if (HttpRuntime.Cache.Get(GetCacheKeyForUPC(upc)) == null)
					UPCList.Add(upc);
			}

			Product[] products = BestBuyFiller.DoByUpcs(UPCList.ToArray());

			foreach (var product in products)
			{
				Cache(product, CacheType.Simple);
			}
		}

		public static void WarmUpProductsBySkus(IEnumerable<string> SKUs)
		{
			List<string> SKUList = new List<string>();
			foreach (string sku in SKUs)
			{
				if (HttpRuntime.Cache.Get(GetCacheKeyForBBYSku(sku)) == null)
					SKUList.Add(sku);
			}

			Product[] products = BestBuyFiller.DoBySkus(SKUList.ToArray());

			foreach (var product in products)
			{
				Cache(product, CacheType.Simple);
			}
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
				ProductWikiFiller.Do(product);
			}
			return product;
		}

		//
		private static Product GetAFilledProductBySKU(string SKU, CacheType cacheType)
		{
			Product product = new Product();
			product.BBYSKU = SKU;

			BestBuyFiller.Do(product);
			if (cacheType == CacheType.Full)
			{
				AmazonFiller.Do(product);
				BestBuySpecFiller.Do(product);
				BestBuyReviewsFiller.Do(product); 
				ProductWikiFiller.Do(product);
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
				ProductWikiFiller.Do(product);
			}
			return product;
		}


		#endregion
	}

	public class ProductWrapper
	{
		public Product Product;
		public CacheType CacheType;
		public DateTime LastUsedTime = DateTime.Now;
	}

	public enum CacheType
	{
		Simple,
		Full, // full(int) must greater than simple
	}
}
