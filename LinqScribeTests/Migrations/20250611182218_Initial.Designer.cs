﻿// <auto-generated />
using System;
using LinqScribeTests.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LinqScribeTests.Migrations
{
    [DbContext(typeof(LinqScribeClientDbContext))]
    [Migration("20250611182218_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GeoCoordinateId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GeoCoordinateId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExpectedSalary")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<decimal>("IndexFundsRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long>("NumberOfDays")
                        .HasColumnType("bigint");

                    b.Property<long>("NumberOfFriends")
                        .HasColumnType("bigint");

                    b.Property<byte>("NumberOfRelatives")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("NumberOfSeconds")
                        .HasColumnType("decimal(20,0)");

                    b.Property<short>("NumberOfSiblings")
                        .HasColumnType("smallint");

                    b.Property<short>("Salary")
                        .HasColumnType("smallint");

                    b.Property<double>("SocialInsuranceRate")
                        .HasColumnType("float");

                    b.Property<float>("TaxRate")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+GeoCoordinate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("GeoCoordinates");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Address", b =>
                {
                    b.HasOne("LinqScribeTests.Migrations.LinqScribeClientDbContext+GeoCoordinate", "GeoCoordinate")
                        .WithMany()
                        .HasForeignKey("GeoCoordinateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GeoCoordinate");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Customer", b =>
                {
                    b.HasOne("LinqScribeTests.Migrations.LinqScribeClientDbContext+Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+GeoCoordinate", b =>
                {
                    b.HasOne("LinqScribeTests.Migrations.LinqScribeClientDbContext+Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Order", b =>
                {
                    b.HasOne("LinqScribeTests.Migrations.LinqScribeClientDbContext+Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("LinqScribeTests.Migrations.LinqScribeClientDbContext+Customer", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
