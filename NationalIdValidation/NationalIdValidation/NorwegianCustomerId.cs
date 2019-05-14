using System.Text.RegularExpressions;

namespace NationalIdValidation
{
    /// <summary>
    /// Class for input validating Norwegian bank KID (Customer Id)
    /// </summary>
    public class NorwegianCustomerId
    {
        /// <summary>
        /// Returns whether the string input was validated
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Returns the used validation routine
        /// </summary>
        public CustomerIdValidationRoutine ValidationRoutine { get; }

        /// <summary>
        /// Creates a NorwegianCustomerId object
        /// </summary>
        /// <param name="kidString">Any Norwegian KID (Customer ID)</param>
        /// <example><code>
        /// var id = new NorwegianCustomerId("123456782");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The KID validates using " + id.ValidationRoutine.ToString());
        /// }
        /// </code></example>
        /// <example><code>
        /// var id = new NorwegianCustomerId("123456785");
        /// if (id.IsValid) {
        ///     Console.WriteLine("The KID validates using " + id.ValidationRoutine.ToString());
        /// }
        /// </code></example>
        public NorwegianCustomerId(string kidString)
        {
            ValidationRoutine = CustomerIdValidationRoutine.Modulus10;
            IsValid = false;
            if (string.IsNullOrEmpty(kidString)) return;
            var regMod10 = Regex.Match(kidString, @"^\d{2,25}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            var regMod11 = Regex.Match(kidString, @"^\d{2,24}(\d|-)?$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (!regMod10.Success && !regMod11.Success) return;
            if (regMod10.Success && IsValidModulus10(kidString))
            {
                ValidationRoutine = CustomerIdValidationRoutine.Modulus10;
                IsValid = true;
            }
            else if (regMod11.Success && IsValidModulus11(kidString))
            {
                ValidationRoutine = CustomerIdValidationRoutine.Modulus11;
                IsValid = true;
            }
        }

        /// <summary>
        /// Creates a NorwegianCustomerId object
        /// </summary>
        /// <param name="kidString">Any Norwegian KID (Customer ID)</param>
        /// <param name="validationRoutine">Whether KID should be checked using Modulo 10 or Modulo 11 routine</param>
        /// <example><code>
        /// var id = new NorwegianCustomerId("123456782", CustomerIdValidationRoutine.Modulo10);
        /// if (id.IsValid) {
        ///     Console.WriteLine("The KID validates");
        /// }
        /// </code></example>
        /// <example><code>
        /// var id = new NorwegianCustomerId("123456785", CustomerIdValidationRoutine.Modulo11);
        /// if (id.IsValid) {
        ///     Console.WriteLine("The KID validates");
        /// }
        /// </code></example>
        public NorwegianCustomerId(string kidString, CustomerIdValidationRoutine validationRoutine)
        {
            IsValid = false;
            ValidationRoutine = validationRoutine;
            if (string.IsNullOrEmpty(kidString)) return;
            switch (validationRoutine)
            {
                case CustomerIdValidationRoutine.Modulus10:
                {
                    var reg = Regex.Match(kidString, @"^\d{2,25}$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    if (!reg.Success) return;
                    break;
                }
                case CustomerIdValidationRoutine.Modulus11:
                {
                    var reg = Regex.Match(kidString, @"^\d{2,24}(\d|-)?$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    if (!reg.Success) return;
                    break;
                }
                default:
                    return;
            }
            switch (validationRoutine)
            {
                    case CustomerIdValidationRoutine.Modulus10:
                        IsValid = IsValidModulus10(kidString);
                        return;
                    case CustomerIdValidationRoutine.Modulus11:
                        IsValid = IsValidModulus11(kidString);
                        return;
                    default:
                        return;
            }
        }

        private static bool IsValidModulus10(string kidString)
        {
            var length = kidString.Length;
            var c1 = int.Parse(kidString.Substring(length - 1));
            var product = 0;
            var currentMultiplier = 2;
            for (var i = length - 2; i >= 0; i--)
            {
                var thisProduct = int.Parse(kidString.Substring(i, 1)) * currentMultiplier;
                var digits = thisProduct.ToString();
                for (var j = 0; j <= digits.Length -1; j++)
                {
                    product += int.Parse(digits.Substring(j, 1));
                }
                currentMultiplier = currentMultiplier == 2 ? 1 : 2;
            }
            var r1 = product % 10;
            int s1;
            switch (r1)
            {
                case 0:
                    s1 = 0;
                    break;
                default:
                    s1 = 10 - r1;
                    break;
            }
            return s1 == c1;
        }

        private static bool IsValidModulus11(string kidString)
        {
            var length = kidString.Length;
            var c1 = kidString.Substring(length - 1);
            var product = 0;
            var currentMultiplier = 2;
            for (var i = length - 2; i >= 0; i--)
            {
                product += int.Parse(kidString.Substring(i, 1)) * currentMultiplier;
                currentMultiplier++;
                if (currentMultiplier == 8) currentMultiplier = 2;
            }
            var r1 = product % 11;
            string s1;
            switch (r1)
            {
                case 0:
                    s1 = 0.ToString();
                    break;
                case 10:
                    s1 = "-";
                    break;
                default:
                    s1 = (11 - r1).ToString();
                    break;
            }
            return s1 == c1;
        }
    }

    /// <summary>
    /// Norwegian customer id KID is using either Modulus 10 or 11 based on a setting on the bank account
    /// This keeps track of which modulo that validates, and let's you confirm the correct modulo setting
    /// </summary>
    public enum CustomerIdValidationRoutine
    {
        /// <summary>
        /// This was validated using Modulus 10
        /// </summary>
        Modulus10,
        /// <summary>
        /// This was validated using Modulus 11
        /// </summary>
        Modulus11
    }
}
