using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    /// <summary>
    /// No "real" personal ids will be used, only randomized mathematically correct numbers
    /// </summary>
    [TestClass]
    public class SwedishPersonalIdUnitTests
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
                "811218-9876"
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
                "000000\u00000",
                "0000008\0",
                "000000-88888",
                "400000-22288",
                "004000-22288",
                "000000-00000\0",
                // ReSharper disable once StringLiteralTypo
                "abcdefghijk",
                "01.05.94.51843",
                "010594.51843",
                "811218-9875"
            };
            MaleIds = new List<string>
            {
                "811218-9876"
            };
            FemaleIds = new List<string>
            {
                "811218-9868"
            };
        }

        [TestMethod]
        public void ValidatesValidSwedishPersonalIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new SwedishPersonalId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidSwedishPersonalIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new SwedishPersonalId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }

        [TestMethod]
        public void IdentifiesSwedishPersonalMaleIds()
        {
            foreach (var idObject in MaleIds.Select(id => new SwedishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Male, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesSwedishPersonalFemaleIds()
        {
            foreach (var idObject in FemaleIds.Select(id => new SwedishPersonalId(id)))
            {
                Assert.AreEqual(Gender.Female, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesSwedishPersonalBirthDate()
        {
            var birthDateTests = new Dictionary<string, DateTime>
            {
                {"811218-9876", new DateTime(1981, 12, 18)},
                {"811218-9868", new DateTime(1981, 12, 18)}
            };
            foreach (var (key, birthDate) in birthDateTests)
            {
                var idObject = new SwedishPersonalId(key);
                Assert.AreEqual(birthDate, idObject.BirthDate);
            }
        }

        [TestMethod]
        public void InvalidatesInvalidSwedishPersonalDatePart()
        {
            var birthDateTests = new List<string> { "150231-9872" };
            foreach (var dateTest in birthDateTests)
            {
                var idObject = new SwedishPersonalId(dateTest);
                Assert.IsFalse(idObject.IsValid);
            }
        }

        [TestMethod]
        public void IdentifiesSwedishPersonalCoordinationNumber()
        {
            var coordinationNumbers = new List<string> { "811278-9873" };
            foreach (var testId in coordinationNumbers)
            {
                var idObject = new SwedishPersonalId(testId);
                Assert.AreEqual(SwedishPersonalIdType.CoordinationNumber, idObject.SwedishPersonalIdType);
            }
        }

        [TestMethod]
        public void IdentifiesSwedishCompanyNumber()
        {
            var orgNumbers = new List<string> { "212000-0142", "212000-1355", "556036-0793" };
            foreach (var testId in orgNumbers)
            {
                var idObject = new SwedishPersonalId(testId);
                Assert.AreEqual(SwedishPersonalIdType.OrganizationNumber, idObject.SwedishPersonalIdType);
            }
        }
    }
}
