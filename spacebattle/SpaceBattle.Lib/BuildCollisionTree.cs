using Hwdtech;

namespace SpaceBattle.Lib;

public class BuildCollisionTreeCommand : ICommand
{
    private readonly string new_path;

    public BuildCollisionTreeCommand(string path)
    {
        new_path = path;
    }

    public void Execute()
    {
        IoC.Resolve<ITrieBuilder>("Game.CollisionTree.Builder").BuildTreeFromFile(new_path);
    }
}
