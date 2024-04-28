namespace SpaceBattle.Lib
{
    public interface IGameCommandMessage
    {
        public int GameID { get; }
        public int ObjectID { get; }
        public string TypeCommand { get; }
        public IDictionary<string, object> Args { get; }
    }
}
