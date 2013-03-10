using System;
using System.ServiceModel;
using System.Xml;
using ProcutVS.Amazon.ECS;


namespace ProcutVS
{
	public class AmazonFiller
	{
		// Use one of the following destinations, according to the region you are
		// interested in:
		// 
		//      US: ecs.amazonaws.com 
		//      CA: ecs.amazonaws.ca 
		//      UK: ecs.amazonaws.co.uk 
		//      DE: ecs.amazonaws.de 
		//      FR: ecs.amazonaws.fr 
		//      JP: ecs.amazonaws.jp
		//
		// Note: protocol must be https for signed SOAP requests.
		const String DESTINATION = "https://ecs.amazonaws.com/onca/soap?Service=AWSECommerceService";

		// Set your AWS Access Key ID and AWS Secret Key here.
		// You can obtain them at:
		// http://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key
		const String MY_AWS_ID = "AKIAJIZ5JPCUEWF5MA7A";
		const String MY_AWS_SECRET = "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";


		/// <summary>
		/// by UPC or ASIN
		/// </summary>
		/// <param name="product"></param>
		public static void Do(Product product)
		{
			// create a WCF Amazon ECS client
			AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
				new BasicHttpBinding(BasicHttpSecurityMode.Transport)
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferSize=int.MaxValue,
						ReaderQuotas = new XmlDictionaryReaderQuotas()
						{
							MaxStringContentLength = int.MaxValue
						}
					},
				new EndpointAddress(DESTINATION));

			// add authentication to the ECS client
			client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(MY_AWS_ID, MY_AWS_SECRET));

			// prepare an ItemSearch request
			ItemLookup itemLookup = new ItemLookup();
			itemLookup.AWSAccessKeyId = MY_AWS_ID;
			itemLookup.AssociateTag = "modkitdesgui-20";

			ItemLookupRequest itemLookupRequest = new ItemLookupRequest();

			if (!string.IsNullOrEmpty(product.UPC))
			{
				itemLookupRequest.IdType = ItemLookupRequestIdType.UPC;
				itemLookupRequest.IdTypeSpecified = true;
				itemLookupRequest.ItemId = new[] { product.UPC };
			}
			else if (!string.IsNullOrEmpty(product.AmazonASIN))
			{
				itemLookupRequest.IdType = ItemLookupRequestIdType.ASIN;
				itemLookupRequest.IdTypeSpecified = true;
				itemLookupRequest.ItemId = new[] { product.AmazonASIN };
			}
			else
			{
				return;
			}

			itemLookupRequest.SearchIndex = "All";
			itemLookupRequest.ResponseGroup = new String[] { "Large" };
			itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };

			// make the call and print the title if it succeeds
			try
			{
				ItemLookupResponse itemLookupResponse = client.ItemLookup(itemLookup);
				//search by upc, it may return more than 1 items!!!

				if (itemLookupResponse.Items.Length == 0
					|| itemLookupResponse.Items[0].Item == null
					|| itemLookupResponse.Items[0].Item.Length == 0)
					return;

				Item item = itemLookupResponse.Items[0].Item[0];
				foreach (Item ritem in itemLookupResponse.Items[0].Item)
				{
					if (!string.IsNullOrEmpty(product.UPC) && ritem.ItemAttributes.UPC == product.UPC)
					{
						item = ritem;
						break;
					}
				}

				//
				if (item.ItemAttributes.Feature != null)
					foreach (var feature in item.ItemAttributes.Feature)
					{
						product.Features.Add(new Feature() { Desc = feature });
					}

				//
				product.AmazonDetailsUrl = item.DetailPageURL;
				int.TryParse(item.SalesRank, out product.AmazonSaleRank);
				product.AmazonASIN = item.ASIN;
				product.UPC = item.ItemAttributes.UPC;

				if (string.IsNullOrEmpty(product.Name))
					product.Name = item.ItemAttributes.Title;

				if (item.ItemAttributes.ListPrice != null)
				{
					decimal.TryParse(item.ItemAttributes.ListPrice.Amount, out product.AmazonPrice);
				}

				if (item.OfferSummary.LowestNewPrice != null)
				{
					decimal lowestPrice;
					if (decimal.TryParse(item.OfferSummary.LowestNewPrice.Amount, out lowestPrice)
						&& lowestPrice > 0)
						product.AmazonPrice = lowestPrice;
				}
				product.AmazonPrice /= 100;

				if (string.IsNullOrEmpty(product.LargeImageUrl) || product.LargeImageUrl.Contains("default_hardlines"))
					product.LargeImageUrl = item.LargeImage.URL;
				if (string.IsNullOrEmpty(product.ThumbnailImageUrl) || product.LargeImageUrl.Contains("default_hardlines"))
					product.ThumbnailImageUrl = item.SmallImage.URL;

				//
				if (item.SimilarProducts != null)
				{
					foreach (var similarProduct in item.SimilarProducts)
					{
						product.SimilarProducts.Add(new Product() { AmazonASIN = similarProduct.ASIN, Name = similarProduct.Title });
					}
				}
			}
			catch (Exception e)
			{
				Logger.Error("Amazon Error", e);
			}

		}
	}
}
