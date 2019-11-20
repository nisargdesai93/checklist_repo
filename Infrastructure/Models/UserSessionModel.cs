using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Models
{
    public class UserSessionModel
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime LoggedInAt { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        

        public UserSessionModel()
        {
            FullName = this.FirstName + " " + this.LastName; 
            LoggedInAt = DateTime.UtcNow;
            IsAuthenticated = true;
        }
    }
}
