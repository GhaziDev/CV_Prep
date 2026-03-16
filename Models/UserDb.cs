using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options)
        : base(options) { }

    public DbSet<User> User => Set<User>();
}
