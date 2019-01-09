namespace InsuranceCompany.Exceptions
{
    using System;

    public class PolicyExistInThisDateException : Exception
    {
        public PolicyExistInThisDateException()
        {
        }

        public PolicyExistInThisDateException(string message) : base(message)
        {
        }

        public PolicyExistInThisDateException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
