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
        /// The unique identifier for the person.
        /// </summary>
        public int Id { get; set; }

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

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
            
        }


        public PersonModel(string firstName, string lastName, string email, string cellPhone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CellPhone = cellPhone;  
        }

        public PersonModel(string id, string firstName, string lastName, string email, string cellPhone)
        {
            Id = int.Parse(id);
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CellPhone = cellPhone;

        }
        public PersonModel()
        {

        }
    }
}
