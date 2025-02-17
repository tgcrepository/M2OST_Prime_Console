// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.AnswerResponse
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Collections.Generic;

namespace IBHFL.Models
{
    public class AnswerResponse
    {
        public int ID_CONTENT { get; set; }

        public int ID_CATEGORY { get; set; }

        public int ID_THEME { get; set; }

        public string CONTENT_TITLE { get; set; }

        public string CONTENT_QUESTION { get; set; }

        public string EXPIRYDATE { get; set; }

        public int ID_CONTENT_ANSWER { get; set; }

        public string CONTENT_ANSWER_TITLE { get; set; }

        public string CONTENT_ANSWER_HEADER { get; set; }

        public string CONTENT_ANSWER1 { get; set; }

        public string CONTENT_ANSWER2 { get; set; }

        public string CONTENT_ANSWER3 { get; set; }

        public string CONTENT_ANSWER4 { get; set; }

        public string CONTENT_ANSWER5 { get; set; }

        public string CONTENT_ANSWER6 { get; set; }

        public string CONTENT_ANSWER7 { get; set; }

        public string CONTENT_ANSWER8 { get; set; }

        public string CONTENT_ANSWER9 { get; set; }

        public string CONTENT_ANSWER10 { get; set; }

        public string CONTENT_ANSWER_IMG1 { get; set; }

        public string CONTENT_ANSWER_IMG2 { get; set; }

        public string CONTENT_ANSWER_IMG3 { get; set; }

        public string CONTENT_ANSWER_IMG4 { get; set; }

        public string CONTENT_ANSWER_IMG5 { get; set; }

        public string CONTENT_ANSWER_IMG6 { get; set; }

        public string CONTENT_ANSWER_IMG7 { get; set; }

        public string CONTENT_ANSWER_IMG8 { get; set; }

        public string CONTENT_ANSWER_IMG9 { get; set; }

        public string CONTENT_ANSWER_IMG10 { get; set; }

        public string CONTENT_ANSWER_BANNER { get; set; }

        public string BANNER_REDIRECTION_URL { get; set; }

        public string CONTENT_ANSWER_COUNTER { get; set; }

        public bool HAS_ANSWER_STEP { get; set; }

        public List<SearchResponce> LinkedQuestion { get; set; }

        public List<SearchResponce> RelatedQuestion { get; set; }

        public bool has_feedback { get; set; }

        public int ID_FEEDBACK_BANK { get; set; }

        public string FEEDBACK_NAME { get; set; }

        public string FEEDBACK_QUESTION { get; set; }

        public string FEEDBACK_CHOICES { get; set; }

        public string FEEDBACK_IMAGE { get; set; }

        public string STATUS { get; set; }

        public string MESSAGE { get; set; }

        public string ASSESSMENT_FLAG { get; set; }

        public string CONTENT_BANNER { get; set; }

        public string CONTENT_BANNER_URL { get; set; }

        public string CONTENT_BANNER_IMG { get; set; }

        public double video_timer { get; set; }
        public int ID_ORGANIZATION { get; set; }
    }
}
