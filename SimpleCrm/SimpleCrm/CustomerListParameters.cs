using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCrm
{
    public class CustomerListParameters
    {

        /// <summary>
        /// The 1-based page number.
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// The page size, or number of records to take in a page.
        /// </summary>
        public int Take { get; set; } = 50;
        /// <summary>
        /// Any valid order specification over customer properties.
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// If specified, the exact lastname value to match (not a partial match).
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// A common term to search amoung all 'searchable' fields. This is a partial
        /// value to find within any of those searchable fields.
        /// </summary>
        public string Term { get; set; }
    }
}
