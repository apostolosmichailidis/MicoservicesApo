namespace Apo.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; } = string.Empty;
        public static string ProductAPIBase { get; set; } = string.Empty;
        public static string AuthAPIBase { get; set; } = string.Empty;
        public static string ShoppingCartAPIBase { get; set; } = string.Empty;

        public static string TokenCookie = "JWTToken";
        public enum ApiType 
        {
            GET,
            POST, 
            PUT,
            DELETE
        }
    }
}
