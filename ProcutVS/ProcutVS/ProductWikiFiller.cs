using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ProductWiki;

namespace ProcutVS
{
	public class ProductWikiFiller
	{
		/// <summary>
		/// by upc or sku
		/// </summary>
		/// <param name="product">must contain upc or sku</param>
		public static void Do(Product product)
		{
			pw_api_results result = Server.GetProductByUPC(product.UPC);

			if (result.products == null || result.products.Length == 0)
				return;

			//pros cons, 
			var wikiPorduct = result.products[0];
			if (wikiPorduct.proscons != null)
			{
				foreach (var pros in wikiPorduct.proscons.pros)
				{
					int score = 0;
					Int32.TryParse(pros.score, out score);
					product.ProductWikiProsCons.ProsList.Add(new ProsConsStatement()
																{
																	Text = pros.text,
																	Score = score
																});
				}
				foreach (var cons in wikiPorduct.proscons.cons)
				{
					int score = 0;
					Int32.TryParse(cons.score, out score);
					product.ProductWikiProsCons.ConsList.Add(new ProsConsStatement()
																{
																	Text = cons.text,
																	Score = score
																});
				}
			}

			//tags, 
			foreach (var tag in wikiPorduct.tags)
			{
				product.Tags.Add(new Tag()
									{
										Title = tag.title,
										Number = tag.number_of_taggers
									});
			}

			//image
			if (wikiPorduct.images.Length > 0)
			{
				if (string.IsNullOrEmpty(product.LargeImageUrl) || product.LargeImageUrl.Contains("default_hardlines"))
					product.LargeImageUrl = wikiPorduct.images[0].largeimage;
				if (string.IsNullOrEmpty(product.ThumbnailImageUrl) || product.ThumbnailImageUrl.Contains("default_hardlines"))
					product.ThumbnailImageUrl = wikiPorduct.images[0].mediumimage;
			}

			// features
			if (product.Features.Count == 0 && wikiPorduct.key_features.Length > 0)
			{
				foreach (var keyFeature in wikiPorduct.key_features)
				{
					product.Features.Add(new Feature()
											{
												Desc = string.Format("{0}: {1}", keyFeature.title, keyFeature.values)
											});
				}
			}
		}
	}
}
