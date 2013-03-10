using System;
using System.Collections.Generic;
using System.Text;

namespace ProcutVS
{
	public class Product
	{
		public string UPC;
		public string Name;
		public string Desc;
		public string DescShort;
		public Remix.Category[] BBYCategoryPath;
		public List<Feature> Features = new List<Feature>();
		public Dictionary<string, Specification> SpecDic;
		public List<Tag> Tags = new List<Tag>();
		public List<Product> SimilarProducts = new List<Product>();
		public List<Product> Accessories = new List<Product>();
		public List<Review> Reviews = new List<Review>();
		public BlogPost[] BlogPosts;

		public decimal BBYSalePrice;
		public decimal BBYRegularPrice;

		public string BBYSKU;
		public string BBYUrl;
		public string BBYCJAffiliateUrl;

		public string BBYClassId;
		public string BBYClassName;
		public string BBYSubClassId;
		public string BBYSubClassName;

		public int BBYSalesRankShortTerm;
		public int BBYSalesRankMediumTerm;
		public int BBYSalesRankLongTerm;

		public int BBYCustomerReviewCount;
		public decimal BBYCustomerReviewAverage;

		public int AmazonSaleRank;
		public string AmazonDetailsUrl;
		public string AmazonASIN;
		public decimal AmazonPrice;

		public string LargeImageUrl;
		public string ThumbnailImageUrl;

		public ProsCons ProductWikiProsCons = new ProsCons();
		public string ProductWikiUrl;

		public DateTime UpdatedTime = DateTime.Now;

		public override bool Equals(object obj)
		{
			Product product2 = obj as Product;

			if (product2 == null)
				return base.Equals(product2);

			return UPC == product2.UPC;
		}
	}

	public class Feature
	{
		public string Desc;
	}


	public class Tag
	{
		public string Title;
		public int Number;
	}

	public class Specification
	{
		public string Name;
		public string Value;
		public string NameForDisplay;
		public bool CanDiff;
	}

	public class Review
	{
		public string Title;
		public string Content;
		public decimal Rating;
		public DateTime PubTime;
		public string Author;
		public string Source;
		public string SourceUrl;
	}

	public class BlogPost
	{
		public string Title;
		public string Abstract;
		public string Url;
		public string Author;
		public DateTime PubTime;
	}


	public class Products : List<Product>
	{
		public int CurrentPage;
		public int TotalPages;
		public int From;
		public int To;
		public int Total;
		public string QueryTime;

		public AggregationType Type;
		public string CategoryId;
		public string SearchPhase;

		public string Id
		{
			get
			{
				switch (Type)
				{
					case AggregationType.ByCategory:
						return Type + "_" + CategoryId;
						break;
					case AggregationType.BySearch:
						return Type + "_" + SearchPhase.Replace(" ", "-");
						break;
					default:
						return "";
						break;
				}
			}
		}

		public Products() { }
	}

	public enum AggregationType
	{
		ByCategory,
		BySearch,
	}

	public class ProsCons
	{
		public List<ProsConsStatement> ProsList=new List<ProsConsStatement>();
		public List<ProsConsStatement> ConsList=new List<ProsConsStatement>();
	}
	public class ProsConsStatement
	{
		public string Text;
		public int Score;
	}
}
