using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake1v1.Interfaces
{
    public interface IEntity
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void OnCollision(IEntity other);
    }
}
