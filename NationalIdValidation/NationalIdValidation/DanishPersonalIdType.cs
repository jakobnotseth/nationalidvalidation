namespace NationalIdValidation
{
    /// <summary>
    /// The Danish id-convention caters several different numbering systems
    /// The id-type let's you know what id-convention is used
    /// Your application is itself responsible for following other business rules based on type
    /// </summary>
    public enum DanishPersonalIdType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Officially assigned birth number
        /// </summary>
        BirthNumber,
        /// <summary>
        /// Officially assigned replacement number
        /// </summary>
        ReplacementNumber
    }
}
