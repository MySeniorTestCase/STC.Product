namespace STC.ProductCatalog.Application.Utilities.Prices;

public static class PriceHelper
{
    public static long ToPenny(decimal dollar) => Convert.ToInt64(dollar * 100);
    public static decimal ToDollar(long penny) => Convert.ToDecimal(penny / 100);
}