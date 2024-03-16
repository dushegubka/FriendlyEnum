using FluentAssertions;
using FriendlyEnum.Tests.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FriendlyEnum.Tests;

public class SourceCodeBuilderTests
{
    [Fact(DisplayName = "BuildExtensionClassSourceCode should build unformatted FriendlyEnumExtension class source code")]
    public void BuildExtensionClassSourceCode_Should_Build_Unformatted_FriendlyEnumExtension_Class_Source()
    {
        // Arrange
        var enumDeclaration = GeneratorUtils.BuildEnumDeclaration();
        var enumMembers = enumDeclaration.Members.ToList();

        var expectedSource = File.ReadAllText(Path.Combine("TestData", "UnformattedExtensionClassCode.txt"));

        // Act
        var buildResult = SourceCodeBuilder.BuildExtensionClassSourceCode("FriendlyEnumExtensions",
            new Dictionary<EnumDeclarationSyntax, List<EnumMemberDeclarationSyntax>>
                { { enumDeclaration, enumMembers } });

        // Assert
        buildResult
            .ToString()
            .Trim()
            .Should()
            .BeEquivalentTo(expectedSource);
    }
}