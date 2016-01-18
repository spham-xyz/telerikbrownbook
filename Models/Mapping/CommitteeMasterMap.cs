using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LW1190.Models.Mapping
{
   public class CommitteeMasterMap : EntityTypeConfiguration<CommitteeMaster>
   {
      public CommitteeMasterMap()
      {
         // Primary Key
         this.HasKey(t => t.cnum);

         // Properties
         this.Property(t => t.cnum)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

         // Table & Column Mappings
         this.ToTable("lw_pds_comm_master");
         this.Property(t => t.ccode).HasColumnName("ccode");
         this.Property(t => t.cdescription).HasColumnName("cdescription");
         this.Property(t => t.cnum).HasColumnName("cnum");

      }
   }
}