using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

public class SchedulerLogDb : DbContext
{
    public SchedulerLogDb(DbContextOptions<SchedulerLogDb> options)
        : base(options) { }

    public DbSet<SchedulerLog> SchedulerLog => Set<SchedulerLog>();
}
