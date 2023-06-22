using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjetMonogame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        MenuPage currentPage;

        Color backgroundColor;
        Color selectedColor = Color.Green;

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
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            currentPage = MenuPage.StartGame;
            backgroundColor = Color.Black;
            showMenu = true;
            menuEnabled = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (menuEnabled)
            {
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
                    menuEnabled = true; // Réactiver le contrôle du menu
                    backgroundColor = Color.Black; // Réinitialiser la couleur de l'arrière-plan à noir
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin();

            if (showMenu)
            {
                DrawMenu();
            }
            else
            {
                // Dessiner l'écran sans le menu ici
                // Par exemple :
                spriteBatch.DrawString(Content.Load<SpriteFont>("Font"), GetScreenText(), new Vector2(100, 100), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            Vector2 startGamePosition = new Vector2(100, 100);
            Vector2 leaderboardPosition = new Vector2(100, 150);
            Vector2 howToPlayPosition = new Vector2(100, 200);
            Vector2 exitPosition = new Vector2(100, 250);

            spriteBatch.DrawString(font, 
                startGameText, startGamePosition, currentPage == MenuPage.StartGame ? selectedColor : Color.White);

            spriteBatch.DrawString(font, 
                leaderboardText, leaderboardPosition, currentPage == MenuPage.Leaderboard ? selectedColor : Color.White);

            spriteBatch.DrawString(font, 
                howToPlayText, howToPlayPosition, currentPage == MenuPage.HowToPlay ? selectedColor : Color.White);

            spriteBatch.DrawString(font, 
                exitText, exitPosition, currentPage == MenuPage.Exit ? selectedColor : Color.White);
        }

        private string GetScreenText()
        {
            switch (currentPage)
            {
                case MenuPage.StartGame:
                    return "Game Screen";
                case MenuPage.Leaderboard:
                    return "Board Screen";
                case MenuPage.HowToPlay:
                    return "Instructions Screen";
                default:
                    return string.Empty;
            }
        }

        public class Program
        {
            static void Main(string[] args)
            {
                using (var game = new Game1())
                {
                    game.Run();
                }
            }
        }
    }
}
