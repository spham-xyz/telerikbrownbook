using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;

namespace LW1190.Models
{
   public partial class Salary
   {
      public string TimeKeep { get; set; }
      public int Seq_no { get; set; }
      public Decimal LocalSalary { get; set; }
      public string CurrCode { get; set; }
      public DateTime EffectiveDate { get; set; }
      public string Comment { get; set; }

      //[ForeignKey("TimeKeep")]
      public virtual Attorney Attorney { get; set; }

   }
}