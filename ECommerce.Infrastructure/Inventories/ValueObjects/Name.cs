namespace ECommerce.Infrastructure.Inventories.ValueObjects;

using Exceptions;

public record Name
{
    private const int MaxLength = 50;
    private const int MinLength = 2;
    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    private Name(string value)
    {
        Value = value;
    }

    public static Name Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidNullOrEmptyNameException(value);

        if (value.Length < MinLength)
            throw new ShortLengthNameException(value, MinLength);

        if (value.Length > MaxLength)
            throw new LongLengthNameException(value, MaxLength);

        return new Name(value);
    }

    public static implicit operator string(Name name) => name.Value;
}
