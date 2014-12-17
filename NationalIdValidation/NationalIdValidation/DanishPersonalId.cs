using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Danish personal ID-numbers (CPR-numbers)
    /// </summary>
    public class DanishPersonalId
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; private set; }
        /// <summary>
        /// In case of a validated input, returns Male or Female, otherwise Unknown
        /// </summary>
        public Gender Gender { get; private set; }
        /// <summary>
        /// In case of a validated input, returns a valid date, otherwise returns DateTime.MinValue
        /// </summary>
        public DateTime BirthDate { get; private set; }
        /// <summary>
        /// In case of a validated input, returns whether the id is a birth number or replacement number, otherwise unknown
        /// </summary>
        public DanishPersonalIdType DanishPersonalIdType { get; private set; }
        /// <summary>
        /// Returns whether the id is valid according to the old modulus rule
        /// </summary>
        public bool IsModuloValid { get; set; }

        /// <summary>
        /// Creates a DanishPersonalId object
        /// </summary>
        /// <param name="danishIdString">Any Danish personal id string (CPR-number) with or without divider (dash) between date and sequence numbers</param>
        /// <example><code>
        /// var id = new DanishPersonalId("1111111111");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The personal id validates");
        ///     Console.WriteLine("The person was born " + id.BirthDate.ToString());
        ///     if (id.Gender == Gender.Male)
        ///         Console.WriteLine("The person is a male");
        ///     else
        ///         Console.WriteLine("The person is a female");
        /// }
        /// </code></example>
        /// <remarks>
        /// CPR numbers in Denmark can no longer be modulo 11 validated since CPR-numbers can now contain numbers without valid control digits
        /// </remarks>
        public DanishPersonalId(string danishIdString)
        {
            IsValid = false;
            IsModuloValid = false;
            Gender = Gender.Unknown;
            BirthDate = DateTime.MinValue;
            DanishPersonalIdType = DanishPersonalIdType.Unknown;
            if (string.IsNullOrEmpty(danishIdString)) return;
            var reg = Regex.Match(danishIdString, @"^(?<d1>[0-9])(?<d2>[0-9])(?<m1>[0-1])(?<m2>[0-9])(?<y3>[0-9])(?<y4>[0-9])-?(?<s1>\d)(?<s2>\d)(?<s3>\d)(?<s4>\d)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!reg.Success) return;
            var d1 = int.Parse(reg.Groups["d1"].Value); // day 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // day 2
            var m1 = int.Parse(reg.Groups["m1"].Value); // month 1
            var m2 = int.Parse(reg.Groups["m2"].Value); // month 2
            var y3 = int.Parse(reg.Groups["y3"].Value); // year 1
            var y4 = int.Parse(reg.Groups["y4"].Value); // year 2
            var s1 = int.Parse(reg.Groups["s1"].Value); // sequential 1
            var s2 = int.Parse(reg.Groups["s2"].Value); // sequential 2
            var s3 = int.Parse(reg.Groups["s3"].Value); // sequential 3
            var s4 = int.Parse(reg.Groups["s4"].Value); // sequential 4
            // The kind of number can be defined by the presence of a added 6 to first digit of day, otherwise birth number
            if (d1 >= 6)
            {
                DanishPersonalIdType = DanishPersonalIdType.Replacementnumber;
                d1 -= 6;
            }
            else
                DanishPersonalIdType = DanishPersonalIdType.BirthNumber;
            // The gender can be determined by the fourth sequence number, odd digit is male, even is female
            Gender = s4 % 2 == 0 ? Gender.Female : Gender.Male;
            var i = int.Parse(string.Format("{0}{1}{2}{3}", s1, s2, s3, s4));
            var y = int.Parse(string.Format("{0}{1}", y3, y4));
            if (i <= 3999 || (i <= 4999 && y >= 37) || (i >= 9000 && i <= 9999 && y >= 37))
            {
                y += 1900;
            }
            else if (i >= 5000 && i <= 8999 && y >= 58)
            {
                y += 1800;
            }
            else
            {
                y += 2000;
            }
            // The date should parse to a valid DateTime object
            DateTime bDate;
            if (!DateTime.TryParseExact(string.Format("{0}{1}{2}{3}{4}", y, m1, m2, d1, d2), "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out bDate)) return;
            BirthDate = bDate;
            IsValid = true;
            // CPR numbers can no longer be modulo 11 validated!
            // Old modulo function:
            // if (((d1 * 4) + (d2 * 3) + (m1 * 2) + (m2 * 7) + (y3 * 6) + (y4 * 5) + (s1 * 4) + (s2 * 3) + (s3 * 2) + (s4)) % 11 == 0)
            var r1 = ((d1 * 4) + (d2 * 3) + (m1 * 2) + (m2 * 7) + (y3 * 6) + (y4 * 5) + (s1 * 4) + (s2 * 3) + (s3 * 2)) % 11; // result 1
            if (s4 == 11 - r1)
            {
                IsModuloValid = true;
            }
        }
    }
}
