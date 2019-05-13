using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class NorwegianCustomerIdUnitTests
    {
        private List<string> ValidModulo10IdNumbers { get; set; }
        private List<string> InvalidModulo10IdNumbers { get; set; }
        private List<string> ValidModulo11IdNumbers { get; set; }
        private List<string> InvalidModulo11IdNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidModulo10IdNumbers = new List<string>
            {
                "123456782",
                "1234567830",
            };
            InvalidModulo10IdNumbers = new List<string>
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
                "123456785",
                "12345678901234567890123456",
            };
            ValidModulo11IdNumbers = new List<string>
            {
                "123456785",
                "1234567930",
                "123456784-",
            };
            InvalidModulo11IdNumbers = new List<string>
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
                "123456782",
                "12345678901234567890123456",
            };
        }

        [TestMethod]
        public void ValidatesValidNorwegianCustomerIds()
        {
            foreach (var id in ValidModulo10IdNumbers)
            {
                var idObject = new NorwegianCustomerId(id, CustomerIdValidationRoutine.Modulus10);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically modulus 10 number does not validate: {id}");
                Assert.AreEqual(CustomerIdValidationRoutine.Modulus10, idObject.ValidationRoutine);
            }
            foreach (var id in ValidModulo11IdNumbers)
            {
                var idObject = new NorwegianCustomerId(id, CustomerIdValidationRoutine.Modulus11);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically modulus 11 number does not validate: {id}");
                Assert.AreEqual(CustomerIdValidationRoutine.Modulus11, idObject.ValidationRoutine);
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNorwegianCustomerIds()
        {
            foreach (var id in InvalidModulo10IdNumbers)
            {
                var idObject = new NorwegianCustomerId(id, CustomerIdValidationRoutine.Modulus10);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically modulus 10 number does validate: {id}");
                Assert.AreEqual(CustomerIdValidationRoutine.Modulus10, idObject.ValidationRoutine);
            }
            foreach (var id in InvalidModulo11IdNumbers)
            {
                var idObject = new NorwegianCustomerId(id, CustomerIdValidationRoutine.Modulus11);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically modulus 11 number does validate: {id}");
                Assert.AreEqual(CustomerIdValidationRoutine.Modulus11, idObject.ValidationRoutine);
            }
        }
    }
}
