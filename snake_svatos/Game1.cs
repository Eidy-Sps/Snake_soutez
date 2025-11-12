using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Snake1v1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D Pixel; // kvůli Item a Snake
        const int tileSize = 20;
        const int gridWidth = 40;
        const int gridHeight = 30;

        Vector2 direction = new Vector2(1, 0);
        List<Vector2> snake = new List<Vector2>();
        Vector2 food;
        double moveTimer = 0;
        double moveInterval = 150; // ms

        Random random = new Random();
        bool gameOver = false;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = gridWidth * tileSize;
            graphics.PreferredBackBufferHeight = gridHeight * tileSize;
        }

        protected override void Initialize()
        {
            snake.Clear();
            snake.Add(new Vector2(10, 10));
            SpawnFood();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
            try { font = Content.Load<SpriteFont>("DefaultFont"); } catch { font = null; }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameOver = false;
                    Initialize();
                }
                return;
            }

            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Up) && direction.Y == 0) direction = new Vector2(0, -1);
            if (ks.IsKeyDown(Keys.Down) && direction.Y == 0) direction = new Vector2(0, 1);
            if (ks.IsKeyDown(Keys.Left) && direction.X == 0) direction = new Vector2(-1, 0);
            if (ks.IsKeyDown(Keys.Right) && direction.X == 0) direction = new Vector2(1, 0);

            moveTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (moveTimer >= moveInterval)
            {
                MoveSnake();
                moveTimer = 0;
            }

            base.Update(gameTime);
        }

        void MoveSnake()
        {
            Vector2 newHead = snake[0] + direction;

            // narazil do zdi?
            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= gridWidth || newHead.Y >= gridHeight)
            {
                gameOver = true;
                return;
            }

            // narazil do sebe?
            foreach (var part in snake)
                if (part == newHead) { gameOver = true; return; }

            snake.Insert(0, newHead);

            // snědl jídlo?
            if (newHead == food)
                SpawnFood();
            else
                snake.RemoveAt(snake.Count - 1);
        }

        void SpawnFood()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(random.Next(0, gridWidth), random.Next(0, gridHeight));
            } while (snake.Contains(pos));
            food = pos;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            // jídlo
            spriteBatch.Draw(Pixel, new Rectangle((int)food.X * tileSize, (int)food.Y * tileSize, tileSize, tileSize), Color.Red);

            // had
            for (int i = 0; i < snake.Count; i++)
            {
                var color = i == 0 ? Color.LimeGreen : Color.Green;
                spriteBatch.Draw(Pixel, new Rectangle((int)snake[i].X * tileSize, (int)snake[i].Y * tileSize, tileSize - 1, tileSize - 1), color);
            }

            // Game over text
            if (gameOver && font != null)
            {
                string msg = "GAME OVER - Press ENTER";
                var size = font.MeasureString(msg);
                spriteBatch.DrawString(font, msg, new Vector2((graphics.PreferredBackBufferWidth - size.X) / 2, 200), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
