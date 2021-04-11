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
         MaxMembers = 8;
         GroupMembers = new System.Collections.Generic.HashSet<global::BackendModel.GroupMember>();
         PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.Challenge>();

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
      /// <param name="maxmembers">Maximum number of members allowed</param>
      /// <param name="challenge">Challenge of this group</param>
      public Group(global::BackendModel.Challenge challenge, int maxmembers = 8)
      {
         this.MaxMembers = maxmembers;

         if (challenge == null) throw new ArgumentNullException(nameof(challenge));
         this.Challenge = challenge;

         this.GroupMembers = new System.Collections.Generic.HashSet<global::BackendModel.GroupMember>();
         this.PrevChallenges = new System.Collections.Generic.HashSet<global::BackendModel.Challenge>();
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="maxmembers">Maximum number of members allowed</param>
      /// <param name="challenge">Challenge of this group</param>
      public static Group Create(global::BackendModel.Challenge challenge, int maxmembers = 8)
      {
         return new Group(challenge, maxmembers);
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
      /// SignalR group id associated with this group
      /// </summary>
      [System.ComponentModel.Description("SignalR group id associated with this group")]
      public string SignalRId { get; set; }

      /// <summary>
      /// Required, Default value = 8
      /// Maximum number of members allowed
      /// </summary>
      [Required]
      [System.ComponentModel.Description("Maximum number of members allowed")]
      public int MaxMembers { get; set; }

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
      /// Group this member belongs to
      /// </summary>
      [Description("Group this member belongs to")]
      public virtual ICollection<global::BackendModel.GroupMember> GroupMembers { get; private set; }

      public virtual ICollection<global::BackendModel.Challenge> PrevChallenges { get; private set; }

   }
}

