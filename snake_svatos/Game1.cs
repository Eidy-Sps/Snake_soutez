using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameSnake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Grid and game settings
        private const int CellSize = 20; // pixels per cell
        private const int GridWidth = 30; // cells horizontally
        private const int GridHeight = 20; // cells vertically
        private int _screenWidth => GridWidth * CellSize;
        private int _screenHeight => GridHeight * CellSize;

        // Game state
        private LinkedList<Point> _snake; // head at First
        private Point _direction; // current moving direction
        private Point _apple;
        private double _moveTimer; // accumulates elapsed time
        private double _moveInterval = 0.12; // seconds between moves (speed)
        private bool _isGameOver;
        private Random _rand = new Random();

        // Drawing resources
        private Texture2D _pixel;
        private Texture2D _background; // background image
        private SpriteFont _font; // optional

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "MonoGame - Snake";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight + 40; // extra for HUD
            _graphics.ApplyChanges();

            ResetGame();

            base.Initialize();
        }

        private void ResetGame()
        {
            _snake = new LinkedList<Point>();
            var start = new Point(GridWidth / 2, GridHeight / 2);
            _snake.AddFirst(start);
            _snake.AddLast(new Point(start.X - 1, start.Y));
            _snake.AddLast(new Point(start.X - 2, start.Y));

            _direction = new Point(1, 0);
            PlaceApple();
            _moveTimer = 0;
            _isGameOver = false;
            _moveInterval = 0.12;
        }

        private void PlaceApple()
        {
            while (true)
            {
                var x = _rand.Next(0, GridWidth);
                var y = _rand.Next(0, GridHeight);
                var p = new Point(x, y);
                bool collides = false;
                foreach (var s in _snake)
                {
                    if (s == p)
                    {
                        collides = true;
                        break;
                    }
                }
                if (!collides)
                {
                    _apple = p;
                    break;
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 1x1 white pixel texture used to draw rectangles
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Load background image (musíš mít v Content)
            try
            {
                _background = Content.Load<Texture2D>("background"); // název souboru bez přípony
            }
            catch
            {
                _background = null;
            }

            // Load font if available
            try
            {
                _font = Content.Load<SpriteFont>("DefaultFont");
            }
            catch
            {
                _font = null;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var k = Keyboard.GetState();

            if (!_isGameOver)
            {
                // Read input and change direction (no opposite allowed)
                if (IsKeyPressed(k, Keys.Left) && _direction != new Point(1, 0)) _direction = new Point(-1, 0);
                if (IsKeyPressed(k, Keys.Right) && _direction != new Point(-1, 0)) _direction = new Point(1, 0);
                if (IsKeyPressed(k, Keys.Up) && _direction != new Point(0, 1)) _direction = new Point(0, -1);
                if (IsKeyPressed(k, Keys.Down) && _direction != new Point(0, -1)) _direction = new Point(0, 1);

                _moveTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_moveTimer >= _moveInterval)
                {
                    _moveTimer -= _moveInterval;
                    Step();
                }
            }
            else
            {
                // On game over, press R to restart
                if (k.IsKeyDown(Keys.R)) ResetGame();
            }

            base.Update(gameTime);
        }

        private KeyboardState _prevState = Keyboard.GetState();
        private bool IsKeyPressed(KeyboardState state, Keys key)
        {
            return state.IsKeyDown(key) && !_prevState.IsKeyDown(key);
        }

        private void Step()
        {
            var head = _snake.First.Value;
            var newHead = new Point(head.X + _direction.X, head.Y + _direction.Y);

            // Wrap around screen
            if (newHead.X < 0) newHead.X = GridWidth - 1;
            if (newHead.X >= GridWidth) newHead.X = 0;
            if (newHead.Y < 0) newHead.Y = GridHeight - 1;
            if (newHead.Y >= GridHeight) newHead.Y = 0;

            // Check collision with self
            foreach (var part in _snake)
            {
                if (part == newHead)
                {
                    _isGameOver = true;
                    return;
                }
            }

            _snake.AddFirst(newHead);

            // Eat apple?
            if (newHead == _apple)
            {
                PlaceApple();
                _moveInterval = Math.Max(0.04, _moveInterval - 0.005);
            }
            else
            {
                _snake.RemoveLast();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw background
            if (_background != null)
                _spriteBatch.Draw(_background, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.White);
            else
                DrawRect(0, 0, _screenWidth, _screenHeight, Color.Black); // fallback

            // Draw apple
            DrawCell(_apple, Color.Red);

            // Draw snake
            bool head = true;
            foreach (var part in _snake)
            {
                DrawCell(part, head ? Color.YellowGreen : Color.LightGreen);
                head = false;
            }

            // Draw HUD
            DrawRect(0, _screenHeight, _screenWidth, 40, Color.DarkGray * 0.6f);
            string hud = _isGameOver ? "GAME OVER - Press R to restart" : $"Score: {_snake.Count - 3}  Speed: {Math.Round(1.0 / _moveInterval, 1)}";
            if (_font != null)
                _spriteBatch.DrawString(_font, hud, new Vector2(6, _screenHeight + 8), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);

            _prevState = Keyboard.GetState();
        }

        private void DrawCell(Point cell, Color color)
        {
            int x = cell.X * CellSize;
            int y = cell.Y * CellSize;
            DrawRect(x + 1, y + 1, CellSize - 2, CellSize - 2, color);
        }

        private void DrawRect(int x, int y, int w, int h, Color color)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(x, y, w, h), color);
        }
    }
}
