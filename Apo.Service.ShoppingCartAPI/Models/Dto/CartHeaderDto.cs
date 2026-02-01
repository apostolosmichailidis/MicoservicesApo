namespace Apo.Service.ShoppingCartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public double CartTotal { get; set; }
    }
}
