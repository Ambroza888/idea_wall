    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;



    namespace CS_proj.Models
    {
        public class User
        {

            [Key]
            public int UserId { get; set; }

            [Required]
            [RegularExpression(@"[\p{L} ]+$", ErrorMessage="Name Shuld be only letters and spaces")]
            [MinLength(2, ErrorMessage="First Name must be more than 2")]
            public string FirstName { get; set; }


            [Required]
            [MinLength(2, ErrorMessage="Alias must be more than 2")]
            [RegularExpression(@"^\w+$", ErrorMessage="Alias shuld be letters and numbers only")]
            public string LastName { get; set; }

            
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage= "Password Must Contain at least one letter, one number, and one special character")]
            [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
            public string Password { get; set; }


            public List<Idea> Ideas {get;set;}
            public List<Like> Likes {get;set;}
            // -----------------------------------------------------------------
            // date
            // -----------------------------------------------------------------
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;

            [NotMapped]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string Confirm {get;set;}
        }
    }