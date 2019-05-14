namespace NationalIdValidation
{
    /// <summary>
    /// The norwegian id-convention caters several different numberings
    /// The id-type let's you know what id-convention is used
    /// Your application is itself responsible for following other business rules based on type
    /// </summary>
    /// <remarks>
    /// HNumber type is only valid if you also have information of the issuer of the id-number
    /// </remarks>
    /// <remarks>
    /// FHNumber and HNumber is only valid within the Norwegian healthcare sections
    /// </remarks>
    public enum NorwegianPersonalIdType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// BirthNumber is an official Norwegian identity
        /// </summary>
        BirthNumber,
        /// <summary>
        /// DNumber is an official Norwegian identity for immigrants and foreign tax payers in Norway
        /// </summary>
        DNumber,
        /// <summary>
        /// HNumber is a temporary identity number used in healthcare, invalid without information about the issuing company
        /// </summary>
        HNumber,
        /// <summary>
        /// FHNumber is an officially reserved number for healthcare on a unknown patient or foreign national without other identification methods
        /// </summary>
        // ReSharper disable once InconsistentNaming
        FHNumber
    }
}
