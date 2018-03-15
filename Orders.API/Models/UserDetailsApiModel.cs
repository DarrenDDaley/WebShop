using System.ComponentModel.DataAnnotations;

namespace Orders.API.Models
{
    public class UserDetailsApiModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}
