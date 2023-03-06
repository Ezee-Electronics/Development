// Decompiled with JetBrains decompiler
// Type: wfaEzeeTracking.FormMain
// Assembly: EzeeTracking, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2B71D31-42BC-4314-AD74-C95526D885BF
// Assembly location: C:\Users\Mikhail\Downloads\EzeeTracking .exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace wfaEzeeTracking
{
  public class FormMain : Form
  {
    private Tracking tracking;
    private Log log;
    public int itemsQty = 0;
    public string filePath;
    private IContainer components = (IContainer) null;
    private Label labelDataCount;
    private Button btnOpenFile;
    private Button btnStart;
    public ProgressBar progressBar;
    public Label labelFedex;
    public Label labelUps;
    public Label labelUsps;
    private OpenFileDialog openFileDialog1;
    public Label labelCheckingStatus;

    public FormMain()
    {
      this.InitializeComponent();
      this.log = new Log();
      this.tracking = new Tracking(this, this.log);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      this.btnOpenFile.Enabled = false;
      this.btnStart.Enabled = false;
      this.tracking.checkStatus();
      if (this.tracking.isGood())
      {
        int num = (int) MessageBox.Show("Done. Check your report");
        this.log.AddToLog("Cycle ended. OK");
        this.log.CloseLog();
        this.btnOpenFile.Enabled = true;
        this.btnStart.Enabled = true;
        this.progressBar.Value = 0;
      }
      else
      {
        int num = (int) MessageBox.Show("For better results, please, reopen the app and try again");
        this.log.AddToLog("Cycle ended. Has problems");
        this.log.CloseLog();
      }
    }

    private void btnOpenFile_Click(object sender, EventArgs e)
    {
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.itemsQty = this.tracking.ReadData(this.openFileDialog1.FileName);
      this.labelDataCount.Text = "Readed from file: " + this.itemsQty.ToString() + " Track Numbers";
      if (this.itemsQty > 0)
        this.btnStart.Enabled = true;
      else
        this.btnStart.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormMain));
      this.labelDataCount = new Label();
      this.progressBar = new ProgressBar();
      this.labelCheckingStatus = new Label();
      this.btnOpenFile = new Button();
      this.btnStart = new Button();
      this.labelFedex = new Label();
      this.labelUps = new Label();
      this.labelUsps = new Label();
      this.openFileDialog1 = new OpenFileDialog();
      this.SuspendLayout();
      this.labelDataCount.AutoSize = true;
      this.labelDataCount.Location = new Point(12, 24);
      this.labelDataCount.Name = "labelDataCount";
      this.labelDataCount.Size = new Size(76, 13);
      this.labelDataCount.TabIndex = 0;
      this.labelDataCount.Text = "File not loaded";
      this.progressBar.Location = new Point(12, 75);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(492, 23);
      this.progressBar.TabIndex = 1;
      this.labelCheckingStatus.AutoSize = true;
      this.labelCheckingStatus.Location = new Point(12, 59);
      this.labelCheckingStatus.Name = "labelCheckingStatus";
      this.labelCheckingStatus.Size = new Size(85, 13);
      this.labelCheckingStatus.TabIndex = 2;
      this.labelCheckingStatus.Text = "Checking Status";
      this.btnOpenFile.Location = new Point(152, 229);
      this.btnOpenFile.Name = "btnOpenFile";
      this.btnOpenFile.Size = new Size(75, 23);
      this.btnOpenFile.TabIndex = 3;
      this.btnOpenFile.Text = "Open File";
      this.btnOpenFile.UseVisualStyleBackColor = true;
      this.btnOpenFile.Click += new EventHandler(this.btnOpenFile_Click);
      this.btnStart.Enabled = false;
      this.btnStart.Location = new Point(265, 229);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(75, 23);
      this.btnStart.TabIndex = 4;
      this.btnStart.Text = "Start";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.labelFedex.AutoSize = true;
      this.labelFedex.Location = new Point(12, 122);
      this.labelFedex.Name = "labelFedex";
      this.labelFedex.Size = new Size(43, 13);
      this.labelFedex.TabIndex = 5;
      this.labelFedex.Text = "FedEx: ";
      this.labelFedex.Visible = false;
      this.labelUps.AutoSize = true;
      this.labelUps.Location = new Point(12, 146);
      this.labelUps.Name = "labelUps";
      this.labelUps.Size = new Size(32, 13);
      this.labelUps.TabIndex = 6;
      this.labelUps.Text = "UPS:";
      this.labelUps.Visible = false;
      this.labelUsps.AutoSize = true;
      this.labelUsps.Location = new Point(12, 170);
      this.labelUsps.Name = "labelUsps";
      this.labelUsps.Size = new Size(42, 13);
      this.labelUsps.TabIndex = 7;
      this.labelUsps.Text = "USPS: ";
      this.labelUsps.Visible = false;
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "CSV files|*.csv";
      this.openFileDialog1.Title = "Open file";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(516, 269);
      this.Controls.Add((Control) this.labelUsps);
      this.Controls.Add((Control) this.labelUps);
      this.Controls.Add((Control) this.labelFedex);
      this.Controls.Add((Control) this.btnStart);
      this.Controls.Add((Control) this.btnOpenFile);
      this.Controls.Add((Control) this.labelCheckingStatus);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.labelDataCount);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MaximumSize = new Size(532, 308);
      this.Name = nameof (FormMain);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Ezee Tracking";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
