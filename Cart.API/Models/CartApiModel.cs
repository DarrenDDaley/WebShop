using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cart.API.Models
{
    public class CartApiModel : IValidatableObject
    {

        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Items == null || Items.Count == 0) {
                yield return new ValidationResult("The cart cant be empty.");
            }
        }
    }
}
