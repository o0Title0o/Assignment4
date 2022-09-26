using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Assignment4
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D ballTexture;
        Texture2D charTexture;

        Vector2 charPosition;
        Vector2[] ballPosition = new Vector2[4];
        int[] ballcolor = new int[4];


        bool personHit;

        //move
        int direction = 0;
        int speed = 5;

        //Animation
        //int frame;
        int totalFrames;
        int framepersec;
        float timePerFrame;
        float totalElapsed;

        Random r = new Random();

        KeyboardState keyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            charTexture = Content.Load<Texture2D>("Char01");
            ballTexture = Content.Load<Texture2D>("ball");

            frame = 0;
            totalFrames = 4;
            framepersec = 8;
            timePerFrame = (float)1 / framepersec;
            totalElapsed = 0;

            for (int i = 0; i < 4; i++)
            {
                ballPosition[i].X = r.Next(graphics.GraphicsDevice.Viewport.Width - ballTexture.Width / 6);
                ballPosition[i].Y = r.Next(graphics.GraphicsDevice.Viewport.Height - ballTexture.Height);
                ballcolor[i] = r.Next(6);
                bool repeat;
                bool pass = false;
                while (!pass)
                {
                    repeat = false;
                    for (int j = 0; j < 4; j++)
                    {
                        if (i != j)
                        {
                            if (ballcolor[i] == ballcolor[j])
                            {
                                repeat = true;
                            }
                        }
                    }
                    if (repeat)
                    {
                        ballcolor[i] = r.Next(6);
                    }
                    else
                    {
                        pass = true;
                    }
                }
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                direction = 0;
                charPosition.Y += speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                direction = 1;
                charPosition.X -= speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                direction = 2;
                charPosition.X += speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                direction = 3;
                charPosition.Y -= speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            Rectangle charRectangle = new Rectangle((int)charPosition.X, (int)charPosition.Y, 32, 48);

            for (int i = 0; i < 4; i++)
            {
                Rectangle blockRectangle = new Rectangle((int)ballPosition[i].X, (int)ballPosition[i].Y, 24, 24);
                if (charRectangle.Intersects(blockRectangle) == true)
                {
                    personHit = true;
                    ballPosition[i].X = r.Next(graphics.GraphicsDevice.Viewport.Width - ballTexture.Width / 6);
                    ballPosition[i].Y = r.Next(graphics.GraphicsDevice.Viewport.Height - ballTexture.Height);
                    ballcolor[i] = r.Next(6);
                    bool repeat;
                    bool pass = false;
                    while (!pass)
                    {
                        repeat = false;
                        for (int j = 0; j < 4; j++)
                        {
                            if (i != j)
                            {
                                if (ballcolor[i] == ballcolor[j])
                                {
                                    repeat = true;
                                }
                            }
                        }
                        if (repeat)
                        {
                            ballcolor[i] = r.Next(6);
                        }
                        else
                        {
                            pass = true;
                        }
                    }
                    break;
                }
                else if (charRectangle.Intersects(blockRectangle) == false)
                {
                    personHit = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            if (personHit == true)
            {
                device.Clear(Color.Red);
            }

            else
            {
                device.Clear(Color.CornflowerBlue);
            }
            spriteBatch.Begin();

            for (int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(ballTexture, ballPosition[i], new Rectangle(24 * ballcolor[i], 0, 24, 24), Color.White);
            }
            spriteBatch.Draw(charTexture, charPosition, new Rectangle(32 * frame, 48 * direction, 32, 48), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void UpdateFrame(float elaped)
        {
            totalElapsed += elaped;
            if (totalElapsed > timePerFrame)
            {
                frame = (frame + 1) % totalFrames;
                totalElapsed -= timePerFrame;
            }
        }
        //Hello
    }
}
