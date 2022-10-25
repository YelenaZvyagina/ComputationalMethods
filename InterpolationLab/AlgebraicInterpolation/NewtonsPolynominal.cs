namespace AlgebraicInterpolation;

public class NewtonsPolynomial 
{
    private readonly double[][] _dividedDifferences;
    private readonly (double x, double y)[] _interpolationNodes;

    public NewtonsPolynomial((double x, double y)[] interpolationNodes)
    {
        _dividedDifferences = new double[interpolationNodes.Length][];
        _interpolationNodes = interpolationNodes;
        for (var i = 0; i < _dividedDifferences.Length; ++i)
        {
            _dividedDifferences[i] = new double[_dividedDifferences.Length - i];

            for (var j = 0; j < _dividedDifferences[i].Length; ++j)
            {
                _dividedDifferences[i][j] = GetDividedDifference(j, j + i);
            }
        }
    }

    private double GetDividedDifference(int index0, int indexK)
    {
        if (index0 == indexK)
        {
            return _interpolationNodes[index0].y;
        }

        return (_dividedDifferences[indexK - index0 - 1][index0 + 1] - _dividedDifferences[indexK - index0 - 1][index0]) /
            (_interpolationNodes[indexK].x - _interpolationNodes[index0].x);
    }

    public Func<double, double> GetNewtonPolynominalResult()
    {
        var coefficients = new double[_interpolationNodes.Length];
        var xPoints = new double[_interpolationNodes.Length];

        for (var i = 0; i < coefficients.Length; ++i)
        {
            coefficients[i] = _dividedDifferences[_dividedDifferences.Length - i - 1][0];
            xPoints[i] = _interpolationNodes[_interpolationNodes.Length - i - 1].x;
        }

        return x =>
        {
            var result = coefficients[0];

            for (var i = 1; i < coefficients.Length; ++i)
            {
                result = result * (x - xPoints[i]) + coefficients[i];
            }

            return result;
        };
    }
}