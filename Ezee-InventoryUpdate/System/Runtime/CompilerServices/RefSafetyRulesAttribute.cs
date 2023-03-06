// Decompiled with JetBrains decompiler
// Type: System.Runtime.CompilerServices.RefSafetyRulesAttribute
// Assembly: Ezee-InventoryUpdate_Win, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 38DB088E-0BD5-4F2B-AADB-C465C9966808
// Assembly location: C:\Users\Mikhail\Downloads\Ezee-InventoryUpdate - Copy\Ezee-InventoryUpdate_Win.dll

using Microsoft.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
  [CompilerGenerated]
  [Embedded]
  [AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
  internal sealed class RefSafetyRulesAttribute : Attribute
  {
    public readonly int Version;

    public RefSafetyRulesAttribute([In] int obj0) => this.Version = obj0;
  }
}
