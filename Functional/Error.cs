namespace Functional;

public readonly struct Error<T>
{
    public T Value { get; }

    public Error(T value)
    {
        Value = value;
    }
}