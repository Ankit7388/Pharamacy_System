using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public int OrderedQuantity { get; set; }
        public double OrderPrice { get; set; }
        public string DrugName { get; set; }
        public DateTime OrderedDate { get; set; }

    }
}
