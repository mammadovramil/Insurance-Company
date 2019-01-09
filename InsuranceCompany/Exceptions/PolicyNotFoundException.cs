using System;

namespace InsuranceCompany.Exceptions
{
    public class PolicyNotFoundException : Exception
    {
        public PolicyNotFoundException()
        {
        }

        public PolicyNotFoundException(string message) : base(message)
        {
        }

        public PolicyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
