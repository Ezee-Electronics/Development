// Decompiled with JetBrains decompiler
// Type: wfaEzeeTracking.TrackItem
// Assembly: EzeeTracking, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2B71D31-42BC-4314-AD74-C95526D885BF
// Assembly location: C:\Users\Mikhail\Downloads\EzeeTracking .exe

namespace wfaEzeeTracking
{
  internal class TrackItem
  {
    private Tracking _tracking;
    private string trackNumber;
    private string status;
    private string carrier;

    public TrackItem(Tracking tracking) => this._tracking = tracking;

    public string TrackNumber
    {
      get => this.trackNumber;
      set => this.trackNumber = value;
    }

    public string Status
    {
      get => this.status;
      set => this.status = value;
    }

    public string Carrier
    {
      get => this.carrier;
      set => this.carrier = value;
    }
  }
}
