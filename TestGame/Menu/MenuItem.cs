using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestGame.Menu
{
    public class MenuItem
    {
        public string Title { get; private set; }

        public Game Link { get; private set; }

        public TextLabel Label { get; private set; }


        public MenuItem(string title, Game link)
        {
            Link = link;
            Title = title;
        }

        public void SetLabel(TextLabel textLabel)
        {
            Label = textLabel;
        }

    }
}
