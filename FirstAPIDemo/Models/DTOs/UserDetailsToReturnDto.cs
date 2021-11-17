using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPIDemo.Models.DTOs
{
    public class UserDetailsDto
    {
        [Required]
        [MaxLength(5, ErrorMessage ="Last name must not be morethan 5 characters")]
        [Display(Name ="Last name")]
        //[DataType(DataType.Password)]
        public string LastName { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "First name must be between 3 - 5 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        //[RegularExpression(@"^\+\d{3}\-\d{9,10}")]
        public string Title { get; set; }

        //[ValidateAge(10, 18, ErrorMessage="Age out of range!")]
        //[RegularExpression(@"^\d{4}")]
        public int Age { get; set; }


        public string Position { get; set; }
    }

    public class ValidateAge : ValidationAttribute
    {
        private readonly int _minAge, _maxAge;
        public ValidateAge(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ageYear = Convert.ToDateTime(value);
            var currentDate = DateTime.Now.Year;

            var age = currentDate - ageYear.Year;
            if ((ageYear.Year + age) > currentDate)
                age--;

            if(age >= _minAge && age <= _maxAge)
                return base.IsValid(age, validationContext);

            return base.IsValid(value, validationContext);
        }
    }
}
