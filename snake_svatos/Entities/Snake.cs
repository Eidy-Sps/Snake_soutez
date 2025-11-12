using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake1v1.Interfaces;
using System.Collections.Generic;

namespace Snake1v1.Entities
{
    public enum Direction { Up, Down, Left, Right }

    public class Snake : Character
    {
        public List<Vector2> Body = new List<Vector2>();
        public Direction CurrentDirection = Direction.Right;

        public Snake(Vector2 startPosition)
        {
            Position = startPosition;
            Body.Add(Position);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 movement = Vector2.Zero;
            switch (CurrentDirection)
            {
                case Direction.Up: movement = new Vector2(0, -Speed); break;
                case Direction.Down: movement = new Vector2(0, Speed); break;
                case Direction.Left: movement = new Vector2(-Speed, 0); break;
                case Direction.Right: movement = new Vector2(Speed, 0); break;
            }

            Position += movement;
            Body.Insert(0, Position);
            Body.RemoveAt(Body.Count - 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var segment in Body)
            {
                spriteBatch.Draw(Game1.Pixel, new Rectangle((int)segment.X, (int)segment.Y, 20, 20), Color.Green);
            }
        }

        public override void OnCollision(IEntity other)
        {
            if (other is Item item)
            {
                if (item.Type == ItemType.Food)
                {
                    Body.Add(Body[Body.Count - 1]); // prodloužení hada
                }
                else if (item.Type == ItemType.Weapon)
                {
                    // zpomalíme AI
                    Game1.BotRef.SlowTimer = 120; // cca 2 sekundy
                }
            }
        }
    }
}
