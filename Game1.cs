using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProjetMonogame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _startZoneImage;
        private Texture2D _safeZoneImage;
        private Texture2D _roadImage;

        private Texture2D _playerTexture;
        private Texture2D _carTexture;
        private Rectangle _playerRectangle;
        private List<Rectangle> _carRectangles;
        private const int CarSpeed = 12;

        private const int ScreenWidth = 1080;
        private const int ScreenHeight = 720;

        private Texture2D _howToPlayImage;
        private Texture2D _leaderboardImage;
        private Texture2D _menuImage;

        private Rectangle _bonusRectangle;
        private Rectangle _malusRectangle;

        private Texture2D _bonusTexture;
        private Texture2D _malusTexture;

        private int _bonusTimer;
        private int _malusTimer;

        private Random _random;

        private int _score;
        private bool _gameOver;
        private bool _gameWon;

        private Rectangle _safeZone;
        private Rectangle _startZone;

        private SpriteFont _font;

        private MenuPage currentPage;

        Color backgroundColor;
        Color selectedColor = Color.DarkRed;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        bool showMenu;
        bool menuEnabled;

        string startGameText = "Start Game";
        string leaderboardText = "Leaderboard";
        string howToPlayText = "How to Play";
        string exitText = "Exit";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            _random = new Random();
            currentPage = MenuPage.StartGame;
            backgroundColor = Color.Black;
            showMenu = true;
            menuEnabled = true;

            _bonusRectangle = new Rectangle(_random.Next(0, ScreenWidth - 40), _random.Next(0, ScreenHeight - 40), 40, 40);
            _malusRectangle = new Rectangle(_random.Next(0, ScreenWidth - 40), _random.Next(0, ScreenHeight - 40), 40, 40);


            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _howToPlayImage = Content.Load<Texture2D>("howtoplay");
            _leaderboardImage = Content.Load<Texture2D>("leaderboard");
            _menuImage = Content.Load<Texture2D>("bgmainmenu");

            _startZoneImage = Content.Load<Texture2D>("arrivalzone"); // faire l'image de départ 
            _safeZoneImage = Content.Load<Texture2D>("arrivalzone");
            _roadImage = Content.Load<Texture2D>("arrivalzone"); //Créer une route 

            _playerTexture = Content.Load<Texture2D>("frog");
            _carTexture = Content.Load<Texture2D>("car");

            _bonusTexture = new Texture2D(GraphicsDevice, 1, 1);
            _bonusTexture.SetData(new Color[] { Color.Red });

            _malusTexture = new Texture2D(GraphicsDevice, 1, 1);
            _malusTexture.SetData(new Color[] { Color.Blue });



            _playerRectangle = new Rectangle(ScreenWidth / 2 - 25, ScreenHeight - 75, 50, 50);

            _carRectangles = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int x = -200 - random.Next(0, 800);
                int y = 100 + i * 100;
                Rectangle carRectangle = new Rectangle(x, y, 100, 50);
                _carRectangles.Add(carRectangle);
            }

            _score = 0;
            _gameOver = false;
            _gameWon = false;

            // Définition de la zone de départ (en violet)
            int startZoneHeight = 80;
            int startZoneY = ScreenHeight - startZoneHeight;
            _startZone = new Rectangle(0, startZoneY, ScreenWidth, startZoneHeight);

            // Définition de la zone d'arrivée (en jaune)
            int safeZoneHeight = 80;
            int safeZoneY = 0;
            _safeZone = new Rectangle(0, safeZoneY, ScreenWidth, safeZoneHeight);

            _font = Content.Load<SpriteFont>("Font");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (menuEnabled)
            {
                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();

                if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
                {
                    currentPage++;
                    if ((int)currentPage > (int)MenuPage.Exit)
                        currentPage = MenuPage.StartGame;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    currentPage--;
                    if (currentPage < MenuPage.StartGame)
                        currentPage = MenuPage.Exit;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    switch (currentPage)
                    {
                        case MenuPage.StartGame:
                            backgroundColor = Color.Red;
                            showMenu = false;
                            menuEnabled = false;
                            break;
                        case MenuPage.Leaderboard:
                            backgroundColor = Color.Green;
                            showMenu = false;
                            menuEnabled = false;
                            break;
                        case MenuPage.HowToPlay:
                            backgroundColor = Color.Blue;
                            showMenu = false;
                            menuEnabled = false;
                            break;
                        case MenuPage.Exit:
                            Exit();
                            break;
                    }
                }
            }
            else
            {
                if (currentKeyboardState.IsKeyDown(Keys.Back) && previousKeyboardState.IsKeyUp(Keys.Back))
                {
                    showMenu = true;
                    menuEnabled = true;
                    backgroundColor = Color.Black;
                }

                if (!_gameOver && !_gameWon)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        _playerRectangle.X -= 5;
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        _playerRectangle.X += 5;
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        _playerRectangle.Y -= 5;
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        _playerRectangle.Y += 5;

                    if (_playerRectangle.Intersects(_safeZone))
                    {
                        _gameWon = true;
                    }

                    foreach (var carRectangle in _carRectangles)
                    {
                        if (_playerRectangle.Intersects(carRectangle) && !_safeZone.Contains(_playerRectangle))
                        {
                            _gameOver = true;
                            break;
                        }
                    }

                    for (int i = 0; i < _carRectangles.Count; i++)
                    {
                        Rectangle carRectangle = _carRectangles[i];
                        carRectangle.X += CarSpeed;
                        _carRectangles[i] = carRectangle;

                        if (carRectangle.X > ScreenWidth)
                        {
                            carRectangle.X = -200;
                            _carRectangles[i] = carRectangle;
                            _score++;
                        }
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        RestartGame();
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Back) && previousKeyboardState.IsKeyUp(Keys.Back))
                {
                    showMenu = true;
                    menuEnabled = true;
                    backgroundColor = Color.Black;
                }
            }
            if (_playerRectangle.Intersects(_bonusRectangle))
            {
                _score++;
                // Faire apparaître le bonus à un nouvel endroit aléatoire
                _bonusRectangle.X = _random.Next(0, ScreenWidth - 40);
                _bonusRectangle.Y = _random.Next(0, ScreenHeight - 40);
            }

            if (_playerRectangle.Intersects(_malusRectangle))
            {
                _score--;
                // Faire apparaître le malus à un nouvel endroit aléatoire
                _malusRectangle.X = _random.Next(0, ScreenWidth - 40);
                _malusRectangle.Y = _random.Next(0, ScreenHeight - 40);
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            if (showMenu)
            {
                // Draw the full background for the main menu
                _spriteBatch.Draw(_menuImage, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                DrawMenu();
            }
            else
            {
                if (currentPage == MenuPage.StartGame)
                {
                    // Draw the road image for the in-between zone.
                    _spriteBatch.Draw(_roadImage, new Rectangle(0, _startZone.Height, ScreenWidth, ScreenHeight - _startZone.Height - _safeZone.Height), Color.White);

                    // Draw the start and safe zones with the newly loaded images.
                    _spriteBatch.Draw(_startZoneImage, _startZone, Color.White);
                    _spriteBatch.Draw(_safeZoneImage, _safeZone, Color.White);

                    _spriteBatch.Draw(_bonusTexture, _bonusRectangle, Color.White);
                    _spriteBatch.Draw(_malusTexture, _malusRectangle, Color.White);


                    // Now drawing the player and cars with their new images.
                    _spriteBatch.Draw(_playerTexture, _playerRectangle, Color.White);

                    foreach (var carRectangle in _carRectangles)
                    {
                        _spriteBatch.Draw(_carTexture, carRectangle, Color.White);
                    }

                    _spriteBatch.DrawString(
                        _font,
                        "Score: " + _score,
                        new Vector2(10, 10),
                        Color.Black
                    );

                    _spriteBatch.DrawString(
                            _font,
                            "Press R to Restart",
                            new Vector2(10, 50),
                            Color.OrangeRed
                        );

                    if (_gameWon)
                    {
                        _spriteBatch.DrawString(
                            _font,
                            "You Win!",
                            new Vector2(ScreenWidth / 2 - 60, ScreenHeight / 2),
                            Color.Green
                        );
                    }
                    else if (_gameOver)
                    {
                        _spriteBatch.DrawString(
                            _font,
                            "Game Over",
                            new Vector2(ScreenWidth / 2 - 80, ScreenHeight / 2),
                            Color.Red
                        );
                    }
                }
                else if (currentPage == MenuPage.HowToPlay)
                {
                    // Draw the full background for the "How to Play" page
                    _spriteBatch.Draw(_howToPlayImage, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                }
                else if (currentPage == MenuPage.Leaderboard)
                {
                    // Draw the full background for the "Leaderboard" page
                    _spriteBatch.Draw(_leaderboardImage, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            Vector2 startGamePosition = new Vector2(100, 100);
            Vector2 leaderboardPosition = new Vector2(100, 150);
            Vector2 howToPlayPosition = new Vector2(100, 200);
            Vector2 exitPosition = new Vector2(100, 250);

            _spriteBatch.DrawString(_font,
                startGameText, startGamePosition, currentPage == MenuPage.StartGame ? selectedColor : Color.White);

            _spriteBatch.DrawString(_font,
                leaderboardText, leaderboardPosition, currentPage == MenuPage.Leaderboard ? selectedColor : Color.White);

            _spriteBatch.DrawString(_font,
                howToPlayText, howToPlayPosition, currentPage == MenuPage.HowToPlay ? selectedColor : Color.White);

            _spriteBatch.DrawString(_font,
                exitText, exitPosition, currentPage == MenuPage.Exit ? selectedColor : Color.White);

        }

        private void RestartGame()
        {
            _playerRectangle.X = ScreenWidth / 2 - 25;
            _playerRectangle.Y = ScreenHeight - 75;

            Random random = new Random();
            for (int i = 0; i < _carRectangles.Count; i++)
            {
                int x = -200 - random.Next(0, 800);
                int y = 100 + i * 100;
                Rectangle carRectangle = _carRectangles[i];
                carRectangle.X = x;
                carRectangle.Y = y;
                _carRectangles[i] = carRectangle;
            }

            _score = 0;
            _gameOver = false;
            _gameWon = false;
        }

        public static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}
