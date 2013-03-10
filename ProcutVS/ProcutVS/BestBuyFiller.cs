using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcutVS
{
	public class BestBuyFiller
	{
		/// <summary>
		/// by upc or sku
		/// </summary>
		/// <param name="product">must contain upc or sku</param>
		public static void Do(Product product)
		{
			Remix.Server server = new Remix.Server("", "");
			Remix.Products remixProducts = null;
			if (!string.IsNullOrEmpty(product.UPC))
				remixProducts = server.GetProduct(product.UPC);
			else if (!string.IsNullOrEmpty(product.BBYSKU))
				remixProducts = server.GetProduct(product.BBYSKU, false);

			if (remixProducts != null && remixProducts.Count > 0)
			{
				Remix.Product p = remixProducts[0];
				product.BBYSKU = p.Sku;
				product.UPC = p.UPC;
				product.Name = p.Name;
				product.BBYCategoryPath = p.CategoryPath.ToArray();
				product.LargeImageUrl = string.IsNullOrEmpty(p.LargeFrontImageUrl) ?
					p.LargeImageUrl : p.LargeFrontImageUrl;
				product.LargeImageUrl = string.IsNullOrEmpty(product.LargeImageUrl) ?
					p.ImageUrl : product.LargeImageUrl;
				product.ThumbnailImageUrl = string.IsNullOrEmpty(p.ImageUrl) ?
					p.ThumbnailimageUrl : p.ImageUrl;
				product.BBYUrl = p.BBYUrl;
				product.BBYCJAffiliateUrl = p.CJAffiliateUrl;
				product.BBYClassId = p.ClassId;
				product.BBYClassName = p.Class;
				product.BBYSubClassId = p.SubclassId;
				product.BBYSubClassName = p.Subclass;

				int.TryParse(p.CustomerReviewCount, out product.BBYCustomerReviewCount);
				decimal.TryParse(p.CustomerReviewAverage, out product.BBYCustomerReviewAverage);

				product.Desc = p.LongDescription;
				product.DescShort = p.ShortDescription;
				decimal.TryParse(p.SalePrice, out product.BBYSalePrice);
				decimal.TryParse(p.RegularPrice, out product.BBYRegularPrice);

				int.TryParse(p.SalesRankLongTerm, out product.BBYSalesRankLongTerm);
				int.TryParse(p.SalesRankMediumTerm, out product.BBYSalesRankMediumTerm);
				int.TryParse(p.SalesRankShortTerm, out product.BBYSalesRankShortTerm);

				if (p.RelatedProducts != null)
				{
					foreach (var sku in p.RelatedProducts)
					{
						product.SimilarProducts.Add(new Product() { BBYSKU = sku });
					}
				}

				if (p.Accessories != null)
				{
					foreach (var sku in p.Accessories)
					{
						product.Accessories.Add(new Product() { BBYSKU = sku });
					}
				}
			}
		}


		private const int BBY_PAGE_SIZE = 10;
		public static Product[] DoByUpcs(string[] upcs)
		{
			if (upcs == null || upcs.Length == 0)
				return new Product[] { };

			// process 1 page once.
			List<string> upcList = new List<string>(upcs);
			
			Remix.Server server = new Remix.Server("", "");
			List<Product> products = new List<Product>();
			for (int i = 0; i < upcList.Count; i += BBY_PAGE_SIZE)
			{
				Remix.Products remixProducts = server.GetProductByUpcs(upcList.GetRange(i,
					upcList.Count - i > BBY_PAGE_SIZE ? BBY_PAGE_SIZE : upcList.Count - i).ToArray());
				products.AddRange(Fill(remixProducts));
			}

			return products.ToArray();
		}

		public static Product[] DoBySkus(string[] skus)
		{
			if (skus == null || skus.Length == 0)
				return new Product[] { };

			// process 1 page once.
			List<string> skuList = new List<string>(skus);


			Remix.Server server = new Remix.Server("", "");
			List<Product> products = new List<Product>();
			for (int i = 0; i < skuList.Count; i += BBY_PAGE_SIZE)
			{
				Remix.Products remixProducts = server.GetProductBySkus(skuList.GetRange(i,
					skuList.Count - i > BBY_PAGE_SIZE ? BBY_PAGE_SIZE : skuList.Count - i).ToArray());
				products.AddRange(Fill(remixProducts));
			}

			return products.ToArray();
		}

		private static List<Product> Fill(Remix.Products remixProducts)
		{
			List<Product> products = new List<Product>();
			if (remixProducts == null || remixProducts.Count == 0)
				return products;


			foreach (var remixProduct in remixProducts)
			{
				Product product = new Product();
				product.BBYSKU = remixProduct.Sku;
				product.UPC = remixProduct.UPC;
				product.Name = remixProduct.Name;
				product.BBYCategoryPath = remixProduct.CategoryPath.ToArray();
				product.LargeImageUrl = string.IsNullOrEmpty(remixProduct.LargeFrontImageUrl) ?
																								remixProduct.LargeImageUrl : remixProduct.LargeFrontImageUrl;
				product.LargeImageUrl = string.IsNullOrEmpty(product.LargeImageUrl) ?
																						remixProduct.ImageUrl : product.LargeImageUrl;
				product.ThumbnailImageUrl = string.IsNullOrEmpty(remixProduct.ImageUrl) ?
																							remixProduct.ThumbnailimageUrl : remixProduct.ImageUrl;
				product.BBYUrl = remixProduct.BBYUrl;
				product.BBYClassId = remixProduct.ClassId;
				product.BBYClassName = remixProduct.Class;
				product.BBYSubClassId = remixProduct.SubclassId;
				product.BBYSubClassName = remixProduct.Subclass;

				product.Desc = remixProduct.LongDescription;
				product.DescShort = remixProduct.ShortDescription;
				decimal.TryParse(remixProduct.SalePrice, out product.BBYSalePrice);
				decimal.TryParse(remixProduct.RegularPrice, out product.BBYRegularPrice);

				int.TryParse(remixProduct.SalesRankLongTerm, out product.BBYSalesRankLongTerm);
				int.TryParse(remixProduct.SalesRankMediumTerm, out product.BBYSalesRankMediumTerm);
				int.TryParse(remixProduct.SalesRankShortTerm, out product.BBYSalesRankShortTerm);

				if (remixProduct.RelatedProducts != null)
				{
					foreach (var sku in remixProduct.RelatedProducts)
					{
						product.SimilarProducts.Add(new Product() { BBYSKU = sku });
					}
				}

				products.Add(product);
			}

			return products;
		}
	}
}
