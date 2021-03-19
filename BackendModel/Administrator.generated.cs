//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v3.0.3.2
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
   /// Manager of the CornellGO data
   /// </summary>
   [System.ComponentModel.Description("Manager of the CornellGO data")]
   public partial class Administrator
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Administrator()
      {
         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static Administrator CreateAdministratorUnsafe()
      {
         return new Administrator();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="email">Email of the admin</param>
      public Administrator(string email)
      {
         if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
         this.Email = email;

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="email">Email of the admin</param>
      public static Administrator Create(string email)
      {
         return new Administrator(email);
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
      /// Email of the admin
      /// </summary>
      [Required]
      [System.ComponentModel.Description("Email of the admin")]
      public string Email { get; set; }

   }
}

