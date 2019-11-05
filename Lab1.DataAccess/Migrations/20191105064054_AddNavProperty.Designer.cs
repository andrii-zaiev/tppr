﻿// <auto-generated />
using System;
using Lab1.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Lab1.DataAccess.Migrations
{
    [DbContext(typeof(PhoneDatabaseContext))]
    [Migration("20191105064054_AddNavProperty")]
    partial class AddNavProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lab1.DataAccess.Models.Parameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("Optimality");

                    b.Property<int>("Scale");

                    b.Property<int>("Type");

                    b.Property<string>("Unit")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.ParameterValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ParameterId");

                    b.Property<double>("Value");

                    b.Property<string>("ValueText")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ParameterId");

                    b.ToTable("ParameterValues");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.Phone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.PhoneParameterValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ParameterValueId");

                    b.Property<Guid>("PhoneId");

                    b.HasKey("Id");

                    b.HasIndex("ParameterValueId");

                    b.HasIndex("PhoneId");

                    b.ToTable("PhoneParameterValues");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.PhoneSelection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PhoneId");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PhoneId");

                    b.HasIndex("UserId");

                    b.ToTable("PhoneSelections");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Competence");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.ParameterValue", b =>
                {
                    b.HasOne("Lab1.DataAccess.Models.Parameter", "Parameter")
                        .WithMany()
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.PhoneParameterValue", b =>
                {
                    b.HasOne("Lab1.DataAccess.Models.ParameterValue", "ParameterValue")
                        .WithMany()
                        .HasForeignKey("ParameterValueId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab1.DataAccess.Models.Phone", "Phone")
                        .WithMany("PhoneParameterValues")
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lab1.DataAccess.Models.PhoneSelection", b =>
                {
                    b.HasOne("Lab1.DataAccess.Models.Phone", "Phone")
                        .WithMany()
                        .HasForeignKey("PhoneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab1.DataAccess.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
