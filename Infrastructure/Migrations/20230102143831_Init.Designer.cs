﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(BetContext))]
    [Migration("20230102143831_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("ApplicationCore.Models.Bet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuxiliaryLocator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("BetTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Score")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("ApplicationCore.Models.Coefficient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BetId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("BetTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsMadeBet")
                        .HasColumnType("INTEGER");

                    b.Property<double>("RatioFirst")
                        .HasColumnType("REAL");

                    b.Property<double>("RatioSecond")
                        .HasColumnType("REAL");

                    b.Property<double>("RatioThird")
                        .HasColumnType("REAL");

                    b.Property<string>("Score")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BetId");

                    b.ToTable("Coefficients");
                });

            modelBuilder.Entity("ApplicationCore.Models.Coefficient", b =>
                {
                    b.HasOne("ApplicationCore.Models.Bet", "Bet")
                        .WithMany("Coefficients")
                        .HasForeignKey("BetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bet");
                });

            modelBuilder.Entity("ApplicationCore.Models.Bet", b =>
                {
                    b.Navigation("Coefficients");
                });
#pragma warning restore 612, 618
        }
    }
}
