﻿// <auto-generated />
using System;
using LiveMap.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace LiveMap.Persistence.Migrations
{
    [DbContext(typeof(LiveMapContext))]
    partial class LiveMapContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LiveMap.Domain.Models.ApprovalStatus", b =>
                {
                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Status");

                    b.ToTable("ApprovalStatuses");
                });

            modelBuilder.Entity("LiveMap.Domain.Models.Category", b =>
                {
                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("Category");

                    b.HasKey("CategoryName");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("LiveMap.Domain.Models.PointOfInterestStatus", b =>
                {
                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Status");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlMap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Polygon>("Border")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Point>("Position")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.HasKey("Id");

                    b.ToTable("Map", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlOpeningHours", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("time");

                    b.Property<Guid>("PoiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("PoiId");

                    b.ToTable("OpeningHours", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlPointOfInterest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsWheelchairAccessible")
                        .HasColumnType("bit");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Point>("Position")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryName");

                    b.HasIndex("MapId");

                    b.HasIndex("StatusName");

                    b.ToTable("PointOfInterest", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlRequestForChange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApprovalStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ApprovedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("PoiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SubmittedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SuggestedPoiId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalStatus");

                    b.HasIndex("PoiId");

                    b.HasIndex("SuggestedPoiId")
                        .IsUnique()
                        .HasFilter("[SuggestedPoiId] IS NOT NULL");

                    b.ToTable("RequestForChange", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsWheelchairAccessible")
                        .HasColumnType("bit");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Point>("Position")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<Guid?>("RFCId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryName");

                    b.HasIndex("MapId");

                    b.ToTable("SuggestedPointOfInterest", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlOpeningHours", b =>
                {
                    b.HasOne("LiveMap.Persistence.DbModels.SqlPointOfInterest", "Poi")
                        .WithMany("OpeningHours")
                        .HasForeignKey("PoiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Poi");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlPointOfInterest", b =>
                {
                    b.HasOne("LiveMap.Domain.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LiveMap.Persistence.DbModels.SqlMap", "Map")
                        .WithMany("PointOfInterests")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LiveMap.Domain.Models.PointOfInterestStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Map");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlRequestForChange", b =>
                {
                    b.HasOne("LiveMap.Domain.Models.ApprovalStatus", "ApprovalStatusProp")
                        .WithMany()
                        .HasForeignKey("ApprovalStatus")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LiveMap.Persistence.DbModels.SqlPointOfInterest", "Poi")
                        .WithMany()
                        .HasForeignKey("PoiId");

                    b.HasOne("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", "SuggestedPoi")
                        .WithOne("RFC")
                        .HasForeignKey("LiveMap.Persistence.DbModels.SqlRequestForChange", "SuggestedPoiId");

                    b.Navigation("ApprovalStatusProp");

                    b.Navigation("Poi");

                    b.Navigation("SuggestedPoi");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", b =>
                {
                    b.HasOne("LiveMap.Domain.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryName");

                    b.HasOne("LiveMap.Persistence.DbModels.SqlMap", "Map")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Map");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlMap", b =>
                {
                    b.Navigation("PointOfInterests");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlPointOfInterest", b =>
                {
                    b.Navigation("OpeningHours");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", b =>
                {
                    b.Navigation("RFC")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
