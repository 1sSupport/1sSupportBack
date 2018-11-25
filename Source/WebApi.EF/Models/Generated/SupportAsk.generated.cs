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
   public partial class SupportAsk
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected SupportAsk()
      {
         AskTitle = new System.Collections.Generic.HashSet<WebApi.EF.Models.AskTitle>();

         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_text"></param>
      /// <param name="_contactinfo"></param>
      /// <param name="_session0"></param>
      public SupportAsk(string _text, string _contactinfo, WebApi.EF.Models.Session _session0)
      {
         if (string.IsNullOrEmpty(_text)) throw new ArgumentNullException(nameof(_text));
         Text = _text;
         if (string.IsNullOrEmpty(_contactinfo)) throw new ArgumentNullException(nameof(_contactinfo));
         ContactInfo = _contactinfo;
         if (_session0 == null) throw new ArgumentNullException(nameof(_session0));
         _session0.SupportAsk.Add(this);

         AskTitle = new HashSet<WebApi.EF.Models.AskTitle>();
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_text"></param>
      /// <param name="_contactinfo"></param>
      /// <param name="_session0"></param>
      public static SupportAsk Create(string _text, string _contactinfo, WebApi.EF.Models.Session _session0)
      {
         return new SupportAsk(_text, _contactinfo, _session0);
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
      public string ContactInfo { get; set; }

      // Persistent navigation properties

      public virtual ICollection<WebApi.EF.Models.AskTitle> AskTitle { get; set; }

   }
}
