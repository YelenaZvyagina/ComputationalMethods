using AlgebraicInterpolation;

Console.OutputEncoding = System.Text.Encoding.UTF8;

double Function(double x) =>  2 * Math.Sin(x) - x / 2;

Console.WriteLine("Лабораторная работа №2\n" +
                  "Задача алгебраического интерполирования\n" +
                  "Интерполяционный многочлен в форме Ньютона и в форме Лагранжа\n" +
                  "Вариант 8");

Console.WriteLine("Введите число значений в таблице функции: ");

var numberOfValues = 0;
while (!int.TryParse(Console.ReadLine(), out numberOfValues) || numberOfValues < 2)
{
    Console.Write("Некорректное значение: введите целое число, большее единицы: ");
}

Console.Write("Введите начало отрезка: ");

var start = 0.0;
while (!double.TryParse(Console.ReadLine(), out start))
{
    Console.Write("Некорректное значение: введите вещественное число: ");
}

Console.Write("Введите конец отрезка: ");

var end = 0.0;
while (!double.TryParse(Console.ReadLine(), out end) || end <= start)
{
    Console.Write("Некорректное значение: введите вещественное число, большее чем начало отрезка: ");
}

var segmentLength = (end - start) / (numberOfValues - 1);

var functionTable = new (double x, double y)[numberOfValues];

for (var i = 0; i < numberOfValues; ++i)
{
    var x = start + segmentLength * i;
    functionTable[i] = (x, Function(x));
}

Console.WriteLine($"\nТаблица значений функции (число значений: {numberOfValues})");
Console.WriteLine(functionTable.PrintTable("x", "f(x)"));

Interpolation.InterpolationProcess(functionTable, Function);

Console.WriteLine("Хотите ввести другие значения для точки интерполирования и степени многочлена? Введите 'да' или 'нет'");
var processWithDifferentValues = Console.ReadLine() == "да";

while (processWithDifferentValues)
{
    Interpolation.InterpolationProcess(functionTable, Function);
    Console.WriteLine("Хотите ввести другие значения для точки интерполирования и степени многочлена? Введите 'да' или 'нет'");
    processWithDifferentValues = Console.ReadLine() == "да";
}
