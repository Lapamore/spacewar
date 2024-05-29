
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CompileStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var sourceCode = (string)args[0];

            var generatedAssemblyName = IoC.Resolve<string>("Assembly.Name.Create");
            var metadataReferences = IoC.Resolve<IEnumerable<MetadataReference>>("Compile.References");
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var parsedSyntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var csharpCompilation = CSharpCompilation.Create(generatedAssemblyName).AddReferences(metadataReferences);
            csharpCompilation = csharpCompilation.WithOptions(compilationOptions).AddSyntaxTrees(parsedSyntaxTree);

            Assembly compiledAssembly;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                var emitResult = csharpCompilation.Emit(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                compiledAssembly = Assembly.Load(memoryStream.ToArray());
            }

            return compiledAssembly;
        }
    }

}

