using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NationalIdValidation.Tests
{
    [TestClass]
    public class EnglishNationalInsuranceUnitTests
    {
        private List<string> ValidIdNumbers { get; set; }
        private List<string> InvalidIdNumbers { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            ValidIdNumbers = new List<string>
            {
                "AB 12 34 56 C"
            };
            InvalidIdNumbers = new List<string>
            {
                null,
                "",
                "\0",
                ":",
                "0",
                "DA 00 00 00 \0",
                "FA 00 00 00-",
                "IA 00 00 00\u0100",
                "QA 00 00 00 A\0",
                "QQ 12 34 56 C",
                "BO 00 00 10 A"
            };
        }

        [TestMethod]
        public void ValidatesValidNationalInsuranceIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new EnglishNationalInsuranceId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void ValidatesValidBenefitsDay()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new EnglishNationalInsuranceId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
                Assert.AreEqual(DayOfWeek.Wednesday, idObject.BenefitsDay);
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNationalInsuranceIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new EnglishNationalInsuranceId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }
    }
}
