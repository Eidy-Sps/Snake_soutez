using Microsoft.Xna.Framework;
using System;

namespace Snake1v1.Entities
{
    public class AI : Snake
    {
        Random rnd = new Random();

        public AI(Point startPos, Color color) : base(startPos, color) { }

        public void UpdateAI(Point playerHead)
        {
            // jednoduché chase AI
            int dx = Math.Sign(playerHead.X - Body[0].X);
            int dy = Math.Sign(playerHead.Y - Body[0].Y);

            if (Math.Abs(playerHead.X - Body[0].X) > Math.Abs(playerHead.Y - Body[0].Y))
                TrySetDirection(new Point(dx, 0));
            else
                TrySetDirection(new Point(0, dy));

            // občas náhodná změna
            if (rnd.NextDouble() < 0.05)
            {
                var dirs = new[]
                {
                    new Point(1,0),
                    new Point(-1,0),
                    new Point(0,1),
                    new Point(0,-1)
                };
                TrySetDirection(dirs[rnd.Next(4)]);
            }
        }

        private void TrySetDirection(Point dir)
        {
            // zabrání otočení do sebe
            if (Body.Count > 1)
            {
                Point next = new Point(Body[0].X + dir.X * 20, Body[0].Y + dir.Y * 20);
                if (Body[1] == next)
                    return;
            }

            Direction = dir;
        }
    }
}
