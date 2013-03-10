using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ProcutVS;
using Yahoo.Answer;
using System.Web;

namespace ProductWiki
{
	/// <summary>
	/// ProductWiki
	/// sample:
	/// http://api.productwiki.com/connect/api.aspx?op=product&format=xml&idtype=UPC&idvalue=885909212132&key=2b532933357b4383a9d01199d0dd6f20&
	/// </summary>
	public class Server
	{
		private const string KEY = "2b532933357b4383a9d01199d0dd6f20";
		const string BASE_URL = "http://api.productwiki.com/connect/api.aspx?key=" + KEY;

		public static pw_api_results GetProductByUPC(string UPC)
		{
			pw_api_results results = DataAccess.ReadProductWikiByUPC(UPC);

			if (results == null)
			{
				results = GetProductByUPCFromServer(UPC);
				DataAccess.WriteProductWikiByUPC(UPC,results);
			}
			else if (results == null)
			{
				results = new pw_api_results();
			}

			return results;
		}

		static pw_api_results GetProductByUPCFromServer(string UPC)
		{
			if (string.IsNullOrEmpty(UPC))
				return new pw_api_results();

			string queryUrl = string.Format(BASE_URL + "&op=product&format=xml&idtype=UPC&idvalue={0}", UPC);
			HttpWebRequest request;
			HttpWebResponse response = null;
			StreamReader reader;
			StringBuilder sbSource;
			pw_api_results resultSet = null;

			try
			{
				// Create and initialize the web request
				request = WebRequest.Create(queryUrl) as HttpWebRequest;
				// Set timeout to 10 seconds
				request.Timeout = 10 * 1000;

				// Get response
				response = request.GetResponse() as HttpWebResponse;

				if (request.HaveResponse == true && response != null)
				{
					// Get the response stream
					reader = new StreamReader(response.GetResponseStream());

					// Read it into a StringBuilder
					sbSource = new StringBuilder(reader.ReadToEnd());
					resultSet = UTF8XmlSerializer.Deserialize<pw_api_results>(sbSource.ToString());
				}
			}
			catch (WebException wex)
			{
				// This exception will be raised if the server didn't return 200 - OK
				// Try to retrieve more information about the network error
				if (wex.Response != null)
				{
					using (HttpWebResponse errorResponse = (HttpWebResponse)wex.Response)
					{
						Logger.Error(string.Format("The server returned '{0}' with the status code {1} ({2:d}).",
							errorResponse.StatusDescription, errorResponse.StatusCode,
							errorResponse.StatusCode), wex);
					}
				}
			}
			finally
			{
				if (response != null) { response.Close(); }
			}

			return resultSet;
		}
	}
}
