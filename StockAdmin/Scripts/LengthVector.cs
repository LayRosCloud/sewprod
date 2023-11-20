namespace StockAdmin.Scripts;

public struct LengthVector
{
    public LengthVector(int minimum, int maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public int Maximum { get; }
    public int Minimum { get; }
}