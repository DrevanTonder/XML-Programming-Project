using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BL.Test
{
    [TestClass]
    public class XMLTests
    {
        [TestMethod]
        public void SaveTest()
        {
            //Assign
            var items = new List<Item>()
            {
                new Item()
                {
                    Code = "123",
                    Description = "Testing",
                    CurrentCount = 123,
                    OnOrder = true
                }
            };
            var style = Stylesheet.Style.Table;

            //Act
            using (var stream = File.Create(@"Files/test.xml")) {
                XML.SaveItems(items, stream, style);
            }
            

            //Assert
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(@"Files/test.xml");

            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml("" +
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<?xml-stylesheet href='table.css' type='text/css'?>" +
                "<items>" +
                    "<header>" +
                        "<name>Code</name>" +
                        "<name>Description</name>" +
                        "<name>Current Count</name>" +
                        "<name>On Order</name>" +
                    "</header>" +
                    "<item>" +
                        "<code>123</code>" +
                        "<description>Testing</description>" +
                        "<current-count>123</current-count>" +
                        "<on-order>true</on-order>" +
                    "</item>" +
               "</items>");

            Assert.AreEqual(doc2.OuterXml, doc1.OuterXml);


            // Has the stylesheet been copied?
            var stylesheetPath = Path.Combine(@"Files/" + Stylesheet.GetStyleFilename(style));
            Assert.AreEqual(true, File.Exists(stylesheetPath));
        }
    }
}
