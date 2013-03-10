using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ProcutVS
{
	public class Logger
	{
		private static readonly string VAR_FILE_PATH = ConfigurationManager.AppSettings["VAR_FILE_PATH"];
		private static readonly string LOG_CONFIG_FILE = ConfigurationManager.AppSettings["LOG_CONFIG_FILE"];

		private delegate void LogHandler(object message, Exception ex);
		private static readonly log4net.ILog logger;

		static Logger()
		{
			log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(LOG_CONFIG_FILE));
			logger = log4net.LogManager.GetLogger("ProductVS");
		}

		static public void Info(string message)
		{
			Log(logger.Info, message, null);
		}

		static public void Error(string message)
		{
			Log(logger.Error, message, null);
		}
		static public void Error(object message, Exception ex)
		{
			Log(logger.Error, message, ex);
		}

		static public void Fatal(string message)
		{
			Log(logger.Fatal, message, null);
		}
		static public void Fatal(object message, Exception ex)
		{
			Log(logger.Fatal, message, ex);
		}

		static public void Warning(string message)
		{
			Log(logger.Warn, message, null);
		}
		static public void Warning(object message, Exception ex)
		{
			Log(logger.Warn, message, ex);
		}

		static public void Debug(string message)
		{
			Log(logger.Debug, message, null);
		}
		public static void Debug(object message, Exception ex)
		{
			Log(logger.Debug, message, ex);
		}

		static private void Log(LogHandler logHandler, object message, Exception exception)
		{
			try
			{
				if (HttpContext.Current != null && HttpContext.Current.Request != null)
				{
					message += " - URL:" + HttpContext.Current.Request.Url + ", Refer:" + HttpContext.Current.Request.UrlReferrer + ", Browser:" + HttpContext.Current.Request.Browser.Id + ", IP:" + HttpContext.Current.Request.UserHostAddress;
				}
				logHandler(message, exception);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
