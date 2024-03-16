namespace FriendlyEnum.Tests;

public static class SourceCodeConstants
{
    public const string ProgramSource =
        """
            namespace FriendlyEnum.Playground;
            
            public class Program
            {
                public static void Main(string[] args)
                {
                    Console.WriteLine("Hello World!");
                }
            }
        """;

    public const string EnumSource =
        """
            namespace FriendlyEnum.Tests.Enums
            {
                [FriendlyEnum]
                public enum TestEnum
                {
                    [FriendlyName(Value = "First")]
                    One,
                    Two,
                    Three
                }
            }
        """;
}