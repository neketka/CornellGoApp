using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public record GroupMember(string Id, ImageSource Avatar, bool IsYou, bool IsHost, bool IsReady, string Username, int Score);
}
