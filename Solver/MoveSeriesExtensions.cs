using System.Collections.Generic;
using System.Linq;

static class MoveSeriesExtensions
{
    public static IEnumerable<Move> Complementary(this IEnumerable<Move> series) =>
        series
            .Select(MoveExtensions.Complementary);

    public static IEnumerable<Move> Symmetrical(this IEnumerable<Move> series) =>
        series
            .Select(MoveExtensions.Symmetrical)
            .Reverse();
}