﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.EF.Models;

namespace WebApi.Migrations
{
    [DbContext(typeof(EFContext))]
    partial class EFContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.EF.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArticleDependenciesId");

                    b.Property<DateTime?>("EditDate");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<double?>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("ArticleDependenciesId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("WebApi.EF.Models.ArticleDependencies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Configuration1CId");

                    b.HasKey("Id");

                    b.HasIndex("Configuration1CId");

                    b.ToTable("ArticleDependencies");
                });

            modelBuilder.Entity("WebApi.EF.Models.ArticleTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArticleId");

                    b.Property<int?>("TagId");

                    b.Property<double>("Weight");

                    b.HasKey("ID");

                    b.HasIndex("ArticleId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTag");
                });

            modelBuilder.Entity("WebApi.EF.Models.Configuration1C", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Configuration1C");
                });

            modelBuilder.Entity("WebApi.EF.Models.OpenedArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArticleId");

                    b.Property<bool?>("IsLastOpened");

                    b.Property<int?>("ListNumber");

                    b.Property<int?>("Mark");

                    b.Property<int>("OpenedNumber");

                    b.Property<int?>("SearchingQueryId");

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("SearchingQueryId");

                    b.ToTable("OpenedArticle");
                });

            modelBuilder.Entity("WebApi.EF.Models.Provider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ContractEndTime");

                    b.Property<string>("LogoUrl")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("WebApi.EF.Models.SearchingQuery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("SessionId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<DateTime>("Time");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("SearchingQuery");
                });

            modelBuilder.Entity("WebApi.EF.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CloseTime");

                    b.Property<DateTime>("OpenTime");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("WebApi.EF.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("WebApi.EF.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("INN")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<int?>("ProviderId");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebApi.EF.Models.Article", b =>
                {
                    b.HasOne("WebApi.EF.Models.ArticleDependencies", "ArticleDependencies")
                        .WithMany("Article")
                        .HasForeignKey("ArticleDependenciesId");
                });

            modelBuilder.Entity("WebApi.EF.Models.ArticleDependencies", b =>
                {
                    b.HasOne("WebApi.EF.Models.Configuration1C", "Configuration1C")
                        .WithMany("ArticleDependencies")
                        .HasForeignKey("Configuration1CId");
                });

            modelBuilder.Entity("WebApi.EF.Models.ArticleTag", b =>
                {
                    b.HasOne("WebApi.EF.Models.Article", "Article")
                        .WithMany("ArticleTag")
                        .HasForeignKey("ArticleId");

                    b.HasOne("WebApi.EF.Models.Tag", "Tag")
                        .WithMany("ArticleTag")
                        .HasForeignKey("TagId");
                });

            modelBuilder.Entity("WebApi.EF.Models.OpenedArticle", b =>
                {
                    b.HasOne("WebApi.EF.Models.Article", "Article")
                        .WithMany("OpenedArticle")
                        .HasForeignKey("ArticleId");

                    b.HasOne("WebApi.EF.Models.SearchingQuery", "SearchingQuery")
                        .WithMany("OpenedArticle")
                        .HasForeignKey("SearchingQueryId");
                });

            modelBuilder.Entity("WebApi.EF.Models.SearchingQuery", b =>
                {
                    b.HasOne("WebApi.EF.Models.Session", "Session")
                        .WithMany("SearchingQuery")
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("WebApi.EF.Models.Session", b =>
                {
                    b.HasOne("WebApi.EF.Models.User", "User")
                        .WithMany("Session")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.EF.Models.User", b =>
                {
                    b.HasOne("WebApi.EF.Models.Provider", "Provider")
                        .WithMany("User")
                        .HasForeignKey("ProviderId");
                });
#pragma warning restore 612, 618
        }
    }
}
