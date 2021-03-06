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

namespace BackendModel
{
   /// <summary>
   /// Status of the admin account
   /// </summary>
   [System.ComponentModel.Description("Status of the admin account")]
   public enum AdminAccountStatus : Int32
   {
      /// <summary>
      /// Admin access has been rejected
      /// </summary>
      [System.ComponentModel.Description("Admin access has been rejected")]
      Rejected,
      /// <summary>
      /// Admin status has been approved
      /// </summary>
      [System.ComponentModel.Description("Admin status has been approved")]
      Approved,
      /// <summary>
      /// Admin is awaiting approval
      /// </summary>
      [System.ComponentModel.Description("Admin is awaiting approval")]
      Awaiting
   }
}
