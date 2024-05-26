using Microsoft.EntityFrameworkCore;
using RingoMedia_test.Models;
namespace RingoMedia_test.Data
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<SubDepartment>()
        //        .HasOne(SubD =>SubD.Parent)
        //        .WithMany(SubD => SubD.Children)
        //        .HasForeignKey(SubD => SubD.ParentId);
        //    modelBuilder.Entity<SubDepartment>()
        //        .HasOne(SubD =>SubD.Department)
        //        .WithMany(SubD => SubD.SubDepartments)
        //        .HasForeignKey(SubD => SubD.DepartmentId);
        //}
    }
}
