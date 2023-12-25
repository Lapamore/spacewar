using Hwdtech;

namespace SpaceBattle.Lib;

public interface ITrieBuilder
{
    public void BuildTreeFromFile(string path);
}

public class TrieBuilder : ITrieBuilder
{
    private static IEnumerable<IEnumerable<int>> ReadFileData(string path)
    {
        return File.ReadAllLines(path).Select(line => line.Split().Select(int.Parse));
    }

    public void BuildTreeFromFile(string path)
    {
        ReadFileData(path).ToList().ForEach(vector =>
        {
            var collisionTree  = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");
            vector.ToList().ForEach(f =>
            {
                collisionTree.TryAdd(f, new Dictionary<int, object>());
                collisionTree  = (IDictionary<int, object>)collisionTree [f];
            });
        });
    }
}