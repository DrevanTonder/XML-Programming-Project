﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL
{
    /// <summary> 
    ///  This class contains the methods to read and write items too a csv file
    /// </summary> 
    public class ItemRepository
    {
        private Dictionary<string, Item> items;

        /// <summary>
        /// IEnumerable of the items
        /// </summary>
        public IEnumerable<Item> Items
        {
            get
            {
                return items.Select(keyValuePair => keyValuePair.Value);
            }
        }
        /// <summary> 
        ///  ItemRepository Constructor
        /// </summary>
        public ItemRepository() {
            items = new Dictionary<string, Item>();
        }

        /// <summary>
        /// Retrieves a list of items from the supplied stream
        /// </summary>
        /// <param name="stream">The stream to read to</param>
        /// <returns>A IEnumerable of the items in the stream</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the stream is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when the stream has incomplete items</exception>
        public IEnumerable<Item> Retrieve(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream) + " can not be null", nameof(stream));

            List<Item> itemList;

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvHelper.CsvReader(reader))
            {
                csv.Configuration.TypeConverterCache.AddConverter(typeof(bool), new MyBooleanConverter());
                csv.Configuration.RegisterClassMap<ItemMap>();
                try
                {
                    itemList = new List<Item>(csv.GetRecords<Item>());
                }
                catch (CsvHelper.MissingFieldException e)
                {
                    throw new ArgumentException(nameof(stream) + " has incomplete items", e);
                }
                
            }
            
            foreach(var item in itemList)
            {
                items[item.Code] = item;
            }

            return itemList;
        }

        /// <summary>
        /// This class just converts Yes/No values into True/False
        /// </summary>
        private class MyBooleanConverter : CsvHelper.TypeConversion.DefaultTypeConverter
        {
            public override object ConvertFromString(string text, CsvHelper.IReaderRow row, CsvHelper.Configuration.MemberMapData memberMapData)
            {
                if (text == null)
                {
                    return string.Empty;
                }

                return text.ToLower() == "yes" ? true : false;
            }
        }

        /// <summary>
        /// This class tells CsvHelper how to convert the file rows into items and vice versa
        /// </summary>
        private sealed class ItemMap : CsvHelper.Configuration.ClassMap<Item>
        {
            public ItemMap()
            {
                Map(m => m.Code).Name("Item Code");
                Map(m => m.Description).Name("Item Description");
                Map(m => m.CurrentCount).Name("Current Count");
                Map(m => m.OnOrder).Name("On Order");
            }
        }
    }
}
