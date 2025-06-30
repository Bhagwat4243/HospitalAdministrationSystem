using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class CommonDetailsDto
    {                  // Unique identifier
        public string Name { get; set; }          
        public DateOnly DateOfBirth { get; set; }      // Date of birth
        public string Gender { get; set; }             // Gender (e.g., Male, Female, Other)
        public string PhoneNumber { get; set; }        // Contact number
        public string Email { get; set; }              // Email address
  
        public string City { get; set; }               // City
        public string State { get; set; }              // State
        public string Pincode { get; set; }            // Postal code
      
        public string AadharNo { get; set; }
    }
}
