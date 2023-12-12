using TechTalk.SpecFlow;
using SpaceBattle.Lib;

namespace XUnit.Tests
{
    [Binding]
    public sealed class UnitTest1
    {
        private readonly ScenarioContext _context;
        private Mock<IMovable> movable = new Mock<IMovable>();
        private Exception? exception;

        public UnitTest1(ScenarioContext context)
        {
            _context = context;
        }

        [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
        public void GivenSpacecraftSetLocation(int x, int y)
        {
            movable.SetupGet(m => m.Position).Returns(new Vector(x, y));
        }

        [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
        public void GivenSpacecraftSetSpeed(int boostX, int boostY)
        {
            movable.SetupGet(m => m.Velocity).Returns(new Vector(boostX, boostY));
        }

        [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
        public void GivenSpacecraftSetLocationNull()
        {
            movable.Setup(m => m.Position).Throws<Exception>();
        }

        [Given(@"скорость корабля определить невозможно")]
        public void GivenSpacecraftSetSpeedNull()
        {
            movable.Setup(m => m.Velocity).Throws<Exception>();
        }

        [Given(@"изменить положение в пространстве космического корабля невозможно")]
        public void GivenSpacecraftSetChangeFalse()
        {
            movable.Setup(m => m.Position).Throws<Exception>();
        }

        [When(@"происходит прямолинейное равномерное движение без деформации")]
        public void WhenSpacecraftMove()
        {
            try
            {
                MoveCommand moveCommand = new MoveCommand(movable.Object);
                moveCommand.Execute();
            }
            catch (Exception e)
            {
                exception = e;
            }
        }

        [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
        public void ThenSpacecraftMovedCoord(int correctX, int correctY)
        {
            movable.VerifySet(m => m.Position = new Vector(correctX, correctY), Times.Once);
        }

        [Then(@"возникает ошибка Exception")]
        public void ThenErrorException()
        {
            Assert.IsType<Exception>(this.exception);
        }
    }
}
