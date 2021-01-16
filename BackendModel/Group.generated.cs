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
   /// A group containing members
   /// </summary>
   [System.ComponentModel.Description("A group containing members")]
   public partial class Group
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Group()
      {
         PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.Challenge>();
         GroupMembers = new System.Collections.Generic.HashSet<global::BackendModel.GroupMember>();
         Challenge = global::BackendModel.Challenge.CreateChallengeUnsafe();

         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static Group CreateGroupUnsafe()
      {
         return new Group();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="challenge"></param>
      public Group(global::BackendModel.Challenge challenge)
      {
         if (challenge == null) throw new ArgumentNullException(nameof(challenge));
         this.Challenge = challenge;

         this.PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.Challenge>();
         this.GroupMembers = new System.Collections.Generic.HashSet<global::BackendModel.GroupMember>();

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="challenge"></param>
      public static Group Create(global::BackendModel.Challenge challenge)
      {
         return new Group(challenge);
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
      /// Modification version of this group
      /// </summary>
      [ConcurrencyCheck]
      [Required]
      [System.ComponentModel.Description("Modification version of this group")]
      public byte[] Version { get; set; }

      /// <summary>
      /// SignalR group id associated with this group
      /// </summary>
      [System.ComponentModel.Description("SignalR group id associated with this group")]
      public string SignalRId { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required&lt;br/&gt;
      /// Challenge of this group
      /// </summary>
      [Description("Challenge of this group")]
      public virtual global::BackendModel.Challenge Challenge { get; set; }

      /// <summary>
      /// Union of all users&apos; visited places
      /// </summary>
      [Description("Union of all users' visited places")]
      public virtual ICollection<global::BackendModel.Challenge> PrevChallenges { get; private set; }

      /// <summary>
      /// Group this member belongs to
      /// </summary>
      [Description("Group this member belongs to")]
      public virtual ICollection<global::BackendModel.GroupMember> GroupMembers { get; private set; }

   }
}

