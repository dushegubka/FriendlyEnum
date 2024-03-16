### FriendlyEnum — генератор исходного кода для удобной конвертации Enum в String
***

### Установка 
***
``` 
dotnet add package Dushegubka.FriendlyEnum
```

### Использование 
***
```csharp
[FriendlyEnum]
public enum MyAwesomeEnum
{
    [FriendlyName(Value = "First")]
    One,
    [FriendlyName(Value = "2nd")]
    Two,
    Three
}
```
На выходе генерируется следующий extension-класс

```csharp
public static class FriendlyEnumExtensions
{
    public static string ToFriendlyName(this MyAwesomeEnum value)
    {
        return value switch
        {
            MyAwesomeEnum.One => "First",
            MyAwesomeEnum.Two => "2nd",
            MyAwesomeEnum.Three => nameof(MyAwesomeEnum.Three),
            _ => throw new ArgumentException(nameof(value))};
    }
}
```
***

### Пример

```csharp
Console.WriteLine(MyAwesomeEnum.One.ToFriendlyName());
Console.WriteLine(MyAwesomeEnum.Two.ToFriendlyName());
Console.WriteLine(MyAwesomeEnum.Three.ToFriendlyName());
```

### Консольный вывод

> First 
> 
> 2nd
> 
> Three