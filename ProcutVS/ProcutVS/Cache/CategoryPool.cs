using System;
using System.Collections.Generic;
using System.Text;
using Remix;
using System.Web;

namespace ProcutVS
{
	public class CategoryPool
	{
		public static void Cache(Category category)
		{
			if (category == null) return;

			if (HttpRuntime.Cache.Get(GetCacheKey(category.Id)) == null)
			{
				HttpRuntime.Cache.Insert(GetCacheKey(category.Id),
					category,
					null,
					DateTime.Now.AddHours(24),
					System.Web.Caching.Cache.NoSlidingExpiration,
					System.Web.Caching.CacheItemPriority.Normal,
					null);
			}
		}


		public static Category GetById(string Id)
		{
			Category category = (Category)HttpRuntime.Cache.Get(GetCacheKey(Id));

			if (category == null)
			{
				category = GetAFilledCategory(Id);
				Cache(category);
			}

			return category;
		}

		public static void WarmUpCategories(IEnumerable<string> ids)
		{
			List<string> IdList = new List<string>();
			foreach (string id in ids)
			{
				if (HttpRuntime.Cache.Get(GetCacheKey(id)) == null)
					IdList.Add(id);
			}

			Categories categories = GetAFilledCategory(IdList.ToArray());
			foreach (var category in categories)
			{
				Cache(category);
			}
		}



		static string GetCacheKey(string Id)
		{
			return "BBYCategory_" + Id;
		}

		#region GetFromSource
		private static Category GetAFilledCategory(string id)
		{
			Remix.Server server = new Remix.Server("", "");

			Categories categories = server.GetCategory(id, 1);
			if (categories.Count > 0)
				return categories[0];
			else
				return null;
		}
		private static Categories GetAFilledCategory(string[] ids)
		{
			Remix.Server server = new Remix.Server("", "");

			return server.GetCategory(ids, 1);
		}

		#endregion
	}
}
