using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class FinnishPersonalIdUnitTests
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
                "311280-888Y",
                "311280-999J"
            };
            InvalidIdNumbers = new List<string>
            {
                null,
                "",
                "\0",
                "4",
                "0",
                "0\0",
                "00",
                "00\0",
                "000000+\0",
                "000000+3",
                "000000+\u0100",
                "000000+3\0",
                "000000+33",
                "000000+9380",
                "000000+3430\0\0",
                "000000+033Z",
                "000000+3339",
                "000000+933A",
                "000000+033H",
                "000000+333A",
                "311280-888X",
                "311280-999I"
            };
            MaleIds = new List<string>
            {
                "311280-999J"
            };
            FemaleIds = new List<string>
            {
                "311280-888Y"
            };
        }

        [TestMethod]
        public void ValidatesValidFinnishPersonalIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new FinnishPersonalId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidFinnishPersonalIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new FinnishPersonalId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }

        [TestMethod]
        public void IdentifiesFinnishPersonalMaleIds()
        {
            foreach (var idObject in MaleIds.Select(id => new FinnishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Male, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesFinnishPersonalFemaleIds()
        {
            foreach (var idObject in FemaleIds.Select(id => new FinnishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Female, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesFinnishPersonalBirthDate()
        {
            var birthDateTests = new Dictionary<string, DateTime>
            {
                {"311280-888Y", new DateTime(1980, 12, 31)},
                {"311280-999J", new DateTime(1980, 12, 31)},
                {"311280+888Y", new DateTime(1880, 12, 31)},
                {"311280+999J", new DateTime(1880, 12, 31)},
                {"311280A888Y", new DateTime(2080, 12, 31)},
                {"311280A999J", new DateTime(2080, 12, 31)}
            };
            foreach (var id in birthDateTests)
            {
                var idObject = new FinnishPersonalId(id.Key);
                Assert.AreEqual(id.Value, idObject.BirthDate);
            }
        }

        [TestMethod]
        public void InvalidatesFinnishPersonalInvalidDatePart()
        {
            var birthDateTests = new List<string> { "310280-3341", "000000+333A" };
            foreach (var dateTest in birthDateTests)
            {
                var idObject = new FinnishPersonalId(dateTest);
                Assert.IsFalse(idObject.IsValid);
            }
        }
    }
}
