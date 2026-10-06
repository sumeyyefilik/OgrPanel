using System.Data.Entity;

namespace OgrPanel
{
    public class OkulDBEntities : DbContext
    {
        public OkulDBEntities() : base("data source=.\\SQLEXPRESS;initial catalog=OkulDB;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<Student> Student { get; set; }
    }
}