﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models
{
    public partial class LogsDBContext : DbContext
    {
        private IHostingEnvironment _hostingEnvironment;
        public LogsDBContext(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public LogsDBContext(DbContextOptions<LogsDBContext> options)
            : base(options)
        {
        }

        public DbSet<TransactionLogs> TransactionLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var parent = System.IO.Directory.GetParent(_hostingEnvironment.ContentRootPath).FullName;
                var path = Path.Combine(parent, @"DAL\DataBase\DatabaseLogs.mdf");
                optionsBuilder.UseSqlServer(@$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={path};Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionLogs>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            base.OnModelCreating(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}