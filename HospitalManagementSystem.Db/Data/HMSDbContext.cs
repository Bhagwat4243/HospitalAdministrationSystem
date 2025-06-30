using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Db.Model.AuthModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Data
{
    public class HMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public HMSDbContext(DbContextOptions<HMSDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Appointment - Patient relation
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(u => u.PatientAppointments)   // ApplicationUser me ye collection hona chahiye
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Doctor relation
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u.DoctorAppointments)    // ApplicationUser me ye collection hona chahiye
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientLabTest>()
       .HasOne(p => p.Patient)
       .WithMany()
       .HasForeignKey(p => p.PatientId)
       .OnDelete(DeleteBehavior.Cascade); // or NoAction

            modelBuilder.Entity<PatientLabTest>()
                .HasOne(p => p.Doctor)
                .WithMany()
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Patient> Patient_Tbl { get; set; }
        public DbSet<Nurse> Nurse_Tbl { get; set; }
        public DbSet<LabTechnician> LabTechnician_Tbl { get; set; }
        public DbSet<Doctor> Doctor_Tbl { get; set; }
        public DbSet<Admin> Admin_Tbl { get; set; }
        public DbSet<Chemist> Chemist_Tbl { get; set; }
        public DbSet<Appointment> Appointment_Tbl { get; set; }
        public DbSet<Prescriptions> Prescription_Tbl { get; set; }
        public DbSet<PrescriptionDetails> PrescriptionDetail_Tbl { get; set; }
        public DbSet<LabTest> LabTest_Tbl { get; set; }
        public DbSet<PatientLabTest> PatientLabTest_Tbl { get; set; }
        public DbSet<AppointmentPayment> AppointmentPayment_Tbl { get; set; }
    }
}
