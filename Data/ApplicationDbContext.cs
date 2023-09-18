using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database_RelationShips.Models;
using Database_RelationShips.ViewModel;

namespace Database_RelationShips.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Database_RelationShips.Models.Employee> Employee { get; set; } = default!;

        public DbSet<Database_RelationShips.Models.EmployeeAddress> EmployeeAddress { get; set; } = default!;

        public DbSet<Database_RelationShips.ViewModel.EmployeeViewModel> EmployeeViewModel { get; set; } = default!;
    }
}
