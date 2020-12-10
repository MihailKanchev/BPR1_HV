﻿// <auto-generated />
using System;
using BachelorApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BachelorApp.Migrations
{
    [DbContext(typeof(HVDBcontext))]
    [Migration("20201210112623_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("BachelorApp.Data.Reading", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<float>("Niveau")
                        .HasColumnType("real");

                    b.Property<float>("P1OperatingTime")
                        .HasColumnType("real");

                    b.Property<float>("P1StartQuantity")
                        .HasColumnType("real");

                    b.Property<float>("P2OperatingTime")
                        .HasColumnType("real");

                    b.Property<float>("P2StartQuantity")
                        .HasColumnType("real");

                    b.Property<float>("Rain")
                        .HasColumnType("real");

                    b.Property<int>("day")
                        .HasColumnType("integer");

                    b.Property<int>("hour")
                        .HasColumnType("integer");

                    b.Property<string>("label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("month")
                        .HasColumnType("integer");

                    b.Property<float>("probability")
                        .HasColumnType("real");

                    b.HasKey("id");

                    b.ToTable("reading");
                });

            modelBuilder.Entity("BachelorApp.Data.SensorData", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<float>("pres")
                        .HasColumnType("real");

                    b.Property<float>("temp")
                        .HasColumnType("real");

                    b.Property<DateTime>("time")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("id");

                    b.ToTable("sensor");
                });
#pragma warning restore 612, 618
        }
    }
}
