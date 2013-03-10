using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace ProcutVS
{
	public class HotProductPairManager
	{
		const int MAX_QUEUE_LENGTH = 100;
		private const int REFRESH_THRESHOLD = 10;


		private static Queue<ProductPair> ProductVisitQueue = null;
		static Dictionary<ProductPair, int> ProductPairCountDic = new Dictionary<ProductPair, int>();
		
		static readonly object snycLock = new object();
		private static int updateCounter = 0;

		static HotProductPairManager()
		{
			IO2Queue();
		}

		private static void IO2Queue()
		{
			ProductVisitQueue =DataAccess.ReadHotProductVisitQueue();
			foreach (var productPair in ProductVisitQueue)
			{
				FillPairCountDic(productPair);
			}
		}

		private static void FillPairCountDic(ProductPair productPair)
		{
			if (ProductPairCountDic.ContainsKey(productPair))
				ProductPairCountDic[productPair]++;
			else
				ProductPairCountDic[productPair] = 1;
		}

		public static KeyValuePair<ProductPair, int>[] GetHostProductPairs()
		{
			List<KeyValuePair<ProductPair, int>> hotPairs = new List<KeyValuePair<ProductPair, int>>(ProductPairCountDic);

			hotPairs.Sort(CompareHot);

			return hotPairs.ToArray();
		}

		static int CompareHot(KeyValuePair<ProductPair, int> firstPair, KeyValuePair<ProductPair, int> nextPair)
		{
			return (-1) * firstPair.Value.CompareTo(nextPair.Value);
		}


		public static void AddVisit(string upc1, string upc2)
		{
			lock (snycLock)
			{

				ProductPair productPair = new ProductPair() {UPC1 = upc1, UPC2 = upc2};

				ProductVisitQueue.Enqueue(productPair);

				if (ProductVisitQueue.Count > MAX_QUEUE_LENGTH)
				{
					ProductPair dequeuedPair = ProductVisitQueue.Dequeue();

					ProductPairCountDic[dequeuedPair]--;

					if (ProductPairCountDic[dequeuedPair] <= 0)
						ProductPairCountDic.Remove(dequeuedPair);
				}

				FillPairCountDic(productPair);

				// for performace reason, use REFRESH_THRESHOLD
				if (updateCounter++ > REFRESH_THRESHOLD)
				{
					updateCounter = 0;

					Queue2IO();
				}
			}
		}

		private static void Queue2IO()
		{
			DataAccess.WriteHotProductVisitQueue(ProductVisitQueue);
		}
	}

	public class ProductPair
	{
		public string UPC1;
		public string UPC2;
		public string Key
		{
			get { return UPC1 + "_" + UPC2; }
		}

		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is ProductPair)
				return Key == ((ProductPair)obj).Key;

			return base.Equals(obj);
		}
	}

}
