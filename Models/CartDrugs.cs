
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class CartDrugs
    {
        [Key]
        public int CartID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DrugId is Required.")]
        [Display(Name = "Drug Id: ")]
        public int DrugID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserId is Required.")]
        [Display(Name = "User Id: ")]
        public string UserEmail { get; set; }
        [Display(Name = "Drug Quantity Required: ")]
        public int Quantity { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is Required.")]
        public double Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
