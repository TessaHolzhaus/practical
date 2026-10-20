using Practical.ProgramVisitors;
namespace Practical.Commands;

public interface ICommand
{ 
    public void Accept(ProgramVisitor visitor);
    public void Execute(Player p) {}
}