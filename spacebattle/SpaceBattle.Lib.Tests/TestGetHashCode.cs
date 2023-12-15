namespace SpaceBattle.Lib.Tests
{
    using Xunit;

    public class TestGetHashCode
    {
        [Fact]
        public void GetHashCode_SameAngles_ShouldReturnSameHashCode()
        {
            var angle1 = new Angle(45);
            var angle2 = new Angle(45);

            var hashCode1 = angle1.GetHashCode();
            var hashCode2 = angle2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCode_DifferentAngles_ShouldReturnDifferentHashCodes()
        {
            var angle1 = new Angle(45);
            var angle2 = new Angle(90);

            var hashCode1 = angle1.GetHashCode();
            var hashCode2 = angle2.GetHashCode();

            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
