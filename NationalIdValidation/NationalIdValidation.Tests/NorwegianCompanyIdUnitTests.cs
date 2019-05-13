using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class NorwegianCompanyIdUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidIdNumbers = new List<string>
            {
                "974760673",
                "974 760 673",
                "974-760-673",
                "NO974760673",
                "NO 974 760 673",
                "NO-974-760-673",
                "974760673MVA",
                "974 760 673 MVA",
                "974-760-673-MVA",
                "NO974760673MVA",
                "NO 974 760 673 MVA",
                "NO-974-760-673-MVA"
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
                "974760672"
            };
        }

        [TestMethod]
        public void ValidatesValidNorwegianCompanyIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new NorwegianCompanyId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void ValidatesVatRegisteredWhereGiven()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new NorwegianCompanyId(id);
                var endsWithMva = id.EndsWith("MVA");
                if(endsWithMva)
                    Assert.IsTrue(idObject.VatRegistered, $"VatRegistered not detected: {id}");
                else
                    Assert.IsFalse(idObject.VatRegistered, $"VatRegistered detected: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNorwegianCompanyIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new NorwegianCompanyId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }
    }
}
