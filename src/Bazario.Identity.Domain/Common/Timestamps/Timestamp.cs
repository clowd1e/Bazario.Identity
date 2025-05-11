using Bazario.AspNetCore.Shared.Domain;
using Bazario.AspNetCore.Shared.Results;

namespace Bazario.Identity.Domain.Common.Timestamps
{
    public sealed class Timestamp : ValueObject
    {
        private Timestamp(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; }

        public static Result<Timestamp> Create(DateTime value)
        {
            return new Timestamp(value);
        }

        public static Result<Timestamp> UtcNow()
        {
            return new Timestamp(DateTime.UtcNow);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
