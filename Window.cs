using System;
using System.Security.Cryptography;
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
        class RandomNum 
        {
            public int randX,randY;
            public RandomNum()
            {
                Random rd = new Random();
                randX=rd.Next(0,40);
                randY=rd.Next(0,30);
            }
        }
        


        class snakeVars
        {
            public int dir,posX,posY,fruitX,fruitY;
            public bool [,] board = new bool[40,30];
            public Pair[] queue = new Pair[1201]; //Looping queue expanding backwards with eveyr fruit
            public int end,begin,iter; //Queue pointers  "begin" "end" and "iter" (head position)
            public bool gameOver;

            public snakeVars() //Initialize starting vars
            {
                RandomNum random = new RandomNum();
                end=1200;
                begin=1194;
                iter=1194;
                posX=19;
                posY=14;
                fruitX=random.randX;
                fruitY=random.randY;
                gameOver=false;
                //Declare starting snake
                queue[1194]= new Pair(19,8);    queue[1195]= new Pair(19,9);
                queue[1196]= new Pair(19,10);   queue[1197]= new Pair(19,11);
                queue[1198]= new Pair(19,12);   queue[1199]= new Pair(19,13);
                queue[1200]= new Pair(19,14);

                board[19,8]=true;   board[19,9]=true;
                board[19,10]=true;  board[19,11]=true;
                board[19,12]=true;  board[19,13]=true;
                board[19,14]=true;
            }

            public void moveSnake()
            {   
                 //Move to currently faced dir
                switch(dir) 
                {
                    case 0:
                        ++posY;
                    break;
                    case 1:
                        --posY;
                    break;
                    case 2:
                        --posX;
                    break;
                    case 3:
                        ++posX;
                    break;
                }
                if(posX>39 || posX<0 || posY>29 || posY<0) //Check if out of bounds
                {
                    gameOver=true;
                    return;
                }

                if(board[posX,posY]) //Check if snake collides with its tail
                {
                    gameOver=true;
                    return;
                }

                if(posX==fruitX && posY==fruitY) // Check if it ate a fruit
                {
                    
                    --begin;
                    --iter;
                    RandomNum random = new RandomNum();
                    while(board[random.randX,random.randY]) //Randomize until its not on snake's tail
                    {
                        random = new RandomNum();
                    }
                    fruitX=random.randX;
                    fruitY=random.randY;
                    queue[begin]= new Pair(0,0); //Update the queue
                    for(int i=begin;i<iter;++i)
                    {
                        Pair tempPair = queue[i];
                        queue[i]= queue[i+1];
                        queue[i+1] = tempPair;
                    }

                }
                board[queue[iter].first,queue[iter].second]=false; //Remove the Tail
                queue[iter]= new Pair(posX,posY);
                board[posX,posY]=true; //Add the Head
                if(iter<end) {++iter;} //Move foward in queue
                else {iter=begin;}

            }
        }


        snakeVars snake = new snakeVars();

        public void Debug() 
        {
            /*
            Console.WriteLine(snake.posX + " " + snake.posY + " " + snake.fruitX + " " + snake.fruitY + " " +snake.begin + " " +snake.iter + " " + snake.end);
            
            for(int i=snake.begin;i<=snake.end;++i)
            {
                Console.WriteLine(snake.queue[i].first + " " + snake.queue[i].second);
            }
            
            Console.WriteLine("");
            */
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
    }
}