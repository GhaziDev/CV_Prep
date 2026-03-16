using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

public class JobDb : DbContext
{
    public JobDb(DbContextOptions<JobDb> options)
        : base(options) { }

    public DbSet<Job> Job => Set<Job>();
}
