// Decompiled with JetBrains decompiler
// Type: EzeeBCInventory.Program
// Assembly: Ezee-InventoryUpdate_Win, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 38DB088E-0BD5-4F2B-AADB-C465C9966808
// Assembly location: C:\Users\Mikhail\Downloads\Ezee-InventoryUpdate - Copy\Ezee-InventoryUpdate_Win.dll

using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json.Nodes;
using System.Timers;


#nullable enable
namespace EzeeBCInventory
{
  public class Program
  {
    private static float seconds = 0.0f;
    private static Timer aTimer = new Timer();
    private static List<Item> FileItems;
    private static List<Item> WebItems;
    private Log log = new Log();
    private static string file = "inventory.tsv";
    private static string req1AllItemsFormSite = "https://api.bigcommerce.com/stores/1si1pxdrnc/v3/catalog/products?page=1&limit=250";
    private static string req2AllItemsFormSite = "https://api.bigcommerce.com/stores/1si1pxdrnc/v3/catalog/products?page=2&limit=250";

    private static void Main(string[] args)
    {
      Log.AddToLog("Start " + DateTime.Now.ToString());
      Program.aTimer.Elapsed += new ElapsedEventHandler(Program.OnTimedEvent);
      Program.aTimer.Interval = 1000.0;
      Program.aTimer.Enabled = true;
      Program.DownloadFile();
      Program.UpdateWebInventory(Program.file);
      Program.aTimer.Stop();
      Program.aTimer.Dispose();
      float num = Program.seconds;
      string str = "sec.";
      if ((double) Program.seconds > 60.0)
      {
        num = Program.seconds / 60f;
        str = "min.";
      }
      Log.AddToLog("Done. " + DateTime.Now.ToString() + ", Time: " + num.ToString() + " " + str);
      Log.CloseLog();
      Environment.Exit(0);
    }

    private static void OnTimedEvent(object? sender, ElapsedEventArgs e) => ++Program.seconds;

    private static void DownloadFile()
    {
      try
      {
        Log.AddToLog("Download inventory file...");
        using (WebClient webClient = new WebClient())
          webClient.DownloadFile("https://docs.google.com/spreadsheets/d/1BCvYn0nzNDIriaNqs8I7xms6wyLSnkS5fyJ78yBosKU/export?format=tsv", Program.file);
        Log.AddToLog("Inventory file was downloaded");
      }
      catch (Exception ex)
      {
        Log.AddToLog(ex.Message);
      }
    }

    private static void UpdateWebInventory(string file)
    {
      try
      {
        Program.WebItems = new List<Item>();
        Program.FileItems = new List<Item>();
        Program.FileItems = Program.ReadFile(file);
        Program.GetAllItemsFromWebSite();
        foreach (Item obj in Program.CompareQty(Program.WebItems, Program.FileItems))
          Program.Update(obj);
      }
      catch (Exception ex)
      {
        Log.AddToLog(ex.Message);
      }
    }

    private static void GetAllItemsFromWebSite()
    {
      Log.AddToLog("Start reading WebSite inventory...");
      List<Item> itemsFromWebSite1 = Program.GetAllItemsFromWebSite(Program.req1AllItemsFormSite);
      List<Item> itemsFromWebSite2 = Program.GetAllItemsFromWebSite(Program.req2AllItemsFormSite);
      foreach (Item obj in itemsFromWebSite1)
        Program.WebItems.Add(obj);
      foreach (Item obj in itemsFromWebSite2)
        Program.WebItems.Add(obj);
      Log.AddToLog("Total readed from Website " + Program.WebItems.Count.ToString());
    }

    private static List<Item> CompareQty(List<Item> web, List<Item> file)
    {
      int num1 = 0;
      int num2 = 0;
      List<Item> objList = new List<Item>();
      Log.AddToLog("Start comparing qty...");
      foreach (Item obj1 in web)
      {
        Log.AddToLogFull(obj1.upc + " - " + obj1.qty);
        ++num1;
        bool flag = false;
        foreach (Item obj2 in file)
        {
          if (obj1.upc == obj2.upc && obj1.upc.Length > 0)
          {
            ++num2;
            if (obj1.qty != obj2.qty)
            {
              flag = true;
              obj1.qty = obj2.qty;
              Log.AddToLogFull(obj1.upc + " - " + obj1.qty + " / " + obj2.upc + " - " + obj2.qty);
              break;
            }
            break;
          }
        }
        if (flag)
          objList.Add(obj1);
      }
      Log.AddToLog("Done!\nItems Checked - " + num1.ToString() + "\nUPC Matched - " + num2.ToString() + "\nNeed to update - " + objList.Count.ToString());
      return objList;
    }

