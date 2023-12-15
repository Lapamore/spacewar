namespace SpaceBattle.Lib.Tests
{
    using Xunit;

    public class AngleTests
    {
        [Fact]
        public void Equals_SameAngle_ShouldReturnTrue()
        {
            var angle = new Angle(45);

            var result = angle.Equals(angle);

            Assert.True(result);
        }

        [Fact]
        public void Equals_NullObject_ShouldReturnFalse()
        {
            var angle = new Angle(45);

            var result = angle.Equals(null);

            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentTypeObject_ShouldReturnFalse()
        {
            var angle = new Angle(45);
            var differentObject = new Mock<object>().Object;

            var result = angle.Equals(differentObject);

            Assert.False(result);
        }

        [Fact]
        public void Equals_EqualAngles_ShouldReturnTrue()
        {
            var angle1 = new Angle(45);
            var angle2 = new Angle(45);

            var result = angle1.Equals(angle2);

            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentAngles_ShouldReturnFalse()
        {
            var angle1 = new Angle(45);
            var angle2 = new Angle(90);

            var result = angle1.Equals(angle2);

            Assert.False(result);
        }
    }
}
