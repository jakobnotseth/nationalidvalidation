using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Norwegian personal ID-numbers
    /// </summary>
    public class NorwegianPersonalId
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }
        /// <summary>
        /// In case of a validated input, returns Male or Female, otherwise Unknown
        /// </summary>
        public Gender Gender { get; }
        /// <summary>
        /// In case of a validated input, returns a valid date, otherwise returns DateTime.MinValue
        /// </summary>
        public DateTime BirthDate { get; }
        /// <summary>
        /// In case of a validated input, returns whether the id is a birth number, d-number or h-number, otherwise unknown
        /// </summary>
        public NorwegianPersonalIdType NorwegianPersonalIdType { get; }

        /// <summary>
        /// Creates a NorwegianPersonalId object
        /// </summary>
        /// <param name="norwegianIdString">Any Norwegian personal id string with or without divider (dash) between date and individual number</param>
        /// <example><code>
        /// var id = new NorwegianPersonalId("11111111111");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The personal id validates");
        ///     Console.WriteLine("The person was born " + id.BirthDate.ToString());
        ///     if (id.Gender == Gender.Male)
        ///         Console.WriteLine("The person is a male");
        ///     else
        ///         Console.WriteLine("The person is a female");
        /// }
        /// </code></example>
        public NorwegianPersonalId(string norwegianIdString)
        {
            IsValid = false;
            Gender = Gender.Unknown;
            BirthDate = DateTime.MinValue;
            NorwegianPersonalIdType = NorwegianPersonalIdType.Unknown;
            if (string.IsNullOrEmpty(norwegianIdString)) return;
            var reg = Regex.Match(norwegianIdString, @"^(?<d1>[0-7])(?<d2>[0-9])(?<m1>[0-5])(?<m2>[0-9])(?<y3>[0-9])(?<y4>[0-9])-?(?<i1>\d)(?<i2>\d)(?<i3>\d)(?<c1>\d)(?<c2>\d)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            // ^ --> beginning of line
            // (?<d1>[0-3]) --> first day digit of days 01-31 (d-no 41-71)
            // (?<d2>[0-9]) --> second day digit of days 01-31
            // (?<m1>[0-1]) --> first month digit of months 01-12 (h-no 41-52)
            // (?<m2>[0-9]) --> second month digit of months 01-12
            // (?<y3>[0-9]) --> third year digit
            // (?<y4>[0-9]) --> fourth year digit
            // -? --> optional divider between date part and individual part
            // (?<i1>\d) --> first digit in individual number
            // (?<i2>\d) --> second digit in individual number
            // (?<i3>\d) --> third digit in individual number
            // (?<c1>\d) --> first digit in control number
            // (?<c2>\d) --> first digit in control number
            // $ --> end of line
            if (!reg.Success) return;
            var d1 = int.Parse(reg.Groups["d1"].Value); // day 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // day 2
            var m1 = int.Parse(reg.Groups["m1"].Value); // month 1
            var m2 = int.Parse(reg.Groups["m2"].Value); // month 2
            var y3 = int.Parse(reg.Groups["y3"].Value); // year 1
            var y4 = int.Parse(reg.Groups["y4"].Value); // year 2
            var i1 = int.Parse(reg.Groups["i1"].Value); // individual 1
            var i2 = int.Parse(reg.Groups["i2"].Value); // individual 2
            var i3 = int.Parse(reg.Groups["i3"].Value); // individual 3
            var c1 = int.Parse(reg.Groups["c1"].Value); // control 1
            var c2 = int.Parse(reg.Groups["c2"].Value); // control 2
            var r1 = ((d1 * 3) + (d2 * 7) + (m1 * 6) + m2 + (y3 * 8) + (y4 * 9) + (i1 * 4) + (i2 * 5) + (i3 * 2)) % 11; // result 1
            var r2 = ((d1*5) + (d2*4) + (m1*3) + (m2*2) + (y3*7) + (y4*6) + (i1*5) + (i2*4) + (i3*3) + (c1*2))%11;// result 2
            int s1; // sum 1 --> control 1
            int s2; // sum 2 --> control 2
            if (r1 == 0)
                s1 = 0;
            else
                s1 = 11 - r1;
            if (r2 == 0)
                s2 = 0;
            else
                s2 = 11 - r2;
            // The control digits and sum must match or else you must have a typo
            if (s1 != c1 || s2 != c2) return;
            // The kind of number can be defined by the presence of a added 4 to either first digit of month or day, otherwise birth number
            // In the odd case someone tries entering a combination of d and h number, the date will not be validated below
            if (d1 >= 4 && d1 <= 7)
            {
                NorwegianPersonalIdType = NorwegianPersonalIdType.DNumber;
                d1 -= 4;
            }
            else if (d1 >= 8)
            {
                NorwegianPersonalIdType = NorwegianPersonalIdType.FHNumber;
                IsValid = true;
                return; // no birthdate or gender can be extrapolated from fh-numbers
            }
            else if (m1 >= 4 && m1 <= 5)
            {
                NorwegianPersonalIdType = NorwegianPersonalIdType.HNumber;
                m1 -= 4;
            }
            else
                NorwegianPersonalIdType = NorwegianPersonalIdType.BirthNumber;
            // The gender can be determined by the third individual number, odd digit is male, even is female
            Gender = i3%2 == 0 ? Gender.Female : Gender.Male;
            // We only have the last two digits in the year element, we get the first two digits using the following table
            // Individual number  Years (y3, y4)   Century
            // 500 - 749          > 54             1855 - 1899
            // 000 - 499                           1900 - 1999
            // 900 - 999          > 39             1940 - 1999
            // 500 - 999          < 40             2000 - 2039
            var i = int.Parse($"{i1}{i2}{i3}");
            var y = int.Parse($"{y3}{y4}");
            if (i >= 500 && i <= 749 && y >= 55)
            {
                y += 1800;
            }
            else if ((i <= 499) || (i >= 900 && y >= 40))
            {
                y += 1900;
            }
            else
            {
                y += 2000;
            }
            // The date should parse to a valid DateTime object
            if (!DateTime.TryParseExact($"{y}{m1}{m2}{d1}{d2}", "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var bDate)) return;
            BirthDate = bDate;
            IsValid = true;
        }
    }
}
