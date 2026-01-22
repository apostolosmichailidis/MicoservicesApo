using System.ComponentModel.DataAnnotations;

namespace Apo.Services.CouponAPI.Models
{
    public class Coupon
    {   
        [Key]
        public int CouponId { get; set; }

        [Required]
        public string CouponCode { get; set; }

        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmmount { get; set; }
    }
}
