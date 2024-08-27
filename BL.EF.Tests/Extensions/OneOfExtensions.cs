using BL.EF.Tests.Assertions;
using OneOf;

namespace BL.EF.Tests.Extensions;

public static class OneOfExtensions
{
    public static OneOfAssertions Should(this IOneOf instance)
    {
        return new OneOfAssertions(instance);
    }
}