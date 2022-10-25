using NonlinearEquations;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// f(x)= 4∙cos(x) + 0,3∙x [A, B] = [-15; 5] ε = 10-5
double Function(double x) =>  4 * Math.Cos(x) + 0.3 * x;

double Derivative(double x) => -4 * Math.Sin(x) + 0.3;

double SecondDerivative(double x) => -4 * Math.Cos(x);

const double epsilon = 0.000000000001;

//(int Start, int End) interval = (-15, 5);

Console.WriteLine("Задание №1\n" +
              "Численные методы решения нелинейных уравнений\n" +
              "Исходные параметры задачи: \n" +
              "Функция: f(x) = 4 * cos(x) + 0.3 * x \n" +
              "A = -15; B = 5; eps = 0.00001");


double start;
double end;

Console.WriteLine("Введите A");
while (!double.TryParse(Console.ReadLine(), out start))
{
    Console.WriteLine("Введите A");
}

Console.WriteLine("Введите B");
while (!double.TryParse(Console.ReadLine(), out end))
{
    Console.WriteLine("Введите B");
}

int n;
Console.WriteLine("Введите N. N должен быть целым числом, больше или равным 2");
while (!int.TryParse(Console.ReadLine(), out n) && n < 2)
{
    Console.WriteLine("Введите N. N должен быть целым числом, больше или равным 2");
}


var interval = (start, end);

try
{
    var intervalsList = ApproximateRootExtractor.Separate(Function, interval, n);
    foreach (var result in intervalsList)
    {
        ApproximateRootExtractor.Bisection(Function, result, epsilon);
        ApproximateRootExtractor.Newtons(Function, Derivative, SecondDerivative, result, epsilon);
        ApproximateRootExtractor.NewtonsModified(Function, Derivative, SecondDerivative, result, epsilon);
        ApproximateRootExtractor.Secant(Function, result, epsilon);
    }
}
catch (Exception e)
{
    Console.WriteLine($"Произошла ошибка. {e.Message}");
}
