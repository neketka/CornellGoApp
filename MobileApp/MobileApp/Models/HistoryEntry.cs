using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp.Models
{
    public record HistoryEntry(string Id, ImageSource Preview, DateTime Date, string Name, int Points);
}
