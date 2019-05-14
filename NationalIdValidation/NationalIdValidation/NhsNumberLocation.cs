namespace NationalIdValidation
{
    /// <summary>
    /// NHS assigns numbers across great britain to keep a single medical record per patient
    /// This enum will tell which region a NHS number is assigned to
    /// </summary>
    public enum NhsNumberLocation
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// Scotland
        /// </summary>
        Scotland,
        /// <summary>
        /// Northern Ireland
        /// </summary>
        NorthernIreland,
        /// <summary>
        /// England, Wales and Isle of Man
        /// </summary>
        EnglandWalesAndIsleOfMan
    }
}
