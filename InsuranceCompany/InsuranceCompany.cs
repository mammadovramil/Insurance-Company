namespace InsuranceCompany
{
    using System;
    using System.Collections.Generic;
    using global::InsuranceCompany.Validations;
    using System.Linq;
    using global::InsuranceCompany.Exceptions;

    public class InsuranceCompany : IInsuranceCompany
    {
        private IDictionary<string, IList<IPolicy>> insuredCompanies;

        public string Name { get; }
        public IList<Risk> AvailableRisks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceCompany"/> class.
        /// <param name="name">Name of Insurance company</param>
        /// </summary>
        public InsuranceCompany(string name)
        {
            Validation.IsNullOrEmptyOrWhiteSpace(name);
            Name = name;
            insuredCompanies = new Dictionary<string, IList<IPolicy>>();
            AvailableRisks = new List<Risk>();
        }

        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
        {
            Validation.IsNullOrEmptyOrWhiteSpace(nameOfInsuredObject);

            Validation.IsNull(risk);
            Validation.IsNullOrEmptyOrWhiteSpace(risk.Name);
            Validation.IsLessThanOrEqualTo(risk.YearlyPrice, 0);

            Validation.IsStartDateAfterEndDate(DateTime.Now.Date, validFrom);

            insuredCompanies.TryGetValue(nameOfInsuredObject, out IList<IPolicy> policesOfInsuredCompany);
            Validation.IsCollectionNullOrEmpty(policesOfInsuredCompany);

            IPolicy policy = policesOfInsuredCompany.FirstOrDefault(p =>
                                    p.NameOfInsuredObject.Equals(nameOfInsuredObject)
                                    && p.ValidFrom <= effectiveDate
                                    && p.ValidTill >= effectiveDate);
            if (policy != null)
            {
                policy.InsuredRisks.Add(risk);
            }
            else
            {
                throw new PolicyNotFoundException($"Can not find active policy with name '{nameOfInsuredObject}' and the effective date '{effectiveDate}'");
            }
        }

        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
        {
            Validation.IsNullOrEmptyOrWhiteSpace(nameOfInsuredObject);

            Validation.IsNull(risk);
            Validation.IsNullOrEmptyOrWhiteSpace(risk.Name);
            Validation.IsLessThanOrEqualTo(risk.YearlyPrice, 0);

            Validation.IsLessThanOrEqualTo(validTill, DateTime.Now);

            insuredCompanies.TryGetValue(nameOfInsuredObject, out IList<IPolicy> policesOfInsuredCompany);
            Validation.IsCollectionNullOrEmpty(policesOfInsuredCompany);

            IPolicy policy = policesOfInsuredCompany.FirstOrDefault(p =>
                                    p.NameOfInsuredObject.Equals(nameOfInsuredObject)
                                    && p.ValidFrom <= effectiveDate &&
                                    p.ValidTill >= effectiveDate);
            if (policy != null)
            {
                var riskForRemove = policy.InsuredRisks.FirstOrDefault(i => i.Name.Equals(risk.Name) && i.YearlyPrice == risk.YearlyPrice);
                if (riskForRemove != null)
                {
                    policy.InsuredRisks.Remove(riskForRemove);
                }
            }
            else
            {
                throw new PolicyNotFoundException($"Can not find active policy with name '{nameOfInsuredObject}' and the effective date '{effectiveDate}'");
            }
        }

        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            Validation.IsNullOrEmptyOrWhiteSpace(nameOfInsuredObject);

            insuredCompanies.TryGetValue(nameOfInsuredObject, out IList<IPolicy> policesOfInsuredCompany);
            if (policesOfInsuredCompany == null)
            {
                throw new PolicyNotFoundException($"Can not find active policy with name '{nameOfInsuredObject}'");
            }

            return policesOfInsuredCompany.FirstOrDefault(p => p.ValidFrom <= effectiveDate && p.ValidTill >= effectiveDate);
        }

        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            Validation.IsNullOrEmptyOrWhiteSpace(nameOfInsuredObject);
            Validation.IsStartDateAfterEndDate(DateTime.Now.Date, validFrom);
            Validation.IsLessThanOrEqualTo(validMonths, 0);
            Validation.IsCollectionNullOrEmpty(selectedRisks);

            IList<Risk> insertingRisk = selectedRisks.Where(sr => AvailableRisks.Any(ar => sr.Name.Equals(ar.Name) && sr.YearlyPrice == ar.YearlyPrice)).ToList();
            Validation.IsCollectionNullOrEmpty(insertingRisk);

            IPolicy policy = new Policy
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = validFrom,
                ValidTill = validFrom.AddMonths(validMonths),
                InsuredRisks = insertingRisk
            };

            insuredCompanies.TryGetValue(nameOfInsuredObject, out IList<IPolicy> policesOfInsuredCompany);
            if (policesOfInsuredCompany == null)
            {
                insuredCompanies.Add(nameOfInsuredObject, new List<IPolicy>
                   {
                       policy
                   });
                return policy;
            }

            if (policesOfInsuredCompany.Any(p => policy.ValidFrom.CompareTo(p.ValidTill) > 0 || policy.ValidFrom.CompareTo(p.ValidFrom) < 0 &&
                                                 policy.ValidTill.CompareTo(p.ValidTill) > 0 || policy.ValidTill.CompareTo(p.ValidFrom) < 0))
            {
                policesOfInsuredCompany.Add(policy);
                return policy;
            }

            throw new PolicyExistInThisDateException();
        }
    }
}
