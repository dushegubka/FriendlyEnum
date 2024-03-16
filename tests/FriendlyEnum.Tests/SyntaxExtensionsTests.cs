using FluentAssertions;
using FriendlyEnum.Tests.Utils;
using Microsoft.CodeAnalysis.Text;

namespace FriendlyEnum.Tests;

public class SyntaxExtensionsTests
{
    [Fact(DisplayName = "ReformatAndCleanup should reformat source code spacings")]
    public void ReformatAndCleanup_Should_Reformat_Spacings()
    {
        // Arrange
        var unformattedSourceCode = File.ReadAllText(Path.Combine("TestData", "UnformattedExtensionClassCode.txt"));
        var formattedSource = File.ReadAllText(Path.Combine("TestData", "FormattedExtensionClassCode.txt"));

        // Act
        var inputSourceText = SourceText.From(unformattedSourceCode);

        var outputSourceText = inputSourceText
            .ReformatAndCleanup();

        // Assert
        outputSourceText
            .ToString()
            .Should()
            .BeEquivalentTo(formattedSource);
    }

    [Fact(DisplayName = "GetFriendlyNameAttributeValue should return \"First\" string value")]
    public void GetFriendlyNameAttributeValue_Should_Return_FriendlyName_Attribute_Value()
    {
        // Arrange
        var expectedValue = "\"First\"";
        
        // The code for generation is taken from the file SourceCodeConstants.EnumSource
        var enumDeclaration = GeneratorUtils.BuildEnumDeclaration();
        var firstEnumMember = enumDeclaration.Members.First();
        
        // Act
        var actualValue = firstEnumMember.GetFriendlyNameAttributeValue();
        
        // Assert
        actualValue.Should().Be(expectedValue);
    }
}