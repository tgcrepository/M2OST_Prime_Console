// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.AssessmentList
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
    public class AssessmentList
    {
        public int id_assessment_sheet { get; set; }

        public int id_assessment { get; set; }

        public string assessment_name { get; set; }

        public string assessment_description { get; set; }

        public string expiry_date { get; set; }

        public int? assessment_type { get; set; }

        public string answer_description { get; set; }

        public int IsCourseCompleted { get; set; }
        public int IsContentRead { get; set; }
        public string LastAttemptGap { get; set; }
    }
}
