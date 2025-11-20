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

        public static Texture2D Pixel;

        const int CELL = 20;
        const int WIDTH = 32;
        const int HEIGHT = 24;

        List<Point> snake = new List<Point>();
        Point direction = new Point(1, 0);

        Random rnd = new Random();
        Point food;

        double timer = 0;
        double step = 0.15; // <<< POMALÝ POHYB

        bool gameOver = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = WIDTH * CELL;
            graphics.PreferredBackBufferHeight = HEIGHT * CELL;
        }

        protected override void Initialize()
        {
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            ResetGame();

            base.Initialize();
        }

        void ResetGame()
        {
            snake.Clear();
            snake.Add(new Point(WIDTH / 2, HEIGHT / 2));
            direction = new Point(1, 0);
            SpawnFood();
            gameOver = false;
        }

        void SpawnFood()
        {
            food = new Point(rnd.Next(0, WIDTH), rnd.Next(0, HEIGHT));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    ResetGame();
                return;
            }

            HandleInput();

            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= step)
            {
                timer = 0;
                MoveSnake();
            }

            base.Update(gameTime);
        }

        void HandleInput()
        {
            var k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Up) && direction.Y != 1) direction = new Point(0, -1);
            if (k.IsKeyDown(Keys.Down) && direction.Y != -1) direction = new Point(0, 1);
            if (k.IsKeyDown(Keys.Left) && direction.X != 1) direction = new Point(-1, 0);
            if (k.IsKeyDown(Keys.Right) && direction.X != -1) direction = new Point(1, 0);
        }

        void MoveSnake()
        {
            Point head = snake[0];
            Point newHead = new Point(head.X + direction.X, head.Y + direction.Y);

            // kolize se zdí
            if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= WIDTH || newHead.Y >= HEIGHT)
            {
                gameOver = true;
                return;
            }

            // kolize se sebou
            if (snake.Contains(newHead))
            {
                gameOver = true;
                return;
            }

            snake.Insert(0, newHead);

            // jídlo?
            if (newHead == food)
                SpawnFood();
            else
                snake.RemoveAt(snake.Count - 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();

            // food
            spriteBatch.Draw(Pixel, new Rectangle(food.X * CELL, food.Y * CELL, CELL, CELL), Color.Red);

            // snake
            foreach (var p in snake)
                spriteBatch.Draw(Pixel, new Rectangle(p.X * CELL, p.Y * CELL, CELL, CELL), Color.Lime);

            if (gameOver)
            {
                spriteBatch.DrawString(Content.Load<SpriteFont>("DefaultFont"),
                    "GAME OVER\nPress SPACE to restart",
                    new Vector2(100, 200),
                    Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
