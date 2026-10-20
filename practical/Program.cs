using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;
using Practical.Writing;

var logger = new Logger();
// List<ICommand> program =
// [
//     new Turn(RotationDirection.Left) { Logger = logger },
//     new Move(10) { Logger = logger },
//
//     new Repeat(3,
//     [
//         new Move(5) { Logger = logger },
//         new Turn(RotationDirection.Right) { Logger = logger },
//
//         new Repeat(2,
//         [
//             new Move(8) { Logger = logger },
//             new Turn(RotationDirection.Left) { Logger = logger },
//             new Move(3) { Logger = logger }
//         ]),
//
//         new Move(12) { Logger = logger },
//         new Turn(RotationDirection.Left) { Logger = logger }
//     ]),
//
//     new Move(20) { Logger = logger },
//     new Turn(RotationDirection.Right) { Logger = logger },
//
//     new Repeat(4,
//     [
//         new Repeat(2,
//         [
//             new Move(6) { Logger = logger },
//             new Turn(RotationDirection.Right) { Logger = logger }
//         ]),
//
//         new Move(10) { Logger = logger },
//         new Turn(RotationDirection.Left) { Logger = logger }
//     ]),
//
//     new Move(5) { Logger = logger },
// ];

List<ICommand> program = new CommandParser().CommandParse();
var programPrinter = new CommandStreamer(Console.Out) {
    Indent = new Indent(' ', 4)
};

var player = new Player();
foreach (var command in program)
{
    command.Execute(player);
}

Console.WriteLine(logger.LoggedActions);
Console.WriteLine($"End state ({player.Position.X},{player.Position.Y}) facing {player.Direction}");