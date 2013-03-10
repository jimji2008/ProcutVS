using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcutVS
{
	public class BestBuyCategoryProductsFiller
	{
		public static void Do(Products products, Remix.Category category)
		{
			products.Type = AggregationType.ByCategory;

			Remix.Server server = new Remix.Server("", "");
			Remix.Products remixProducts = server.GetProduct(category, products.CurrentPage);

			List<string> relatedSkuList = new List<string>();
			if (remixProducts != null && remixProducts.Count > 0)
			{
				int.TryParse(remixProducts.TotalPages, out products.TotalPages);
				int.TryParse(remixProducts.Total, out products.Total);
				int.TryParse(remixProducts.To, out products.To);
				int.TryParse(remixProducts.From, out products.From);
				int.TryParse(remixProducts.CurrentPage, out products.CurrentPage);

				foreach (var remixProduct in remixProducts)
				{
					Product product = new Product();
					product.BBYSKU = remixProduct.Sku;
					product.UPC = remixProduct.UPC;
					product.Name = remixProduct.Name;
					product.BBYCategoryPath = remixProduct.CategoryPath.ToArray();
					product.ThumbnailImageUrl = string.IsNullOrEmpty(remixProduct.ImageUrl) ?
						remixProduct.ThumbnailimageUrl : remixProduct.ImageUrl;
					product.LargeImageUrl = remixProduct.LargeImageUrl;
					product.BBYUrl = remixProduct.BBYUrl;

					product.BBYSubClassId = remixProduct.SubclassId;
					product.BBYSubClassName = remixProduct.Subclass;

					product.DescShort = remixProduct.ShortDescription;
					decimal.TryParse(remixProduct.SalePrice, out product.BBYSalePrice);

					if (remixProduct.RelatedProducts != null)
					{
						foreach (var sku in remixProduct.RelatedProducts)
						{
							product.SimilarProducts.Add(new Product() { BBYSKU = sku });
							relatedSkuList.Add(sku);
						}
					}

					products.Add(product);
				}
			}

			//

		}
	}
}
