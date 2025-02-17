// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.PasswordEncryption
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IBHFL.Models
{
  public static class PasswordEncryption
  {
    public static string ToMD5Hash(this byte[] bytes)
    {
      StringBuilder hash = new StringBuilder();
      ((IEnumerable<byte>) MD5.Create().ComputeHash(bytes)).ToList<byte>().ForEach((Action<byte>) (b => hash.AppendFormat("{0:x2}", (object) b)));
      return hash.ToString();
    }

    public static string ToMD5Hash(this string inputString) => Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
  }
}
