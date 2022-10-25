namespace AlgebraicInterpolation;

public static class Interpolation
{
    public static void InterpolationProcess((double x, double y)[] functionTable, Func<double, double> function)
    {
        Console.Write("Введите точку интерполирования (вещественное число): ");

        var interpolationPoint = 0.0;
        while (!double.TryParse(Console.ReadLine(), out interpolationPoint))
        {
            Console.Write("Некорректное значение. Введите вещественное число: ");
        }

        Console.Write($"Введите степень интерполяционного многочлена (<= {functionTable.Length - 1}): ");

        var interpolationDegree = 0;
        while (!int.TryParse(Console.ReadLine(), out interpolationDegree) ||
               interpolationDegree < 1 || interpolationDegree >= functionTable.Length)
        {
            Console.Write("Некорректное значение. Введите целое число, большее нуля и меньшее числа значений в " +
                          "таблице функции: ");
        }

        var interpolationNodes = functionTable.OrderBy(value => Math.Abs(interpolationPoint - value.x))
            .Take(interpolationDegree + 1).ToArray();

        Console.WriteLine($"\nУзлы интерполяции");
        Console.WriteLine(interpolationNodes.PrintTable("x", "f(x)"));

        Console.WriteLine($"Точка интерполирования: {interpolationPoint}\n" +
                          $"Степень интерполяционного многочлена: {interpolationDegree}\n");


        var newtonInterpolated = new NewtonsPolynomial(interpolationNodes);
        var newtonInterpolationPointY = newtonInterpolated.GetNewtonPolynominalResult()(interpolationPoint);
        
        Console.WriteLine($"Значение интерполяционного многочлена Ньютона в точке {interpolationPoint}: " +
                                  $"{newtonInterpolationPointY.Format()}");
        Console.WriteLine($"Абсолютная фактическая погрешность: " +
                                  $"{Math.Abs(function(interpolationPoint) - newtonInterpolationPointY).Format()} \n");

        var lagrangeInterpolated = new LagrangePolynomial(interpolationNodes);
        var lagrangeInterpolationPointY = lagrangeInterpolated.GetLagrangePolynominalResult()(interpolationPoint);
        
        Console.WriteLine($"Значение интерполяционного многочлена Лагранжа в точке {interpolationPoint}: " +
                          $"{lagrangeInterpolationPointY.Format()}");

        var originalValue = function(interpolationPoint);

        Console.WriteLine($"Абсолютная фактическая погрешность: " +
                          $"{Math.Abs(originalValue - lagrangeInterpolationPointY).Format()} \n");

        Console.WriteLine($"Значение исходной функции в точке интерполяции: " +
                          $"{originalValue.Format()} \n");
        
    }
}