using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    /// <summary>
    /// this Address class model bonded to the Identity and
    /// represented the Identity's address to ship to
    /// </summary>
    public class Address
    {
        // parameterless constructor is used when we 
        // implements a migrations to populate the Address table
        public Address()
        {            
        }

        // parametered constructor
        public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
        {
            Zipcode = zipcode;
            State = state;
            City = city;
            Street = street;
            LastName = lastName;
            FirstName = firstName;            
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
