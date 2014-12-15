using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Finnish personal ID-numbers (Finnish: Henkilötunnus (HETU), Swedish: Personbeteckning)
    /// </summary>
    public class FinnishPersonalId
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
        /// Creates a FinnishPersonalId object
        /// </summary>
        /// <param name="finnishIdString">Any Finnish personal id string</param>
        /// <example><code>
        /// var id = new DanishPersonalId("111111-111Y");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The personal id validates");
        ///     Console.WriteLine("The person was born " + id.BirthDate.ToString());
        ///     if (id.Gender == Gender.Male)
        ///         Console.WriteLine("The person is a male");
        ///     else
        ///         Console.WriteLine("The person is a female");
        /// }
        /// </code></example>
        public FinnishPersonalId(string finnishIdString)
        {
            IsValid = false;
            Gender = Gender.Unknown;
            BirthDate = DateTime.MinValue;
            if (string.IsNullOrEmpty(finnishIdString)) return;
            var reg = Regex.Match(finnishIdString, @"^(?<d1>[0-3])(?<d2>[0-9])(?<m1>[0-1])(?<m2>[0-9])(?<y3>[0-9])(?<y4>[0-9])(?<divider>[+-A])(?<i1>\d)(?<i2>\d)(?<i3>\d)(?<c1>[\dA-Y])$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!reg.Success) return;
            var d1 = int.Parse(reg.Groups["d1"].Value); // day 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // day 2
            var m1 = int.Parse(reg.Groups["m1"].Value); // month 1
            var m2 = int.Parse(reg.Groups["m2"].Value); // month 2
            var y3 = int.Parse(reg.Groups["y3"].Value); // year 1
            var y4 = int.Parse(reg.Groups["y4"].Value); // year 2
            var divider = reg.Groups["divider"].Value; // divider
            var i1 = int.Parse(reg.Groups["i1"].Value); // individual 1
            var i2 = int.Parse(reg.Groups["i2"].Value); // individual 2
            var i3 = int.Parse(reg.Groups["i3"].Value); // individual 3
            var c1 = reg.Groups["c1"].Value; // control 1
            var sum = int.Parse(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", d1, d2, m1, m2, y3, y4, i1, i2, i3)) / 31;
            var controls = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "H", "J", "K", "L", "M", "N", "P", "R", "S", "T", "U", "V", "W", "X", "Y" };
            if (c1 != controls[sum]) return;
            Gender = i3 % 2 == 0 ? Gender.Female : Gender.Male;
            var y = int.Parse(string.Format("{0}{1}", y3, y4));
            if (divider == "-")
            {
                y += 1800;
            }
            else if (divider == "+")
            {
                y += 1900;
            }
            else if (divider == "A")
            {
                y += 2000;
            }
            else return;
            // The date should parse to a valid DateTime object
            DateTime bDate;
            if (!DateTime.TryParseExact(string.Format("{0}{1}{2}{3}{4}", y, m1, m2, d1, d2), "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out bDate)) return;
            BirthDate = bDate;
            IsValid = true;
        }
    }
}
