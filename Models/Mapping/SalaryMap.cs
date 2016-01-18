using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LW1190.Models.Mapping
{
   public class SalaryMap : EntityTypeConfiguration<Salary>
   {
      public SalaryMap()
      {
         // Primary Key
         this.HasKey(t => new { t.TimeKeep, t.Seq_no });

         // Properties
         this.Property(t => t.TimeKeep)
             .IsRequired()
             .HasMaxLength(11);

         this.Property(t => t.Seq_no)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

         // Table & Column Mappings
         this.ToTable("lw_bb_salary");
         this.Property(t => t.TimeKeep).HasColumnName("TimeKeep");
         this.Property(t => t.Seq_no).HasColumnName("Seq_no");
         this.Property(t => t.LocalSalary).HasColumnName("LocalSalary");
         this.Property(t => t.CurrCode).HasColumnName("CurrCode");
         this.Property(t => t.EffectiveDate).HasColumnName("EffectiveDate");
         this.Property(t => t.Comment).HasColumnName("Comment");

         this.HasRequired(c => c.Attorney)
             .WithMany(a => a.Salaries)
             .HasForeignKey(p => p.TimeKeep)
             .WillCascadeOnDelete(true);

      }            
   }
}