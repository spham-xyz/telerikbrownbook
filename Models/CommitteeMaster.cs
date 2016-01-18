using System;
using System.Collections.Generic;

namespace LW1190.Models
{
   public partial class CommitteeMaster
   {
      public string ccode { get; set; }
      public string cdescription { get; set; }
      public int cnum { get; set; }

      public virtual ICollection<Committee> Committees { get; set; }

      public CommitteeMaster()
      {
         this.Committees = new List<Committee>();
      }

   }
}