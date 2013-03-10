using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ProcutVS;
using Yahoo.Answer;
using System.Web;

namespace Yahoo.Answer
{
	public class Server
	{
		const string BaseUrl = "http://answers.yahooapis.com/AnswersService/V1/questionSearch?appid=ProductVS&type=resolved&query={0}&results={1}";

		public static ResultSet GetQuestions(string query, int count)
		{
			if (string.IsNullOrEmpty(query))
				return new ResultSet();

			string queryUrl = string.Format(BaseUrl, HttpUtility.UrlEncode(query), count);
			HttpWebRequest request;
			HttpWebResponse response = null;
			StreamReader reader;
			StringBuilder sbSource;
			ResultSet resultSet = null;

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
					resultSet = UTF8XmlSerializer.Deserialize<ResultSet>(sbSource.ToString());
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

		public static List<QuestionType> GetQuestions(IEnumerable<string> queries, int count)
		{
			List<QuestionType> qList = new List<QuestionType>();
			foreach (string query in queries)
			{
				ResultSet resultSet = Yahoo.Answer.Server.GetQuestions(query, 5);
				if (resultSet != null && resultSet.Questions != null)
					qList.AddRange(resultSet.Questions);
			}
			qList = qList.Distinct(new QuestionComparer()).ToList();

			return qList;
		}

	}

	class QuestionComparer : EqualityComparer<QuestionType>
	{
		public override bool Equals(QuestionType x, QuestionType y)
		{
			return x.id == y.id;
		}

		public override int GetHashCode(QuestionType obj)
		{
			return obj.id.GetHashCode();
		}
	}
}
