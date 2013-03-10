using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace ProcutVS
{
	class RankedProductManager
	{
		private static void TestRankedProduct()
		{
			Dictionary<string, Dictionary<string, RankedProduct>> classRankedProductsDic =
				new Dictionary<string, Dictionary<string, RankedProduct>>();

			//
			for (int classId = 0; classId < 100; classId++)
			{
				Dictionary<string, RankedProduct> upcRankedProduct = new Dictionary<string, RankedProduct>();
				for (int UPC = 0; UPC < 100; UPC++)
				{
					RankedProduct RankedProduct = new RankedProduct()
					{
						UPC = "UPC_" + UPC,
						ClassId = classId.ToString(),
						Rank = 100
					};
					upcRankedProduct[RankedProduct.UPC] = RankedProduct;
				}
				classRankedProductsDic[classId.ToString()] = upcRankedProduct;
			}

			// cpu 50% - 75%
			for (int i = 0; i < 100; i++)
			{
				string str = JavaScriptConvert.SerializeObject(classRankedProductsDic);
				File.WriteAllText("rankedProducts.json", str);

				//
				classRankedProductsDic =
					JavaScriptConvert.DeserializeObject<Dictionary<string, Dictionary<string, RankedProduct>>>(str);
			}

			foreach (var classId in classRankedProductsDic.Keys)
			{
				foreach (var UPC in classRankedProductsDic[classId].Keys)
					Console.WriteLine(classRankedProductsDic[classId][UPC].UPC);
			}
		}

	}

	class RankedProduct
	{
		public string UPC;
		public int Rank;
		public string ClassId;
	}
}
