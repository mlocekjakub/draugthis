using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public class Pawn
    {   

        public System.Media.SoundPlayer sound;
        
        public string Color { get; set; }
        public string Icon { get; set; }
        public string CrownedIcon { get; set; }
        public bool Highlight { get; set; } = false;
        public bool IsCrowned { get; set; }
        public Coords Position { get; set; }
        public Pawn(string color,Coords position)
        {
            Color = color;
            IsCrowned = false;
            Position = position;
            if (color == "white")
            {
                Icon = " 🐕 ";
                CrownedIcon = " 🐺 ";
                sound = new System.Media.SoundPlayer(@"..\..\dog.wav");
            }
            else
            {
                Icon = " 🐈 ";
                CrownedIcon = " 🐯 ";
                sound = new System.Media.SoundPlayer(@"..\..\cat.wav");
            }
        }
    }
}
