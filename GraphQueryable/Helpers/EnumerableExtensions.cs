using System.Collections;
using System.Linq;

namespace GraphQueryable.Helpers
{
    internal class ValueEquality
    {
        public static bool Equal<T>(T first, T second)
        {
            if (ReferenceEquals(null, first))
                return ReferenceEquals(null, second);

            if (first is IEnumerable enumerableFirst && second is IEnumerable enumerableSecond)
                return enumerableFirst.Cast<object>().SequenceEqual(enumerableSecond.Cast<object>());

            return first.Equals(second);
        }

        public static int GetHashCode<T>(T first)
        {
            if (ReferenceEquals(null, first))
                return 0;
            
            if (first is IEnumerable enumerableFirst)
            {
                unchecked
                {
                    return enumerableFirst.Cast<object>()
                        .Aggregate(0, (agg, curr) => (agg * 397) ^ (curr?.GetHashCode() ?? 0));
                }
            }

            return first.GetHashCode();
        }
    }
}