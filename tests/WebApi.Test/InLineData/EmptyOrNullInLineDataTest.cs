using System.Collections;

namespace WebApi.Test.InLineData;

public class EmptyOrNullInLineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "" };
        yield return new object[] { "          " };
        yield return new object[] { null };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}