using Practical;
using Practical.Commands;


Console.WriteLine("What program do you want to run? (<path> | default)");
var response = Console.ReadLine() ?? throw new NullReferenceException("why null!!");

var programReader = response switch
{
    "default" => new StreamReader("../../../importedCommands.txt"),
    var path => new StreamReader(path)
};

var logger = new Logger();
var program = new CommandParser(logger).CommandParse(programReader);

var player = new Player();
foreach (var command in program)
{
    command.Execute(player);
}

Console.WriteLine(logger.LoggedActions);
Console.WriteLine($"End state ({player.Position.X},{player.Position.Y}) facing {player.Direction}");