﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using src.Repository;

namespace src.Migrations
{
    [DbContext(typeof(CSharpPlaygroundContext))]
    partial class CSharpPlaygroundContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("src.Repository.Board", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("NumberOfColumn")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfRows")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("src.Repository.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ConfiguredBoardId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Draw")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConfiguredBoardId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("src.Repository.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("src.Repository.Movement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("BoardId")
                        .HasColumnType("uuid");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<Guid?>("WhoMadeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("WhoMadeId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("src.Repository.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Computer")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("src.Repository.PlayerBoard", b =>
                {
                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("PlayerId", "BoardId");

                    b.HasIndex("BoardId");

                    b.ToTable("PlayerBoard");
                });

            modelBuilder.Entity("src.Repository.TodoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("src.Repository.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("src.Repository.Game", b =>
                {
                    b.HasOne("src.Repository.Board", "ConfiguredBoard")
                        .WithMany()
                        .HasForeignKey("ConfiguredBoardId");

                    b.HasOne("src.Repository.Player", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId");
                });

            modelBuilder.Entity("src.Repository.Movement", b =>
                {
                    b.HasOne("src.Repository.Board", "Board")
                        .WithMany("Movements")
                        .HasForeignKey("BoardId");

                    b.HasOne("src.Repository.Player", "WhoMade")
                        .WithMany()
                        .HasForeignKey("WhoMadeId");
                });

            modelBuilder.Entity("src.Repository.PlayerBoard", b =>
                {
                    b.HasOne("src.Repository.Board", "Board")
                        .WithMany("PlayerBoards")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("src.Repository.Player", "Player")
                        .WithMany("PlayerBoards")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("src.Repository.User", b =>
                {
                    b.HasOne("src.Repository.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
