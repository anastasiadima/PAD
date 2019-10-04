﻿// <auto-generated />
using System;
using MessageBroker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MessageBroker.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190930151616_changes2")]
    partial class changes2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MessageBroker.ClientRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Room");

                    b.HasKey("Id");

                    b.ToTable("ClientRooms");
                });

            modelBuilder.Entity("MessageBroker.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsRedirect");

                    b.Property<int>("RoomType");

                    b.Property<DateTime>("SendDate");

                    b.Property<int?>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MessageBroker.Models.SocketModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IpAddress");

                    b.Property<int>("Port");

                    b.HasKey("Id");

                    b.ToTable("SocketModels");
                });

            modelBuilder.Entity("MessageBroker.Message", b =>
                {
                    b.HasOne("MessageBroker.Models.SocketModel", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });
#pragma warning restore 612, 618
        }
    }
}