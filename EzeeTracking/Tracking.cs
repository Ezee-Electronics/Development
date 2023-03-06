// Decompiled with JetBrains decompiler
// Type: wfaEzeeTracking.Tracking
// Assembly: EzeeTracking, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2B71D31-42BC-4314-AD74-C95526D885BF
// Assembly location: C:\Users\Mikhail\Downloads\EzeeTracking .exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace wfaEzeeTracking
{
  internal class Tracking
  {
    private FormMain _mainFrom;
    private Log _log;
    private List<TrackItem> _items;
    private List<TrackItem> _itemNotDelivered;
    private string endLine = "\n";
    private string wrapper = "\"";
    private string delimiter = ",";
    private bool isOK = true;

    public Tracking(FormMain mainFrom, Log log)
    {
      this._mainFrom = mainFrom;
      this._log = log;
      this._items = new List<TrackItem>();
      this._itemNotDelivered = new List<TrackItem>();
    }

    public List<TrackItem> getTrackNumbers() => this._items;

    public int ReadData(string file)
    {
      int num1 = 0;
      try
      {
        this._log.AddToLog("Opening file - " + file);
        string[] strArray = new StreamReader((Stream) System.IO.File.OpenRead(file)).ReadToEnd().Split(Convert.ToChar(this.endLine));
        for (int index1 = 1; index1 < strArray.Length - 1; ++index1)
        {
          string[] data = strArray[index1].Split(Convert.ToChar(this.delimiter));
          for (int index2 = 0; index2 < data.Length; ++index2)
            data[index2] = data[index2].Trim(Convert.ToChar(this.wrapper));
          this._items.Add(this.createTrackNumber(data));
          ++num1;
        }
        this._log.AddToLog("Reading file is complete. Count of Tracknig numbers " + num1.ToString());
        return num1;
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(ex.Message);
        this._log.AddToLog("Count of Tracking numbers" + num1.ToString() + Environment.NewLine + "ERROR:" + ex.Message);
        return num1;
      }
    }

    public bool isGood() => this.isOK;

    private TrackItem createTrackNumber(string[] data)
    {
      TrackItem trackNumber = new TrackItem(this);
      try
      {
        trackNumber.TrackNumber = data[0];
        trackNumber.Status = "0";
        trackNumber.Carrier = this.getCarrier(data[0]);
      }
      catch (Exception ex)
      {
        this.isOK = false;
        int num = (int) MessageBox.Show(ex.Message);
        this._log.AddToLog("Create object of Tracking Number " + data[0]);
        this._log.AddToLog("ERROR: " + ex.Message);
      }
      return trackNumber;
    }

    private string getCarrier(string TrackNumber)
    {
      string carrier = "none";
      switch (TrackNumber.Substring(0, 1))
      {
        case "2":
          carrier = "Fedex";
          break;
        case "9":
          carrier = "USPS";
          break;
        case "1":
          carrier = "UPS";
          break;
      }
      return carrier;
    }

    public void checkStatus()
    {
      this._mainFrom.labelCheckingStatus.Text = "Status checking";
      this._mainFrom.progressBar.Maximum = this._mainFrom.itemsQty + 1;
      this._mainFrom.progressBar.Step = 1;
      this._mainFrom.Update();
      int num1 = 0;
      this._log.AddToLog("Status checking...");
      try
      {
        foreach (TrackItem trackItem in this._items)
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.bing.com/packagetrackingv2?packNum=" + trackItem.TrackNumber + "&carrier=" + trackItem.Carrier + "&FORM=PCKTR1");
          httpWebRequest.Method = "GET";
          string end = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd();
          int num2;
          if (end == "")
          {
            Log log = this._log;
            string trackNumber = trackItem.TrackNumber;
            num2 = this._mainFrom.progressBar.Value;
            string str = num2.ToString();
            string Text = "Respond form server is null. " + trackNumber + ". Row " + str;
            log.AddToLog(Text);
            this._itemNotDelivered.Add(trackItem);
            num2 = this._mainFrom.progressBar.Value++;
            ++num1;
            this._mainFrom.labelFedex.Visible = true;
            this._mainFrom.labelFedex.Text = "NOT CHECKED Tracking numbers: " + num1.ToString();
            this._mainFrom.Update();
          }
          else
          {
            string str = end.Substring(77, 9);
            if (str == "Delivered")
            {
              trackItem.Status = "Delivered";
            }
            else
            {
              trackItem.Status = str;
              this._itemNotDelivered.Add(trackItem);
            }
            num2 = this._mainFrom.progressBar.Value++;
            Log log = this._log;
            string[] strArray = new string[7];
            num2 = this._mainFrom.progressBar.Value;
            strArray[0] = num2.ToString();
            strArray[1] = " ";
            strArray[2] = trackItem.TrackNumber;
            strArray[3] = " ";
            strArray[4] = trackItem.Carrier;
            strArray[5] = " ";
            strArray[6] = trackItem.Status;
            string Text = string.Concat(strArray);
            log.AddToLog(Text);
          }
        }
        this._log.AddToLog("Status checking is complete");
        if (num1 > 0)
        {
          int num3 = (int) MessageBox.Show(this._mainFrom.labelFedex.Text + ". Check Log File for details");
        }
        this.report(this._itemNotDelivered);
      }
      catch (Exception ex)
      {
        this.isOK = false;
        int num4 = (int) MessageBox.Show(ex.Message);
        this._log.AddToLog("ERROR " + ex.Message);
        this.report(this._itemNotDelivered);
      }
    }

    private void report(List<TrackItem> items)
    {
      try
      {
        this._mainFrom.labelCheckingStatus.Text = "Creating report";
        this._mainFrom.progressBar.Maximum = items.Count + 1;
        this._mainFrom.progressBar.Step = 1;
        this._mainFrom.progressBar.Value = 0;
        this._log.AddToLog("Creating Report...");
        string path = Application.StartupPath + "/report.csv";
        string str = ", ";
        if (!System.IO.File.Exists(path))
        {
          string contents = "Tracking Number" + str + "Carrier" + str + "Status" + Environment.NewLine;
          System.IO.File.WriteAllText(path, contents);
        }
        else
        {
          System.IO.File.Delete(Application.StartupPath + "/report.csv");
          string contents = "Tracking Number" + str + "Carrier" + str + "Status" + Environment.NewLine;
          System.IO.File.WriteAllText(path, contents);
        }
        foreach (TrackItem trackItem in items)
        {
          string contents = "\"" + trackItem.TrackNumber + "\"" + str + "\"" + trackItem.Carrier + "\"" + str + "\"" + trackItem.Status + "\"" + Environment.NewLine;
          System.IO.File.AppendAllText(path, contents);
          ++this._mainFrom.progressBar.Value;
        }
        this._log.AddToLog("Creating report is complete. " + items.Count.ToString() + " tracking numbers");
      }
      catch (Exception ex)
      {
        this.isOK = false;
        int num = (int) MessageBox.Show(ex.Message);
        this._log.AddToLog("ERROR " + ex.Message);
      }
    }
  }
}
