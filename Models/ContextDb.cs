using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

public class ContextDb : DbContext
{
    public ContextDb(DbContextOptions<UserDb> options)
        : base(options) { }

    public DbSet<User> User => Set<User>();
    public DbSet<Job> Job => Set<Job>();
    public DbSet<SchedulerLog> SchedulerLog => Set<SchedulerLog>();
    public DbSet<UserInfo> UserInfo => Set<UserInfo>();
}
