using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;

namespace LW1190.Models
{
   public partial class Attorney
   {
      // *** 'string' must be a non-nullable value type in order to use as parameter in AttorneyMap() ***
      public string tstk { get; set; }
      public string tsloc { get; set; }
      //public string? LocDesc { get; set; }                        // *** Not mapped to table lw_pdsm ***
      //public string tsgr { get; set; }                            //Not used
      public DateTime? tsdob { get; set; }
      //public string tscommittee { get; set; }                     //Not used 
      public Double? tsunits { get; set; }
      public Double? tsunitval { get; set; }
      public Double? tsadjunits { get; set; }
      public Double? tsfund { get; set; }
      public Double? tspfund { get; set; }
      public Double? tspstd { get; set; }
      public string tstatus { get; set; }
      public string tscomment { get; set; }
      public string tstype { get; set; }
      //public Double tssalary { get; set; }                        //Not used
      public Double? tsbonus { get; set; }
      public string tsclass { get; set; }
      public string tsdeparture { get; set; }
      public string tsaudit_op { get; set; }
      public DateTime? tsdepartdt { get; set; }
      //public Double tslocalsal { get; set; }                      //Not used
      //public string tslocalcurr { get; set; }                     //Not used
      public string tsevalclass { get; set; }
      public string tsppgroup { get; set; }
      public DateTime? tsepadmindt { get; set; }
      //public int tsppgid { get; set; }                            //Not used
      public string tsBarState { get; set; }
      public string ProperCaseName { get; set; }
      public int? IPLevel { get; set; }
      public DateTime? LastModified { get; set; }

      public virtual ICollection<Committee> Committees { get; set; }
      public virtual ICollection<Salary> Salaries { get; set; }

      public Attorney()
      {
         this.Committees = new List<Committee>();
         this.Salaries = new List<Salary>();
      }

   }
}