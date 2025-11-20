using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake1v1.Interfaces;
using System.Collections.Generic;

namespace Snake1v1.Entities
{
    public class Snake : IEntity
    {
        public List<Point> Body = new List<Point>();
        public Point Direction = new Point(1, 0);
        public Color Color;

        public Snake(Point startPos, Color color)
        {
            Body.Add(startPos);
            Color = color;
        }

        public virtual void Update(GameTime gameTime)
        {
            Point head = Body[0];
            Point newHead = new Point(head.X + Direction.X * 20, head.Y + Direction.Y * 20);

            Body.Insert(0, newHead);
            Body.RemoveAt(Body.Count - 1);
        }

        public void Grow()
        {
            Body.Add(Body[Body.Count - 1]);
        }

        public bool CheckSelfCollision()
        {
            Point head = Body[0];
            for (int i = 1; i < Body.Count; i++)
                if (Body[i] == head)
                    return true;

            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var p in Body)
                spriteBatch.Draw(Game1.Pixel, new Rectangle(p.X, p.Y, 20, 20), Color);
        }

        public void OnCollision(IEntity other) { }
    }
}
