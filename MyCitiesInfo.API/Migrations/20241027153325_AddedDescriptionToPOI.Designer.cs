﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCitiesInfo.API.DbContexts;

#nullable disable

namespace MyCitiesInfo.API.Migrations
{
    [DbContext(typeof(MyCitiesInfoContext))]
    [Migration("20241027153325_AddedDescriptionToPOI")]
    partial class AddedDescriptionToPOI
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("MyCitiesInfo.API.Entities.MyCity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MyCities");
                });

            modelBuilder.Entity("MyCitiesInfo.API.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("MyCityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MyCityId");

                    b.ToTable("PointOfInterests");
                });

            modelBuilder.Entity("MyCitiesInfo.API.Entities.PointOfInterest", b =>
                {
                    b.HasOne("MyCitiesInfo.API.Entities.MyCity", "MyCity")
                        .WithMany("MyCityPointOfInterests")
                        .HasForeignKey("MyCityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MyCity");
                });

            modelBuilder.Entity("MyCitiesInfo.API.Entities.MyCity", b =>
                {
                    b.Navigation("MyCityPointOfInterests");
                });
#pragma warning restore 612, 618
        }
    }
}
