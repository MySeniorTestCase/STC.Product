namespace STC.ProductCatalog.Application.Utilities.Logging;

public static class LogExtensions
{
    public static string ToMaskedString(this string value, int maskLength = 4)
    {
        if (value.Length <= maskLength)
            return value;

        string data = value[..maskLength];

        return data.PadLeft(totalWidth: value.Length - maskLength, paddingChar: '*');
    }

    public static string ToMaskedString(this string value, bool takeHalf = true)
    {
        if (takeHalf is false)
            throw new InvalidOperationException(message: "You should use the overload with mask length.");
        
        if (value.Length is 0 or 1)
            return value;

        string data = value[..(value.Length / 2)];

        return data.PadLeft(totalWidth: value.Length / 2, paddingChar: '*');
    }
}