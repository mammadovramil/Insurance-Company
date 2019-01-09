namespace InsuranceCompany
{
    /// <summary>
    /// The model of risk
    /// </summary>
    public class Risk
    {
        /// <summary>
        /// Unique name of the risk
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Risk yearly price
        /// </summary>
        public decimal YearlyPrice { get; set; }
    }
}