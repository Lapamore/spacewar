namespace SpaceBattle.Lib
{
    public interface ITurn
    {
        Angle AngleNow { get; set; }
        Angle AngleSpeed { get; }
    }
    public class TurnCommand : ICommand
    {
        private readonly ITurn turn;

        public TurnCommand(ITurn turn)
        {
            this.turn = turn;
        }

        public void Execute()
        {
            turn.AngleNow += turn.AngleSpeed;
        }
    }
}
