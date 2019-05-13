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
        Unknown,
        BirthNumber,
        DNumber,
        HNumber,
        // ReSharper disable once InconsistentNaming
        FHNumber
    }
}
