using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class DanishPersonalIdUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }
        private List<string> MaleIds { get; set; }
        private List<string> FemaleIds { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidIdNumbers = new List<string>
            {
                "211062-5629",
                "2110625629"
            };
            InvalidIdNumbers = new List<string>
            {
                null,
                "",
                "\0",
                ":",
                "0",
                "000000\0",
                "000000-",
                "000000\u0100",
                "0000008\0",
                "600000-2288",
                "000000-1029",
                "000000-5588\0"
            };
            MaleIds = new List<string>
            {
                "211062-5629",
            };
            FemaleIds = new List<string>
            {
                "211062-5628",
            };
        }

        [TestMethod]
        public void ValidatesValidDanishPersonalIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new DanishPersonalId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidDanishPersonalIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new DanishPersonalId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }

        [TestMethod]
        public void IdentifiesDanishPersonalMaleIds()
        {
            foreach (var idObject in MaleIds.Select(id => new DanishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Male, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesDanishPersonalFemaleIds()
        {
            foreach (var idObject in FemaleIds.Select(id => new DanishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Female, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesDanishPersonalBirthDate()
        {
            var birthDateTests = new Dictionary<string, DateTime>
            {
                {"3012753002", new DateTime(1975, 12, 30)},
                {"0808920287", new DateTime(1992, 08, 08)},
                {"1403850993", new DateTime(1985, 03, 14)},
                {"0712965885", new DateTime(1896, 12, 07)},
                {"1808354542", new DateTime(2035, 08, 18)},
                {"0502821285", new DateTime(1982, 02, 05)},
                {"2109213799", new DateTime(1921, 09, 21)},
                {"2411670778", new DateTime(1967, 11, 24)},
                {"0512450604", new DateTime(1945, 12, 05)},
                {"1202824280", new DateTime(1982, 02, 12)},
                {"2903655341", new DateTime(1865, 03, 29)},
                {"2312135782", new DateTime(2013, 12, 23)}
            };
            foreach (var id in birthDateTests)
            {
                var idObject = new DanishPersonalId(id.Key);
                Assert.AreEqual(id.Value, idObject.BirthDate);
            }
        }

        [TestMethod]
        public void InvalidatesInvalidDanishPersonalDatePart()
        {
            var birthDateTests = new List<string> { "3102155008", "6954899906", "0000005051" };
            foreach (var dateTest in birthDateTests)
            {
                var idObject = new DanishPersonalId(dateTest);
                Assert.IsFalse(idObject.IsValid);
            }
        }

        [TestMethod]
        public void IdentifiesDanishPersonalReplacementNumber()
        {
            var replacementNumbers = new List<string> { "9012753002", "6808920287" };
            foreach (var testId in replacementNumbers)
            {
                var idObject = new DanishPersonalId(testId);
                Assert.AreEqual(DanishPersonalIdType.ReplacementNumber, idObject.DanishPersonalIdType);
            }
        }
        [TestMethod]
        public void IdentifiesDanishPersonalValidModulo()
        {
            var moduloValid = new List<string> { "211062-5629" };
            foreach (var testId in moduloValid)
            {
                var idObject = new DanishPersonalId(testId);
                Assert.IsTrue(idObject.IsModuloValid);
            }
        }
    }
}
