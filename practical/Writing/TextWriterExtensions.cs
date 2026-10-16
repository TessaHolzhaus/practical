namespace Practical.Writing;

public static class TextWriterExtensions
{
    public static void WriteSpace(this TextWriter writer)
        => writer.Write(' ');
}