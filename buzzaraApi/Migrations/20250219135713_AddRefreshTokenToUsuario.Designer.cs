﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using buzzaraApi.Data;

#nullable disable

namespace buzzaraApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250219135713_AddRefreshTokenToUsuario")]
    partial class AddRefreshTokenToUsuario
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("buzzaraApi.Models.Agendamento", b =>
                {
                    b.Property<int>("AgendamentoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AgendamentoID"));

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime2");

                    b.Property<int>("PerfilAcompanhanteID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AgendamentoID");

                    b.HasIndex("ClienteID");

                    b.HasIndex("PerfilAcompanhanteID");

                    b.ToTable("Agendamentos");
                });

            modelBuilder.Entity("buzzaraApi.Models.PerfilAcompanhante", b =>
                {
                    b.Property<int>("PerfilAcompanhanteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PerfilAcompanhanteID"));

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Localizacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Tarifa")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("PerfilAcompanhanteID");

                    b.HasIndex("UsuarioID");

                    b.ToTable("PerfisAcompanhantes");
                });

            modelBuilder.Entity("buzzaraApi.Models.Servico", b =>
                {
                    b.Property<int>("ServicoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServicoID"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ServicoID");

                    b.ToTable("Servicos");
                });

            modelBuilder.Entity("buzzaraApi.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioID"));

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioID");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("buzzaraApi.Models.Agendamento", b =>
                {
                    b.HasOne("buzzaraApi.Models.Usuario", "Cliente")
                        .WithMany("Agendamentos")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("buzzaraApi.Models.PerfilAcompanhante", "PerfilAcompanhante")
                        .WithMany("Agendamentos")
                        .HasForeignKey("PerfilAcompanhanteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("PerfilAcompanhante");
                });

            modelBuilder.Entity("buzzaraApi.Models.PerfilAcompanhante", b =>
                {
                    b.HasOne("buzzaraApi.Models.Usuario", "Usuario")
                        .WithMany("PerfisAcompanhantes")
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("buzzaraApi.Models.PerfilAcompanhante", b =>
                {
                    b.Navigation("Agendamentos");
                });

            modelBuilder.Entity("buzzaraApi.Models.Usuario", b =>
                {
                    b.Navigation("Agendamentos");

                    b.Navigation("PerfisAcompanhantes");
                });
#pragma warning restore 612, 618
        }
    }
}
