//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor
//     https://github.com/msawczyn/EFDesigner
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.EF.Models
{
   /// <inheritdoc/>
   public partial class EFContext : Microsoft.EntityFrameworkCore.DbContext
   {
      #region DbSets
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.Article> Articles { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.ArticleDependencies> ArticleDependencies { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.ArticleTag> ArticleTags { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.AskTitle> AskTitle { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.Configuration1C> Configurations1C { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.OpenedArticle> OpenedArticles { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.Provider> Providers { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.SearchingQuery> SearchingQueryes { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.Session> Sessions { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.SessionQuery> SessionQueries { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.SupportAsk> SupportAsk { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.Tag> Tags { get; set; }
      public Microsoft.EntityFrameworkCore.DbSet<WebApi.EF.Models.User> Users { get; set; }
      #endregion DbSets

      /// <inheritdoc />
      public EFContext() : base()
      {
      }

      /// <inheritdoc />
      public EFContext(DbContextOptions<EFContext> options) : base(options)
      {
      }

      partial void CustomInit(DbContextOptionsBuilder optionsBuilder);

      /// <inheritdoc />
      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         CustomInit(optionsBuilder);
      }

      partial void OnModelCreatingImpl(ModelBuilder modelBuilder);
      partial void OnModelCreatedImpl(ModelBuilder modelBuilder);

      /// <inheritdoc />
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);
         OnModelCreatingImpl(modelBuilder);

         modelBuilder.HasDefaultSchema("dbo");

         modelBuilder.Entity<WebApi.EF.Models.Article>()
                     .ToTable("Article")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.Article>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.Article>()
                     .Property(t => t.Title)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Article>()
                     .Property(t => t.FileName)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Article>()
                     .HasOne(x => x.ArticleDependencies)
                     .WithMany(x => x.Article);

         modelBuilder.Entity<WebApi.EF.Models.ArticleDependencies>()
                     .ToTable("ArticleDependencies")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.ArticleDependencies>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.ArticleDependencies>()
                     .HasOne(x => x.Configuration1C)
                     .WithMany(x => x.ArticleDependencies);

         modelBuilder.Entity<WebApi.EF.Models.ArticleTag>()
                     .ToTable("ArticleTag")
                     .HasKey(t => t.ID);
         modelBuilder.Entity<WebApi.EF.Models.ArticleTag>()
                     .Property(t => t.ID)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.ArticleTag>()
                     .Property(t => t.Weight)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.ArticleTag>()
                     .HasOne(x => x.Article)
                     .WithMany(x => x.ArticleTag);
         modelBuilder.Entity<WebApi.EF.Models.ArticleTag>()
                     .HasOne(x => x.Tag)
                     .WithMany(x => x.ArticleTag);

         modelBuilder.Entity<WebApi.EF.Models.AskTitle>()
                     .ToTable("AskTitle")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.AskTitle>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.AskTitle>()
                     .Property(t => t.Text)
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.Configuration1C>()
                     .ToTable("Configuration1C")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.Configuration1C>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.Configuration1C>()
                     .Property(t => t.Name)
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .ToTable("OpenedArticle")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .Property(t => t.OpenedNumber)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .Property(t => t.Time)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .HasOne(x => x.Article)
                     .WithMany(x => x.OpenedArticle);
         modelBuilder.Entity<WebApi.EF.Models.OpenedArticle>()
                     .HasOne(x => x.SearchingQuery)
                     .WithMany(x => x.OpenedArticle);

         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .ToTable("Provider")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .Property(t => t.Name)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .Property(t => t.LogoUrl)
                     .HasMaxLength(300);
         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .Property(t => t.ContractEndTime)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Provider>()
                     .Property(t => t.SupportEmail)
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.SearchingQuery>()
                     .ToTable("SearchingQuery")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.SearchingQuery>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.SearchingQuery>()
                     .Property(t => t.Text)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.SearchingQuery>()
                     .Property(t => t.Amount)
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.Session>()
                     .ToTable("Session")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.Session>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.Session>()
                     .Property(t => t.OpenTime)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Session>()
                     .HasMany(x => x.SupportAsk)
                     .WithOne()
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.Session>()
                     .HasOne(x => x.User)
                     .WithMany(x => x.Session);

         modelBuilder.Entity<WebApi.EF.Models.SessionQuery>()
                     .ToTable("SessionQuery")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.SessionQuery>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.SessionQuery>()
                     .Property(t => t.Time)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.SessionQuery>()
                     .HasOne(x => x.Session)
                     .WithMany(x => x.SearchingQuery);
         modelBuilder.Entity<WebApi.EF.Models.SessionQuery>()
                     .HasOne(x => x.SearchingQuery)
                     .WithMany(x => x.SessionQuery);

         modelBuilder.Entity<WebApi.EF.Models.SupportAsk>()
                     .ToTable("SupportAsk")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.SupportAsk>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.SupportAsk>()
                     .Property(t => t.Text)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.SupportAsk>()
                     .Property(t => t.ContactInfo)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.SupportAsk>()
                     .HasMany(x => x.AskTitle)
                     .WithOne()
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.Tag>()
                     .ToTable("Tag")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.Tag>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.Tag>()
                     .Property(t => t.Value)
                     .HasMaxLength(120)
                     .IsRequired();

         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .ToTable("User")
                     .HasKey(t => t.Id);
         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .Property(t => t.Id)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .Property(t => t.Login)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .Property(t => t.Email)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .Property(t => t.INN)
                     .HasMaxLength(12)
                     .IsRequired();
         modelBuilder.Entity<WebApi.EF.Models.User>()
                     .HasOne(x => x.Provider)
                     .WithMany(x => x.User);

         OnModelCreatedImpl(modelBuilder);
      }
   }
}
