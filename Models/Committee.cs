using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;

namespace LW1190.Models
{
   public partial class Committee
   {
      public string tkinit { get; set; }
      public int cnum { get; set; }
      public string cchair { get; set; }

      //[ForeignKey("tkinit")]
      public virtual Attorney Attorney { get; set; }
      //[ForeignKey("cnum")]
      public virtual CommitteeMaster CommitteeMaster { get; set; }

   }
}