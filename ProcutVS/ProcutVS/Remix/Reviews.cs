using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ProcutVS;

namespace Remix
{
	[XmlRoot("reviews")]
    public class Reviews : List<Review>
    {
        [XmlAttribute("currentPage")]
        public string CurrentPage;

        [XmlAttribute("totalPages")]
        public string TotalPages;

        [XmlAttribute("from")]
        public string From;

        [XmlAttribute("to")]
        public string To;

        [XmlAttribute("total")]
        public string Total;

        [XmlAttribute("queryTime")]
        public string QueryTime;

        [XmlAttribute("totalTime")]
        public string TotalTime;

        [XmlAttribute("canonicalUrl")]
        public string CanonicalURL;

        [XmlAttribute("guid")]
        public string Guid;

        public Reviews() { }

        public override bool Equals(object obj)
        {
            bool bRet = false;
			Reviews reviews = ((Reviews)obj);

            if (this.Count == reviews.Count)
            {
                for (int x = 0; x < this.Count; x++)
                {
					Review p1 = this[x];
					Review p2 = reviews[x];
                    bRet = p1.Equals(p2);
                    if (!bRet) break;
                }
            }

            return bRet;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

       
        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }

    [XmlType("review")]
    public class Review
    {
        [XmlElement("sku")]
        public string Sku = "";

        [XmlElement("id")]
        public string Id = "";

        [XmlElement("reviewer")]
        public BestBuyReviewer Reviewer;

        [XmlElement("rating")]
        public string Rating = "";

        [XmlElement("title")]
        public string Title = "";

        [XmlElement("comment")]
        public string Comment = "";

        [XmlElement("submissionTime")]
        public string SubmissionTime = "";

        [XmlAttribute("guid")]
        public string Guid;

        public Review() { }

        public override bool Equals(object obj)
        {
			return obj is Review &&
				((Review)obj).Sku == Sku &&
				((Review)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public String ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }

	[XmlType("reviewer")]
	public class BestBuyReviewer
	{
		[XmlAttribute("name")]
		public string Name;

	}

}
