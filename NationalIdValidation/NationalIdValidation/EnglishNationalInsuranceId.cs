using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating National Insurance numbers from United Kingdom
    /// </summary>
    public class EnglishNationalInsuranceId
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; private set; }

        public DayOfWeek BenefitsDay { get; private set; }

        /// <summary>
        /// Creates a EnglishNationalInsuranceId object
        /// </summary>
        /// <param name="nationalInsuranceString">Any national insurance number (NINO) string with or without divider (space) between groups</param>
        /// <example><code>
        /// var id = new EnglishNationalInsuranceId("AB 12 34 56 C");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The NINO number validates");
        ///     Console.WriteLine(string.Format("This number is payable for social benefits on day {0} of the week", id.BenefitsDay))
        /// }
        /// </code></example>
        public EnglishNationalInsuranceId(string nationalInsuranceString)
        {
            IsValid = false;
            if (string.IsNullOrEmpty(nationalInsuranceString)) return;
            var reg = Regex.Match(nationalInsuranceString, @"^[ABCEGHJKLMNOPRSTWXYZ][A-NP-Z] ?\d{2} ?\d{2} ?(?<bDay>\d{2}) ?[ABCD]$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!reg.Success) return;
            var benefitsDay = int.Parse(reg.Groups["bDay"].Value);
            if (benefitsDay <= 19)
                    BenefitsDay = DayOfWeek.Monday;
            else if (benefitsDay >= 20 && benefitsDay <= 39)
                BenefitsDay = DayOfWeek.Tuesday;
            else if (benefitsDay >= 40 && benefitsDay <= 59)
                BenefitsDay = DayOfWeek.Wednesday;
            else if (benefitsDay >= 60 && benefitsDay <= 79)
                BenefitsDay = DayOfWeek.Thursday;
            else if (benefitsDay >= 80 && benefitsDay <= 99)
                BenefitsDay = DayOfWeek.Friday;
            IsValid = true;
        }
    }
}
