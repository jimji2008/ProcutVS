using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProcutVS
{
	public class BestBuyReviewsFiller
	{
		/// <summary>
		/// by BestBuySKU
		/// </summary>
		/// <param name="product">BestBuySKU</param>
		public static void Do(Product product)
		{
			if (string.IsNullOrEmpty(product.BBYSKU))
				return;

			Remix.Server server = new Remix.Server("", "");
			Remix.Reviews remixReviews = server.GetReview(product.BBYSKU, 1);
			if (remixReviews.Count > 0)
			{
				foreach (var remixReview in remixReviews)
				{
					Review review = new Review()
										{
											Title = remixReview.Title,
											Content = remixReview.Comment,
											Author = remixReview.Reviewer.Name,
											Source = "Best Buy",
											SourceUrl = ""
										};
					DateTime.TryParse(remixReview.SubmissionTime, out review.PubTime);
					decimal.TryParse(remixReview.Rating, out review.Rating);

					product.Reviews.Add(review);
				}
			}
		}
	}
}
