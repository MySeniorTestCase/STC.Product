using STC.ProductCatalog.Domain._Shared.Events;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Domain._Shared.Types.Concretes;

public abstract class ValueObjectBase : DomainEventWrapper, IValueObject<ValueObjectBase>
{
    public static bool operator ==(ValueObjectBase? a, ValueObjectBase? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObjectBase? a, ValueObjectBase? b) =>
        !(a == b);

    public virtual bool Equals(ValueObjectBase? other) =>
        other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj) =>
        obj is ValueObjectBase valueObject && ValuesAreEqual(valueObject);

    public override int GetHashCode() =>
        GetAtomicValues().Aggregate(
            default(int),
            (hashcode, value) =>
                HashCode.Combine(hashcode, value.GetHashCode()));

    protected abstract IEnumerable<object> GetAtomicValues();

    private bool ValuesAreEqual(ValueObjectBase valueObjectBase) =>
        GetAtomicValues().SequenceEqual(valueObjectBase.GetAtomicValues());
}