﻿// <auto-generated />
using System;
using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infra.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241106024802_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infra.Dto.ClienteDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clientes", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("efee2d79-ce89-479a-9667-04f57f9e2e5e"),
                            Ativo = true,
                            Cpf = "08062759016",
                            Email = "joao-teste@gmail.com",
                            Nome = "João"
                        },
                        new
                        {
                            Id = new Guid("fdff63d2-127f-49c5-854a-e47cae8cedb9"),
                            Ativo = true,
                            Cpf = "05827307084",
                            Email = "maria-teste@gmail.com",
                            Nome = "Maria"
                        });
                });

            modelBuilder.Entity("Infra.Dto.FuncionarioDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Funcionarios", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("34a86719-0082-4ef5-a620-35d55f076c31"),
                            Ativo = true,
                            Email = "sof.arc.6soat@gmail.com",
                            Nome = "Mario"
                        });
                });

            modelBuilder.Entity("Infra.Dto.ProdutoDb", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.Property<decimal>("Preco")
                        .HasPrecision(2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Produtos", "dbo");

                    b.HasData(
                        new
                        {
                            Id = new Guid("efee2d79-ce89-479a-9667-04f57f9e2e5e"),
                            Ativo = true,
                            Categoria = "Lanche",
                            Descricao = "Pão brioche, hambúrguer (150g), queijo prato, pepino, tomate italiano e alface americana.",
                            Nome = "X-SALADA",
                            Preco = 30m
                        },
                        new
                        {
                            Id = new Guid("fdff63d2-127f-49c5-854a-e47cae8cedb9"),
                            Ativo = true,
                            Categoria = "Lanche",
                            Descricao = "Pão brioche, hambúrguer (150g), queijo prato, bacon, pepino, tomate italiano e alface americana.",
                            Nome = "X-BACON",
                            Preco = 33m
                        },
                        new
                        {
                            Id = new Guid("eee57eb1-1dde-4162-998f-d7148d0c2417"),
                            Ativo = true,
                            Categoria = "Lanche",
                            Descricao = "Pão brioche, hambúrguer (150g) e queijo prato.",
                            Nome = "X-BURGUER",
                            Preco = 28m
                        },
                        new
                        {
                            Id = new Guid("719bc73f-b90a-4bb0-b6d0-8060ea9f1d4c"),
                            Ativo = true,
                            Categoria = "Lanche",
                            Descricao = "Pão smash, 2 hambúrgueres (150g cada), maionese do feio ,2 queijos cheddar e muito bacon.",
                            Nome = "X-DUPLO BACON",
                            Preco = 36m
                        },
                        new
                        {
                            Id = new Guid("50ba333a-c804-4d2a-a284-9ff1d147df4e"),
                            Ativo = true,
                            Categoria = "Acompanhamento",
                            Descricao = "Porção individual de batata frita (100g)",
                            Nome = "BATATA FRITA",
                            Preco = 9m
                        },
                        new
                        {
                            Id = new Guid("1bb2aef8-97d7-4fb0-94f5-53bff2f3a618"),
                            Ativo = true,
                            Categoria = "Acompanhamento",
                            Descricao = "Anéis de cebola (100g)",
                            Nome = "ONION RINGS",
                            Preco = 10m
                        },
                        new
                        {
                            Id = new Guid("111cb598-2df6-41bf-b51b-d4e0f292bda3"),
                            Ativo = true,
                            Categoria = "Bebida",
                            Descricao = "350ml",
                            Nome = "PEPSI LATA",
                            Preco = 7m
                        },
                        new
                        {
                            Id = new Guid("c0eab3dc-2ddf-4dde-a64f-392f2412201f"),
                            Ativo = true,
                            Categoria = "Bebida",
                            Descricao = "350ml",
                            Nome = "GUARANÁ ANTARCTICA LATA",
                            Preco = 7m
                        },
                        new
                        {
                            Id = new Guid("3de0c5e7-787b-4885-8ec8-020866971d3b"),
                            Ativo = true,
                            Categoria = "Bebida",
                            Descricao = "500ml",
                            Nome = "ÁGUA",
                            Preco = 5m
                        },
                        new
                        {
                            Id = new Guid("b17f425e-e0ff-41cd-92a6-00d78172d7a5"),
                            Ativo = true,
                            Categoria = "Sobremesa",
                            Descricao = "70g",
                            Nome = "BROWNIE CHOCOLATE",
                            Preco = 10m
                        },
                        new
                        {
                            Id = new Guid("e206c795-d6d6-491e-90ed-fdc08e057939"),
                            Ativo = true,
                            Categoria = "Sobremesa",
                            Descricao = "70g",
                            Nome = "BROWNIE CHOCOLATE BRANCO",
                            Preco = 10m
                        },
                        new
                        {
                            Id = new Guid("c398d290-d1a1-4f2e-a907-ef55e92beef6"),
                            Ativo = true,
                            Categoria = "Sobremesa",
                            Descricao = "100g",
                            Nome = "SORVETE DE CHOCOLATE",
                            Preco = 12m
                        },
                        new
                        {
                            Id = new Guid("782725ea-70a5-49db-95b2-c4eea841ebca"),
                            Ativo = true,
                            Categoria = "Sobremesa",
                            Descricao = "100g",
                            Nome = "SORVETE DE CREME",
                            Preco = 12m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
