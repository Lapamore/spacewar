using TechTalk.SpecFlow;
using SpaceBattle.Lib;

namespace XUnit.Tests
{
    [Binding]
    public sealed class UnitTest1
    {
        private readonly ScenarioContext _context;
        private Mock<ITurn> movable = new Mock<ITurn>();
        private Exception? exception;

        public UnitTest1(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void GivenSpacecraftSetLocation(int angle)
        {
            movable.SetupGet(m => m.AngleNow).Returns(new Angle(angle));
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void GivenSpacecraftSetSpeed(int boostAngle)
        {
            movable.SetupGet(m => m.AngleSpeed).Returns(new Angle(boostAngle));
        }

        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void GivenSpacecraftSetLocationNull()
        {
            movable.Setup(m => m.AngleNow).Throws<Exception>();
        }

        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void GivenSpacecraftSetSpeedNull()
        {
            movable.Setup(m => m.AngleSpeed).Throws<Exception>();
        }

        [Given(@"невозможно изменить угол наклона к оси OX космического корабля")]
        public void GivenSpacecraftSetChangeFalse()
        {
            movable.Setup(m => m.AngleNow).Throws<Exception>();
        }

        [When(@"происходит вращение вокруг собственной оси")]
        public void WhenSpacecraftTurn()
        {
            try
            {
                TurnCommand turnCommand = new TurnCommand(movable.Object);
                turnCommand.Execute();
            }
            catch (Exception e)
            {
                exception = e;
            }
        }

        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ThenSpacecraftTurn(int correctAngle)
        {
            movable.VerifySet(m => m.AngleNow = new Angle(correctAngle), Times.Once);
        }

        [Then(@"возникает ошибка Exception")]
        public void ThenErrorException()
        {
            Assert.IsType<Exception>(this.exception);
        }
    }
}