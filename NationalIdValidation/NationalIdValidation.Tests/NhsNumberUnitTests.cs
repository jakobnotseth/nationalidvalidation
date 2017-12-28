using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class NhsNumberUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidIdNumbers = new List<string>
            {
                "401 023 2137",
                "626 826 0147",
                "942-368-1816",
                "185-898-7857",
                "245 616 7724",
                "825 479 6572",
                "579 368 0179",
                "482 704 1768",
                "822 668 5257",
                "660 514 0206",
                "598 760 8295",
            };
            InvalidIdNumbers = new List<string>
            {
                null,
                "",
                "\0",
                ":",
                "0",
                "000\0",
                "000-",
                "000\u00000",
                "008\0",
                "401 023 2138",
            };
        }

        [TestMethod]
        public void ValidatesValidNhsNumbers()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new NhsNumber(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNhsNumbers()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new NhsNumber(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }
    }
}
