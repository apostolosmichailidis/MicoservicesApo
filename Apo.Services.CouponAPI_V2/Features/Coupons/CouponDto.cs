namespace Apo.Services.CouponAPI_V2.Features.Coupons
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmmount { get; set; }
    }
}
