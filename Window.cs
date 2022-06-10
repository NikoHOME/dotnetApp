using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace Snake
{
          
    
    // This is where all OpenGL code will be written.
    // OpenToolkit allows for several functions to be overriden to extend functionality; this is how we'll be writing code.
    public unsafe class Window : GameWindow
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
            -0.9f, -0.9f, 0.0f, 
            0.9f, -0.9f, 0.0f, 
            0.9f,  0.45f, 0.0f,
            -0.9f, 0.45f, 0.0f   
        };

        public class snakePart
        {
            public float[] position= new float[3];
            public float[] colour  = new float[4];
        }

        public snakePart[] snakeParts = new  snakePart[1201];

        private int[] VAO = new int[2];
        private int[] VBO = new int[2];
        private int EBO;


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

            VBO[0] = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer,VBO[0]);
            
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VAO[0] = GL.GenVertexArray();
            GL.BindVertexArray(VAO[0]);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            VBO[1] = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer,VBO[1]);

            GL.BufferData(BufferTarget.ArrayBuffer, 1201*8*sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);

            
            
            VAO[1] = GL.GenVertexArray();
            GL.BindVertexArray(VAO[1]);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, 1201*3*sizeof(uint), IntPtr.Zero, BufferUsageHint.DynamicDraw);


            base.OnLoad();

        }

        

        float offset = 0.045f;
        List<float> snakeVerticesList = new List<float>();
        List<uint> snakeIndices = new List<uint>();

         protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            GL.BindVertexArray(VAO[0]);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);

            snakeVerticesList = new List<float>();
            
            for(int i=snake.begin;i<=snake.end;++i)
            {
                float posX=snake.queue[i].first*offset;
                float posY=snake.queue[i].second*offset;
                float[] node =
                {
                    -0.9f+posX, -0.9f+posY, 0.0f, 
                    -0.9f+posX, -0.855f+posY, 0.0f,  
                    -0.855f+posX, -0.855f+posY, 0.0f,
                    -0.855f+posX, -0.9f+posY, 0.0f
                };
                //Console.WriteLine(posX + " " + posY);
                snakeVerticesList.AddRange(node);
            }
            
            float[] snakeVertices = snakeVerticesList.ToArray();
            
            /*
            
            float[] node ={
            -0.9f, -0.625f, 0.0f, 
            -0.9f, -0.580f, 0.0f,  
             -0.855f, -0.580f, 0.0f,
             -0.855f, -0.625f, 0.0f,
            };
            snakeVerticesList.AddRange(node);
            float[] snakeVertices = snakeVerticesList.ToArray();
            */
            
            snakeIndices = new List<uint>();

            for(uint i=0;i<snakeVertices.Length/3;i+=4)
            {
                uint[] node = 
                {
                    i,i+1,i+3,
                    i+1,i+3,i+2
                };
                snakeIndices.AddRange(node);
            }

            uint[] indices = snakeIndices.ToArray();


            /*
            uint[] indices = {  
                0, 1, 3,   
                1, 3, 2,
                4, 5, 7,
                5, 7, 6,
                8, 9, 11,
                11, 9, 10    
            };
            */



            GL.BindBuffer(BufferTarget.ArrayBuffer,VBO[1]);
            GL.BufferSubData(BufferTarget.ArrayBuffer,(IntPtr)0,sizeof(float)*snakeVertices.Length,snakeVertices);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer,(IntPtr)0,sizeof(uint)*indices.Length,indices);
            GL.BindVertexArray(VAO[1]);


            //GL.DrawArrays(PrimitiveType.Triangles, 0, snakeVertices.Length);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);



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
                foreach(float i in snakeVerticesList) {Console.Write(i + ", ");}
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