namespace AlgebraicInterpolation;

public class LagrangePolynomial 
{
    private readonly (double x, double y)[] _interpolationNodes;

    public LagrangePolynomial((double x, double y)[] interpolationNodes)
    {
        _interpolationNodes = interpolationNodes;
    }

    public Func<double, double> GetLagrangePolynominalResult()
    {
        return x =>
        {
            var result = 0.0;

            for (var i = 0; i < _interpolationNodes.Length; ++i)
            {
                var sumTerm = _interpolationNodes[i].y;

                for (var j = 0; j < _interpolationNodes.Length; ++j)
                {
                    if (i != j)
                    {
                        sumTerm *= (x - _interpolationNodes[j].x) / (_interpolationNodes[i].x - _interpolationNodes[j].x);
                    }
                }

                result += sumTerm;
            }

            return result;
        };
    }
}
