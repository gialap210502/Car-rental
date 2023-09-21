﻿// <auto-generated />
using System;
using Car_rental.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Car_rental.Migrations
{
    [DbContext(typeof(Car_rentalContext))]
    partial class Car_rentalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Car_rental.Models.Conversation", b =>
                {
                    b.Property<int>("ConversationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ConversationID");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("Car_rental.Models.Images", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("carId")
                        .HasColumnType("int");

                    b.Property<string>("nameFile")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("carId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Car_rental.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageID"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConversationID")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("MessageID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("UserID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Car_rental.Models.Participation", b =>
                {
                    b.Property<int>("ParticipationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParticipationID"));

                    b.Property<int>("ConversationID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ParticipationID");

                    b.HasIndex("ConversationID");

                    b.HasIndex("UserID");

                    b.ToTable("Participation");
                });

            modelBuilder.Entity("Car_rental.Models.bookings", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("CarBack")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TakeCar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("endDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("startDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<double?>("totalAmount")
                        .HasColumnType("float");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("bookings");
                });

            modelBuilder.Entity("Car_rental.Models.car", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("AirConditioning")
                        .HasColumnType("bit");

                    b.Property<bool>("AudioInput")
                        .HasColumnType("bit");

                    b.Property<bool>("Bluetooth")
                        .HasColumnType("bit");

                    b.Property<bool>("CarKit")
                        .HasColumnType("bit");

                    b.Property<bool>("ChildSeat")
                        .HasColumnType("bit");

                    b.Property<bool>("ClimateControl")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("GPS")
                        .HasColumnType("bit");

                    b.Property<bool>("LongTermTrips")
                        .HasColumnType("bit");

                    b.Property<bool>("Luggage")
                        .HasColumnType("bit");

                    b.Property<long?>("Mileage")
                        .HasColumnType("bigint");

                    b.Property<bool>("Music")
                        .HasColumnType("bit");

                    b.Property<bool>("OnboardComputer")
                        .HasColumnType("bit");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("RemoteCentralLocking")
                        .HasColumnType("bit");

                    b.Property<bool>("SeatBelt")
                        .HasColumnType("bit");

                    b.Property<bool>("SleepingBed")
                        .HasColumnType("bit");

                    b.Property<string>("Transmission")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Water")
                        .HasColumnType("bit");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("available")
                        .HasColumnType("int");

                    b.Property<string>("brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("category_id")
                        .HasColumnType("int");

                    b.Property<string>("color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("discount_id")
                        .HasColumnType("int");

                    b.Property<string>("model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("seat")
                        .HasColumnType("int");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("discount_id");

                    b.HasIndex("user_id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("Car_rental.Models.category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("Deleted_Status")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("category");
                });

            modelBuilder.Entity("Car_rental.Models.discount", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("endDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("percentage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("startDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("discount");
                });

            modelBuilder.Entity("Car_rental.Models.payment", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<double>("amount")
                        .HasColumnType("float");

                    b.Property<int>("booking_id")
                        .HasColumnType("int");

                    b.Property<int>("carId")
                        .HasColumnType("int");

                    b.Property<DateTime>("paymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("paymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("status")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("booking_id");

                    b.HasIndex("carId");

                    b.ToTable("payment");
                });

            modelBuilder.Entity("Car_rental.Models.paymentHistory", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("timeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("UserID");

                    b.ToTable("paymentHistory");
                });

            modelBuilder.Entity("Car_rental.Models.rating", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("carId")
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("dateRating")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("carId");

                    b.HasIndex("userId");

                    b.ToTable("rating");
                });

            modelBuilder.Entity("Car_rental.Models.roles", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Car_rental.Models.user", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("citizen_identification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("coins")
                        .HasColumnType("float");

                    b.Property<DateTime?>("dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("driver_license")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("flag")
                        .HasColumnType("int");

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Car_rental.Models.userRole", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("roleId");

                    b.HasIndex("userId");

                    b.ToTable("userRole");
                });

            modelBuilder.Entity("Car_rental.Models.Images", b =>
                {
                    b.HasOne("Car_rental.Models.car", "car")
                        .WithMany("images")
                        .HasForeignKey("carId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("car");
                });

            modelBuilder.Entity("Car_rental.Models.Message", b =>
                {
                    b.HasOne("Car_rental.Models.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("Messages")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.Participation", b =>
                {
                    b.HasOne("Car_rental.Models.Conversation", "Conversation")
                        .WithMany("Participations")
                        .HasForeignKey("ConversationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("Participations")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.bookings", b =>
                {
                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("bookings")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.car", b =>
                {
                    b.HasOne("Car_rental.Models.category", "category")
                        .WithMany("cars")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.discount", "Discount")
                        .WithMany("cars")
                        .HasForeignKey("discount_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("cars")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("category");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.payment", b =>
                {
                    b.HasOne("Car_rental.Models.bookings", "booking")
                        .WithMany("payments")
                        .HasForeignKey("booking_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.car", "car")
                        .WithMany("payments")
                        .HasForeignKey("carId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("booking");

                    b.Navigation("car");
                });

            modelBuilder.Entity("Car_rental.Models.paymentHistory", b =>
                {
                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("paymentHistory")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.rating", b =>
                {
                    b.HasOne("Car_rental.Models.car", "car")
                        .WithMany("ratings")
                        .HasForeignKey("carId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("ratings")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("car");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.userRole", b =>
                {
                    b.HasOne("Car_rental.Models.roles", "role")
                        .WithMany("userRoles")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Car_rental.Models.user", "user")
                        .WithMany("userRoles")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Car_rental.Models.Conversation", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Participations");
                });

            modelBuilder.Entity("Car_rental.Models.bookings", b =>
                {
                    b.Navigation("payments");
                });

            modelBuilder.Entity("Car_rental.Models.car", b =>
                {
                    b.Navigation("images");

                    b.Navigation("payments");

                    b.Navigation("ratings");
                });

            modelBuilder.Entity("Car_rental.Models.category", b =>
                {
                    b.Navigation("cars");
                });

            modelBuilder.Entity("Car_rental.Models.discount", b =>
                {
                    b.Navigation("cars");
                });

            modelBuilder.Entity("Car_rental.Models.roles", b =>
                {
                    b.Navigation("userRoles");
                });

            modelBuilder.Entity("Car_rental.Models.user", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Participations");

                    b.Navigation("bookings");

                    b.Navigation("cars");

                    b.Navigation("paymentHistory");

                    b.Navigation("ratings");

                    b.Navigation("userRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
