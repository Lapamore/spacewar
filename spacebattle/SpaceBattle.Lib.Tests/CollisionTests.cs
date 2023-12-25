using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Tests;
using IDict = IDictionary<int, object>;

public class CollisionTreeCommandTest
{
    public CollisionTreeCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        var tree = new Dictionary<int, object>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.CollisionTree",
            (object[] args) => tree
        ).Execute();

        var trieBuilder = new TrieBuilder();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.CollisionTree.Builder",
            (object[] args) => trieBuilder
        ).Execute();

    }

    [Fact]
    public void IncorrectFilePathInputThrowExceptionWhenBuildingTree()
    {
        var buildingTree = new BuildCollisionTreeCommand("error_files.txt");
        Assert.Throws<FileNotFoundException>(buildingTree.Execute);
    }

    [Fact]
    public void SuccessfullyBuildingCollisionTreeFromFileWithSomBranches()
    {
        var path = "../../../CollisionFile/collision_file.txt";
        var buildingTree = new BuildCollisionTreeCommand(path);

        buildingTree.Execute();

        var resultTree = IoC.Resolve<IDict>("Game.CollisionTree");

        Assert.True(resultTree.ContainsKey(9));
        Assert.True(((IDict)resultTree[9]).ContainsKey(7));
        Assert.True(((IDict)((IDict)resultTree[9])[7]).ContainsKey(2));
        Assert.True(((IDict)((IDict)((IDict)resultTree[9])[7])[2]).ContainsKey(1));
    }
    
}