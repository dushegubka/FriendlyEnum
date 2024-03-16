using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FriendlyEnum;

[Generator]
public class FriendlyEnumGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new FriendlyEnumSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        RegisterAttributes(context);
        
        CreateFriendlyEnumExtensionClass(context);
    }

    private void CreateFriendlyEnumExtensionClass(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not FriendlyEnumSyntaxReceiver receiver || receiver.Enums.Count == 0)
            return;

        var source = SourceCodeBuilder
            .BuildExtensionClassSourceCode(GeneratorConstants.ExtensionClassName, receiver.Enums)
            .ReformatAndCleanup();

        context.AddSource($"{GeneratorConstants.ExtensionClassName}.g.cs", source);
    }


    private void RegisterAttributes(GeneratorExecutionContext context)
    {
        context.AddSource(
            $"{GeneratorConstants.FriendlyEnumAttributeName}.g.cs", 
            GeneratorConstants.GeneratorAttributeSourceCode);
        context.AddSource(
            $"{GeneratorConstants.FriendlyNameAttributeName}.g.cs", 
            GeneratorConstants.FriendlyNameAttributeSourceCode);
    }
}