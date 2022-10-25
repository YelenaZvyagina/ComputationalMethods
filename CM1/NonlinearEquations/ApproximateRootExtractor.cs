namespace NonlinearEquations;

public static class ApproximateRootExtractor
{
    public static IEnumerable<(double Start, double End)> Separate(Func<double, double> function, (double Start, double End) interval, int separationStepCount)
    {
        var separationStep = (interval.End - interval.Start) / separationStepCount;
        
        Console.WriteLine("Выполним отделение корней:\n" +
                          $"Число шагов: {separationStepCount}\n");

        var intervals = new List<(double Start, double End)>();
        
        var x1 = interval.Start;
        var x2 = x1 + separationStep;

        var y1 = function(x1);
        var y2 = function(x2);

        for (var i = 1; i <= separationStepCount; ++i)
        {
            if (y1 * y2 <= 0)
            {
                intervals.Add((x1, x2));
            }
                
            if (i == separationStepCount) break;

            x1 = x2;
            y1 = y2;

            x2 = i == separationStepCount - 1 ? interval.End : (x1 + separationStep);
            y2 = function(x2);
        }

        Console.WriteLine($"Найдено {intervals.Count} промежутков перемены знака \n" +
                          "Промежутки: ");

        foreach (var result in intervals)
        {
            Console.WriteLine($"[{result.Start}; {result.End}]");
        }
        return intervals;
    }
    
    public static (IntervalResult Result, double LastLength) Bisection(Func<double, double> function,
        (double Start, double End) interval, double epsilon)
    {
        Console.WriteLine("\n Выполним уточнение корней методом бисекции:\n" +
                          $"Начальное приближение к корню: [{interval.Start.ToFormattedString()}; {interval.End.ToFormattedString()}]");
        

        if (function(interval.Start) * function(interval.End) >= 0)
        {
            throw new ArgumentException("Значения функции на концах интервала должны иметь разные знаки.");
        }

        var start = interval.Start;
        var end = interval.End;
        var center = (start + end) / 2;
        var length = end - start;
        var yCenter = function(center);
        
        var result = new IntervalResult { IterationCount = 1 };
        
        while (length > 2 * epsilon)
        {
            if (function(start) * yCenter <= 0)
            {
                end = center;
            }
            else
            {
                start = center;
            }

            center = (start + end) / 2;
            yCenter = function(center);
            length = end - start;
            
            ++result.IterationCount;
        }

        result.AbsoluteResidual = Math.Abs(yCenter);
        result.Root = center;
        var lastLength = length / 2.0;

        Console.WriteLine($"Приближенное решение: {result.Root}\n" +
                          $"Итераций: {result.IterationCount}\n" +
                          $"Абсолютная величина невязки: {result.AbsoluteResidual}\n" +
                          $"Длина последнего отрезка: {lastLength.ToFormattedString()}\n");

        return (result, lastLength);
    }

    public static IntervalResult Newtons(Func<double, double> function, Func<double, double> derivative,
        Func<double, double> sDerivative, (double Start, double End) interval, double epsilon)
    {
        Console.WriteLine("Выполним уточнение корней методом Ньютона:\n" +
                          $"Начальное приближение к корню: [{interval.Start}; {interval.End.ToFormattedString()}]");
        
        // Выберем x_0. Он должен удовлетворять теореме о сходимости
        var startPoint = GetStartPoint(function, sDerivative, interval);

        var result = new IntervalResult { IterationCount = 1 };
        
        var previous = startPoint;
        var current = previous - function(previous) / derivative(previous);

        while (Math.Abs(current - previous) > epsilon)
        {
            previous = current;
            current = previous - function(previous) / derivative(previous);

            ++result.IterationCount;
        }
        result.Root = current;
        result.AbsoluteResidual = Math.Abs(function(result.Root));

        Console.WriteLine($"Приближенное решение: {result.Root}\n" +
                          $"Итераций: {result.IterationCount}\n" +
                          $"Абсолютная величина невязки: {result.AbsoluteResidual} \n " +
                          $"|x_m - x_(m-1)|: {Math.Abs(current - previous)}");

        return result;
    }

