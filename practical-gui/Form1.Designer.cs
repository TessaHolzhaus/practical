namespace practical_gui;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        bindingSource1 = new System.Windows.Forms.BindingSource(components);
        splitContainer1 = new System.Windows.Forms.SplitContainer();
        tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
        RunButton = new System.Windows.Forms.Button();
        MetricsButton = new System.Windows.Forms.Button();
        ProgramTextBox = new System.Windows.Forms.TextBox();
        OutputTextBox = new System.Windows.Forms.TextBox();
        menuStrip = new System.Windows.Forms.MenuStrip();
        loadProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        basicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        expertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        fromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        flowLayoutPanel2.SuspendLayout();
        menuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // splitContainer1
        // 
        splitContainer1.BackColor = System.Drawing.SystemColors.Control;
        splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        splitContainer1.Location = new System.Drawing.Point(0, 33);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
        splitContainer1.Size = new System.Drawing.Size(1206, 545);
        splitContainer1.SplitterDistance = 402;
        splitContainer1.SplitterWidth = 10;
        splitContainer1.TabIndex = 9;
        splitContainer1.Text = "splitContainer1";
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(flowLayoutPanel2, 0, 1);
        tableLayoutPanel1.Controls.Add(ProgramTextBox, 0, 0);
        tableLayoutPanel1.Controls.Add(OutputTextBox, 0, 2);
        tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72F));
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28F));
        tableLayoutPanel1.Size = new System.Drawing.Size(402, 545);
        tableLayoutPanel1.TabIndex = 9;
        // 
        // flowLayoutPanel2
        // 
        flowLayoutPanel2.AutoSize = true;
        flowLayoutPanel2.Controls.Add(RunButton);
        flowLayoutPanel2.Controls.Add(MetricsButton);
        flowLayoutPanel2.Location = new System.Drawing.Point(3, 353);
        flowLayoutPanel2.Name = "flowLayoutPanel2";
        flowLayoutPanel2.Size = new System.Drawing.Size(248, 52);
        flowLayoutPanel2.TabIndex = 12;
        // 
        // RunButton
        // 
        RunButton.AutoSize = true;
        RunButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)51)), ((int)((byte)166)), ((int)((byte)76)));
        RunButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        RunButton.Font = new System.Drawing.Font("Tw Cen MT", 12F, System.Drawing.FontStyle.Bold);
        RunButton.ForeColor = System.Drawing.Color.WhiteSmoke;
        RunButton.Location = new System.Drawing.Point(3, 3);
        RunButton.Name = "RunButton";
        RunButton.Size = new System.Drawing.Size(118, 46);
        RunButton.TabIndex = 5;
        RunButton.Text = "Run";
        RunButton.UseVisualStyleBackColor = false;
        RunButton.Click += RunButton_Click;
        // 
        // MetricsButton
        // 
        MetricsButton.AutoSize = true;
        MetricsButton.BackColor = System.Drawing.Color.FromArgb(((int)((byte)0)), ((int)((byte)133)), ((int)((byte)252)));
        MetricsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        MetricsButton.Font = new System.Drawing.Font("Tw Cen MT", 12F, System.Drawing.FontStyle.Bold);
        MetricsButton.ForeColor = System.Drawing.Color.WhiteSmoke;
        MetricsButton.Location = new System.Drawing.Point(127, 3);
        MetricsButton.Name = "MetricsButton";
        MetricsButton.Size = new System.Drawing.Size(118, 46);
        MetricsButton.TabIndex = 6;
        MetricsButton.Text = "Metrics";
        MetricsButton.UseVisualStyleBackColor = false;
        MetricsButton.Click += MetricsButton_Click;
        // 
        // ProgramTextBox
        // 
        ProgramTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
        ProgramTextBox.Location = new System.Drawing.Point(3, 3);
        ProgramTextBox.Multiline = true;
        ProgramTextBox.Name = "ProgramTextBox";
        ProgramTextBox.Size = new System.Drawing.Size(396, 344);
        ProgramTextBox.TabIndex = 10;
        // 
        // OutputTextBox
        // 
        OutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
        OutputTextBox.Font = new System.Drawing.Font("Consolas", 9F);
        OutputTextBox.Location = new System.Drawing.Point(3, 411);
        OutputTextBox.Multiline = true;
        OutputTextBox.Name = "OutputTextBox";
        OutputTextBox.ReadOnly = true;
        OutputTextBox.Size = new System.Drawing.Size(396, 131);
        OutputTextBox.TabIndex = 11;
        // 
        // menuStrip
        // 
        menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
        menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { loadProgramToolStripMenuItem });
        menuStrip.Location = new System.Drawing.Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new System.Drawing.Size(1206, 33);
        menuStrip.TabIndex = 10;
        menuStrip.Text = "menuStrip1";
        // 
        // loadProgramToolStripMenuItem
        // 
        loadProgramToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        loadProgramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { basicToolStripMenuItem, advancedToolStripMenuItem, expertToolStripMenuItem, toolStripSeparator1, fromFileToolStripMenuItem });
        loadProgramToolStripMenuItem.Name = "loadProgramToolStripMenuItem";
        loadProgramToolStripMenuItem.Size = new System.Drawing.Size(142, 29);
        loadProgramToolStripMenuItem.Text = "Load program";
        // 
        // basicToolStripMenuItem
        // 
        basicToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        basicToolStripMenuItem.Name = "basicToolStripMenuItem";
        basicToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
        basicToolStripMenuItem.Text = "Basic";
        basicToolStripMenuItem.Click += basicToolStripMenuItem_Click;
        // 
        // advancedToolStripMenuItem
        // 
        advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
        advancedToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
        advancedToolStripMenuItem.Text = "Advanced";
        advancedToolStripMenuItem.Click += advancedToolStripMenuItem_Click;
        // 
        // expertToolStripMenuItem
        // 
        expertToolStripMenuItem.Name = "expertToolStripMenuItem";
        expertToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
        expertToolStripMenuItem.Text = "Expert";
        expertToolStripMenuItem.Click += expertToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
        // 
        // fromFileToolStripMenuItem
        // 
        fromFileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";
        fromFileToolStripMenuItem.Size = new System.Drawing.Size(196, 34);
        fromFileToolStripMenuItem.Text = "From file...";
        fromFileToolStripMenuItem.Click += fromFileToolStripMenuItem_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1206, 578);
        Controls.Add(splitContainer1);
        Controls.Add(menuStrip);
        Text = "Form1";
        ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        flowLayoutPanel2.ResumeLayout(false);
        flowLayoutPanel2.PerformLayout();
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    private System.Windows.Forms.ToolStripMenuItem loadProgramToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem basicToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem expertToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem;

    private System.Windows.Forms.MenuStrip menuStrip;

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Button RunButton;
    private System.Windows.Forms.Button MetricsButton;
    private System.Windows.Forms.TextBox ProgramTextBox;
    private System.Windows.Forms.TextBox OutputTextBox;

    private System.Windows.Forms.SplitContainer splitContainer1;

    private System.Windows.Forms.BindingSource bindingSource1;

    #endregion
}