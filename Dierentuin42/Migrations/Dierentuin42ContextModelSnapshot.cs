﻿// <auto-generated />
using System;
using Dierentuin42.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dierentuin42.Migrations
{
    [DbContext(typeof(Dierentuin42Context))]
    partial class Dierentuin42ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dierentuin42.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalActivityPattern")
                        .HasColumnType("int");

                    b.Property<int>("AnimalDiet")
                        .HasColumnType("int");

                    b.Property<int>("AnimalSize")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("EnclosureId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Prey")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SecurityRequirement")
                        .HasColumnType("int");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<double>("spaceRequirement")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EnclosureId");

                    b.ToTable("Animal");
                });

            modelBuilder.Entity("Dierentuin42.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Dierentuin42.Models.Enclosure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EnclosureClimate")
                        .HasColumnType("int");

                    b.Property<int>("EnclosureHabitatType")
                        .HasColumnType("int");

                    b.Property<int>("EnclosureSecurityLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<double>("Size")
                        .HasColumnType("float");

                    b.Property<int?>("ZooId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ZooId");

                    b.ToTable("Enclosure");
                });

            modelBuilder.Entity("Dierentuin42.Models.Zoo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Zoo");
                });

            modelBuilder.Entity("Dierentuin42.Models.Animal", b =>
                {
                    b.HasOne("Dierentuin42.Models.Category", "Category")
                        .WithMany("Animals")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Dierentuin42.Models.Enclosure", "Enclosure")
                        .WithMany("Animals")
                        .HasForeignKey("EnclosureId");

                    b.Navigation("Category");

                    b.Navigation("Enclosure");
                });

            modelBuilder.Entity("Dierentuin42.Models.Enclosure", b =>
                {
                    b.HasOne("Dierentuin42.Models.Zoo", "Zoo")
                        .WithMany("Enclosures")
                        .HasForeignKey("ZooId");

                    b.Navigation("Zoo");
                });

            modelBuilder.Entity("Dierentuin42.Models.Category", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("Dierentuin42.Models.Enclosure", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("Dierentuin42.Models.Zoo", b =>
                {
                    b.Navigation("Enclosures");
                });
#pragma warning restore 612, 618
        }
    }
}
