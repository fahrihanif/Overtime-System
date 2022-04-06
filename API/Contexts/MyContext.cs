using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<EmployeeOvertime> EmployeeOvertimes { get; set; }
        public DbSet<Overtime> Overtimes { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSnakeCaseNamingConvention();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .HasMany(e => e.Employees)
                .WithOne(j => j.Job)
                .HasForeignKey(fk => fk.JobId);

            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(fk => fk.NIK)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeOvertime>()
                .HasKey(pk => pk.Id);
            modelBuilder.Entity<EmployeeOvertime>()
                .HasOne(e => e.Employee)
                .WithMany(eo => eo.EmployeeOvertimes)
                .HasForeignKey(fk => fk.EmployeeId);
            modelBuilder.Entity<EmployeeOvertime>()
                .HasOne(o => o.Overtime)
                .WithMany(eo => eo.EmployeeOvertimes)
                .HasForeignKey(fk => fk.OvertimeId);

            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => ar.Id);
            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Account)
                .WithMany(a => a.AccountRoles)
                .HasForeignKey(a => a.AccountNIK)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Role)
                .WithMany(r => r.AccountRoles)
                .HasForeignKey(ar => ar.RoleId);
        }
    }
}
