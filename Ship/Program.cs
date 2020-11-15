using System;
using System.Numerics;
using Raylib_cs;

namespace Ship
{
    class Program
    {
        static void Main(string[] args)
        {
        //Basic Rules
            int width = 1000;   //The width of the window (Ish) width*1.5
            int height = 1000;  //The height of the window
            int FPS;            //How fast the game runs
            int gridsize = 20;  //Hard to use over arund 100   //May still be a problem with some numbers
            int border = 20;    //Can be changed but pls dont
            int gameStage = 1;  //0 = Menu, 1 = Game
            int gamePhase = 0;  //0 = Build Ships, 1 = Play
            int shipStage = 0;  //What ship player is building
            string ship = "";   //Name of ship being built
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //The english alphabet, simplest way, in use when converting numbers to letters
            int activePlayer = 0;   //What player is currently playing

            Vector2 gridMainPos = new Vector2(((width-(border*2))/gridsize), ((height-(border*2))/gridsize));
            Vector2 gridMainSize = new Vector2((width/gridsize)-((border*2)/gridsize), (height/gridsize)-((border*2)/gridsize));

            Vector2 gridSidePos = new Vector2(((width-(border*2))/gridsize)/2, ((height-(border*2))/gridsize)/2);

            Color water = new Color(0, 70, 170, 255);
            Color waterLines = new Color(0, 0, 0, 255);

        //Player 1
            bool[,] player1 = new bool[gridsize, gridsize]; //If true that position has a ship on it? Should maybe be an int to know what ship being shot
            Color p1Color = new Color(200,20,20,255);
        //Player 2
            bool[,] player2 = new bool[gridsize, gridsize]; //If true that position has a ship on it? Should maybe be an int to know what ship being shot
            Color p2Color = new Color(190,190,0,255);
        //Cursor
            int cursorX = gridsize/2;
            int cursorY = gridsize/2;
            int cursorTime = 0;
            char cursorXletter = ' ';
            string cursorXstr = "";
            string cursorYstr = "";

            Color cursor = new Color(0, 200, 0, 128);

        //Start Program
            Raylib.SetTargetFPS(60);        //Prefereble FPS
            Raylib.InitWindow(width+width/2, height, "Battleships");    //Makeing the window
            while(!Raylib.WindowShouldClose())                          //Game Loop
            {
                Raylib.SetExitKey(0);       //Dont exit with Esc
                Raylib.BeginDrawing();      //Start drawing (Including everything to be able to draw at any point)
                Raylib.ClearBackground(Color.BLACK);    //Basic Background
                FPS = Raylib.GetFPS();      //The FPS
                Raylib.DrawText($"FPS: {FPS}", width-80, height-18, 16, Color.WHITE);   //Draws FPS, remove?
                if(gameStage == 0)
                {
                    menu();
                }
                else if(gameStage == 1)
                {
                    game();
                }
                else
                {
                    gameStage = 0;
                    menu();
                }

                Raylib.EndDrawing();
            }
            void menu()
            {
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    gameStage = 1;
                }
            }
            void game()
            {
                if(activePlayer == 0)
                {
                    Raylib.ClearBackground(p1Color);
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        activePlayer = 1;
                        cursorX = gridsize/2;
                        cursorY = gridsize/2;
                    }
                }
                else if(activePlayer == 1)
                {
                    Raylib.ClearBackground(p2Color);
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        activePlayer = 0;
                        cursorX = gridsize/2;
                        cursorY = gridsize/2;
                    }
                }
                else
                {
                   Raylib.ClearBackground(Color.BLACK); 
                }

