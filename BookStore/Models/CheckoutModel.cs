using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    
        public class CheckoutModel
        {
            public int Value { get; set; }

            [Required(ErrorMessage = "First Name is required.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required.")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Mobile Number is required.")]
            public long MobileNumber { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Address is required.")]
            public string Address { get; set; }

            public string Address2 { get; set; }

            [Required(ErrorMessage = "Postcode is required.")]
            [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid 6-digit postcode.")]
            public string Postcode { get; set; }

            [Required(ErrorMessage = "Country is required.")]
            public string City { get; set; }

            [Required(ErrorMessage = "State is required.")]
            public string State { get; set; }

            [Required(ErrorMessage = "Payment Method is required.")]
            public string PaymentMethod { get; set; }

            
    }

    
}
