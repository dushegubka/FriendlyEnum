using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FriendlyEnum.Tests.Utils;

public static class GeneratorUtils
{
    public static GeneratorRunResult RunGenerator(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var generator = new FriendlyEnumGenerator();
        var driver = CSharpGeneratorDriver.Create(generator);
        var compilation = CSharpCompilation.Create("FriendlyEnum", new[] { syntaxTree });

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        return new GeneratorRunResult(outputCompilation, diagnostics);
    }

    public static EnumDeclarationSyntax BuildEnumDeclaration()
    {
        var result = RunGenerator(SourceCodeConstants.EnumSource);

        return result
            .OutputCompilation
            .SyntaxTrees
            .First()
            .GetRoot()
            .DescendantNodes()
            .OfType<EnumDeclarationSyntax>()
            .Single();
    }
}

public record GeneratorRunResult(Compilation OutputCompilation, ImmutableArray<Diagnostic> Diagnostics);