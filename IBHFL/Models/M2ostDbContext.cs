// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.M2ostDbContext
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Data.Entity;

namespace IBHFL.Models
{
  public class M2ostDbContext : DbContext
  {
    static M2ostDbContext() => Database.SetInitializer<M2ostDbContext>((IDatabaseInitializer<M2ostDbContext>) null);

    public M2ostDbContext()
      : base("name=dbconnectionstring")
    {
    }
  }
}
