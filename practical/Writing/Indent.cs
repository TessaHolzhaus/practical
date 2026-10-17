namespace Practical.Writing;

public record Indent(char Char, int RepetitionsPerIndent = 1)
{
    public override string ToString() => new(Char, RepetitionsPerIndent);
}

public static class IndentExtensions
{
    public static void WriteIndent(this TextWriter writer, Indent indent, int nesting)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(nesting);
        
        int count = indent.RepetitionsPerIndent * nesting;

        for (int i = 0; i < count; i++)
            writer.Write(indent.Char);
    }
}