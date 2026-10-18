using Practical.Commands;
using Practical.ProgramVisitors;
using Practical.Writing;

// List<ICommand> program =
// [
//     new Turn(RotationDirection.Left),
//     new Move(10),
//
//     new Repeat(3,
//     [
//         new Move(5),
//         new Turn(RotationDirection.Right),
//
//         new Repeat(2,
//         [
//             new Move(8),
//             new Turn(RotationDirection.Left),
//             new Move(3)
//         ]),
//
//         new Move(12),
//         new Turn(RotationDirection.Left)
//     ]),
//
//     new Move(20),
//     new Turn(RotationDirection.Right),
//
//     new Repeat(4,
//     [
//         new Repeat(2,
//         [
//             new Move(6),
//             new Turn(RotationDirection.Right)
//         ]),
//
//         new Move(10),
//         new Turn(RotationDirection.Left)
//     ]),
//
//     new Move(5)
// ];
List<ICommand> program = new CommandParser().CommandParse();
var programPrinter = new CommandStreamer(Console.Out) {
    Indent = new Indent(' ', 4)
};
programPrinter.Visit(program);