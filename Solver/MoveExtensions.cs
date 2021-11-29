static class MoveExtensions
{
    public static Move Complementary(this Move move) =>
        new Move(move.From.Complementary(), move.To.Complementary());

    public static Move Symmetrical(this Move move) =>
        new Move(move.From.Symmetrical(), move.To.Symmetrical())
            .Reverse();

    public static Move Reverse(this Move move) =>
        new Move(move.To, move.From);
}