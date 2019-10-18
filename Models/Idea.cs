    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;



    namespace CS_proj.Models
    {
        public class Idea
        {
            public int IdeaId {get;set;}

            [Required]
            [MinLength(5,ErrorMessage="Your idea needs to be more than 5 chars")]
            public string IdeaText {get;set;}
            
            public int UserId {get;set;}
            public User User {get;set;}
            public List<Like> Likes {get;set;}
            // -----------------------------------------------------------------
            // date
            // -----------------------------------------------------------------
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
        }
    }