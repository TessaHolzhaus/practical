using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;

string AskProgram()
{
    Console.WriteLine("What program do you want to run? (<path> | basic (1) | advanced (2) | expert (3)");
    var response = Console.ReadLine();
    
    var defaultPath = "../../../CodeExamples";
    return response switch
    {
        "1" => (defaultPath + "/Basic.txt"),
        "2" => (defaultPath + "/Advanced.txt"),
        "3" => (defaultPath + "/Expert.txt"),
        var path => File.Exists(response) ? response : AskProgram()
    };
}

var executePath = AskProgram();
StreamReader programReader = new(executePath);
var logger = new Logger();
var program = new CommandParser(logger).CommandParse(programReader);
DetermineAction();

void DetermineAction()
{
    Console.WriteLine("What do you want to do? execute (1) | metrics (2)");
    var response = Console.ReadLine();
    switch (response)
    {
        case "1":
            ExecuteCode(); 
            break;
        case "2":
            Metrics();
            break;
        default: 
            DetermineAction();
            break;
    }
}

void Metrics()
{
    CommandCountMetric ccm = new();
    ccm.Visit(program);
    NestedCountMetric ncm = new();
    ncm.Visit(program);
    RepeatCountMetric rcm = new();
    rcm.Visit(program);
    Console.WriteLine($"Number of commands: {ccm.CommandCount}");
    Console.WriteLine($"Max nesting: {ncm.MaxNesting}");
    Console.WriteLine($"Number of repeats: {rcm.RepeatCount}");
}

void ExecuteCode()
{
    var player = new Player();
    foreach (var command in program)
    {
        command.Execute(player);
    }

    Console.WriteLine(logger.LoggedActions);
    Console.WriteLine($"End state ({player.Position.X},{player.Position.Y}) facing {player.Direction}");
}