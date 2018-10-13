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
   public partial class SearchingQuery
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected SearchingQuery()
      {
         OpenedArticle = new System.Collections.Generic.HashSet<WebApi.EF.Models.OpenedArticle>();

         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_text"></param>
      /// <param name="_session"></param>
      public SearchingQuery(string _text, WebApi.EF.Models.Session _session)
      {
         if (string.IsNullOrEmpty(_text)) throw new ArgumentNullException(nameof(_text));
         Text = _text;
         if (_session == null) throw new ArgumentNullException(nameof(_session));
         Session = _session;

         OpenedArticle = new HashSet<WebApi.EF.Models.OpenedArticle>();
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_text"></param>
      /// <param name="_session"></param>
      public static SearchingQuery Create(string _text, WebApi.EF.Models.Session _session)
      {
         return new SearchingQuery(_text, _session);
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
      public string Text { get; set; }

      /// <summary>
      /// Required
      /// </summary>
      [Required]
      public string Time { get; internal set; }

      // Persistent navigation properties

      public virtual ICollection<WebApi.EF.Models.OpenedArticle> OpenedArticle { get; set; }

      /// <summary>
      /// Required
      /// </summary>
      public virtual WebApi.EF.Models.Session Session { get; set; }

   }
}
