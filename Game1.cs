using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiple_Screens_Monogame
{
    public class Game1 : Game
    {
        Texture2D introBackground;
        Texture2D level1Background;
        Texture2D level2Background;

        Texture2D earthTexture;
        Texture2D enterpriseTexture;
        Texture2D enterpriseFront;

        SpriteFont regularFont;
        SpriteFont introFont;
        SpriteFont enfFont;
        Color textColor;
        int level;

        Vector2 enterpriseLocation;
        Rectangle enterpriseRect;
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

            textColor = Color.White;
            level = 0;

            // TODO: use this.Content to load your game content here
            introBackground = Content.Load<Texture2D>("tng_intro");
            introFont = Content.Load<SpriteFont>("IntroInstructions");
            enfFont = Content.Load<SpriteFont>("EndScreenFont");

            level1Background = Content.Load<Texture2D>("space_background");
            level2Background = Content.Load<Texture2D>("endBackground");
            earthTexture = Content.Load<Texture2D>("earth");
            enterpriseTexture = Content.Load<Texture2D>("enterprise_side_small");
            enterpriseRect = new Rectangle(0, 0, enterpriseTexture.Width, enterpriseTexture.Height);

            enterpriseFront = Content.Load<Texture2D>("enterprise_front");


            _graphics.PreferredBackBufferWidth = introBackground.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = introBackground.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

        }

        protected override void Update(GameTime gameTime)
        {
            //Depending on the gamestate, as stored in level, we will updat appropriatley

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Enter is hit to start the game from the main screen
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && level == 0)
            {
                level = 1;
                _graphics.PreferredBackBufferWidth = level1Background.Width;  // set this value to the desired width of your window
                _graphics.PreferredBackBufferHeight = level1Background.Height;   // set this value to the desired height of your window
                _graphics.ApplyChanges();
                //enterpriseLocation = new Vector2(350, 50 + earthTexture.Height / 2);
                enterpriseRect.Location = new Point(350, 50 + earthTexture.Height / 2);
            }

            if (level == 1)
            {

                enterpriseRect.X += enterpriseSpeed;
                if (enterpriseRect.X == 0)
                {
                    enterpriseSpeed = 0;
                    enterpriseRect.X = 1;
                    enterpriseRect.Width = enterpriseTexture.Width;
                    enterpriseRect.Height = enterpriseTexture.Height;
                    enterpriseTexture = enterpriseFront;
                }
                if (enterpriseSpeed == 0)
                {
                    enterpriseRect.Width *= 2;
                    enterpriseRect.Height *= 2;
                }
                if (enterpriseRect.Width > level1Background.Width && level != 2)
                {
                    level = 2;
                    _graphics.PreferredBackBufferWidth = level2Background.Width;  // set this value to the desired width of your window
                    _graphics.PreferredBackBufferHeight = level2Background.Height;   // set this value to the desired height of your window
                    _graphics.ApplyChanges();
                }
                    
               
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
                _spriteBatch.DrawString(introFont, "Hit Enter to Continue", new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
            else if (level == 1)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(level1Background, new Vector2(0, 0), Color.White);
                if (enterpriseSpeed != 0)   //Draws ship before planet when moving left
                    _spriteBatch.Draw(enterpriseTexture, enterpriseRect, Color.White);
                _spriteBatch.Draw(earthTexture, new Vector2(350, 50), Color.White);
                if (enterpriseSpeed == 0)   //Draws ship after planet when moving forward
                    _spriteBatch.Draw(enterpriseTexture, enterpriseRect, Color.White);
                _spriteBatch.End();
            }
            else if (level == 2)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(level2Background, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}
