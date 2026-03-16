using Microsoft.EntityFrameworkCore;

namespace cv_prep.Models;

public class UserInfoDb : DbContext
{
    public UserInfoDb(DbContextOptions<UserInfoDb> options)
        : base(options) { }

    public DbSet<UserInfo> UserInfo => Set<UserInfo>();
}
