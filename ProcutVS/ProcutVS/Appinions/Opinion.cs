// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;


namespace ProcutVS.Appinions
{
	public class Opinion : IEquatable<Opinion>
    {
        public string index;
        public string op_id;
        public string sent_id;
        public string doc_id;
        public string doc_link;
        public string doc_title;
        public int dedup;
        public Payloads payloads;
        public DateTime publish_date;
        public string type;
        public string pre_sent;
        public string sent;
        public string post_sent;
        public Topics topics;
        public Publisher publisher;
        public Authors authors;
        public Opholder opholder;
        public int polarity;
        public double polarity_conf;
        public string regions;



		#region IEquatable<Opinion> Members

		public bool Equals(Opinion other)
		{
			return this.doc_link == other.doc_link;
		}

		#endregion
	}
}
