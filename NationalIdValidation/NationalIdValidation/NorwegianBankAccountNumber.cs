using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Norwegian bank account number using CDV 11
    /// </summary>
    public class NorwegianBankAccountNumber
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Creates a NorwegianAccountNumber object
        /// </summary>
        /// <param name="accountNumberString">Any Norwegian bank account number with optional spaces/periods between groups (4-2-5)</param>
        /// <example><code>
        /// var id = new NorwegianAccountNumber("1234 10 56789");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The account number validates");
        /// }
        /// </code></example>
        public NorwegianBankAccountNumber(string accountNumberString)
        {
            IsValid = false;
            if (string.IsNullOrEmpty(accountNumberString)) return;
            var reg = Regex.Match(accountNumberString, @"^(?<d1>\d)(?<d2>\d)(?<d3>\d)(?<d4>\d)( |.)?(?<d5>\d)(?<d6>\d)( |.)?(?<d7>\d)(?<d8>\d)(?<d9>\d)(?<d10>\d)(?<c1>\d)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            // ^ --> beginning of line
            // (?<d1>\d) --> first digit
            // (?<d2>\d) --> second digit
            // (?<d3>\d) --> third digit
            // (?<d4>\d) --> fourth digit
            // ( |.)? --> optional divider between first and second group of digits
            // (?<d5>\d) --> fifth digit
            // (?<d6>\d) --> sixth digit
            // ( |.)? --> optional divider between second and third group of digits
            // (?<d7>\d) --> seventh digit
            // (?<d8>\d) --> eighth digit
            // (?<d9>\d) --> ninth digit
            // (?<d10>\d) --> tenth digit
            // (?<c1>\d) --> control digit
            // $ --> end of line
            if (!reg.Success) return;
            var d1 = int.Parse(reg.Groups["d1"].Value); // digit 1
            var d2 = int.Parse(reg.Groups["d2"].Value); // digit 2
            var d3 = int.Parse(reg.Groups["d3"].Value); // digit 3
            var d4 = int.Parse(reg.Groups["d4"].Value); // digit 4
            var d5 = int.Parse(reg.Groups["d5"].Value); // digit 5
            var d6 = int.Parse(reg.Groups["d6"].Value); // digit 6
            var d7 = int.Parse(reg.Groups["d7"].Value); // digit 7
            var d8 = int.Parse(reg.Groups["d8"].Value); // digit 8
            var d9 = int.Parse(reg.Groups["d9"].Value); // digit 9
            var d10 = int.Parse(reg.Groups["d10"].Value); // digit 10
            var c1 = int.Parse(reg.Groups["c1"].Value); // control 1
            if (d5 == 0 && d6 == 0)
            {
                // cannot validate account group 00
                IsValid = true;
                return;
            }
            var r1 = (d1*5 + d2*4 + d3*3 + d4*2 + d5*7 + d6*6 + d7*5 + d8*4 + d9*3 + d10*2)%11; // result 1
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
        }
    }
}
