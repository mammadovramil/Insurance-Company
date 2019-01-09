namespace InsuranceCompany.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Validation
    {
        /// <summary>Throws if a value null or empty or whitespace</summary>
        /// <param name="value">string value</param>
        public static void IsNullOrEmptyOrWhiteSpace(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Can not be null.", nameof(value));
            }

            if (value.Length == 0 || value.Trim().Length == 0)
            {
                throw new ArgumentException("Can not be empty or whitespace.", nameof(value));
            }
        }

        /// <summary>Throws if a parameter is null</summary>
        /// <param name="value">object value</param>
        public static void IsNull(object value)
        {
            if (value is null)
            {
                throw new ArgumentNullException("Can not be null.", nameof(value));
            }
        }

        /// <summary>Throws if a value is less than or equal to the minimum value</summary>
        /// <param name="value">parameter value</param>
        /// <param name="minValue">minimum value</param>
        /// <typeparam name="T">type of parameter to validate</typeparam>
        public static void IsLessThanOrEqualTo<T>(T value, T minValue) where T : IComparable<T>
        {
            if (value.CompareTo(minValue) <= 0)
            {
                throw new ArgumentOutOfRangeException($"Parameter value of '{value}' is less than or equal to the minimum value of '{minValue}'", nameof(value));
            }
        }


        /// <summary>Throws if collection null or empty.</summary>
        /// <typeparam name="T">Type of parameter to validate</typeparam>
        /// <param name="selectColumns">select columns</param>
        public static void IsCollectionNullOrEmpty<T>(IEnumerable<T> selectColumns)
        {
            if (selectColumns == null || selectColumns.Equals(null) || selectColumns.Any() == false)
            {
                throw new ArgumentException("Cannot be empty or null.", nameof(selectColumns));
            }
        }

        /// <summary>Throws if a start date after end date</summary>
        /// <param name="startDate">start date to check</param>
        /// <param name="endDate">end date to check</param>
        public static void IsStartDateAfterEndDate(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentOutOfRangeException($"Start date '{nameof(startDate)}' must be before end date '{endDate}'");
            }
        }
    }
}

