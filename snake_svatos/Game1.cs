using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Snake1v1.Entities;



namespace Snake1v1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D Pixel;

        public static AI BotRef;

        Snake player;
        AI bot;
        List<Item> items = new List<Item>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Snake(new Vector2(100, 100));
            bot = new AI(new Vector2(400, 100));
            BotRef = bot;

            // spawn itemů
            items.Add(new Item(new Vector2(200, 200), ItemType.Food));
            items.Add(new Item(new Vector2(300, 300), ItemType.Weapon));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up)) player.CurrentDirection = Direction.Up;
            if (ks.IsKeyDown(Keys.Down)) player.CurrentDirection = Direction.Down;
            if (ks.IsKeyDown(Keys.Left)) player.CurrentDirection = Direction.Left;
            if (ks.IsKeyDown(Keys.Right)) player.CurrentDirection = Direction.Right;

            player.Update(gameTime);
            bot.Update(gameTime);

            foreach (var item in items.ToArray())
            {
                if (Vector2.Distance(player.Position, item.Position) < 20)
                {
                    player.OnCollision(item);
                    items.Remove(item);
                }

                if (Vector2.Distance(bot.Position, item.Position) < 20)
                {
                    bot.OnCollision(item);
                    items.Remove(item);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            player.Draw(spriteBatch);
            bot.Draw(spriteBatch);
            foreach (var item in items) item.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
