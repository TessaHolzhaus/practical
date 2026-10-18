namespace Practical.Commands;

public class CommandParser
{
    public List<ICommand> CommandParse() //TODO: Implement error checking
    {
        List<ICommand> commands = new();
        Stack<(int Depth, Repeat Command)> repeatStack = new();
        
        using StreamReader reader = new StreamReader("../../../importedCommands.txt");
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            int indentation = line.Length - line.TrimStart(' ').Length;
            if (indentation % 4 != 0)
            {
                throw new Exception("Indentation invalid");
            }
            
            int depth = indentation / 4;

            while (repeatStack.Count > 0 && repeatStack.Peek().Depth >= depth)
            {
                repeatStack.Pop();
            }

            int expectedDepth;
            if (repeatStack.Count == 0) expectedDepth = 0;
            else
            {
                expectedDepth = repeatStack.Peek().Depth + 1;
            }

            if (depth != expectedDepth)
            {
                throw new Exception($"Invalid indentation in {line}");
            }
            
            ICommand command = GetCommand(line.TrimStart(' '));
            
            if (repeatStack.Count == 0) commands.Add(command);
            else
            {
                repeatStack.Peek().Command.AddCommand(command);
            }

            if (command is Repeat repeat)
            {
                repeatStack.Push((depth,repeat));
            }
        }
        return commands;
    }


    public ICommand GetCommand(string line)
    {
        string[] words = line.Split(' ');

        if (words.Length == 0)
        {
            throw new Exception("Empty command");
        }
        string commandName = words[0];
        string[] arguments = words[1..];
        ICommand command = commandName switch
        {
            "Move" => Move.Parse(arguments),
            "Turn" => Turn.Parse(arguments),
            "Repeat" => Repeat.Parse(arguments),
            _ => throw new Exception($"Unknown command: {commandName}")
        };
        return command;
    }
}