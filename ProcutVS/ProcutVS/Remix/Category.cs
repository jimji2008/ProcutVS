using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using ProcutVS;

namespace Remix
{
	[XmlRoot("categories")]
	public class Categories : List<Category>, IXmlSerializable
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

        public Categories() { }

        public string ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }

		#region XmlAttribute not work when inherited from List<>, so do it self
		public System.Xml.Schema.XmlSchema GetSchema() { return null; }

		public void WriteXml(XmlWriter writer)
		{
			return;
		}

		public void ReadXml(XmlReader reader)
		{
			//reader.Read();
			//reader.ReadStartElement("products");

			this.CurrentPage = reader.GetAttribute("currentPage");
			this.TotalPages = reader.GetAttribute("totalPages");
			this.From = reader.GetAttribute("from"); ;
			this.To = reader.GetAttribute("to");
			this.Total = reader.GetAttribute("total");
			this.QueryTime = reader.GetAttribute("queryTime");
			this.TotalTime = reader.GetAttribute("totalTime");
			this.CanonicalURL = reader.GetAttribute("canonicalUrl");

			reader.Read();

			XmlSerializer serializer = new XmlSerializer(typeof(Category));

			while (reader.NodeType != XmlNodeType.EndElement)
			{
				try
				{
					Category item = (Category)serializer.Deserialize(reader);
					if (item != null) this.Add(item);
				}
				catch (Exception ex)
				{
					Logger.Error("Remix Error",ex);
				}
			}

			reader.ReadEndElement();

		}
		#endregion
    }

	[XmlType("category")]
	public class Category
	{
		[XmlElement("id")]
		public string Id = "";
		[XmlElement("name")]
		public string Name = "";

		[XmlArray("path")]
		[XmlArrayItem(typeof(Category),
		ElementName = "category")]
		public List<Category> Path;

		[XmlArray("subCategories")]
		[XmlArrayItem(typeof(Category),
		ElementName = "category")]
		public List<Category> SubCategories;
	}
}
