namespace NationalIdValidation
{
    /// <summary>
    /// The Swedish id-convention caters several different numbering systems
    /// The id-type let's you know what id-convention is used
    /// Your application is itself responsible for following other business rules based on type
    /// </summary>
    public enum SwedishPersonalIdType
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
        /// Officially assigned coordination number
        /// </summary>
        CoordinationNumber,
        /// <summary>
        /// Officially assigned organization number
        /// </summary>
        /// <remarks>Added here for completeness as this uses the same algorithm as the personal ids</remarks>
        OrganizationNumber
    }
}
