//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.2.0
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
   /// A user with an associated group
   /// </summary>
   [System.ComponentModel.Description("A user with an associated group")]
   public partial class User
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected User()
      {
         PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.PrevChallenge>();
         Suggestions = new System.Collections.Generic.HashSet<global::BackendModel.Suggestion>();
         Feedbacks = new System.Collections.Generic.HashSet<global::BackendModel.Feedback>();
         GroupMember = global::BackendModel.GroupMember.CreateGroupMemberUnsafe();

         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static User CreateUserUnsafe()
      {
         return new User();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="score">Total score of a user</param>
      /// <param name="username">Username of this user</param>
      public User(int score, string username)
      {
         this.Score = score;

         if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
         this.Username = username;

         this.PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.PrevChallenge>();
         this.Suggestions = new System.Collections.Generic.HashSet<global::BackendModel.Suggestion>();
         this.Feedbacks = new System.Collections.Generic.HashSet<global::BackendModel.Feedback>();

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="score">Total score of a user</param>
      /// <param name="username">Username of this user</param>
      public static User Create(int score, string username)
      {
         return new User(score, username);
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
      /// Required
      /// Total score of a user
      /// </summary>
      [Required]
      [System.ComponentModel.Description("Total score of a user")]
      public int Score { get; set; }

      /// <summary>
      /// Required, Min length = 1, Max length = 120
      /// Username of this user
      /// </summary>
      [Required]
      [MinLength(1)]
      [MaxLength(120)]
      [StringLength(120)]
      [System.ComponentModel.Description("Username of this user")]
      public string Username { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Previous challenges of this user
      /// </summary>
      [Description("Previous challenges of this user")]
      public virtual ICollection<global::BackendModel.PrevChallenge> PrevChallenges { get; private set; }

      /// <summary>
      /// User associated with this suggestion
      /// </summary>
      [Description("User associated with this suggestion")]
      public virtual ICollection<global::BackendModel.Suggestion> Suggestions { get; private set; }

      /// <summary>
      /// User creating this feedback
      /// </summary>
      [Description("User creating this feedback")]
      public virtual ICollection<global::BackendModel.Feedback> Feedbacks { get; private set; }

      /// <summary>
      /// Required&lt;br/&gt;
      /// User tied to this member
      /// </summary>
      [Description("User tied to this member")]
      public virtual global::BackendModel.GroupMember GroupMember { get; set; }

   }
}

