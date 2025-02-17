// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.DisplayCategory
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Collections.Generic;

namespace IBHFL.Models
{
    public class DisplayCategory
    {
        public double CategoryHeadingCount { get; set; }

        public string HeadingID { get; set; }

        public string Heading { get; set; }

        public List<details> Categories { get; set; }

        public string Order { get; set; }
        public string NEXTAPIURL { get; set; }
    }
}
