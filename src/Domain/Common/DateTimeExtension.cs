namespace EasyMed.Domain.Common;

/// <summary>
/// It is used to convert a DataTime string to a UTC string
/// </summary>
public static class DateTimeExtension
{
    public static string ToStringUtc(this DateTime time)
    {
        return $"DateTime({time.Ticks}, DateTimeKind.Utc)";
    }
}