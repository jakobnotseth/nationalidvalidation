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
    public class NorwegianPersonalIdUnitTests
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
                "30127530021",
                "08089202877",
                "14038509935",
                "07129658856",
                "18083545425",
                "05028212854",
                "21092137993",
                "24116707787",
                "05124506044",
                "12028242806",
                "19013423205",
                "30121262785",
                "14109952789",
                "04111104056",
                "09112713406",
                "15080402667",
                "20084224902",
                "10020922228",
                "06071441316",
                "22075339627",
                "19116248894",
                "20109733610",
                "16033136688",
                "04125449463",
                "18101108690",
                "26083413831",
                "14090936612",
                "26036704801",
                "27034005309",
                "16073315437",
                "03060481694",
                "17041346655",
                "11068907456",
                "12052840579",
                "26037642435",
                "19112019130",
                "23044418517",
                "16115123754",
                "01014438714",
                "06041012925",
                "14109508048",
                "06093311505",
                "27010997289",
                "03128133703",
                "21039468937",
                "25122712799",
                "12060939045",
                "26105806298",
                "27119012223",
                "03089450755",
                "07057535576",
                "08129616157",
                "05045746269",
                "27031088130",
                "11010681407",
                "24113138811",
                "18126537960",
                "22129346896",
                "09107503452",
                "02107832494",
                "04079543980",
                "27039521771",
                "29110223399",
                "25042235647",
                "06029008254",
                "31123449250",
                "31052128482",
                "24076813821",
                "18072211959",
                "04073030493",
                "03059247920",
                "22120282941",
                "12124043199",
                "13117614004",
                "20104018271",
                "23014931821",
                "180835-07647",
                "210931-47127",
                "010921-47157",
                "021196-15006",
                "281099-97562",
                "240312-45148",
                "230301-89647",
                "150926-46618",
                "040659-45458",
                "070694-24826",
                "100769-29810",
                "240810-67364",
                "041226-03860",
                "040386-03367",
                "141213-10180",
                "300421-48802",
                "090518-24005",
                "150305-66979",
                "121085-21846",
                "120163-42023",
                "200858-22420",
                "050939-33518",
                "010594-51843",
                "040748-02643"
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
                // ReSharper disable once StringLiteralTypo
                "abcdef51843",
                "01059422222",
                "01.05.94.51843",
                "010594 51843",
                "010594.51843",
                "40127530021",
                "09089202877",
                "14138509935",
                "07139658856",
                "18084545425",
                "05028312854",
                "21092147993",
                "24116708787",
                "05124506144",
                "12028242816",
                "19013423206",
                "20121262785",
                "13109952789",
                "04011104056",
                "09102713406",
                "15089402667",
                "20084124902",
                "10020912228",
                "06071440316",
                "22075339527",
                "19116248884",
                "20109733619"
            };
            MaleIds = new List<string>
            {
                "14038509935",
                "21092137993",
                "24116707787",
                "30121262785",
                "14109952789",
                "20084224902",
                "06071441316",
                "27034005309",
                "12052840579",
                "19112019130",
                "23044418517",
                "16115123754",
                "01014438714",
                "06041012925",
                "06093311505",
                "03128133703",
                "21039468937",
                "25122712799",
                "03089450755",
                "07057535576",
                "08129616157",
                "27031088130",
                "18126537960",
                "04079543980",
                "27039521771",
                "29110223399",
                "18072211959",
                "03059247920",
                "22120282941",
                "121240-43199",
                "210931-47127",
                "010921-47157",
                "281099-97562",
                "240312-45148",
                "240810-67364",
                "040386-03367",
                "141213-10180",
                "150305-66979",
                "050939-33518",
            };
            FemaleIds = new List<string>
            {
                "30127530021",
                "08089202877",
                "07129658856",
                "18083545425",
                "05028212854",
                "05124506044",
                "12028242806",
                "19013423205",
                "04111104056",
                "09112713406",
                "15080402667",
                "10020922228",
                "22075339627",
                "19116248894",
                "20109733610",
                "16033136688",
                "04125449463",
                "18101108690",
                "26083413831",
                "14090936612",
                "26036704801",
                "16073315437",
                "03060481694",
                "17041346655",
                "11068907456",
                "26037642435",
                "14109508048",
                "27010997289",
                "12060939045",
                "26105806298",
                "27119012223",
                "05045746269",
                "11010681407",
                "24113138811",
                "22129346896",
                "09107503452",
                "02107832494",
                "25042235647",
                "06029008254",
                "31123449250",
                "31052128482",
                "24076813821",
                "04073030493",
                "13117614004",
                "20104018271",
                "23014931821",
                "18083507647",
                "02119615006",
                "23030189647",
                "15092646618",
                "04065945458",
                "070694-24826",
                "100769-29810",
                "041226-03860",
                "300421-48802",
                "090518-24005",
                "121085-21846",
                "120163-42023",
                "200858-22420",
                "010594-51843",
                "040748-02643"
            };
        }

        [TestMethod]
        public void ValidatesValidNorwegianPersonalIds()
        {
            foreach (var id in ValidIdNumbers)
            {
                var idObject = new NorwegianPersonalId(id);
                Assert.IsTrue(idObject.IsValid, $"A valid mathematically number does not validate: {id}");
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNorwegianPersonalIds()
        {
            foreach (var id in InvalidIdNumbers)
            {
                var idObject = new NorwegianPersonalId(id);
                Assert.IsFalse(idObject.IsValid, $"An invalid mathematically number does validate: {id}");
            }
        }

        [TestMethod]
        public void IdentifiesNorwegianPersonalMaleIds()
        {
            foreach (var idObject in MaleIds.Select(id => new NorwegianPersonalId(id)))
            {
                Assert.AreEqual(Gender.Male, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesNorwegianPersonalFemaleIds()
        {
            foreach (var idObject in FemaleIds.Select(id => new NorwegianPersonalId(id)))
            {
                Assert.AreEqual(Gender.Female, idObject.Gender);
            }
        }

        [TestMethod]
        public void IdentifiesNorwegianPersonalBirthDate()
        {
            var birthDateTests = new Dictionary<string, DateTime>
            {
                {"30127530021", new DateTime(1975, 12, 30)},
                {"08089202877", new DateTime(1992, 08, 08)},
                {"14038509935", new DateTime(1985, 03, 14)},
                {"07129658856", new DateTime(1896, 12, 07)},
                {"18083545425", new DateTime(1935, 08, 18)},
                {"05028212854", new DateTime(1982, 02, 05)},
                {"21092137993", new DateTime(1921, 09, 21)},
                {"24116707787", new DateTime(1967, 11, 24)},
                {"05124506044", new DateTime(1945, 12, 05)},
                {"12028242806", new DateTime(1982, 02, 12)},
                {"29036553411", new DateTime(1865, 03, 29)},
                {"23121357828", new DateTime(2013, 12, 23)}
            };
            foreach (var id in birthDateTests)
            {
                var idObject = new NorwegianPersonalId(id.Key);
                Assert.AreEqual(id.Value, idObject.BirthDate);
            }
        }

        [TestMethod]
        public void InvalidatesInvalidNorwegianPersonalDatePart()
        {
            var birthDateTests = new List<string> { "31021550089", "69548999065", "00000050515" };
            foreach (var dateTest in birthDateTests)
            {
                var idObject = new NorwegianPersonalId(dateTest);
                Assert.IsFalse(idObject.IsValid);
            }
        }

        [TestMethod]
        public void IdentifiesNorwegianPersonalDNumber()
        {
            var dNumbers = new List<string> {"58031320478", "70019950032"};
            foreach (var testId in dNumbers)
            {
                var idObject = new NorwegianPersonalId(testId);
                Assert.AreEqual(NorwegianPersonalIdType.DNumber, idObject.NorwegianPersonalIdType);
            }
        }

        [TestMethod]
        public void IdentifiesNorwegianPersonalHNumber()
        {
            var hNumbers = new List<string> {"18431320467"};
            foreach (var testId in hNumbers)
            {
                var idObject = new NorwegianPersonalId(testId);
                Assert.AreEqual(NorwegianPersonalIdType.HNumber, idObject.NorwegianPersonalIdType);
            }
        }
    }
}
