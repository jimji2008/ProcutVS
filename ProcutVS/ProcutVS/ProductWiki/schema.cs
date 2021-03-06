﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 

namespace ProductWiki
{
	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
	public partial class pw_api_results
	{

		private string messageField;

		private int num_resultsField;

		private product[] productsField;

		/// <remarks/>
		public string message
		{
			get
			{
				return this.messageField;
			}
			set
			{
				this.messageField = value;
			}
		}

		/// <remarks/>
		public int num_results
		{
			get
			{
				return this.num_resultsField;
			}
			set
			{
				this.num_resultsField = value;
			}
		}

		/// <remarks/>
		public product[] products
		{
			get
			{
				return this.productsField;
			}
			set
			{
				this.productsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class product
	{

		private string urlField;

		private int idField;

		private string titleField;

		private string descriptionField;

		private string proscoreField;

		private string number_of_reviewsField;

		private string categoryField;

		private Facet[] key_featuresField;

		private Image[] imagesField;

		private CommunityReview prosconsField;

		private Review[] reviewsField;

		private Tag[] tagsField;

		private Competitor[] competitorsField;

		private product[] relatedField;

		private sku[] skusField;

		/// <remarks/>
		public string url
		{
			get
			{
				return this.urlField;
			}
			set
			{
				this.urlField = value;
			}
		}

		/// <remarks/>
		public int id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public string description
		{
			get
			{
				return this.descriptionField;
			}
			set
			{
				this.descriptionField = value;
			}
		}

		/// <remarks/>
		public string proscore
		{
			get
			{
				return this.proscoreField;
			}
			set
			{
				this.proscoreField = value;
			}
		}

		/// <remarks/>
		public string number_of_reviews
		{
			get
			{
				return this.number_of_reviewsField;
			}
			set
			{
				this.number_of_reviewsField = value;
			}
		}

		/// <remarks/>
		public string category
		{
			get
			{
				return this.categoryField;
			}
			set
			{
				this.categoryField = value;
			}
		}

		/// <remarks/>
		public Facet[] key_features
		{
			get
			{
				return this.key_featuresField;
			}
			set
			{
				this.key_featuresField = value;
			}
		}

		/// <remarks/>
		public Image[] images
		{
			get
			{
				return this.imagesField;
			}
			set
			{
				this.imagesField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public CommunityReview proscons
		{
			get
			{
				return this.prosconsField;
			}
			set
			{
				this.prosconsField = value;
			}
		}

		/// <remarks/>
		public Review[] reviews
		{
			get
			{
				return this.reviewsField;
			}
			set
			{
				this.reviewsField = value;
			}
		}

		/// <remarks/>
		public Tag[] tags
		{
			get
			{
				return this.tagsField;
			}
			set
			{
				this.tagsField = value;
			}
		}

		/// <remarks/>
		public Competitor[] competitors
		{
			get
			{
				return this.competitorsField;
			}
			set
			{
				this.competitorsField = value;
			}
		}

		/// <remarks/>
		public product[] related
		{
			get
			{
				return this.relatedField;
			}
			set
			{
				this.relatedField = value;
			}
		}

		/// <remarks/>
		public sku[] skus
		{
			get
			{
				return this.skusField;
			}
			set
			{
				this.skusField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class Facet
	{

		private string titleField;

		private string[] valuesField;

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		[System.Xml.Serialization.XmlArrayItemAttribute("value", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string[] values
		{
			get
			{
				return this.valuesField;
			}
			set
			{
				this.valuesField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class sku
	{

		private string titleField;

		private string upcField;

		private string mpnField;

		private string asinField;

		private string eanField;

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public string upc
		{
			get
			{
				return this.upcField;
			}
			set
			{
				this.upcField = value;
			}
		}

		/// <remarks/>
		public string mpn
		{
			get
			{
				return this.mpnField;
			}
			set
			{
				this.mpnField = value;
			}
		}

		/// <remarks/>
		public string asin
		{
			get
			{
				return this.asinField;
			}
			set
			{
				this.asinField = value;
			}
		}

		/// <remarks/>
		public string ean
		{
			get
			{
				return this.eanField;
			}
			set
			{
				this.eanField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class Competitor
	{

		private product competitor_productField;

		private int votesField;

		private double percentpreferField;

		/// <remarks/>
		public product competitor_product
		{
			get
			{
				return this.competitor_productField;
			}
			set
			{
				this.competitor_productField = value;
			}
		}

		/// <remarks/>
		public int votes
		{
			get
			{
				return this.votesField;
			}
			set
			{
				this.votesField = value;
			}
		}

		/// <remarks/>
		public double percentprefer
		{
			get
			{
				return this.percentpreferField;
			}
			set
			{
				this.percentpreferField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class ReviewHelpfulness
	{

		private int number_helpfulField;

		private int number_unhelpfulField;

		/// <remarks/>
		public int number_helpful
		{
			get
			{
				return this.number_helpfulField;
			}
			set
			{
				this.number_helpfulField = value;
			}
		}

		/// <remarks/>
		public int number_unhelpful
		{
			get
			{
				return this.number_unhelpfulField;
			}
			set
			{
				this.number_unhelpfulField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class ProductList
	{

		private product[] productsField;

		/// <remarks/>
		public product[] products
		{
			get
			{
				return this.productsField;
			}
			set
			{
				this.productsField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class Tag
	{

		private string titleField;

		private int number_of_taggersField;

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public int number_of_taggers
		{
			get
			{
				return this.number_of_taggersField;
			}
			set
			{
				this.number_of_taggersField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class ReviewUser
	{

		private string titleField;

		private string urlField;

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public string url
		{
			get
			{
				return this.urlField;
			}
			set
			{
				this.urlField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class Review
	{

		private ReviewUser userField;

		private string dateField;

		private CommunityReview prosconsField;

		private string[] commentsField;

		private Tag[] tagsField;

		private ProductList winning_competitorsField;

		private ProductList losing_competitorsField;

		private ReviewHelpfulness helpfulnessField;

		/// <remarks/>
		public ReviewUser user
		{
			get
			{
				return this.userField;
			}
			set
			{
				this.userField = value;
			}
		}

		/// <remarks/>
		public string date
		{
			get
			{
				return this.dateField;
			}
			set
			{
				this.dateField = value;
			}
		}

		/// <remarks/>
		public CommunityReview proscons
		{
			get
			{
				return this.prosconsField;
			}
			set
			{
				this.prosconsField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		[System.Xml.Serialization.XmlArrayItemAttribute("comment", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string[] comments
		{
			get
			{
				return this.commentsField;
			}
			set
			{
				this.commentsField = value;
			}
		}

		/// <remarks/>
		public Tag[] tags
		{
			get
			{
				return this.tagsField;
			}
			set
			{
				this.tagsField = value;
			}
		}

		/// <remarks/>
		public ProductList winning_competitors
		{
			get
			{
				return this.winning_competitorsField;
			}
			set
			{
				this.winning_competitorsField = value;
			}
		}

		/// <remarks/>
		public ProductList losing_competitors
		{
			get
			{
				return this.losing_competitorsField;
			}
			set
			{
				this.losing_competitorsField = value;
			}
		}

		/// <remarks/>
		public ReviewHelpfulness helpfulness
		{
			get
			{
				return this.helpfulnessField;
			}
			set
			{
				this.helpfulnessField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class CommunityReview
	{

		private CommunityReviewStatement[] prosField;

		private CommunityReviewStatement[] consField;

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		[System.Xml.Serialization.XmlArrayItemAttribute("statement", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public CommunityReviewStatement[] pros
		{
			get
			{
				return this.prosField;
			}
			set
			{
				this.prosField = value;
			}
		}

		/// <remarks/>
		[System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		[System.Xml.Serialization.XmlArrayItemAttribute("statement", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public CommunityReviewStatement[] cons
		{
			get
			{
				return this.consField;
			}
			set
			{
				this.consField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class CommunityReviewStatement
	{

		private string textField;

		private string idField;

		private string scoreField;

		private bool submitterField;

		/// <remarks/>
		public string text
		{
			get
			{
				return this.textField;
			}
			set
			{
				this.textField = value;
			}
		}

		/// <remarks/>
		public string id
		{
			get
			{
				return this.idField;
			}
			set
			{
				this.idField = value;
			}
		}

		/// <remarks/>
		public string score
		{
			get
			{
				return this.scoreField;
			}
			set
			{
				this.scoreField = value;
			}
		}

		/// <remarks/>
		public bool submitter
		{
			get
			{
				return this.submitterField;
			}
			set
			{
				this.submitterField = value;
			}
		}
	}

	/// <remarks/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public partial class Image
	{

		private string titleField;

		private string rawimageField;

		private string largeimageField;

		private string mediumimageField;

		private string smallimageField;

		/// <remarks/>
		public string title
		{
			get
			{
				return this.titleField;
			}
			set
			{
				this.titleField = value;
			}
		}

		/// <remarks/>
		public string rawimage
		{
			get
			{
				return this.rawimageField;
			}
			set
			{
				this.rawimageField = value;
			}
		}

		/// <remarks/>
		public string largeimage
		{
			get
			{
				return this.largeimageField;
			}
			set
			{
				this.largeimageField = value;
			}
		}

		/// <remarks/>
		public string mediumimage
		{
			get
			{
				return this.mediumimageField;
			}
			set
			{
				this.mediumimageField = value;
			}
		}

		/// <remarks/>
		public string smallimage
		{
			get
			{
				return this.smallimageField;
			}
			set
			{
				this.smallimageField = value;
			}
		}
	}
}
