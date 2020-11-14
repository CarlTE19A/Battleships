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
            int width = 1000;
            int height = 1000;
            int FPS;
            int gridsize = 10;   //Gridsize should preferebly to be divisible by border*2
            int border = 20;
            int gameStage = 1;
            int gamePhase = 0;  //0 = Build Ships, 1 = Play
            int shipStage = 0;
            string ship = "";
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int activePlayer = 0;

            Vector2 gridMainPos = new Vector2(((width-(border*2))/gridsize), ((height-(border*2))/gridsize));
            Vector2 gridMainSize = new Vector2((width/gridsize)-((border*2)/gridsize), (height/gridsize)-((border*2)/gridsize));

            Vector2 gridSidePos = new Vector2(((width-(border*2))/gridsize)/2, ((height-(border*2))/gridsize)/2);

            Color water = new Color(0, 70, 170, 255);
            Color waterLines = new Color(0, 0, 0, 255);

        //Player 1
            int[,] player1 = new int[gridsize, gridsize];
            Color p1Color = new Color(200,20,20,255);
        //Player 2
            int[,] player2 = new int[gridsize, gridsize];
            Color p2Color = new Color(190,190,0,255);
        //Cursor
            int cursorX = 0;
            int cursorY = 0;
            int cursorTime = 0;
            char cursorXletter = ' ';
            string cursorXstr = "";
            string cursorYstr = "";

            Color cursor = new Color(0, 200, 0, 128);

        //Start Program
            Raylib.SetTargetFPS(60);
            Raylib.InitWindow(width+width/2, height, "Battleships");
            while(!Raylib.WindowShouldClose())
            {
                Raylib.SetExitKey(0);
                //if(Raylib.GetKeyPressed)
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                FPS = Raylib.GetFPS();
                Raylib.DrawText($"FPS: {FPS}", width-80, height-18, 16, Color.WHITE);
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
                }
                else if(activePlayer == 1)
                {
                    Raylib.ClearBackground(p2Color);
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
                cursorXstr = "";
                cursorYstr = "";
                
                for (int i = 0; i < 4; i++)
                {
                    
                }
                if( cursorX >= 0 && cursorX < 26 )
                {
                    cursorXstr+="A";
                }
                else if( cursorX >= 26 && cursorX < 52 )
                {
                    cursorXstr+="B";
                }
                else if( cursorX >= 52 && cursorX < 78 )
                {
                    cursorXstr+="C";
                }
                else if( cursorX >= 78 && cursorX < 104 )
                {
                    cursorXstr+="D";
                }
            //Skip part if gridsize is lower then 27?
                float tryLength = gridsize/26f;
                for (int i = 0; i < Math.Ceiling(tryLength); i++)   //Bad Way because a large amount of breaking of try so lower FPS (break improves for earlier part of 'i')
                {
                    try
                    {
                        cursorXletter = alphabet[cursorX-26*i];
                        cursorXstr+=cursorXletter;
                        break;
                    }
                    catch
                    {}
                }

                if(cursorY < 9)
                {
                    cursorYstr += "0";
                }
                cursorYstr += (cursorY+1);
            //Skip to here
                Raylib.DrawRectangle(border+cursorX*((width-(border*2))/gridsize), border+cursorY*((height-(border*2))/gridsize), (width/gridsize)-((border*2)/gridsize), (height/gridsize)-((border*2)/gridsize), cursor);
                Raylib.DrawText($"{cursorXstr} {cursorYstr}", border/10+width, border/10 + height/2, (border/10)*18, Color.BLACK);
            }
        }
    }
}
