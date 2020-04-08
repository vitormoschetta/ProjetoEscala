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
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("ProjetoEscala.Models.Aviso", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EscalaId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Mensagem")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EscalaId");

                    b.ToTable("Aviso");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Escala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Mes")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Escala");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Evento");
                });

            modelBuilder.Entity("ProjetoEscala.Models.ItemQuadro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocalId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PessoaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuadroId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QuadroId");

                    b.ToTable("ItemQuadro");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Local", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Local");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Pessoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<char>("Ativo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("ProjetoEscala.Models.PessoaLocal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocalId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PessoaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocalId");

                    b.HasIndex("PessoaId");

                    b.ToTable("PessoaLocal");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Quadro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Data")
                        .HasColumnType("TEXT");

                    b.Property<int>("Destaque")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EscalaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EscalaId");

                    b.HasIndex("EventoId");

                    b.ToTable("Quadro");
                });

            modelBuilder.Entity("ProjetoEscala.Models.Aviso", b =>
                {
                    b.HasOne("ProjetoEscala.Models.Escala", "Escala")
                        .WithMany()
                        .HasForeignKey("EscalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetoEscala.Models.ItemQuadro", b =>
                {
                    b.HasOne("ProjetoEscala.Models.Quadro", null)
                        .WithMany("ListaItemQuadro")
                        .HasForeignKey("QuadroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetoEscala.Models.PessoaLocal", b =>
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
