using System;
using OpenTK.Graphics.OpenGL4;
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
        
        
        //Console.WriteLine(board[19,14]);
        //Console.WriteLine(board[19,15]);

        private readonly float[] vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

        private int VAO,VBO;


        snakeVars snake = new snakeVars(); //Initialize the snake

        public void Debug() 
        {
            for(int i=29;i>=0;--i)         //Output to console
            {
                for(int j=0;j<40;++j) 
                {
                    if(snake.board[j,i]) 
                    {
                        Console.Write("#");
                    }
                    else if(j==snake.fruitX && i==snake.fruitY)
                    {
                        Console.Write("@");
                    }
                    else
                    {
                        Console.Write(".");
                    }    
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
        
        bool keyPressed = false;
        public void updateSnakeDir() //Check if key has been pressed and save it
        {
            currentTime=DateTime.Now;
            keyPressed = false;
            int neckX,neckY;
            if(snake.iter-2>=snake.begin)           // Check if the snake is going backwards towards its neck and
            {                                       //find position of its neck in the queue
                neckX=snake.queue[snake.iter-2].first;
                neckY=snake.queue[snake.iter-2].second;
            }
            else
            {
                neckX=snake.queue[snake.end-1+snake.iter-snake.begin].first;
                neckY=snake.queue[snake.end-1+snake.iter-snake.begin].second;
            }
            if (KeyboardState.IsKeyDown(Keys.W) && (neckX!=snake.posX || neckY!=snake.posY+1)) 
                {
                    if((int)(currentTime-previousTime).Milliseconds>=boostLimit) //React instantly if key pressed with x2 speed limit
                    {
                        keyPressed = true;
                        previousTime=currentTime;
                    }
                    snake.dir=0;
                }
                if (KeyboardState.IsKeyDown(Keys.S) &&(neckX!=snake.posX || neckY!=snake.posY-1))
                {
                    if((int)(currentTime-previousTime).Milliseconds>=boostLimit)
                    {
                        keyPressed = true;
                        previousTime=currentTime;
                    }
                    snake.dir=1;
                }
                if (KeyboardState.IsKeyDown(Keys.A) && (neckX!=snake.posX-1 || neckY!=snake.posY))
                {
                    if((int)(currentTime-previousTime).Milliseconds>=boostLimit)
                    {
                        keyPressed = true;
                        previousTime=currentTime;
                    }
                    snake.dir=2;
                }
                if (KeyboardState.IsKeyDown(Keys.D) && (neckX!=snake.posX+1 || neckY!=snake.posY))
                {
                    if((int)(currentTime-previousTime).Milliseconds>=boostLimit)
                    {
                        keyPressed = true;
                        previousTime=currentTime;
                    }
                    snake.dir=3;
                }
        }

        DateTime currentTime, previousTime=DateTime.Now;
        int ticksPerSecond=1000/2, boostLimit=1000/4;

        public void tick() // Execute by tickrate
        {
            currentTime=DateTime.Now;
            if((int)(currentTime-previousTime).Milliseconds >= ticksPerSecond || keyPressed)
            {           
                previousTime=currentTime;
                snake.moveSnake();
                if(snake.gameOver)  {return;}
                Debug();
            }       
        }
        

       
        protected override void OnLoad()
        {

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VBO = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer,VBO);
            
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            base.OnLoad();

        }
         protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();

            base.OnRenderFrame(e);

        }
    
        protected override void OnUpdateFrame(FrameEventArgs e)// This function runs on every update frame.
        {
            
            if (KeyboardState.IsKeyDown(Keys.Escape)) //Close if ESC or Gameover
            {
                Close();
            }
            if(snake.gameOver) 
            {
                Console.WriteLine("Game Over");
                Close();
            }
            updateSnakeDir(); //Check for keys pressed and a ticks
            tick();
            base.OnUpdateFrame(e);
        }
        protected override void OnUnload()
        {
            base.OnUnload();
        }
    }
}