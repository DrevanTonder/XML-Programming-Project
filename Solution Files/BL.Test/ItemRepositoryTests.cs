using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BL.Tests
{
    [TestClass()]  
    public class ItemRepositoryTests
    {

        [TestMethod()]
        public void RetrieveTest()
        {
            //Assign
            var itemRepository = new ItemRepository();
            FileStream stream = File.OpenRead(@"Files/importtest.csv");
            var expected = new Item()
            {
                Code = "A0001",
                Description = "Horse on Wheels",
                CurrentCount = 5,
                OnOrder = false
            };

            //Act
            var items = (List<Item>)itemRepository.Retrieve(stream);

            //Assert
            Assert.AreEqual(expected.Code, items[0].Code);
            Assert.AreEqual(expected.Description, items[0].Description);
            Assert.AreEqual(expected.CurrentCount, items[0].CurrentCount);
            Assert.AreEqual(expected.OnOrder, items[0].OnOrder);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RetrieveTestStreamNull()
        {
            //Assign
            var itemRepository = new ItemRepository();
            FileStream fileStream = null;

            //Act
            itemRepository.Retrieve(fileStream);

            //Assert
        }

        [TestMethod()]
        public void RetrieveTestMissingItems()
        {
            //Assign
            var itemRepository = new ItemRepository();
            var stream = File.OpenRead(@"Files/missingItems.csv");
            var expected = 0;

            //Act
            var items = (List<Item>)itemRepository.Retrieve(stream);

            //Assert
            Assert.AreEqual(expected, items.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void RetrieveTestIncompleteItems()
        {
            //Assign
            var itemRepository = new ItemRepository();
            var stream = File.OpenRead(@"Files/incompleteItems.csv");
            var expected = new Item()
            {
                Code = "A0001",
                Description = "Horse on Wheels",
                CurrentCount = 5,
                OnOrder = false
            };

            //Act
            var items = (List<Item>)itemRepository.Retrieve(stream);

            //Assert
        }
    }
}