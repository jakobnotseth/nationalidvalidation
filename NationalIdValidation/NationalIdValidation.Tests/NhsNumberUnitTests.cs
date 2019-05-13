using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class NhsNumberUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }
        private List<string> NorthernIrelandNumbers { get; set; }
        private List<string> ScotlandNumbers { get; set; }
        private List<string> EnglandWalesIsleOfManNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ScotlandNumbers = new List<string>
            {
                "185-898-7857",
                "245 616 7724",
            };
            NorthernIrelandNumbers = new List<string>
            {
                "3200000015",
            };
            EnglandWalesIsleOfManNumbers = new List<string>
            {
                "401 023 2137",
                "626 826 0147",
                "482 704 1768",
                "660 514 0206",
            };
            ValidIdNumbers = new List<string>
            {
                "401 023 2137",
                "3200000015",
                "626 826 0147",
                "185-898-7857",
                "245 616 7724",
                "482 704 1768",
                "660 514 0206",
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
        public void ValidatesValidScottishChiNumbers()
        {
            foreach (var id in ScotlandNumbers)
            {
                var idObject = new NhsNumber(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
                Assert.AreEqual(NhsNumberLocation.Scotland, idObject.Location);
            }
        }

        [TestMethod]
        public void ValidatesValidNorthernIrelandNhsNumbers()
        {
            foreach (var id in NorthernIrelandNumbers)
            {
                var idObject = new NhsNumber(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
                Assert.AreEqual(NhsNumberLocation.NorthernIreland, idObject.Location);
            }
        }

        [TestMethod]
        public void ValidatesValidEnglishWalesNhsNumbers()
        {
            foreach (var id in EnglandWalesIsleOfManNumbers)
            {
                var idObject = new NhsNumber(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
                Assert.AreEqual(NhsNumberLocation.EnglandWalesAndIsleOfMan, idObject.Location);
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
