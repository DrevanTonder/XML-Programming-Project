using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL
{   
    /// <summary>
    /// XML class to contain XML Logic
    /// </summary>
    public static class XML
    {
        /// <summary>
        /// Save a IEnumerable of items as XML
        /// </summary>
        /// <param name="items">IEnumerable of the items to save</param>
        /// <param name="stream">The stream to save to</param>
        /// <param name="style">The style of the stylesheet</param>
        public static void SaveItems(IEnumerable<Item> items, Stream stream, Stylesheet.Style style)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream) + " can not be null", nameof(stream));
            if (items == null) throw new ArgumentNullException(nameof(items) + " can not be null", nameof(items));

            new XDocument(
                new XProcessingInstruction("xml-stylesheet", $"href='{Stylesheet.GetStyleFilename(style)}' type='text/css'"),
                new XElement("items",
                    Item.XMLHeader(),
                    items.Select(item => item.ToXElement())
                )
            )
            .Save(stream);

            CopyStylesheet(stream, style);
        }


        static void CopyStylesheet(Stream stream, Stylesheet.Style style)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream) + " can not be null", nameof(stream));

            if (stream is FileStream fileStream)
            {
                var directory = Path.GetDirectoryName(fileStream.Name);
                File.Copy(
                    Path.Combine(@"styles/", $"{Stylesheet.GetStyleFilename(style)}"), 
                    Path.Combine(directory, $"{Stylesheet.GetStyleFilename(style)}"),
                    overwrite: true
                );
            }
        }
    }
}
