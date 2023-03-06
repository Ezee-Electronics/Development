// Decompiled with JetBrains decompiler
// Type: wfaEzeeTracking.Program
// Assembly: EzeeTracking, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2B71D31-42BC-4314-AD74-C95526D885BF
// Assembly location: C:\Users\Mikhail\Downloads\EzeeTracking .exe

using System;
using System.Windows.Forms;

namespace wfaEzeeTracking
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new FormMain());
    }
  }
}
