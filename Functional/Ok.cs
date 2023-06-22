namespace Functional;

public readonly struct Ok<T>
{
    public T Value { get; }

    public Ok(T value)
    {
        Value = value;
    }
}