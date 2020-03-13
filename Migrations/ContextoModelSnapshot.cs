﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetoEscala.Context;

namespace ProjetoEscala.Migrations
{
    [DbContext(typeof(Contexto))]
    partial class ContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjetoEscala.Models.Escala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Ano")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Mes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Escala");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Evento");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Local", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Local");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("ProjetoEscala.Models.PessoaQuadro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("LocalId")
                        .HasColumnType("int");

                    b.Property<int>("PessoaId")
                        .HasColumnType("int");

                    b.Property<int>("QuadroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocalId");

                    b.HasIndex("PessoaId");

                    b.HasIndex("QuadroId");

                    b.ToTable("PessoaQuadro");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Quadro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EscalaId")
                        .HasColumnType("int");

                    b.Property<int>("EventoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EscalaId");

                    b.HasIndex("EventoId");

                    b.ToTable("Quadro");
                });

            modelBuilder.Entity("ProjetoEscala.Models.PessoaQuadro", b =>
                {
                    b.HasOne("ProjetoEscala.Models.Local", "Local")
                        .WithMany()
                        .HasForeignKey("LocalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoEscala.Models.Pessoa", "Pessoa")
                        .WithMany()
                        .HasForeignKey("PessoaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoEscala.Models.Quadro", "Quadro")
                        .WithMany("ListaPessoaQuadro")
                        .HasForeignKey("QuadroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetoEscala.Models.Quadro", b =>
                {
                    b.HasOne("ProjetoEscala.Models.Escala", "Escala")
                        .WithMany()
                        .HasForeignKey("EscalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoEscala.Models.Evento", "Evento")
                        .WithMany()
                        .HasForeignKey("EventoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
