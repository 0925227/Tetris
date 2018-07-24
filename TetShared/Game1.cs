using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;


namespace TetShared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //screensize
        int ScreenWidth;
        int ScreenHeight;

        int GridWidth;
        int GridHeight;
        int BlockSize;

        Texture2D block;
        Texture2D NextPieceCanvas;
        SpriteFont Font;

        cButton StartButton;
  
        cButton ExitButton;

        Screen s1;
        Screen s2;
        Song song1;

        int StepTime;
        int ElapsedTime;
        int KeyBoardElapsedTime;

        Board Tetris;
        
        enum GameState
        {
            MainMenu,
            Playing,
            
        }
        GameState CurrentGameState = GameState.MainMenu;

        public Game1()
        {

            StepTime = 500;             // Snelheid waarmee een tetromino omlaag beweegt in ms
            ElapsedTime = 0;            // Verstreken tijd sinds de laatste update
            KeyBoardElapsedTime = 0;    // Verstreken tijd sinds de laatste input


#if __ANDROID__
                        // Hoogte en Breedte van de Grid
            GridWidth = 10;
            GridHeight = 20;
            BlockSize = 100;
                        // Schermresolutie
            ScreenWidth = ((BlockSize * GridWidth) * 2) -350;
            ScreenHeight = BlockSize * GridHeight - 850;

#else

            // Hoogte en Breedte van de Grid
            GridWidth = 10;
            GridHeight = 20;
            BlockSize = 31;

            // Schermresolutie
            ScreenWidth = 600;// (BlockSize * GridWidth) * 2;
            ScreenHeight = 620;// BlockSize * GridHeight;
#endif


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            StartButton = new cButton(Content, "Start", graphics.GraphicsDevice, (ScreenWidth / 2)-75, (ScreenHeight / 2) + 10);

            ExitButton = new cButton(Content, "Exit", graphics.GraphicsDevice, (ScreenWidth / 2)-75, (ScreenHeight / 2) + 210);

            song1 = Content.Load<Song>("OG");
            MediaPlayer.Play(song1);
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.IsRepeating = true;
            s1 = new Screen(Content, "BG2");
            s2 = new Screen(Content, "BG1");

            // Game Textures
#if __ANDROID__
            block = Content.Load<Texture2D>("block2");
            NextPieceCanvas = Content.Load<Texture2D>("nextpiececanvas2");
#else
            block = Content.Load<Texture2D>("block");
            NextPieceCanvas = Content.Load<Texture2D>("nextpiececanvas");

#endif

            // Lettertype
            Font = Content.Load<SpriteFont>("Score");

            // Het tetris bord
            Tetris = new Board(spriteBatch, block, NextPieceCanvas, ScreenWidth, ScreenHeight, GridWidth, GridHeight, BlockSize);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            block.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update de verstreken tijd per iteratie
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            KeyBoardElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            MouseState mouse = Mouse.GetState();
            KeyboardState state = Keyboard.GetState();
            TouchCollection touchCollection = TouchPanel.GetState();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (StartButton.isClicked == true)
                    {

                        CurrentGameState = GameState.Playing;

                    }
                    break;
                case GameState.Playing:
                    //if (Tetris.GameOver)
                    //{
                    //    Tetris.ResetGrid();
                    //    CurrentGameState = GameState.MainMenu;

                    //}
                    break;
            }





                    if (KeyBoardElapsedTime > 200)
            {
                foreach (TouchLocation tl in touchCollection)
                {

                    Console.WriteLine(touchCollection[0].Position);
                    if ((tl.Position.Y > 500 && tl.Position.Y < 2000) && tl.Position.X <700)
                    {
                        Tetris.Move("left");
                        KeyBoardElapsedTime = 0;
                    }
                    if ((tl.Position.Y > 500 && tl.Position.Y < 2000) && tl.Position.X > 700)
                    {
                        Tetris.Move("right");
                        KeyBoardElapsedTime = 0;
                    }
                    if (tl.Position.Y < 500)
                    {
                        Tetris.RotatePiece();
                        KeyBoardElapsedTime = 0;
                    }
                    if (tl.Position.Y > 2000)
                    {
                        ElapsedTime = StepTime + 1;
                        KeyBoardElapsedTime = 175;
                    }
                    if (ElapsedTime > StepTime)
                    {
                        Tetris.BlockFall();
                        if (Tetris.TimeReset)
                        {
                            ElapsedTime = 0;
                        }
                    }

                }
            }

            // Check of er een toets word ingedrukt, zoja voer de opdracht uit
            if (KeyBoardElapsedTime > 200)
            {
                if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
                {
                    Tetris.Move("left");
                    KeyBoardElapsedTime = 0;
                }

                if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
                {
                    Tetris.Move("right");
                    KeyBoardElapsedTime = 0;
                }

                if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                {
                    ElapsedTime = StepTime + 1;
                    KeyBoardElapsedTime = 175;
                }

                if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
                {
                    Tetris.RotatePiece();
                    KeyBoardElapsedTime = 0;
                }

                // If statement checkt of het tijd is om de tetromino omlaag te bewegen
                if (ElapsedTime > StepTime)
                {
                    Tetris.BlockFall();
                    if (Tetris.TimeReset)
                    {
                        ElapsedTime = 0;
                    }
                }
            }
            
            StartButton.Update(mouse);
            ExitButton.Update(mouse);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:

                    s1.Draw(spriteBatch);
                    StartButton.Draw(spriteBatch);

                    ExitButton.Draw(spriteBatch);


                    break;

                case GameState.Playing:

                    Tetris.Draw();



#if __WINDOWS__
                    MuteButton.Draw(spriteBatch);
                    UnmuteButton.Draw(spriteBatch);
                    spriteBatch.DrawString(Font, "Score:" + Tetris.Score, new Vector2(400, 300), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);  // Tekent de huidige score
#else
                    spriteBatch.DrawString(Font, "Score:" + Tetris.Score, new Vector2(1050, 700), Color.Black, 0f, new Vector2(0, 0), 5.00f, SpriteEffects.None, 1f);  // Tekent de huidige score
#endif
                    break;


            }


            //            Tetris.Draw();      // Tekent het tetris bord

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