    private static void Update(Item item)
    {
      RestClient restClient = new RestClient("https://api.bigcommerce.com/stores/1si1pxdrnc/v3/catalog/products/" + item.id + "/variants/" + item.variant);
      RestRequest request = new RestRequest();
      request.Method = Method.Put;
      request.AddHeader("Content-Type", "application/json");
      request.AddHeader("Accept", "application/json");
      request.AddHeader("X-Auth-Token", "rvj8qye06wngbpx32j9qipo7mb1rzl0");
      request.AddParameter("application/json", (object) ("{\n  \"inventory_level\": " + item.qty + "\n}"), ParameterType.RequestBody);
      RestResponse restResponse = restClient.Execute(request);
      if (restResponse.StatusCode == HttpStatusCode.OK)
        Log.AddToLog(item.upc + " " + item.name + " qty updated to " + item.qty);
      else
        Log.AddToLog(item.upc + " qty was not updated." + restResponse.StatusCode.ToString());
    }

    private static void CheckWebAndFileInventory(string file)
    {
      try
      {
        Program.WebItems = new List<Item>();
        Program.FileItems = new List<Item>();
        Program.FileItems = Program.ReadFile(file);
        Log.AddToLog("Total readed from File " + Program.FileItems.Count.ToString());
        Log.AddToLog("Start reading WebSite inventory...");
        List<Item> itemsFromWebSite1 = Program.GetAllItemsFromWebSite(Program.req1AllItemsFormSite);
        List<Item> itemsFromWebSite2 = Program.GetAllItemsFromWebSite(Program.req2AllItemsFormSite);
        foreach (Item obj in itemsFromWebSite1)
          Program.WebItems.Add(obj);
        foreach (Item obj in itemsFromWebSite2)
          Program.WebItems.Add(obj);
        Log.AddToLog("Total readed from Website " + Program.WebItems.Count.ToString());
        Program.CompareItems(Program.WebItems, Program.FileItems);
      }
      catch (Exception ex)
      {
        Log.AddToLog(ex.Message);
      }
    }

    private static List<Item> GetAllItemsFromWebSite(string req)
    {
      RestClient restClient = new RestClient(req);
      RestRequest request = new RestRequest();
      request.Method = Method.Get;
      request.AddHeader("Content-Type", "application/json");
      request.AddHeader("X-Auth-Token", "rvj8qye06wngbpx32j9qipo7mb1rzl0");
      return Program.DeserializeWebObject(restClient.Execute(request));
    }

    private static List<Item> DeserializeWebObject(RestResponse response)
    {
      List<Item> objList = new List<Item>();
      JsonNode jsonNode = (JsonNode) JsonNode.Parse(response.Content)["data"].AsArray();
      string[] separator = new string[2]{ "\n", ":" };
      char[] chArray = new char[6]
      {
        ' ',
        ',',
        '\'',
        '"',
        '"',
        '\r'
      };
      foreach (object obj1 in jsonNode.AsArray())
      {
        string[] strArray = obj1.ToString().Split(separator, StringSplitOptions.None);
        int index = 0;
        foreach (string str in strArray)
        {
          strArray[index] = str.Trim(chArray);
          ++index;
        }
        Item obj2 = new Item();
        int num = 0;
        foreach (string str in strArray)
        {
          if (str == "id")
            obj2.id = strArray[num + 1];
          if (str == "name")
            obj2.name = strArray[num + 1];
          if (str == "sku")
            obj2.sku = strArray[num + 1];
          if (str == "inventory_level")
            obj2.qty = strArray[num + 1];
          if (str == "upc")
            obj2.upc = strArray[num + 1];
          if (str == "base_variant_id")
            obj2.variant = strArray[num + 1];
          ++num;
        }
        objList.Add(obj2);
      }
      return objList;
    }

    private static List<Item> ReadFile(string file)
    {
      try
      {
        List<Item> objList = new List<Item>();
        int num = 0;
        Log.AddToLog("Start reading file " + file + "...");
        using (StreamReader streamReader = new StreamReader(file))
        {
          while (!streamReader.EndOfStream)
          {
            string str = streamReader.ReadLine();
            string[] strArray = str.Split('\t');
            if (str != null && !strArray[1].Contains("QTY"))
            {
              ++num;
              objList.Add(new Item()
              {
                qty = strArray[1],
                upc = strArray[3].Length != 11 ? strArray[3] : "0" + strArray[3]
              });
            }
          }
        }
        Log.AddToLog("Total readed from File " + objList.Count.ToString());
        return objList;
      }
      catch (Exception ex)
      {
        Log.AddToLog(ex.Message);
        Environment.Exit(0);
        return (List<Item>) null;
      }
    }

    private static void CompareItems(List<Item> WebList, List<Item> FileList)
    {
      int num1 = 0;
      int num2 = 0;
      Log.AddToLog("Start comparing...");
      foreach (Item web in WebList)
      {
        ++num2;
        bool flag = false;
        foreach (Item file in FileList)
        {
          if (web.upc == file.upc)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Log.AddToLog(web.upc + " " + web.name + " " + web.qty);
          ++num1;
        }
      }
      Log.AddToLog("Done. Checked Items " + num2.ToString() + ". Find mismatches " + num1.ToString());
    }
  }
}
