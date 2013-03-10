using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace ProcutVS.Appinions
{
	public class Server
	{
		private const string APP_KEY = "7xy57wqjagxhxm5pg6jvwur3";
		private const int MAX_RETURN = 30;
		public static List<Opinion> GetOpinions(string keywords)
		{
			List<Opinion> opinions = new List<Opinion>();
			try
			{
				WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8; 
				string requestUrl =
					string.Format(
						"http://api.appinions.com/search/v2/opinions?appkey={0}&doc_text={1}&start=0&rows={2}&format=xml&dedup=true",
						APP_KEY, keywords, MAX_RETURN);
				string xml = webClient.DownloadString(requestUrl);
				
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);

				XmlNodeList list = doc.SelectNodes("//opinion");
				foreach (XmlNode node in list)
				{
					Opinion o = new Opinion()
					{
						pre_sent = node.SelectSingleNode("pre_sent").InnerText,
						sent = node.SelectSingleNode("sent").InnerText,
						post_sent = node.SelectSingleNode("post_sent").InnerText,
						doc_title = node.SelectSingleNode("doc_title").InnerText,
						doc_link = node.SelectSingleNode("doc_link").InnerText
					};

					DateTime.TryParse(node.SelectSingleNode("publish_date").InnerText, out o.publish_date);

					if (!opinions.Contains(o))
						opinions.Add(o);
				}
			}
			catch (Exception e)
			{
				Logger.Error("Remix Error", e);
			}

			return opinions;
		}
	}
}
