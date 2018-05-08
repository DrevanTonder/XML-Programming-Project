using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BL.Tests
{
    [TestClass()]
    public class ItemTests
    {
        [TestMethod()]
        public void ToXElementTest()
        {
            //Assign
            var item = new Item()
            {
                Code = "123",
                Description = "testing",
                CurrentCount = 123,
                OnOrder = true
            };

            var expected = new XElement("item",
                        new XElement("code", item.Code),
                        new XElement("description", item.Description),
                        new XElement("current-count", item.CurrentCount),
                        new XElement("on-order", item.OnOrder)
                    );

            //Act
            var actual = item.ToXElement();

            //Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod()]
        public void XMLHeaderTest()
        {
            //Assign
            var expected = new XElement("header",
                            new XElement("name", "Code"),
                            new XElement("name", "Description"),
                            new XElement("name", "Current Count"),
                            new XElement("name", "On Order")
                        );

            //Act
            var actual = Item.XMLHeader();

            //Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}