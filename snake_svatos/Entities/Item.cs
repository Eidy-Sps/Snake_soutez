using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake1v1.Interfaces;

namespace Snake1v1.Entities
{
    public enum ItemType { Food, Weapon }

    public class Item : IEntity
    {
        public Vector2 Position;
        public ItemType Type;

        public Item(Vector2 pos, ItemType type)
        {
            Position = pos;
            Type = type;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Type == ItemType.Food ? Color.Red : Color.Yellow;
            spriteBatch.Draw(Game1.Pixel, new Rectangle((int)Position.X, (int)Position.Y, 20, 20), color);
        }

        public void OnCollision(IEntity other) { }
    }
}
