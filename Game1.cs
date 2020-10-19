using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiple_Screens_Monogame
{
    public class Game1 : Game
    {
        // Monogame essentials
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Background Textures
        Texture2D introBackground;
        Texture2D level1Background;
        Texture2D level2Background;

        // Ship Textures
        Texture2D enterpriseTexture;
        Texture2D enterpriseExplode;
        Texture2D enterpriseFront;
        Rectangle enterpriseRect;

        // Item Textures
        Texture2D earthTexture;
        Texture2D asteroidTexture;
        Rectangle asteroidRectangle;
        Texture2D moonTexture;
        Rectangle moonRect;

        //Fonts
        SpriteFont introFont;
        SpriteFont endFont;

        //Sounds
        SoundEffect introTheme;
        SoundEffectInstance introThemeInstance;

        SoundEffect engine;
        SoundEffectInstance engineInstance;

        SoundEffect warp;
        SoundEffectInstance warpInstance;

        SoundEffect endTheme;
        SoundEffectInstance endThemeInstance;

        SoundEffect explosion;
        SoundEffectInstance explosionInstance;

        int level;
        bool start;

        int enterpriseSpeed;
        int asteroidSpeed;

        double enterpriseWidth;
        double enterpriseHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Set initial values
            level = 0;        
            start = true;
            enterpriseSpeed = -2;
            asteroidSpeed = 3;

            base.Initialize();

            // Initial values that are dependent on content being loaded can be done here
            _graphics.PreferredBackBufferWidth = introBackground.Width;     // Game window is the size of background texture
            _graphics.PreferredBackBufferHeight = introBackground.Height;  
            _graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Textures
            introBackground = Content.Load<Texture2D>("tng_intro");
            introFont = Content.Load<SpriteFont>("IntroInstructions");
            endFont = Content.Load<SpriteFont>("EndScreenFont");

            level1Background = Content.Load<Texture2D>("space_background");
            level2Background = Content.Load<Texture2D>("endBackground");
            earthTexture = Content.Load<Texture2D>("earth");
            moonTexture = Content.Load<Texture2D>("moon");
            moonRect = new Rectangle(600, 200, 100, 100);

            asteroidTexture = Content.Load<Texture2D>("asteroid");
            asteroidRectangle = new Rectangle(500, 500, 75, 75);

            enterpriseTexture = Content.Load<Texture2D>("enterprise_side_small");
            enterpriseRect = new Rectangle(0, 0, enterpriseTexture.Width, enterpriseTexture.Height);
            enterpriseExplode = Content.Load<Texture2D>("enterpriseExplosion");


            enterpriseFront = Content.Load<Texture2D>("enterprise_front");
            enterpriseWidth = enterpriseTexture.Width;
            enterpriseHeight = enterpriseTexture.Height;

            // Load Sounds
            introTheme = Content.Load<SoundEffect>("The Next Generation Main Title");
            introThemeInstance = introTheme.CreateInstance();
            introThemeInstance.IsLooped = false;

            engine = Content.Load<SoundEffect>("engine");
            engineInstance = engine.CreateInstance();

            warp = Content.Load<SoundEffect>("warp");
            warpInstance = warp.CreateInstance();

            endTheme = Content.Load<SoundEffect>("Endtng");
            endThemeInstance = endTheme.CreateInstance();

            explosion = Content.Load<SoundEffect>("explosion");
            explosionInstance = explosion.CreateInstance();
            explosionInstance.IsLooped = false;
            
            



       

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

                // Moves asteroid
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    asteroidRectangle.X -= asteroidSpeed;

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    asteroidRectangle.X += asteroidSpeed;

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    asteroidRectangle.Y -= asteroidSpeed;

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    asteroidRectangle.Y += asteroidSpeed;

                enterpriseRect.X += enterpriseSpeed;

                if (enterpriseRect.Intersects(asteroidRectangle) && enterpriseSpeed != 0 && level == 1) // If the ship is moving to the side it can be hit by the asteroid and can be hit, detects a collision
                { 
                    level = 3;  
                    _graphics.PreferredBackBufferWidth = enterpriseExplode.Width;
                    _graphics.PreferredBackBufferHeight = enterpriseExplode.Height;
                    _graphics.ApplyChanges();
                    engineInstance.Stop();
                    explosionInstance.Play();
                }

                if (enterpriseRect.X < 0)   // Detects when the Enterprise is able to go to warp
                {
                    engineInstance.Stop();
                    enterpriseSpeed = 0;
                    enterpriseRect.X = 1;
                    enterpriseRect.Width = enterpriseTexture.Width;
                    enterpriseRect.Height = enterpriseTexture.Height;
                    enterpriseTexture = enterpriseFront;
                }

                if (enterpriseSpeed == 0)   // Detects when the enterprise is at warp
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
            else if (level == 3)    // Enterprise destroyed ending
            {
                if (explosionInstance.State == SoundState.Stopped && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    Exit();
                    
            }
                

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (level == 0) // Intro screen
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(introBackground, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(introFont, "Hit Enter to continue" , new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
            else if (level == 1)    // Main game screen
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(level1Background, new Vector2(0, 0), Color.White);

                if (enterpriseSpeed != 0)   //Draws ship before planet when moving left
                    _spriteBatch.Draw(enterpriseTexture, enterpriseRect, Color.White);

                _spriteBatch.Draw(earthTexture, new Vector2(350, 50), Color.White);
                _spriteBatch.Draw(moonTexture, moonRect, Color.White);
                _spriteBatch.Draw(asteroidTexture, asteroidRectangle, Color.White);

                if (enterpriseSpeed == 0)   //Draws ship after planet when moving forward
                    _spriteBatch.Draw(enterpriseTexture, enterpriseRect, Color.White);

                _spriteBatch.End();
            }
            else if (level == 2)    // End credits when enterprise escapes 
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(level2Background, new Vector2(0, 0), Color.White);
                _spriteBatch.End();
            }
            else if (level == 3)    //End credits when enterprise is destroyed
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(enterpriseExplode, new Vector2(0, 0), Color.White);

                if (explosionInstance.State == SoundState.Stopped)  // Text displayed after explosion sound is finished
                    _spriteBatch.DrawString(endFont, "You destroyed the Federation, hit ENTER to mourn.", new Vector2(10, _graphics.PreferredBackBufferHeight / 2), Color.White); 

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
