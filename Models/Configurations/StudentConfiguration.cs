using Diary.Models.Domains;
using System.Data.Entity.ModelConfiguration;

namespace Diary.Models.Configurations
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            ToTable("dbo.Students");
            HasKey(x => x.Id);

        }
    }
}
