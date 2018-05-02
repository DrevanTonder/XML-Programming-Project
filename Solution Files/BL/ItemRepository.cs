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
        /// Saves a list of items from the supplied stream
        /// </summary>
        /// <param name="stream">The stream to save to</param>
        /// <param name="saveToXML">whether to save to xml or not</param>
        /// <param name="stylesheet">The stylesheet to add</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the stream is null</exception>
        public void Save(Stream stream, bool saveToXML = false, string stylesheet = "style1.css" )
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream) + " can not be null", nameof(stream));

            if (saveToXML)
            {
                new XDocument(
                    new XProcessingInstruction("xml-stylesheet", $"href='{stylesheet}' type='text/css'"),
                    new XElement("items",
                        Item.XMLHeader(),
                        items.Select(item => item.Value.ToXElement())
                    )
                )
                .Save(stream);
            } else
            {
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvHelper.CsvWriter(writer))
                {
                    csv.Configuration.TypeConverterCache.AddConverter(typeof(bool), new MyBooleanConverter());
                    csv.Configuration.RegisterClassMap<ItemMap>();
                    csv.WriteRecords(items.Values);
                }
            }
        }

        /// <summary>
        /// Updates an Item's Current Count.
        /// </summary>
        /// <param name="itemCode">The Item's code used to find the item</param>
        /// <param name="currentCount">The Item's new CurrentCount</param>
        /// <exception cref="System.ArgumentNullException">Thrown when itemCode is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when itemCode is not in the itemRepository items dictionary</exception>
        public void Update(string itemCode, int currentCount)
        {
            if (itemCode == null) throw new ArgumentNullException(nameof(itemCode) + " can not be null", nameof(itemCode));

            try
            {
                items[itemCode].CurrentCount = currentCount;
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentException(nameof(itemCode) + " is not an item code", e);
            }
            
        }

        /// <summary>
        /// This class just converts Yes/No values into True/False
        /// </summary>
        private class MyBooleanConverter : CsvHelper.TypeConversion.DefaultTypeConverter
        {
            public override string ConvertToString(object value, CsvHelper.IWriterRow row, CsvHelper.Configuration.MemberMapData memberMapData)
            {
                if (value == null)
                {
                    return string.Empty;
                }

                var boolValue = (bool)value;

                return boolValue ? "Yes" : "No";
            }

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
