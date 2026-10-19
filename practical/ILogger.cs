namespace Practical;

public interface ILogger
{
    public void LogExecution(string message);
    public void LogExecution(Action<TextWriter> action);
}

public class Logger : ILogger
{
    private readonly StringWriter _writer;
    private readonly List<string> _loggedActions;

    public Logger()
    {
        _writer = new StringWriter();
        _loggedActions = [];
    }


    public void LogExecution(string message)
        => _loggedActions.Add(message);

    public void LogExecution(Action<TextWriter> action)
    {
        _writer.GetStringBuilder().Clear();
        action(_writer);
        LogExecution(_writer.ToString());
    }

    public string LoggedActions => string.Join(", ", _loggedActions) + ".";

}