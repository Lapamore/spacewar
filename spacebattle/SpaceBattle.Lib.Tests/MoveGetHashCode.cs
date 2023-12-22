namespace SpaceBattle.Lib.Tests;

public class TestVector
{
    [Fact]
    public void TestGetHashCode()
    {
        var vector = new Vector(1, 2);

        var excepted = HashCode.Combine(1, 2);

        Assert.Equal(vector.GetHashCode(), excepted);
    }
}
