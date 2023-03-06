// Decompiled with JetBrains decompiler
// Type: wfaEzeeTracking.Log
// Assembly: EzeeTracking, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2B71D31-42BC-4314-AD74-C95526D885BF
// Assembly location: C:\Users\Mikhail\Downloads\EzeeTracking .exe

using System;
using System.IO;
using System.Windows.Forms;

namespace wfaEzeeTracking
{
  internal class Log
  {
    private string path = Application.StartupPath + "/log.txt";

    public Log() => this.CreateLogFile();

    private void CreateLogFile()
    {
      try
      {
        if (File.Exists(this.path))
          return;
        File.WriteAllText(this.path, "Log Ezee Tracking " + Environment.NewLine);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void AddToLog(string Text)
    {
      try
      {
        if (File.Exists(this.path))
          File.AppendAllText(this.path, DateTime.Now.ToString() + " - " + Text + Environment.NewLine);
        else
          File.WriteAllText(this.path, "Log Ezee Tracking " + DateTime.Now.ToString() + Environment.NewLine);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void CloseLog()
    {
      try
      {
        if (!File.Exists(this.path))
          return;
        string contents = "============================================================" + Environment.NewLine;
        string newLine = Environment.NewLine;
        File.AppendAllText(this.path, contents);
        File.AppendAllText(this.path, newLine);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }
  }
}
