using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        int gameCount;
        bool start;

        //Sounds
        SoundEffect introTheme;
        SoundEffectInstance introThemeInstance;

        SoundEffect engine;
        SoundEffectInstance engineInstance;

        SoundEffect warp;
        SoundEffectInstance warpInstance;

        SoundEffect endTheme;
        SoundEffectInstance endThemeInstance;


        Vector2 enterpriseLocation;
        Rectangle enterpriseRect;
        int enterpriseSpeed = -2;
        int frameCount = 0;
        double enterpriseWidth;
        double enterpriseHeight;

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
            gameCount = 0;
            start = true;

            //Sounds
            introTheme = Content.Load<SoundEffect>("The Next Generation Main Title");
            introThemeInstance = introTheme.CreateInstance();
            introThemeInstance.IsLooped = false;

            engine = Content.Load<SoundEffect>("engine");
            engineInstance = engine.CreateInstance();

            warp = Content.Load<SoundEffect>("warp");
            warpInstance = warp.CreateInstance();

            endTheme = Content.Load<SoundEffect>("Endtng");
            endThemeInstance = endTheme.CreateInstance();


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
            enterpriseWidth = enterpriseTexture.Width;
            enterpriseHeight = enterpriseTexture.Height;



            _graphics.PreferredBackBufferWidth = introBackground.Width;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = introBackground.Height;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

        }

        protected override void Update(GameTime gameTime)
        {
            //Depending on the gamestate, as stored in level, we will updat appropriatley
            if (level == 0 && start == true)
            {
                start = false;
                introThemeInstance.Play();
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Enter is hit to start the game from the main screen
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && level == 0 || introThemeInstance.State == SoundState.Stopped && level == 0)
            {
                introThemeInstance.Stop();
                engineInstance.Play();
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
                if (enterpriseRect.X < 0)
                {
                    engineInstance.Stop();
                    enterpriseSpeed = 0;
                    enterpriseRect.X = 1;
                    enterpriseRect.Width = enterpriseTexture.Width;
                    enterpriseRect.Height = enterpriseTexture.Height;
                    enterpriseTexture = enterpriseFront;
                }
                if (enterpriseSpeed == 0)
                {
                    warpInstance.Play();
                    enterpriseWidth *= 1.01;
                    enterpriseHeight *= 1.01;

                    enterpriseRect.Width = (int)enterpriseWidth;
                    enterpriseRect.Height = (int)enterpriseHeight;

                }
                if (enterpriseRect.Width > level1Background.Width && level != 2)
                {
                    warpInstance.Stop();
                    endThemeInstance.Play();
                    level = 2;
                    _graphics.PreferredBackBufferWidth = level2Background.Width;        // set this value to the desired width of your window
                    _graphics.PreferredBackBufferHeight = level2Background.Height;      // set this value to the desired height of your window
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
                _spriteBatch.DrawString(introFont, "Hit Enter to continue" , new Vector2(0, 0), Color.White);
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
