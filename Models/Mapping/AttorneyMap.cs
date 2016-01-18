using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LW1190.Models.Mapping
{
   public class AttorneyMap : EntityTypeConfiguration<Attorney>
   {
      public AttorneyMap()
      {
         // Primary Key
         this.HasKey(t => t.tstk);

         // Properties
         this.Property(t => t.tstk)
             .IsRequired()
             .HasMaxLength(5);

         //this.Property(t => t.tfirst)
         //    .HasMaxLength(50);

         //this.Property(t => t.tlast)
         //    .HasMaxLength(50);

         // Table & Column Mappings
         this.ToTable("lw_pdsm");
         this.Property(t => t.tstk).HasColumnName("tstk");
         this.Property(t => t.tsloc).HasColumnName("tsloc");
         //this.Property(t => t.tsgr).HasColumnName("tsgr");
         this.Property(t => t.tsdob).HasColumnName("tsdob");
         //this.Property(t => t.tscommittee).HasColumnName("tscommittee");
         this.Property(t => t.tsunits).HasColumnName("tsunits");
         this.Property(t => t.tsunitval).HasColumnName("tsunitval");
         this.Property(t => t.tsadjunits).HasColumnName("tsadjunits");
         this.Property(t => t.tsfund).HasColumnName("tsfund");
         this.Property(t => t.tspfund).HasColumnName("tspfund");
         this.Property(t => t.tspstd).HasColumnName("tspstd");
         this.Property(t => t.tstatus).HasColumnName("tstatus");
         this.Property(t => t.tscomment).HasColumnName("tscomment");
         this.Property(t => t.tstype).HasColumnName("tstype");
         //this.Property(t => t.tssalary).HasColumnName("tssalary");
         this.Property(t => t.tsbonus).HasColumnName("tsbonus");
         this.Property(t => t.tsclass).HasColumnName("tsclass");
         this.Property(t => t.tsdeparture).HasColumnName("tsdeparture");
         this.Property(t => t.tsaudit_op).HasColumnName("tsaudit_op");
         this.Property(t => t.tsdepartdt).HasColumnName("tsdepartdt");
         //this.Property(t => t.tslocalsal).HasColumnName("tslocalsal");
         //this.Property(t => t.tslocalcurr).HasColumnName("tslocalcurr");
         this.Property(t => t.tsevalclass).HasColumnName("tsevalclass");
         this.Property(t => t.tsppgroup).HasColumnName("tsppgroup");
         this.Property(t => t.tsepadmindt).HasColumnName("tsepadmindt");
         //this.Property(t => t.tsppgid).HasColumnName("tsppgid");
         this.Property(t => t.tsBarState).HasColumnName("tsBarState");
         this.Property(t => t.ProperCaseName).HasColumnName("ProperCaseName");
         this.Property(t => t.IPLevel).HasColumnName("IPLevel");
         this.Property(t => t.LastModified).HasColumnName("LastModified");

         // Not to Map a CLR Property to a Column in the Database
         //this.Ignore(t => t.LocDesc);

      }

   }
}