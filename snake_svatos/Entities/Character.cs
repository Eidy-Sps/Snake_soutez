using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake1v1.Interfaces;

namespace Snake1v1.Entities
{
    public abstract class Character : IEntity
    {
        public Vector2 Position;
        public int Speed = 2;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void OnCollision(IEntity other);
    }
}
