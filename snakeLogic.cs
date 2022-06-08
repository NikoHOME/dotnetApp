using System;


namespace Snake
{

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
    
    
   
    
}