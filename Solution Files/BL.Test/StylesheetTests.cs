using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Tests
{
    [TestClass()]
    public class StylesheetTests
    {
        [TestMethod()]
        public void GetStyleFilenameTest()
        {
            //Assign
            var style = Stylesheet.Style.Table;

            var expected = "table.css";

            //Act
            var actual = Stylesheet.GetStyleFilename(style);

            Assert.AreEqual(expected, actual);
        }
    }
}