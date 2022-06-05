using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace Snake
{
    // This is where all OpenGL code will be written.
    // OpenToolkit allows for several functions to be overriden to extend functionality; this is how we'll be writing code.
    public class Window : GameWindow
    {
        // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        class snakeVars
        {
            public int dir,posX,posY,fruitX,fruitY;
        
        }

        snakeVars snake = new snakeVars();

        public void updateSnakeDir()
        {
            if (KeyboardState.IsKeyDown(Keys.W) && snake.dir!=1)
                {
                   snake.dir=0;
                }
                if (KeyboardState.IsKeyDown(Keys.S) && snake.dir!=0)
                {
                    snake.dir=1;
                }
                if (KeyboardState.IsKeyDown(Keys.A) && snake.dir!=3)
                {
                    snake.dir=2;
                }
                if (KeyboardState.IsKeyDown(Keys.D) && snake.dir!=2)
                {
                    snake.dir=3;
                }
        }

        DateTime currentTime,previousTime=DateTime.Now;
        int ticksPerSecond = 1000/2;

        public void tick()
        {
            currentTime=DateTime.Now;
            if((int)(currentTime-previousTime).Milliseconds>=ticksPerSecond)
            {
                previousTime=currentTime;
                Console.WriteLine(snake.dir);
            }

            
        }

        

        // This function runs on every update frame.
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }
            updateSnakeDir();
            tick();
            base.OnUpdateFrame(e);
        }
    }
}