using System;
using System.Collections.Generic;
using System.Text;

namespace AppHelp.Models
{
    public enum MenuItemType
    {
        Ajuda,
        Config
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
