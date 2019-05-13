using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Norwegian company ID-numbers
    /// </summary>
    public class NorwegianCompanyId
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Returns whether the string specified tax registration
        /// </summary>
        /// <remarks>
        /// A company might still be tax registered even if not specified in the company ID string
        /// </remarks>
        public bool VatRegistered { get; }

        /// <summary>
        /// Creates a NorwegianCompanyId object
        /// </summary>
        /// <param name="companyIdString">Any Norwegian company id string with or without divider (space) between the groups of three</param>
        /// <example><code>
        /// var id = new NorwegianPersonalId("NO-974760673MVA");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The company id validates");
        /// }
        /// </code></example>
        public NorwegianCompanyId(string companyIdString)
        {
            IsValid = false;
            VatRegistered = false;
            if (string.IsNullOrEmpty(companyIdString)) return;
            var reg = Regex.Match(companyIdString, @"^(?<ISO>\w{2})?(\s|-)?(?<d1>\d)(?<d2>\d)(?<d3>\d)(\s|-)?(?<d4>\d)(?<d5>\d)(?<d6>\d)(\s|-)?(?<d7>\d)(?<d8>\d)(?<c1>\d)(\s|-)?(?<VAT>MVA)?$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            // ^ --> beginning of line
            // (?<ISO>\w{2})? --> optional ISO 3166 alpha 2 for Norwegian with optional divider
            // (\s|-)? --> optional divider between country code and digits
            // (?<d1>\d) --> first digit
            // (?<d2>\d) --> second digit
            // (?<d3>\d) --> third digit
            // (\s|-)? --> optional divider between first and second group of three digits
            // (?<d4>\d) --> fourth digit
            // (?<d5>\d) --> fifth digit
            // (?<d6>\d) --> sixth digit
            // (\s|-)? --> optional divider between second and third group of three digits
            // (?<d7>\d) --> seventh digit
            // (?<d8>\d) --> eighth digit
            // (?<c1>\d) --> control digit
            // (\s|-)? --> optional divider between third group of three digits and tax registration
            // (?<VAT>MVA)? --> optional taxation registration
            // $ --> end of line
            if (!reg.Success) return;
            if (reg.Groups["ISO"].Success)
            {
                var iso = reg.Groups["ISO"].Value;
                if (iso != "NO") return;
            }
            var d1 = int.Parse(reg.Groups["d1"].Value); // digit 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // digit 2
            var d3 = int.Parse(reg.Groups["d3"].Value); // digit 3
            var d4 = int.Parse(reg.Groups["d4"].Value); // digit 4
            var d5 = int.Parse(reg.Groups["d5"].Value); // digit 5
            var d6 = int.Parse(reg.Groups["d6"].Value); // digit 6
            var d7 = int.Parse(reg.Groups["d7"].Value); // digit 7
            var d8 = int.Parse(reg.Groups["d8"].Value); // digit 8
            var c1 = int.Parse(reg.Groups["c1"].Value); // control 1
            var r1 = ((d1 * 3) + (d2 * 2) + (d3 * 7) + (d4 * 6) + (d5 * 5) + (d6 * 4) + (d7 * 3) + (d8 * 2)) % 11; // result 1
            int s1;
            switch (r1)
            {
                case 0:
                    s1 = 0;
                    break;
                case 10:
                    // s1 = "-";
                    return;
                default:
                    s1 = 11 - r1;
                    break;
            }
            if (s1 != c1) return;
            IsValid = true;
            if (!reg.Groups["VAT"].Success) return;
            if (reg.Groups["VAT"].Value == "MVA") VatRegistered = true;
        }
    }
}
