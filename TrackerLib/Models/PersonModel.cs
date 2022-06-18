using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    /// <summary>
    /// Represents one person
    /// </summary>
    public class PersonModel
    {
        /// <summary>
        /// The first name of the person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The email address of the person.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The cell phone number of the person.
        /// </summary>
        public string CellPhone { get; set; }
    }
}
