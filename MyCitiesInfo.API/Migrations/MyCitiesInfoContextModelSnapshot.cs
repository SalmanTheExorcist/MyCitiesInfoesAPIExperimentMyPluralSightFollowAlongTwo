﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyCitiesInfo.API.DbContexts;

#nullable disable

namespace MyCitiesInfo.API.Migrations
{
    [DbContext(typeof(MyCitiesInfoContext))]
    partial class MyCitiesInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The one with that big park.",
                            Name = "New York City"
                        },
                        new
                        {
                            Id = 2,
                            Description = "The one with the cathedral that was never really finished.",
                            Name = "Antwerp"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The one with that big tower.",
                            Name = "Paris"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The most visited urban park in the United States.",
                            MyCityId = 1,
                            Name = "Central Park"
                        },
                        new
                        {
                            Id = 2,
                            Description = "A 102-story skyscraper located in Midtown Manhattan.",
                            MyCityId = 1,
                            Name = "Empire State Building"
                        },
                        new
                        {
                            Id = 3,
                            Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.",
                            MyCityId = 2,
                            Name = "Cathedral"
                        },
                        new
                        {
                            Id = 4,
                            Description = "The the finest example of railway architecture in Belgium.",
                            MyCityId = 2,
                            Name = "Antwerp Central Station"
                        },
                        new
                        {
                            Id = 5,
                            Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.",
                            MyCityId = 3,
                            Name = "Eiffel Tower"
                        },
                        new
                        {
                            Id = 6,
                            Description = "The world's largest museum.",
                            MyCityId = 3,
                            Name = "The Louvre"
                        });
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
