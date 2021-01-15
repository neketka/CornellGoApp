using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public record LeaderboardPlayer(string Id, int Position, ImageSource ProfilePicture, string Username, int Points, bool IsYou);
}