    private static double GetStartPoint(Func<double, double> function, Func<double, double> sDerivative, (double Start, double End) interval)
    {
        var random = new Random();
        var startPoint = interval.Start + (interval.End - interval.Start) * random.NextDouble();
        var isInInterval = startPoint >= interval.Start || startPoint <= interval.End;
        var satisfiesTheorem = function(startPoint) * sDerivative(startPoint) > 0;
        
        while (!isInInterval || !satisfiesTheorem)
        {
            startPoint = interval.Start + (interval.End - interval.Start) * random.NextDouble();
            isInInterval = startPoint >= interval.Start || startPoint <= interval.End;
            satisfiesTheorem = function(startPoint) * sDerivative(startPoint) > 0;
        }

        return startPoint;
    }
    
    public static IntervalResult NewtonsModified(Func<double, double> function, Func<double, double> derivative,
        Func<double, double> sDerivative, (double Start, double End) interval, double epsilon)
    {
        Console.WriteLine("\n Выполним вычисление модифицированным методом Ньютона:\n" +
                          $"Начальное приближение к корню: [{interval.Start.ToFormattedString()}; {interval.End.ToFormattedString()}]");

        if (function(interval.Start) * function(interval.End) >= 0)
        {
            throw new ArgumentException("Значения функции на концах интервала должны иметь разные знаки.");
        }
        
        var startPoint = GetStartPoint(function, sDerivative, interval);
        
        var startPointDerivative = derivative(startPoint);

        var result = new IntervalResult { IterationCount = 1 };

        var previous = startPoint;
        var current = previous - function(previous) / startPointDerivative;

        while (Math.Abs(current - previous) > epsilon)
        {
            previous = current;
            current = previous - function(previous) / startPointDerivative;

            ++result.IterationCount;
        }
        
        result.Root = current;
        result.AbsoluteResidual = Math.Abs(function(result.Root));

        Console.WriteLine($"Приближенное решение: {result.Root}\n" +
                          $"Итераций: {result.IterationCount}\n" +
                          $"Абсолютная величина невязки: {result.AbsoluteResidual} \n" +
                          $"|x_m - x_(m-1)|: {Math.Abs(current - previous)} \n");

        return result;
    }
    
    public static IntervalResult Secant(Func<double, double> function,
        (double Start, double End) interval, double epsilon)
    {
        Console.WriteLine("Выполним уточнение корней методом секущих:\n" +
                          $"Начальное приближение: [{interval.Start.ToFormattedString()}; {interval.End.ToFormattedString()}]");
        
        var random = new Random();
        var startPoint1 = interval.Start + (interval.End - interval.Start) * random.NextDouble();
        var startPoint2 = interval.Start + (interval.End - interval.Start) * random.NextDouble();

        var startPointsAreOk = Math.Abs(startPoint1 - startPoint2) > 0
                               && startPoint1 >= interval.Start && startPoint1 <= interval.End
                               && startPoint2 >= interval.Start && startPoint2 <= interval.End;

        while (!startPointsAreOk)
        {
            startPoint1 = interval.Start + (interval.End - interval.Start) * random.NextDouble();
            startPoint2 = interval.Start + (interval.End - interval.Start) * random.NextDouble();
            startPointsAreOk = Math.Abs(startPoint1 - startPoint2) > 0
                               && startPoint1 >= interval.Start && startPoint1 <= interval.End
                               && startPoint2 >= interval.Start && startPoint2 <= interval.End;
        }

        var result = new IntervalResult { IterationCount = 1 };

        var first = startPoint1;
        var second = startPoint2;
        var ySecond = function(second);
        var third = second - ySecond / (ySecond - function(first)) * (second - first);

        while (Math.Abs(third - second) > epsilon)
        {
            first = second;
            second = third;
            ySecond = function(second);
            third = second - ySecond / (ySecond - function(first)) * (second - first);

            ++result.IterationCount;
        }

        if (third <= interval.Start || third >= interval.End) 
        {
            throw new ApplicationException($"Найденное приближение лежит вне интервала");
        } 

        result.Root = third;
        result.AbsoluteResidual = Math.Abs(function(result.Root));

        Console.WriteLine($"Приближенное решение: {result.Root}\n" +
                          $"Итераций: {result.IterationCount}\n" +
                          $"Абсолютная величина невязки: {result.AbsoluteResidual}\n" +
                          $"|x_m - x_(m-1)|: {Math.Abs(third - first)}");
        
        return result;
    }
    
    private static string ToFormattedString(this double number) => number.ToString("F10");
}