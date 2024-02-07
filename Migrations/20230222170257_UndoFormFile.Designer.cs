﻿// <auto-generated />
using System;
using CarbonFootprint1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarbonFootprint1.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230222170257_UndoFormFile")]
    partial class UndoFormFile
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CarbonFootprint1.Models.BranchDetails", b =>
                {
                    b.Property<int>("CarbornFootprint")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarbornFootprint"), 1L, 1);

                    b.Property<string>("AlternativeSourceOfEnergy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("AmountPaidForDiesel")
                        .HasColumnType("float");

                    b.Property<int?>("AmountPaidForDieselGenerator")
                        .HasColumnType("int");

                    b.Property<double?>("AmountPaidForPetrol")
                        .HasColumnType("float");

                    b.Property<string>("BranchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("BranchSize")
                        .HasColumnType("float");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("CostOfDisposal")
                        .HasColumnType("float");

                    b.Property<double?>("CostOfDrinkableWaterConsumed")
                        .HasColumnType("float");

                    b.Property<double?>("CostOfPaperUsed")
                        .HasColumnType("float");

                    b.Property<double?>("CostOfRegularWaterConsumed")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("DieselCostPerLitre")
                        .HasColumnType("float");

                    b.Property<double?>("DieselGeneratorCO2Emission")
                        .HasColumnType("float");

                    b.Property<double?>("DieselGeneratorCO2KGS")
                        .HasColumnType("float");

                    b.Property<double?>("DieselQuantityConsumed")
                        .HasColumnType("float");

                    b.Property<double?>("DieselVehicleCO2Emission")
                        .HasColumnType("float");

                    b.Property<double?>("ElectricityAmount")
                        .HasColumnType("float");

                    b.Property<double?>("ElectricityCO2Emission")
                        .HasColumnType("float");

                    b.Property<double?>("ElectricityCO2KGS")
                        .HasColumnType("float");

                    b.Property<double?>("ElectricityConsumed")
                        .HasColumnType("float");

                    b.Property<double?>("FuelCO2Emission")
                        .HasColumnType("float");

                    b.Property<string>("FuelType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("HoursUsedByDifferentBranch")
                        .HasColumnType("int");

                    b.Property<string>("MeansOfDisposal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MeterType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NonPermanentStaffNumber")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfPaperUsed")
                        .HasColumnType("int");

                    b.Property<double?>("NumberOfVehicles")
                        .HasColumnType("float");

                    b.Property<int?>("PermanentStaffNumber")
                        .HasColumnType("int");

                    b.Property<double?>("PetrolCO2Emission")
                        .HasColumnType("float");

                    b.Property<double?>("PetrolCostPerLitre")
                        .HasColumnType("float");

                    b.Property<double?>("PetrolQuantityConsumed")
                        .HasColumnType("float");

                    b.Property<int?>("QuantityOfDieselConsumed")
                        .HasColumnType("int");

                    b.Property<int?>("QuantityOfDieselLeft")
                        .HasColumnType("int");

                    b.Property<int?>("QuantityOfDieselPurchased")
                        .HasColumnType("int");

                    b.Property<double?>("QuantityOfDisposal")
                        .HasColumnType("float");

                    b.Property<double?>("QuantityOfDrinkableWaterConsumed")
                        .HasColumnType("float");

                    b.Property<double?>("QuantityOfRegularWaterConsumed")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReportMonth")
                        .HasColumnType("datetime2");

                    b.Property<double?>("StaffDieselEmission")
                        .HasColumnType("float");

                    b.Property<double?>("StaffElectricityConsumption")
                        .HasColumnType("float");

                    b.Property<double?>("StaffElectricityEmission")
                        .HasColumnType("float");

                    b.Property<double?>("StaffFuelEmission")
                        .HasColumnType("float");

                    b.Property<double?>("StaffPaperConsumption")
                        .HasColumnType("float");

                    b.Property<double?>("StaffPetrolEmission")
                        .HasColumnType("float");

                    b.Property<double?>("StaffWasteAccumulation")
                        .HasColumnType("float");

                    b.Property<double?>("StaffWaterConsumption")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TotalRuningGeneratorHours")
                        .HasColumnType("int");

                    b.Property<int?>("TotalStaffNumber")
                        .HasColumnType("int");

                    b.Property<string>("VehicleBranchList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleBranchShare")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VehicleUsers")
                        .HasColumnType("int");

                    b.HasKey("CarbornFootprint");

                    b.ToTable("FootprintTable");
                });

            modelBuilder.Entity("CarbonFootprint1.Models.Branches", b =>
                {
                    b.Property<int>("BranchID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchID"), 1L, 1);

                    b.Property<string>("BraanchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchSize")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BranchID");

                    b.ToTable("BranchesTable");
                });

            modelBuilder.Entity("CarbonFootprint1.Models.PositionRoles", b =>
                {
                    b.Property<int>("PosiitonRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PosiitonRoleId"), 1L, 1);

                    b.Property<string>("Postion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PosiitonRoleId");

                    b.ToTable("PositionRolesTable");
                });

            modelBuilder.Entity("CarbonFootprint1.Models.Positions", b =>
                {
                    b.Property<int>("Position_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Position_Id"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Position_Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position_Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Position_Id");

                    b.ToTable("PositionsTable");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CarbonFootprint1.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("BranchCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BranchSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPosition")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("CarbonFootprint1.Models.Role", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
