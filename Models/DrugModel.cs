using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class DrugModel
    {
        [Key]
        public int DrugId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DrugName is Required.")]
        [Display(Name = "Drug Name: ")]
        public string DrugName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DrugDescription is Required.")]  
        [Display(Name = "Drug Description: ")]
        public string DrugDescription { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DrugPrice is Required.")]
        [Display(Name = "Drug Price: ")]
        public double DrugPrice { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DrugAmount is Required.")]
        [Display(Name = "Drug Quantity Available: ")]
        public int DrugQuantityAvailable { get; set; }
        public string SuccessMessage { get; set; }

    }
}
