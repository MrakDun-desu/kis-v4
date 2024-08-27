using FluentAssertions;
using FluentAssertions.Primitives;
using OneOf;
using OneOf.Types;

namespace BL.EF.Tests.Assertions;

public class OneOfAssertions(IOneOf subject) :
    ReferenceTypeAssertions<IOneOf, OneOfAssertions>(subject)
{
    protected override string Identifier => "oneOf";

    [CustomAssertion]
    public AndConstraint<OneOfAssertions> BeSuccess(string because = "", params object[] becauseArgs)
    {
        Subject.Value.Should().BeEquivalentTo(new Success(), because, becauseArgs);

        return new AndConstraint<OneOfAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<OneOfAssertions> BeNotFound(string because = "", params object[] becauseArgs)
    {
        Subject.Value.Should().BeEquivalentTo(new NotFound(), because, becauseArgs);

        return new AndConstraint<OneOfAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<OneOfAssertions> HaveValue(object? value, string because = "", params object[] becauseArgs)
    {
        Subject.Value.Should().BeEquivalentTo(value, because, becauseArgs);

        return new AndConstraint<OneOfAssertions>(this);
    }
}