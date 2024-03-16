using FluentAssertions;
using FriendlyEnum.Tests.Utils;

namespace FriendlyEnum.Tests;

public class FriendlyEnumGeneratorTests
{
    [Theory(DisplayName = "Should generate FriendlyEnumAttribute and FriendlyNameAttribute")]
    [InlineData("FriendlyEnumAttribute")]
    [InlineData("FriendlyNameAttribute")]
    public void Should_Generate_FriendlyEnumAttribute_And_FriendlyNameAttribute(string className)
    {
        // Arrange
        var expectedSourceFile = File.ReadAllText(Path.Combine("TestData", $"{className}.txt"));

        // Act
        var runResult = GeneratorUtils.RunGenerator(SourceCodeConstants.ProgramSource);

        // Assert
        runResult.Diagnostics
            .IsEmpty
            .Should()
            .BeTrue();

        runResult
            .OutputCompilation
            .SyntaxTrees.Should()
            .Contain(x => x.FilePath.EndsWith($"{className}.g.cs"))
            .Which.GetText()
            .ToString()
            .Should<string>()
            .BeEquivalentTo(expectedSourceFile);
    }
}