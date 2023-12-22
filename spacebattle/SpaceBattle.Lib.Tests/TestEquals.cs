namespace SpaceBattle.Lib.Tests;

using Moq;
using Xunit;

public class VectorTests
{
    [Fact]
    public void Equals_SameVector_ShouldReturnTrue()
    {
        var vector = new Vector(1, 2);

        var result = vector.Equals(vector);

        Assert.True(result);
    }

    [Fact]
    public void Equals_NullObject_ShouldReturnFalse()
    {
        var vector = new Vector(1, 2);

        var result = vector.Equals(null);

        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentTypeObject_ShouldReturnFalse()
    {
        var vector = new Vector(1, 2);
        var differentObject = new Mock<object>().Object;

        var result = vector.Equals(differentObject);

        Assert.False(result);
    }

    [Fact]
    public void Equals_EqualVectors_ShouldReturnTrue()
    {
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(1, 2);

        var result = vector1.Equals(vector2);

        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentVectors_ShouldReturnFalse()
    {
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(3, 4);

        var result = vector1.Equals(vector2);

        Assert.False(result);
    }
}
