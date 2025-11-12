using Microsoft.Xna.Framework;

namespace Snake1v1.Entities
{
    public class AI : Snake
    {
        public int SlowTimer = 0;

        public AI(Vector2 startPosition) : base(startPosition) { }

        public override void Update(GameTime gameTime)
        {
            if (SlowTimer > 0) { Speed = 1; SlowTimer--; } else { Speed = 2; }

            // jednoduchý pohyb AI (náhodný nebo rovně)
            base.Update(gameTime);
        }
    }
}