                for(int a = 0; a < gridsize; a++)
                {
                    for(int b = 0; b < gridsize; b++)   //To improve FPS these are calculated outside of loop then multiplied by 'a' and 'b' inside
                    {
                        Raylib.DrawRectangle(border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, (int)gridMainSize.X, (int)gridMainSize.Y, water);
                        Raylib.DrawRectangleLines(border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, (int)gridMainSize.X, (int)gridMainSize.Y, waterLines);

                        Raylib.DrawRectangle(a*(int)gridSidePos.X+width , border+b*(int)gridSidePos.Y , (int)gridMainSize.X/2 , (int)gridMainSize.Y/2 , water);
                        Raylib.DrawRectangleLines(a*(int)gridSidePos.X+width , border+b*(int)gridSidePos.Y , (int)gridMainSize.X/2 , (int)gridMainSize.Y/2 , waterLines);
                    }
                }
                movement();
                
                if(gamePhase == 0)
                {
                    if(shipStage == 0)
                    {
                        ship = "Patrol Boat";
                    }
                    if(activePlayer == 0)
                    {
                        Raylib.DrawText($"British Captain: Set out your {ship}", border/10, border/10, (border/10)*9, Color.BLACK);
                    }
                    if(activePlayer == 1)
                    {
                        Raylib.DrawText($"Swedish Captain: Set out your {ship}", border/10, border/10, (border/10)*9, Color.BLACK);;
                    }
                }
            }

            void movement()
            {
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    if(cursorY > 0)
                    {
                        cursorY--;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    if(cursorX > 0)
                    {
                        cursorX--;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    if(cursorY < gridsize-1)
                    {
                        cursorY++;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                {
                    if(cursorX < gridsize-1)
                    {
                        cursorX++;
                    }
                }
                
                if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A) || 
                Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W))
                    {
                        if(cursorY > 0 && cursorTime >= (FPS/3)*2)
                        {
                            cursorY--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A))
                    {
                        if(cursorX > 0 && cursorTime >= (FPS/3)*2)
                        {
                            cursorX--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S))
                    {
                        if(cursorY < gridsize-1 && cursorTime >= (FPS/3)*2)
                        {
                            cursorY++;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                    {
                        if(cursorX < gridsize-1 && cursorTime >= (FPS/3)*2)
                        {
                            cursorX++;
                        }
                    }
                    if(cursorTime < (FPS/3)*2)
                    {
                        cursorTime++;
                    }
                    else
                    {
                        cursorTime = FPS/3;
                    }
                }
                else
                {
                    cursorTime = 0;
                }
                
                //Displays X and Y Pos for Cursor
                cursorXstr = "";
                cursorYstr = "";
                
                float tryLength = gridsize/26f;
                for (int i = 0; i < Math.Ceiling(tryLength); i++)   //Bad Way because a large amount of breaking of try so lower FPS (break improves for earlier part of 'i')
                {
                    try
                    {
                        if(cursorX >= (i*26) && cursorX < (i*26)+26)    //Search the correct set, only the correct 'A'X or 'B'X or 'C'X
                        {
                            cursorXstr+=alphabet[i];
                            cursorXletter = alphabet[cursorX-26*(i)];   //Find the exact letter of alphabet to add
                            cursorXstr+=cursorXletter;                  //Adds it to the printed string
                            break;          //Extra Safty by stoping loop if correct point found
                        }
                    }
                    catch{      //If something happens, shouldent but to be safe
                        System.Console.WriteLine($"Error: cursorX out of index{cursorX}");  //If the X position is somehow wrong, Error Messege
                    }
                }

                if(cursorY > gridsize - 10) //To add 0 before for a more systemiced Grid system (Y)
                {
                    cursorYstr += "0";
                }

                cursorYstr += (gridsize-cursorY);   //To show count starting from bottom left for player and not top left
                Raylib.DrawRectangle(border+cursorX*((width-(border*2))/gridsize), border+cursorY*((height-(border*2))/gridsize), (int)gridMainSize.X, (int)gridMainSize.Y, cursor);
                Raylib.DrawText($"{cursorXstr} {cursorYstr}", border/10+width, border/10 + height/2, (border/10)*18, Color.BLACK);
            }
        }
    }
}

//FIX
//Change reset point so random for 4 middle points if even gridsize