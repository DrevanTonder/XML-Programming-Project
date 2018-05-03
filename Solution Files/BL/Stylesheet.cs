using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Stylesheet class to encapsulate stylesheet logic
    /// </summary>
    public static class Stylesheet
    {
        /// <summary>
        /// Style enum
        /// </summary>
        public enum Style
        {
            /// <summary>
            /// "table.css"
            /// </summary>
            Table = 0,
            /// <summary>
            /// "cards.css"
            /// </summary>
            Cards
        }

        /// <summary>
        /// Get the filename for the style
        /// </summary>
        public static string GetStyleFilename(Style style)
        {
            switch (style)
            {
                case Style.Table:
                    return "table.css";
                case Style.Cards:
                    return "cards.css";
                default:
                    return "table.css";
            }
        }
    }
    
}
