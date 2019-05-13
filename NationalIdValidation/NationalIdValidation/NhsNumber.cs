using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating NHS numbers and CHI numbers
    /// </summary>
    public class NhsNumber
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// The numbers used also gives where this NHS/CHI number is generated from as NHS coordinates the numbering system across Great Britain
        /// This includes by the time of writing (13.05.2019)
        /// - England, Wales and Isle Of Man (400 000 0000 to 499 999 9999 and 600 000 0000 to 708 800 0019)
        /// - Northern Ireland (320 000 0010 to 399 999 9999)
        /// - Scotland (010 101 0000 to 311 299 9999)
        /// </summary>
        /// <remarks>
        /// If numbers are discovered outside of these ranges, it will no be invalidated as this information may quickly change
        /// In case you want to invalidate numbers outside these ranges, you should also verify that Location is not Unknown
        /// </remarks>
        public NhsNumberLocation Location { get; }

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
            Location = NhsNumberLocation.Unknown;
            IsValid = false;
            if (string.IsNullOrEmpty(nhsNumberString)) return;
            var reg = Regex.Match(nhsNumberString, @"^(?<d1>\d)(?<d2>\d)(?<d3>\d)(\s|-)?(?<d4>\d)(?<d5>\d)(?<d6>\d)(\s|-)?(?<d7>\d)(?<d8>\d)(?<d9>\d)(?<c1>\d)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
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
            var remainder = ((d1*10) + (d2*9) + (d3*8) + (d4*7) + (d5*6) + (d6*5) + (d7*4) + (d8*3) + (d9*2))%11;
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
            var numberResult = long.Parse(d1.ToString() + d2.ToString() + d3.ToString() + d4.ToString() + d5.ToString() + d6.ToString() + d7.ToString() + d8.ToString() + d9.ToString() + c1.ToString());
            if (numberResult >= 4000000000 && numberResult <= 4999999999 || numberResult >= 6000000000 && numberResult <= 7088000019)
            {
                Location = NhsNumberLocation.EnglandWalesAndIsleOfMan;
            }
            else if (numberResult >= 3200000010 && numberResult <= 3999999999)
            {
                Location = NhsNumberLocation.NorthernIreland;
            }
            else if (numberResult >= 0101010000 && numberResult <= 3112999999)
            {
                Location = NhsNumberLocation.Scotland;
            }
            IsValid = true;
        }
    }
}
