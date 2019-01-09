namespace InsuranceCompany
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Policy : IPolicy
    {
        public string NameOfInsuredObject { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public decimal Premium { get => InsuredRisks.Sum(r => r.YearlyPrice); }
        public IList<Risk> InsuredRisks { get; set; }
    }
}
