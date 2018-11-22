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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WebApi.EF.Models
{
   public partial class Session
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Session()
      {
         SupportAsk = new System.Collections.Generic.HashSet<WebApi.EF.Models.SupportAsk>();
         SearchingQuery = new System.Collections.Generic.HashSet<WebApi.EF.Models.SearchingQuery>();

         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_opentime"></param>
      /// <param name="_user"></param>
      public Session(DateTime _opentime, WebApi.EF.Models.User _user)
      {
         OpenTime = _opentime;
         if (_user == null) throw new ArgumentNullException(nameof(_user));
         User = _user;

         SupportAsk = new HashSet<WebApi.EF.Models.SupportAsk>();
         SearchingQuery = new HashSet<WebApi.EF.Models.SearchingQuery>();
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_opentime"></param>
      /// <param name="_user"></param>
      public static Session Create(DateTime _opentime, WebApi.EF.Models.User _user)
      {
         return new Session(_opentime, _user);
      }

      // Persistent properties

      /// <summary>
      /// Identity, Required, Indexed
      /// </summary>
      [Key]
      [Required]
      public int Id { get; set; }

      /// <summary>
      /// Required
      /// </summary>
      [Required]
      public DateTime OpenTime { get; set; }

      public DateTime? CloseTime { get; protected set; }

      // Persistent navigation properties

      public virtual ICollection<WebApi.EF.Models.SupportAsk> SupportAsk { get; set; }

      public virtual ICollection<WebApi.EF.Models.SearchingQuery> SearchingQuery { get; set; }

      /// <summary>
      /// Required
      /// </summary>
      public virtual WebApi.EF.Models.User User { get; set; }

   }
}

