using System.Text;

namespace AlgebraicInterpolation;

public static class Helper
{
    public static string Format(this double value, int precision = 17)
    {
        return value.ToString($"g{precision}");
    }
    
    public static string PrintTable(this IEnumerable<(double x, double y)> table, string header1, string header2, int columnWidth = 25)
    {
        var result = new StringBuilder();

        result.Append(string.Format($"{{0,{columnWidth}}} | {{1,{-columnWidth}}}\n", header1, header2));

        foreach (var value in table)
        {
            result.Append(string.Format($"{{0,{columnWidth}}} | {{1,{-columnWidth}}}\n", value.x.Format(), value.y.Format()));
        }

        return result.ToString();
    }
}