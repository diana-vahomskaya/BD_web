using Microsoft.EntityFrameworkCore;

namespace Workers.Models
{
    public partial class WorkersContext : DbContext
    {
        public WorkersContext(DbContextOptions<WorkersContext> options)
            : base(options) { }
       
       

        public WorkersContext() { }
        public virtual DbSet<WorkersModel> WorkersTable { get; set; }
    }
}
