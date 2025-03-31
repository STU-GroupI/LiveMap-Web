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

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlPointOfInterest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Point>("Position")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<string>("StatusName")
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ApprovedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PoiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StatusPropStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SubmittedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SuggestedPoiId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PoiId");

                    b.HasIndex("StatusPropStatus");

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

                    b.Property<Guid>("MapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Point>("Position")
                        .IsRequired()
                        .HasColumnType("geometry");

                    b.Property<Guid?>("RFCId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryName");

                    b.HasIndex("MapId");

                    b.HasIndex("RFCId")
                        .IsUnique()
                        .HasFilter("[RFCId] IS NOT NULL");

                    b.HasIndex("StatusName");

                    b.ToTable("SuggestedPointOfInterest", (string)null);
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlPointOfInterest", b =>
                {
                    b.HasOne("LiveMap.Domain.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryName");

                    b.HasOne("LiveMap.Persistence.DbModels.SqlMap", "Map")
                        .WithMany("PointOfInterests")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LiveMap.Domain.Models.PointOfInterestStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusName");

                    b.Navigation("Category");

                    b.Navigation("Map");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlRequestForChange", b =>
                {
                    b.HasOne("LiveMap.Persistence.DbModels.SqlPointOfInterest", "Poi")
                        .WithMany()
                        .HasForeignKey("PoiId");

                    b.HasOne("LiveMap.Domain.Models.ApprovalStatus", "StatusProp")
                        .WithMany()
                        .HasForeignKey("StatusPropStatus")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", "SuggestedPoi")
                        .WithOne()
                        .HasForeignKey("LiveMap.Persistence.DbModels.SqlRequestForChange", "SuggestedPoiId");

                    b.Navigation("Poi");

                    b.Navigation("StatusProp");

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

                    b.HasOne("LiveMap.Persistence.DbModels.SqlRequestForChange", "RFC")
                        .WithOne()
                        .HasForeignKey("LiveMap.Persistence.DbModels.SqlSuggestedPointOfInterest", "RFCId");

                    b.HasOne("LiveMap.Domain.Models.PointOfInterestStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusName");

                    b.Navigation("Category");

                    b.Navigation("Map");

                    b.Navigation("RFC");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("LiveMap.Persistence.DbModels.SqlMap", b =>
                {
                    b.Navigation("PointOfInterests");
                });
#pragma warning restore 612, 618
        }
    }
}
