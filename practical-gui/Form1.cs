using System.ComponentModel;
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

    private void repeatUntilToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadExample("repeatUntil.txt");
    
    private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadExample("goTo.txt");

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
        _worldControl.ResetPlayer();
        
        var program = GetProgram();
        var world = _worldControl.World;
        
        if (world is null) return;
        
        foreach (var command in program)
        {
            command.Execute(world);
        }

        OutputTextBox.Text = new StringBuilder().Append("End state (")
            .Append(world.Player.Position.X)
            .Append(",")
            .Append(world.Player.Position.Y)
            .Append(") facing ")
            .Append(world.Player.Direction)
            .ToString();
    }

    private void LoadWorld(World world)
    {
        _worldControl.World = world;
        _worldControl.ResetPlayer();
    }
    
    private void baseGridToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadWorld(World.Create());

    private void grid2ToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadWorld(World.Create("Grid2.txt"));

    private void grid3ToolStripMenuItem_Click(object sender, EventArgs e)
        => LoadWorld(World.Create("Grid3.txt"));
    
    private void normalToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Choose a location",
            Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() is DialogResult.OK)
        {
            SaveProgramFile(dialog.FileName);
        }
    }

    private void SaveProgramFile(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        
        var streamer = new CommandStreamer(writer);
        
        var program = GetProgram();
        
        streamer.Visit(program);
    }

    private void SafeProgramJson(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        
        var streamer = new JsonWriter(writer);
            
        var program = GetProgram();
            
        streamer.Visit(program);
    }

    private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Save as JSON file",
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            DefaultExt = "json",
            AddExtension = true
        };

        if (dialog.ShowDialog() is DialogResult.OK)
        {
            SafeProgramJson(dialog.FileName);
        }
    }
}