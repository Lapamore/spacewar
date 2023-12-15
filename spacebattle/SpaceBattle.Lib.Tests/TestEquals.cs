namespace SpaceBattle.Lib.Tests;
using Xunit;
using Moq;

public class VectorTests
{
    [Fact]
    public void Equals_SameVector_ShouldReturnTrue()
    {
        // Arrange
        var vector = new Vector(1, 2);

        // Act
        var result = vector.Equals(vector);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_NullObject_ShouldReturnFalse()
    {
        // Arrange
        var vector = new Vector(1, 2);

        // Act
        var result = vector.Equals(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentTypeObject_ShouldReturnFalse()
    {
        // Arrange
        var vector = new Vector(1, 2);
        var differentObject = new Mock<object>().Object;

        // Act
        var result = vector.Equals(differentObject);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_EqualVectors_ShouldReturnTrue()
    {
        // Arrange
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(1, 2);

        // Act
        var result = vector1.Equals(vector2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentVectors_ShouldReturnFalse()
    {
        // Arrange
        var vector1 = new Vector(1, 2);
        var vector2 = new Vector(3, 4);

        // Act
        var result = vector1.Equals(vector2);

        // Assert
        Assert.False(result);
    }
}
