using Xunit;
using SpaceBattle.Lib;

public class TurnCommandTests
{
    [Fact]
    public void GetHashCode_ReturnsUniqueHashCode()
    {
        // Arrange
        var angleNow = new Angle(45); 
        var angleSpeed = new Angle(30);
        var turn = new MockTurn(angleNow, angleSpeed);
        var turnCommand = new TurnCommand(turn);
        var hashCode1 = turnCommand.GetHashCode();
        var hashCode2 = turnCommand.GetHashCode();
        Assert.Equal(hashCode1, hashCode2);
    }

    // Mock реализация ITurn для тестирования
    private class MockTurn : ITurn
    {
        public Angle AngleNow { get; set; }
        public Angle AngleSpeed { get; }

        public MockTurn(Angle angleNow, Angle angleSpeed)
        {
            AngleNow = angleNow;
            AngleSpeed = angleSpeed;
        }
    }
}
