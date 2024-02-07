using CarbonFootprint1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarbonFootprint1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Role> RoleTables { get; set; }
        public DbSet<Positions> PositionsTable { get; set; }
        public DbSet<PositionRoles> PositionRolesTable { get; set; }
        public DbSet<BranchDetails> FootprintTable { get; set; }

        public DbSet<ApplicationUser> ApplicationUserTable { get; set; }

        public DbSet<Branches> BranchesTable { get; set; }
        public DbSet<AirTravel> AirTravelTable { get; set; }

        public DbSet<Departments> DepartmentsTable { get; set; }
    }
}