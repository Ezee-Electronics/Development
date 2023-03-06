// Decompiled with JetBrains decompiler
// Type: EzeeBCInventory.Log
// Assembly: Ezee-InventoryUpdate_Win, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 38DB088E-0BD5-4F2B-AADB-C465C9966808
// Assembly location: C:\Users\Mikhail\Downloads\Ezee-InventoryUpdate - Copy\Ezee-InventoryUpdate_Win.dll

using System;
using System.IO;


#nullable enable
namespace EzeeBCInventory
{
  internal class Log
  {
    private static string path = "Log.Log";
    private static string path1 = "LogByItems.Log";

    public Log() => Log.CreateLogFile();

    public static void CreateLogFile()
    {
      try
      {
        if (!File.Exists(Log.path))
        {
          string contents = "Log Ezee Inventory Update" + Environment.NewLine;
          File.WriteAllText(Log.path, contents);
        }
        if (File.Exists(Log.path1))
          return;
        string contents1 = "Full Log Ezee Inventory Update" + Environment.NewLine;
        File.WriteAllText(Log.path1, contents1);
      }
      catch (Exception ex)
      {
      }
    }

    public static void AddToLog(string Text)
    {
      try
      {
        if (File.Exists(Log.path))
        {
          string contents = DateTime.Now.ToString() + " - " + Text + Environment.NewLine;
          File.AppendAllText(Log.path, contents);
          Console.WriteLine(Text);
        }
        else
        {
          string contents = "Log Ezee Iventory Update " + DateTime.Now.ToString() + Environment.NewLine;
          File.WriteAllText(Log.path, contents);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static void CloseLog()
    {
      try
      {
        if (!File.Exists(Log.path))
          return;
        string contents = "============================================================" + Environment.NewLine;
        string newLine = Environment.NewLine;
        File.AppendAllText(Log.path, contents);
        File.AppendAllText(Log.path, newLine);
        File.AppendAllText(Log.path1, contents);
        File.AppendAllText(Log.path1, newLine);
      }
      catch (Exception ex)
      {
      }
    }

    public static void AddToLogFull(string Text)
    {
      try
      {
        if (File.Exists(Log.path1))
        {
          string contents = DateTime.Now.ToString() + " - " + Text + Environment.NewLine;
          File.AppendAllText(Log.path1, contents);
        }
        else
        {
          string contents = "Log Ezee Iventory Update " + DateTime.Now.ToString() + Environment.NewLine;
          File.WriteAllText(Log.path1, contents);
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
