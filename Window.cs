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
        
        
        //Console.WriteLine(board[19,14]);
        //Console.WriteLine(board[19,15]);
        class Pair
        {
            public int first,second;

            public Pair(int x,int y)
            {
                this.first=x;
                this.second=y;
            }
        }



        class snakeVars
        {
            public int dir,posX,posY,fruitX,fruitY;
            public bool [,] board = new bool[40,30];
            public Pair[] queue = new Pair[1201];
            public int end,begin,iter;
            public bool gameOver;

            public snakeVars()
            {
                end=1200;
                begin=1194;
                iter=1194;
                posX=19;
                posY=14;
                fruitX=19;
                fruitY=20;
                gameOver=false;
                queue[1194]= new Pair(19,8);
                queue[1195]= new Pair(19,9);
                queue[1196]= new Pair(19,10);
                queue[1197]= new Pair(19,11);
                queue[1198]= new Pair(19,12);
                queue[1199]= new Pair(19,13);
                queue[1200]= new Pair(19,14);

                board[19,8]=true;
                board[19,9]=true;
                board[19,10]=true;
                board[19,11]=true;
                board[19,12]=true;
                board[19,13]=true;
                board[19,14]=true;
            }

            public void moveSnake()
            {   
                switch(dir)
                {
                    case 0:
                        //std::cout<<"HE WENT UP\n";
                        ++posY;
                    break;
                    case 1:
                        //std::cout<<"HE WENT DOWN\n";
                        --posY;
                    break;
                    case 2:
                        //std::cout<<"HE WENT LEFT\n";
                        --posX;
                    break;
                    case 3:
                        //std::cout<<"HE WENT RIGHT\n";
                        ++posX;
                    break;
                }
                if(board[posX,posY]) 
                {
                    gameOver=true;
                    return;
                }

                if(posX==fruitX && posY==fruitY)
                {
                    
                    --begin;
                    --iter;
                    for(int i=begin;i<iter;++i)
                    {
                        Pair tempPair = queue[i];
                        queue[i]= queue[i+1];
                        queue[i+1] = tempPair;
                    }
                    
                }
                board[queue[iter].first,queue[iter].second]=false;
                queue[iter]= new Pair(posX,posY);
                board[posX,posY]=true;
                if(iter<end) {++iter;}
                else {iter=begin;}

            }
        }


        snakeVars snake = new snakeVars();

        public void Debug()
        {
            Console.WriteLine(snake.posX + " " + snake.posY + " " + snake.iter);
            /*
            for(int i=snake.begin;i<=snake.end;++i)
            {
                Console.Write(snake.queue[i].first + " " + snake.queue[i].second);
            }
            Console.WriteLine("");
            */
             for(int i=29;i>=0;--i) 
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

        }
        

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
                snake.moveSnake();
                if(snake.gameOver)
                {
                    return;
                }
                Debug();
       
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
            if(snake.gameOver)
            {
                Console.WriteLine("Game Over");
                Close();
            }
            updateSnakeDir();
            tick();
            base.OnUpdateFrame(e);
        }
    }
}