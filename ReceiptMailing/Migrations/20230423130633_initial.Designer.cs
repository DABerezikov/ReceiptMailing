﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReceiptMailing.Data.Context;

#nullable disable

namespace ReceiptMailing.Migrations
{
    [DbContext(typeof(ParcelDB))]
    [Migration("20230423130633_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Base.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Building")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("House")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Province")
                        .HasColumnType("TEXT");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.Property<string>("Room")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Base.Passport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Series")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Passport");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Gardener", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("AddressId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Document")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstEmailAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PassportId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PostAddressId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecondEmailAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PassportId");

                    b.HasIndex("PostAddressId");

                    b.ToTable("Gardeners");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Parcel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CadastralNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Electrification")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GardenerId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HavingHouse")
                        .HasColumnType("INTEGER");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("PlotArea")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GardenerId");

                    b.ToTable("Parcels");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Gardener", b =>
                {
                    b.HasOne("ReceiptMailing.Data.Entities.Base.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("ReceiptMailing.Data.Entities.Base.Passport", "Passport")
                        .WithMany()
                        .HasForeignKey("PassportId");

                    b.HasOne("ReceiptMailing.Data.Entities.Base.Address", "PostAddress")
                        .WithMany()
                        .HasForeignKey("PostAddressId");

                    b.Navigation("Address");

                    b.Navigation("Passport");

                    b.Navigation("PostAddress");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Parcel", b =>
                {
                    b.HasOne("ReceiptMailing.Data.Entities.Gardener", "Gardener")
                        .WithMany("Parcels")
                        .HasForeignKey("GardenerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gardener");
                });

            modelBuilder.Entity("ReceiptMailing.Data.Entities.Gardener", b =>
                {
                    b.Navigation("Parcels");
                });
#pragma warning restore 612, 618
        }
    }
}
