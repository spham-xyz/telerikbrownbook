using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LW1190.Models.Mapping
{
   public class CommitteeMap : EntityTypeConfiguration<Committee>
   {
      public CommitteeMap()
      {
         // Primary Key
         this.HasKey(t => new { t.tkinit, t.cnum });

         // Properties
         this.Property(t => t.tkinit)
             .IsRequired()
             .HasMaxLength(5);

         this.Property(t => t.cnum)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

         // Table & Column Mappings
         this.ToTable("lw_pdsm_comm");
         this.Property(t => t.tkinit).HasColumnName("tkinit");
         this.Property(t => t.cnum).HasColumnName("cnum");
         this.Property(t => t.cchair).HasColumnName("cchair");

         // http://msdn.microsoft.com/en-us/data/hh134698.aspx (Code First Relationships Fluent API)
         // http://msdn.microsoft.com/en-us/data/JJ591620.aspx#RequiredToOptional
         // http://social.msdn.microsoft.com/Forums/en-US/989aa3c7-f84c-4d90-9b16-2f33e7b018f8/ef-4-with-code-first-parent-child-relationship-invalid-column-error
         this.HasRequired(c => c.Attorney)
             .WithMany(a => a.Committees)
             .HasForeignKey(p => p.tkinit)
             .WillCascadeOnDelete(true);

         this.HasRequired(c => c.CommitteeMaster)
             .WithMany(m => m.Committees)
             .HasForeignKey(p => p.cnum)
             .WillCascadeOnDelete(false);

      }
   }
}