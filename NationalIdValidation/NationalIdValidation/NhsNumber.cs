using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating NHS numbers
    /// </summary>
    public class NhsNumber
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Creates a NhsNumber object
        /// </summary>
        /// <param name="nhsNumberString">Any NHS number string with or without divider (dash) between groups</param>
        /// <example><code>
        /// var id = new NhsNumber("401-023-2137");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The NHS number validates");
        /// }
        /// </code></example>
        public NhsNumber(string nhsNumberString)
        {
            IsValid = false;
            if (string.IsNullOrEmpty(nhsNumberString)) return;
            var reg = Regex.Match(nhsNumberString, @"^(?<d1>\d)(?<d2>\d)(?<d3>\d)-?(?<d4>\d)(?<d5>\d)(?<d6>\d)-?(?<d7>\d)(?<d8>\d)(?<d9>\d)(?<c1>\d)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!reg.Success) return;
            var d1 = int.Parse(reg.Groups["d1"].Value); // digit 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // digit 1
            var d3 = int.Parse(reg.Groups["d3"].Value); // digit 1
            var d4 = int.Parse(reg.Groups["d4"].Value); // digit 1
            var d5 = int.Parse(reg.Groups["d5"].Value); // digit 1
            var d6 = int.Parse(reg.Groups["d6"].Value); // digit 1
            var d7 = int.Parse(reg.Groups["d7"].Value); // digit 1
            var d8 = int.Parse(reg.Groups["d8"].Value); // digit 1
            var d9 = int.Parse(reg.Groups["d9"].Value); // digit 1
            var c1 = int.Parse(reg.Groups["c1"].Value); // control 1
            if (d1 == d2 && d2 == d3 && d3 == d4 && d4 == d5 && d5 == d6 && d6 == d7 && d7 == d8 && d8 == d9) return;
            var remainder = ((d1*10) + (d2*9) + (d3*8) + (d4*7) + (d5*6) + (d6*5) + (d7*4) + (d8*3) + (d9*2)%11);
            var control = 11 - remainder;
            switch (control)
            {
                case 10:
                    return;
                case 11:
                    control = 0;
                    break;
            }
            if (c1 != control) return;
            IsValid = true;
        }
    }
}
