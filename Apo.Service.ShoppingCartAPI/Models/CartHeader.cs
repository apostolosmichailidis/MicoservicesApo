using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apo.Service.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
        [NotMapped]
        public double DiscountAmount { get; set; }
        [NotMapped]
        public double CartTotal { get; set; }
    }
}
