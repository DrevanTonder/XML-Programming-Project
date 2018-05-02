using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL
{
    /// <summary>
    /// Item Class
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Whether the Item is On Order or not
        /// </summary>
        public bool OnOrder { get; set; }
        /// <summary>
        /// A Description of the Item
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The Current Count of the amount of this Item in stock
        /// </summary>
        public int CurrentCount { get; set; }
        /// <summary>
        /// The Item's Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Format this object as an XElement
        /// </summary>
        public XElement ToXElement()
        {
            return new XElement("item",
                        new XElement("code", this.Code),
                        new XElement("description", this.Description),
                        new XElement("current-count", this.CurrentCount),
                        new XElement("on-order", this.OnOrder)
                    );
        }

        /// <summary>
        /// Get an XMl Header for this object
        /// </summary>
        public static XElement XMLHeader()
        {
            return new XElement("header",
                            new XElement("name", "Code"),
                            new XElement("name", "Description"),
                            new XElement("name", "Current Count"),
                            new XElement("name", "On Order")
                        );
        }
    }
}
