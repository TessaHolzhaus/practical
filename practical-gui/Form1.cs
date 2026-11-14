using System.Text;
using Practical;
using Practical.Commands;
using Practical.ProgramVisitors;

namespace practical_gui;

public partial class Form1 : Form
{
    private readonly WorldControl _worldControl;
    
    public Form1()
    {
        InitializeComponent();
        
        foreach (ToolStripMenuItem topItem in menuStrip.Items)
        {
            if (topItem.DropDown is ToolStripDropDownMenu dropDown)
            {
                dropDown.ShowImageMargin = false;
            }
        }

        _worldControl = new(PlayerPathView.Factory, new DefaultCellRenderer())
        {
            Dock = DockStyle.Fill
        };
        splitContainer1.Panel2.Controls.Add(_worldControl);
    }

    private void basicToolStripMenuItem_Click(object sender, EventArgs e) 
        => LoadExample("basic.txt");

    private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadExample("advanced.txt");

    private void expertToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadExample("expert.txt");

    private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Select a Program File",
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        };

        if (dialog.ShowDialog() is DialogResult.OK)
        {
            LoadProgramFile(dialog.FileName);
        }
    }
    
    private const string CodeExamplesPath = "../../../../practical/CodeExamples";
    private void LoadExample(string fileName)
    {
        var fullPath = Path.Combine(CodeExamplesPath, fileName);
        LoadProgramFile(fullPath);
    }

    private void LoadProgramFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"File not found:\n{Path.GetFullPath(filePath)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProgramTextBox.Text = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not read file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public List<ICommand> GetProgram()
    {
        using TextReader reader = new StringReader(ProgramTextBox.Text);
        return new CommandParser().CommandParse(reader);
    }

    private void MetricsButton_Click(object sender, EventArgs e)
    {
        using TextReader reader = new StringReader(ProgramTextBox.Text);
        var program = GetProgram();
        OutputTextBox.Text = CreateMetrics(program);
    }

    private static string CreateMetrics(List<ICommand> program)
    {
        CommandCountMetric ccm = new();
        ccm.Visit(program);
        NestedCountMetric ncm = new();
        ncm.Visit(program);
        RepeatCountMetric rcm = new();
        rcm.Visit(program);
        
        static string FormatMetric(string label, int value) => $"{label,-20}{value}";    
        
        var formatedMetrics = new StringBuilder();
        formatedMetrics.AppendLine(FormatMetric("Number of commands:", ccm.CommandCount));
        formatedMetrics.AppendLine(FormatMetric("Max Nesting:", ncm.MaxNesting));
        formatedMetrics.AppendLine(FormatMetric("Number of repeats:", rcm.RepeatCount));
        return formatedMetrics.ToString();
    }

    private void RunButton_Click(object sender, EventArgs e)
    {
        var program = GetProgram();
        var world = _worldControl.World; //TODO: Reset player position and direction on click. 

        if (world is null) return;
        
        world.Player.Direction = Direction.East;
        world.Player.Position = new Coord(0, 0);
        
        foreach (var command in program)
        {
            command.Execute(world);
        }

        // Console.WriteLine(logger.LoggedActions);
        OutputTextBox.Text = new StringBuilder().Append("End state (")
            .Append(world.Player.Position.X)
            .Append(",")
            .Append(world.Player.Position.Y)
            .Append(") facing ")
            .Append(world.Player.Direction)
            .ToString();
    }
}