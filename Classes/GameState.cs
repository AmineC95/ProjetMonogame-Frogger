using Microsoft.Xna.Framework;
using ProjetMonogame.Enums;
using System.Collections.Generic;

namespace ProjetMonogame.Classes
{
    public class GameState
    {
        public Rectangle PlayerRectangle { get; set; }
        public List<Rectangle> CarRectangles { get; set; }
        public Rectangle BonusRectangle { get; set; }
        public Rectangle MalusRectangle { get; set; }
        public int Score { get; set; }
        public GameStateEnum GameStates { get; set; }
    }
}
