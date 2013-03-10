using System;
using System.Collections.Generic;

using System.Text;
using Remix;

namespace ProcutVS
{
	class Program
	{
		static void Main(string[] args)
		{
			//BestBuyFiller.Do();

			//Product product1 = ProductPool.GetByBestBuySKU("1831054", CacheType.Full);
			//Product product2 = ProductPool.GetByBestBuySKU("2260049", CacheType.Full);
			//Printer.PrintProductVSToConsole(product1, product2);

			Printer.PrintCategorylayout(Server.ROOT_CATEGORY_ID);

			//FillOneProduct();

			//for (int i = 1; i < 10; i++)
			//    Printer.PrintCategorylayout("abcat0100000");
		}

		private static void FillOneProduct()
		{
			Product product = new Product();
			product.UPC = "794552240358";
			//product.BestBuySKU = "9691888";

			//BestBuySpecFiller.Do("http://www.bestbuy.com/site/Anji+Mountain+Bamboo+Chairmat+&+Rug+Co.+-+44%22+x+52%22+Bamboo+Roll-Up+Chair+Mat+-+Natural/2715746.p?id=1218346644628&skuId=2715746&cmp=RMX&ky=2isnSVUvS8KgsXhGUincLJamwEoV5QK0b", product);

			BestBuyFiller.Do(product);
			BestBuySpecFiller.Do(product);
			AmazonFiller.Do(product);
			BestBuyReviewsFiller.Do(product);

			DataAccess.SaveProduct(product);
		}
	}
}
