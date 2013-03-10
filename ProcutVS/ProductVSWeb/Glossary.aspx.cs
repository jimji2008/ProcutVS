using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ProcutVS;
using Yahoo.Answer;

public partial class Glossary : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string name = Request["name"];
		string value1 = Request["v1"];
		string value2 = Request["v2"];
		string upc = Request["upc"];


		//wiki + /name

		//name & value 
		//+ better
		//+ difference

		//name
		//+ better 
		//+ difference
		//What is +
		//name + cate name + better  

		// 1.specific 
		// 2.have answers

		List<string> queryList = new List<string>();

		Product product = ProductPool.GetByUPC(upc, CacheType.Simple);
		if (product.BBYCategoryPath != null)
		{
			string categoryName = product.BBYCategoryPath[product.BBYCategoryPath.Length - 1].Name;
			queryList.Add(string.Format("what is {0} {1}", name, categoryName));
			queryList.Add(string.Format("{0} {1} better", name, categoryName));
		}

		if (value1 != value2)
		{
			queryList.Add(string.Format("{0} {1} {2} better ", name, value1, value2));
			queryList.Add(string.Format("{0} {1} {2} difference", name, value1, value2));
		}
		else
		{
			queryList.Add(string.Format("{0} {1} better ", name, value1));
			queryList.Add(string.Format("{0} {1} difference", name, value1));
		}
		queryList.Add(string.Format("{0} better ", name));
		queryList.Add(string.Format("{0} difference", name));


		List<QuestionType> qList = Yahoo.Answer.Server.GetQuestions(queryList, 5);

		//
		string desc = string.Format("What is {0}?", name);
		string title = string.Format("What is {0}?", name);
		if (value1 != value2)
		{
			desc += string.Format(" The difference between {0} and {1} and which one is Better?", value1, value2);
			title += string.Format(" Which one is better between {0} and {1}?", value1, value2);
		}

		//
		StringBuilder sb = new StringBuilder();

		sb.Append("<div>");
		sb.Append("<h2>Q&A About " + name + " - " + title + "</h2>");

		// display list
		sb.Append("<ul class='qa_list'>");
		int i = 0;
		foreach (QuestionType question in qList)
		{
			sb.Append(string.Format(@"<li><a href='#q{0}'>{1}</a></li>", i++, question.Subject));
		}
		sb.Append("</ul>");

		// display all QA
		i = 0;
		sb.Append("<ul class='qa'>");
		foreach (QuestionType question in qList)
		{
			sb.Append(@"
<li>
<ul>
	<li class='subject'><a name='q" + (i++) + "'></a>" + question.Subject + @"</li>
	<li class='content'><pre>" + question.Content + @"</pre></li>
	<li class='title'>Answer:</li>
	<li class='answer'><pre>" + question.ChosenAnswer + @"</pre></li>
	<li class='pubtime'>" + new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToInt32(question.ChosenAnswerAwardTimestamp)).ToShortDateString() + @" - <a href='" + question.Link + @"' target='yahoo'>Yahoo! Answer</a></li>
</ul>
</li>
");
		}
		sb.Append("</ul>");
		sb.Append("</div>");

		Response.Write(HTMLGenerator.GetHeaher(title, name, desc));
		Response.Write(sb.ToString());
		Response.Write(HTMLGenerator.GetFooter());
	}
}
