﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestaurantAPI.Entities;

#nullable disable

namespace RestaurantAPI.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    partial class RestaurantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RestaurantAPI.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Chef", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Chefs");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<string>("RestaurantId1")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId1");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Ingredients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Restaurant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChefId")
                        .HasColumnType("int");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasDelivery")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ChefId")
                        .IsUnique();

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Dish", b =>
                {
                    b.HasOne("RestaurantAPI.Entities.Restaurant", "Restaurant")
                        .WithMany("Dishes")
                        .HasForeignKey("RestaurantId1");

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Ingredients", b =>
                {
                    b.HasOne("RestaurantAPI.Entities.Dish", "Dish")
                        .WithMany("Ingredients")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Restaurant", b =>
                {
                    b.HasOne("RestaurantAPI.Entities.Address", "Address")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantAPI.Entities.Restaurant", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestaurantAPI.Entities.Chef", "Chef")
                        .WithOne("Restaurant")
                        .HasForeignKey("RestaurantAPI.Entities.Restaurant", "ChefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Chef");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Address", b =>
                {
                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Chef", b =>
                {
                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Dish", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("RestaurantAPI.Entities.Restaurant", b =>
                {
                    b.Navigation("Dishes");
                });
#pragma warning restore 612, 618
        }
    }
}