/*    Remix.NET
 * 
 * Remix.NET is licensed under the MIT license:
 * http://www.opensource.org/licenses/mit-license.html
 * 
 * Copyright (c) 2009 Omar Abdelwahed
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ProcutVS;

namespace Remix
{
    [XmlType("store")]
    public class Store
    {
        [XmlElement("storeId")]
        public string StoreID;

        [XmlElement("name")]
        public string Name;

        [XmlElement("address")]
        public string Address;

        [XmlElement("city")]
        public string City;

        [XmlElement("region")]
        public string Region;

        [XmlElement("postalCode")]
        public string PostalCode;

        [XmlElement("fullPostalCode")]
        public string FullPostalCode;

        [XmlElement("country")]
        public string Country;

        [XmlElement("lat")]
        public string Latitude;

        [XmlElement("lng")]
        public string Longitude;

        [XmlElement("phone")]
        public string Phone;

        [XmlElement("hours")]
        public string Hours;

        [XmlElement("distance")]
        public string Distance;

        [XmlElement("guid")]
        public string Guid;

        public Store() { }

        public Store(string address,string city,string country,string distance,string fullpostalcode,
                string hours,string latitude,string longitude,string name,string phone,
                string postalcode,string region,string storeid)
        {
            this.Address = address;
            this.City = city;
            this.Country = country;
            this.Distance = distance;
            this.FullPostalCode = fullpostalcode;
            this.Hours = hours;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
            this.Phone = phone;
            this.PostalCode = postalcode;
            this.Region = region;
            this.StoreID = storeid;
            
        }

        public override bool Equals(object obj)
        {
            return obj is Store &&
                ((Store)obj).Name == Name &&
                ((Store)obj).Address == Address &&
                ((Store)obj).City == City &&
                ((Store)obj).PostalCode == PostalCode &&
                ((Store)obj).Latitude == Latitude &&
                ((Store)obj).Longitude == Longitude;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public String ToXml()
        {
            return UTF8XmlSerializer.Serialize(this);
        }
    }
}
