//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.4.7
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
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
using NetTopologySuite.Geometries;

namespace BackendModel
{
   /// <summary>
   /// Feedback created by the user
   /// </summary>
   [System.ComponentModel.Description("Feedback created by the user")]
   public partial class Feedback
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Feedback()
      {
         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static Feedback CreateFeedbackUnsafe()
      {
         return new Feedback();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="message">The message within a feedback</param>
      /// <param name="timestamp">Creation timestamp</param>
      /// <param name="user">Feedbacks associated with this user</param>
      public Feedback(string message, DateTime timestamp, global::BackendModel.User user)
      {
         if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));
         this.Message = message;

         this.Timestamp = timestamp;

         if (user == null) throw new ArgumentNullException(nameof(user));
         this.User = user;
         user.Feedbacks.Add(this);

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="message">The message within a feedback</param>
      /// <param name="timestamp">Creation timestamp</param>
      /// <param name="user">Feedbacks associated with this user</param>
      public static Feedback Create(string message, DateTime timestamp, global::BackendModel.User user)
      {
         return new Feedback(message, timestamp, user);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// Unique identifier
      /// </summary>
      [Key]
      [Required]
      [System.ComponentModel.Description("Unique identifier")]
      public long Id { get; set; }

      /// <summary>
      /// Required, Min length = 1, Max length = 250
      /// The message within a feedback
      /// </summary>
      [Required]
      [MinLength(1)]
      [MaxLength(250)]
      [StringLength(250)]
      [System.ComponentModel.Description("The message within a feedback")]
      public string Message { get; set; }

      /// <summary>
      /// Required
      /// Creation timestamp
      /// </summary>
      [Required]
      [System.ComponentModel.Description("Creation timestamp")]
      public DateTime Timestamp { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required&lt;br/&gt;
      /// Feedbacks associated with this user
      /// </summary>
      [Description("Feedbacks associated with this user")]
      public virtual global::BackendModel.User User { get; set; }

   }
}

