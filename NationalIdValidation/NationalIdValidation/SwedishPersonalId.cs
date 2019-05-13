using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Swedish personal ID-numbers
    /// </summary>
    public class SwedishPersonalId
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
        /// In case of a validated input, returns whether the id is a birth number, coordination number or organization number, otherwise unknown
        /// </summary>
        public SwedishPersonalIdType SwedishPersonalIdType { get; }

        /// <summary>
        /// Creates a SwedishPersonalId object
        /// </summary>
        /// <param name="swedishIdString">Any Swedish personal id</param>
        /// <example><code>
        /// var id = new SwedishPersonalId("111111-1111");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The personal id validates");
        ///     Console.WriteLine("The person was born " + id.BirthDate.ToString());
        ///     if (id.Gender == Gender.Male)
        ///         Console.WriteLine("The person is a male");
        ///     else
        ///         Console.WriteLine("The person is a female");
        /// }
        /// </code></example>
        public SwedishPersonalId(string swedishIdString)
        {
            IsValid = false;
            Gender = Gender.Unknown;
            BirthDate = DateTime.MinValue;
            SwedishPersonalIdType = SwedishPersonalIdType.Unknown;
            if (string.IsNullOrEmpty(swedishIdString)) return;
            var reg = Regex.Match(swedishIdString,
                @"^(?<y3>[0-9])(?<y4>[0-9])(?<m1>[0-9])(?<m2>[0-9])(?<d1>[0-9])(?<d2>[0-9])(?<divider>[+-])(?<i1>\d)(?<i2>\d)(?<i3>\d)(?<c1>\d)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            // ^ --> beginning of line
            // (?<y3>[0-9]) --> third year digit
            // (?<y4>[0-9]) --> fourth year digit
            // (?<m1>[0-9]) --> first month digit of months 01-12 (organization no 21-32)
            // (?<m2>[0-9]) --> second month digit of months 01-12
            // (?<d1>[0-9]) --> first day digit of days 01-31 (co-no 61-91)
            // (?<d2>[0-9]) --> second day digit of days 01-31
            // (?<divider>[+-]) --> divider between date part and individual part
            // (?<i1>\d) --> first digit in individual number
            // (?<i2>\d) --> second digit in individual number
            // (?<i3>\d) --> third digit in individual number
            // (?<c1>\d) --> control number
            // $ --> end of line
            if (!reg.Success) return;
            var y3 = int.Parse(reg.Groups["y3"].Value); // year 1
            var y4 = int.Parse(reg.Groups["y4"].Value); // year 2
            var m1 = int.Parse(reg.Groups["m1"].Value); // month 1
            var m2 = int.Parse(reg.Groups["m2"].Value); // month 2
            var d1 = int.Parse(reg.Groups["d1"].Value); // day 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // day 2
            var divider = reg.Groups["divider"].Value; // divider
            var i1 = int.Parse(reg.Groups["i1"].Value); // individual 1
            var i2 = int.Parse(reg.Groups["i2"].Value); // individual 2
            var i3 = int.Parse(reg.Groups["i3"].Value); // individual 3
            var c1 = int.Parse(reg.Groups["c1"].Value); // control 1
            var r1 = y3*2;
            if (r1 >= 10)
            {
                var s = r1.ToString(CultureInfo.InvariantCulture);
                var part1 = int.Parse(s.Substring(0, 1));
                var part2 = int.Parse(s.Substring(1, 1));
                r1 = part1 + part2;
            }
            var r3 = m1*2;
            if (r3 >= 10)
            {
                var s = r3.ToString(CultureInfo.InvariantCulture);
                var part1 = int.Parse(s.Substring(0, 1));
                var part2 = int.Parse(s.Substring(1, 1));
                r3 = part1 + part2;
            }
            var r5 = d1*2;
            if (r5 >= 10)
            {
                var s = r5.ToString(CultureInfo.InvariantCulture);
                var part1 = int.Parse(s.Substring(0, 1));
                var part2 = int.Parse(s.Substring(1, 1));
                r5 = part1 + part2;
            }
            var r7 = i1*2;
            if (r7 >= 10)
            {
                var s = r7.ToString(CultureInfo.InvariantCulture);
                var part1 = int.Parse(s.Substring(0, 1));
                var part2 = int.Parse(s.Substring(1, 1));
                r7 = part1 + part2;
            }
            var r9 = i3*2;
            if (r9 >= 10)
            {
                var s = r9.ToString(CultureInfo.InvariantCulture);
                var part1 = int.Parse(s.Substring(0, 1));
                var part2 = int.Parse(s.Substring(1, 1));
                r9 = part1 + part2;
            }
            var sum = r1 + y4 + r3 + m2 + r5 + d2 + r7 + i2 + r9;
            var sumString = sum.ToString(CultureInfo.InvariantCulture);
            sum = int.Parse(sumString.Substring(sumString.Length - 1));
            if (c1 != 10 - sum) return;
            if (d1 >= 6)
            {
                SwedishPersonalIdType = SwedishPersonalIdType.CoordinationNumber;
                d1 -= 6;
            }
            else if (m1 >= 2)
            {
                SwedishPersonalIdType = SwedishPersonalIdType.OrganizationNumber;
                IsValid = true;
                return;
            }
            else
                SwedishPersonalIdType = SwedishPersonalIdType.BirthNumber;
            Gender = i3 % 2 == 0 ? Gender.Female : Gender.Male;
            var y = int.Parse($"{y3}{y4}");
            int cent;
            var thisYear = DateTime.Now.Year;
            if (divider == "+")
            {
                // person is more than or equal to 100 years
                if (y > int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(2, 2)))
                {
                    cent = int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(0, 2)) - 2;
                }
                else
                {
                    cent = int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(0, 2)) - 1;
                }
            }
            else
            {
                // person is less than 100 years
                if (y > int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(2, 2)))
                {
                    cent = int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(0, 2)) - 1;
                }
                else
                {
                    cent = int.Parse(thisYear.ToString(CultureInfo.InvariantCulture).Substring(0, 2));
                }
            }
            y += int.Parse(cent.ToString(CultureInfo.InvariantCulture) + "00");
            // The date should parse to a valid DateTime object
            if (!DateTime.TryParseExact($"{y}{m1}{m2}{d1}{d2}", "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var bDate)) return;
            BirthDate = bDate;
            IsValid = true;
        }
    }
}
