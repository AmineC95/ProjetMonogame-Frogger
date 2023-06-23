using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetMonogame.Interface
{
    public interface IGameInterface
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
