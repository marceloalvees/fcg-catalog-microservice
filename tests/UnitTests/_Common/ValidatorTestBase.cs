using FluentValidation;
using NSubstituteAutoMocker;

namespace UnitTests._Common
{
    public abstract class ValidatorTestBase<TValidator>(FCGFixture fixture) : TestBase(fixture)
        where TValidator : class, IValidator
    {
        protected readonly NSubstituteAutoMocker<TValidator> AutoMocker = new();

        protected TValidator Validator => AutoMocker.ClassUnderTest;
    }
}
