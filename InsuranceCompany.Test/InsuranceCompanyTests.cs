using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InsuranceCompany.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceCompany.Test
{
    [TestClass]
    public class InsuranceCompanyTests
    {
        string companyName;
        string policyName;
        IList<Risk> listOfRisks;

        [TestInitialize]
        public void TestInit()
        {
            companyName = "Insurance Company 1";

            policyName = "Policy 1";

            listOfRisks = new List<Risk>
            {
                new Risk
                {
                    Name = "Risk 1",
                    YearlyPrice = 100,
                },
                new Risk
                {
                    Name = "Risk 2",
                    YearlyPrice = 200,
                },
                new Risk
                {
                    Name = "Risk 3",
                    YearlyPrice = 300
                },
                new Risk
                {
                    Name = "Risk 4",
                    YearlyPrice = 400
                }
            };
        }

        [TestMethod]
        public void Creating_New_Insurance_Company_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            Assert.IsNotNull(insuranceCompany);
            Assert.AreEqual(insuranceCompany.Name, companyName);
            Assert.IsNotNull(insuranceCompany.AvailableRisks);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Creating_New_Null_Insurance_Company_Should_Be_ArgumentNullException()
        {
            InsuranceCompany insuranceCompane = new InsuranceCompany(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_New_Empty_Insurance_Company_Should_Be_ArgumentException()
        {
            InsuranceCompany insuranceCompane = new InsuranceCompany("");
        }

        [TestMethod]
        public void SellPolicy_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(9));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);
        }

        [TestMethod]
        public void SellPolicy_ValidFrom_In_Past_Should_Be_ArgumentOutOfRangeException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            DateTime validFrom = DateTime.Now.AddDays(-1);
            short validMonth = 9;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks));
        }

        [TestMethod]
        public void SellPolicy_With_Zero_ValidMonth_Should_Be_ArgumentOutOfRangeException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => insuranceCompany.SellPolicy(policyName, DateTime.Now, 0, listOfRisks));
        }

        [TestMethod]
        public void SellPolicy_With_Null_Name_Should_Be_ArgumentNullException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            
            Assert.ThrowsException<ArgumentNullException>(() => insuranceCompany.SellPolicy(null, DateTime.Now, 9, listOfRisks));
        }

        [TestMethod]
        public void SellPolicy_With_Empty_Name_Should_Be_ArgumentException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            
            Assert.ThrowsException<ArgumentException>(() => insuranceCompany.SellPolicy("", DateTime.Now, 9, listOfRisks));
        }

        [TestMethod]
        public void SellPolicy_With_Null_Risks_Should_Be_ArgumentOutOfRangeException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => insuranceCompany.SellPolicy(policyName, DateTime.Now, 0, null));
        }

        [TestMethod]
        public void SellPolicy_With_Empty_Risks_Should_Be_ArgumentOutOfRangeException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => insuranceCompany.SellPolicy(policyName, DateTime.Now, 0, new List<Risk>()));
        }

        [TestMethod]
        public void SellPolicy_With_Existed_Policy_Should_be_PolicyExistInThisDateException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);

            validFrom = validFrom.AddMonths(7);

            Assert.ThrowsException<PolicyExistInThisDateException>(() => insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks));

        }

        [TestMethod]
        public void Sell_Policy_With_Existed_Policy_Check_ValidMonth_Should_be_PolicyExistInThisDateException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now.AddMonths(2);
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);

            validFrom = validFrom.AddMonths(-1);

            Assert.ThrowsException<PolicyExistInThisDateException>(() => insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks));
        }

        [TestMethod]
        public void SellPolicy_With_Existed_Policy_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);

            validFrom = validFrom.AddMonths(10);

            policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);
        }

        [TestMethod]
        public void GetPolicy_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);

            policy = insuranceCompany.GetPolicy(policyName, validFrom.AddMonths(2));

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.Premium, listOfRisks.Sum(s => s.YearlyPrice));
            Assert.AreEqual(policy.ValidFrom, validFrom);
            Assert.AreEqual(policy.ValidTill, validFrom.AddMonths(validMonth));
            Assert.AreEqual(policy.NameOfInsuredObject, policyName);
        }

        [TestMethod]
        [ExpectedException(typeof(PolicyNotFoundException))]
        public void GetPolicy_With_Empty_Policy_Should_Be_PolicyNotFoundException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            IPolicy policy = insuranceCompany.GetPolicy(policyName, DateTime.Now.AddMonths(-2));
            insuranceCompany.GetPolicy(null, DateTime.Now);
        }

        [TestMethod]
        public void GetPolicy_With_Null_Name_Should_Be_ArgumentNullException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            Assert.ThrowsException<ArgumentNullException>(() => insuranceCompany.GetPolicy(null, DateTime.Now));
        }

        [TestMethod]
        public void GetPolicy_With_Empty_Name_Should_Be_ArgumentException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);

            Assert.ThrowsException<ArgumentException>(() => insuranceCompany.GetPolicy("", DateTime.Now));
        }

        [TestMethod]
        public void RemoveRisk_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            insuranceCompany.RemoveRisk(policyName, listOfRisks.First(), validFrom.AddMonths(validMonth), DateTime.Now);

            policy = insuranceCompany.GetPolicy(policyName, validFrom.AddMonths(2));

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.InsuredRisks.Count, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(PolicyNotFoundException))]
        public void RemoveRisk_With_Incorrect_Effective_Date_Should_Be_PolicyNotFoundException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            insuranceCompany.RemoveRisk(policyName, listOfRisks.First(), validFrom.AddMonths(validMonth), DateTime.Now.AddMonths(-2));
        }

        [TestMethod]
        public void AddRisk_Should_Be_Correct()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            insuranceCompany.AddRisk(policyName, new Risk
            {
                Name = "Risk 5",
                YearlyPrice = 500
            },
            validFrom, DateTime.Now);

            policy = insuranceCompany.GetPolicy(policyName, validFrom.AddMonths(2));

            Assert.IsNotNull(policy);
            Assert.AreEqual(policy.InsuredRisks.Count, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(PolicyNotFoundException))]
        public void AddRisk_With_Incorrect_Effective_Date_Should_Be_PolicyNotFoundException()
        {
            InsuranceCompany insuranceCompany = new InsuranceCompany(companyName);
            insuranceCompany.AvailableRisks = listOfRisks;

            DateTime validFrom = DateTime.Now;
            short validMonth = 9;

            IPolicy policy = insuranceCompany.SellPolicy(policyName, validFrom, validMonth, listOfRisks);

            insuranceCompany.AddRisk(policyName, new Risk
            {
                Name = "Risk 5",
                YearlyPrice = 500
            },
            validFrom, DateTime.Now.AddMonths(-2));
        }
    }
}
