using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiple_Screens_Monogame
{
    public class Game1 : Game
    {
        Texture2D introBackground;
        Texture2D level1Background;
        Texture2D earthTexture;
        Texture2D enterpriseTexture;
        int level;

        Vector2 enterpriseLocation;
        int enterpriseSpeed = -2;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            level = 0;

            // TODO: use this.Content to load your game content here
            introBackground = Content.Load<Texture2D>("tng_intro");
            level1Background = Content.Load<Texture2D>("space_background");
            earthTexture = Content.Load<Texture2D>("earth");
            enterpriseTexture = Content.Load<Texture2D>("enterprise_side_small");

            _graphics.PreferredBackBufferWidth = introBackground.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = introBackground.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Enter is hit to start the game
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && level == 0)
            {
                level = 1;
                _graphics.PreferredBackBufferWidth = level1Background.Width;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = level1Background.Height;   // set this value to the desired height of your window
                _graphics.ApplyChanges();
                enterpriseLocation = new Vector2(350, 50 + earthTexture.Height / 2);
            }

            if (level == 1)
            {
                enterpriseLocation.X += enterpriseSpeed;
            }
                

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (level == 0)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(introBackground, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
            else if (level == 1)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(level1Background, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(enterpriseTexture, enterpriseLocation, Color.White);
                _spriteBatch.Draw(earthTexture, new Vector2(350, 50), Color.White);
                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}
