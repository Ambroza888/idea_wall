    using System.ComponentModel.DataAnnotations;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;



    namespace CS_proj.Models
    {
        public class Like
        {
            public int LikeId {get;set;}      
            public User User {get;set;}
            public Idea Idea {get;set;}
            public int IdeaId {get;set;}
            public int UserId {get;set;}

            // -----------------------------------------------------------------
            // date
            // -----------------------------------------------------------------
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
        }
    }