using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Domain.Entities;


namespace TatiPharma.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<Customer>().ToTable("tblCustomer");
            modelBuilder.Entity<CustomerType>().ToTable("tblCustomerTypeMaster");  
            modelBuilder.Entity<Employee>().ToTable("tblEmployee");  
            modelBuilder.Entity<DrugMaster>().ToTable("tblDrugMaster");
            modelBuilder.Entity<DrugTypeMaster>().ToTable("tblDrugTypeMaster");
            modelBuilder.Entity<DosageForm>().ToTable("tblDosageForm");
            modelBuilder.Entity<Manufacturer>().ToTable("tblManufacturer");
            modelBuilder.Entity<LoginLogDetail>().ToTable("tblLoginLogDetails");
            modelBuilder.Entity<PaymentRecord>().ToTable("tblPaymentRecord");
            modelBuilder.Entity<PaymentMethod>().ToTable("tblPaymentRMethod");
            modelBuilder.Entity<PrinterMaster>().ToTable("tblPrinterMaster");
            modelBuilder.Entity<Purchase>().ToTable("tblPurchase");
            modelBuilder.Entity<PurchaseDetail>().ToTable("tblPurchaseDetail");
            modelBuilder.Entity<SalesInvoice>().ToTable("tblSalesInvoice");
            modelBuilder.Entity<SalesInvoiceDetail>().ToTable("tblSalesInvoiceDetail1");
            modelBuilder.Entity<Vendor>().ToTable("tblVender");
        }

        public DbSet<User>  Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }  // New
        public DbSet<Employee> Employees { get; set; }  // New
        public DbSet<DrugMaster> DrugMasters { get; set; }
        public DbSet<DrugTypeMaster> DrugTypeMasters { get; set; }
        public DbSet<DosageForm> DosageForms { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<LoginLogDetail> LoginLogDetails { get; set; }
        public DbSet<PaymentRecord> PaymentRecords { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PrinterMaster> PrinterMasters { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

    }
}
