using System;

namespace Orders.API.Models
{
    public class UserDetailsModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
