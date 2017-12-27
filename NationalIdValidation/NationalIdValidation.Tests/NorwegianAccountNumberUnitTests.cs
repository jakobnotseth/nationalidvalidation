using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class NorwegianAccountNumberUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidIdNumbers = new List<string>
            {
                "12341056789",
                "1234.10.56789",
                "1234 10 56789",
                "3705 05 02962",
                "37050502962",
                "3705.05.02962"
            };
            InvalidIdNumbers = new List<string>
            {
                null,
                "",
                "\0",
                ":",
                "0",
                "0000\0",
                "0000-",
                "0000\u00000",
                "0008\0",
                "1234.10.56788"
            };
        }

        [TestMethod]
        public void ValidatesValidNorwegianBankAccountNumbers()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new NorwegianAccountNumber(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNorwegianBankAccountNumbers()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new NorwegianAccountNumber(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }
    }
}
