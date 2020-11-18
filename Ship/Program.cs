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
            int gridSize = 20;  //Hard to use over arund 100   //May still be a problem with some numbers
            int border = 20;    //Can be changed but pls dont
            int gameStage = 0;  //0 = Menu, 1 = Game
            int gamePhase = 0;  //0 = Build Ships, 1 = Play
            int shipStage = 0;  //What ship player is building
            string ship = "";   //Name of ship being built
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //The english alphabet, simplest way, in use when converting numbers to letters
            int activePlayer = 0;   //What player is currently playing
            Vector2 MousePos;
            Random generator = new Random();

        //Grid System
            Vector2 gridMainPos = new Vector2(((width-(border*2))/gridSize), ((height-(border*2))/gridSize));   //The position to calculate the correct spot for each squre
            Vector2 gridMainSize = new Vector2((width/gridSize)-((border*2)/gridSize), (height/gridSize)-((border*2)/gridSize));    //The Size of the squres

            Vector2 gridSidePos = new Vector2(((width-(border*2))/gridSize)/2, ((height-(border*2))/gridSize)/2);   //The position to calculate the side squres

            Color water = new Color(0, 70, 170, 255);       //Background Color, Obselete
            Color waterLines = new Color(20, 20, 20, 100);  //Grid Line Color

            int[,] backgroundTexArray = new int[gridSize, gridSize];
            int[,] backgroundTexSideArray = new int[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                   backgroundTexArray[i,j] = generator.Next(12);
                   backgroundTexSideArray[i,j] = generator.Next(12);
                }
            }
        //Buttons
            int menButScale = 1;
            int normalButWidth = 176;
            int normalButHeight = 64;
            Vector2 butSinVec = new Vector2(border,border);
            Vector2 butMulVec = new Vector2(border,border*2+normalButHeight);
            Vector2 butOptVec = new Vector2(border,border*3+normalButHeight*2);
            Vector2 butCreVec = new Vector2(border,border*4+normalButHeight*3);

    //Not Used for the moment
        //Player 1
            bool[,] player1hitbox = new bool[gridSize, gridSize]; //If true that position has a ship on it? Should maybe be an int to know what ship being shot
            int[,] player1drawing = new int[gridSize, gridSize]; //If true that position has a ship on it? Should maybe be an int to know what ship being shot
            Color p1Color = new Color(200,20,20,255);
        //Player 2
            bool[,] player2 = new bool[gridSize, gridSize]; //If true that position has a ship on it? Should maybe be an int to know what ship being shot
            Color p2Color = new Color(190,190,0,255);
    
    //Used Again
        //Cursor
            Vector2 cursorPos = new Vector2(gridSize/2, gridSize/2);
            int cursorTime = 0;
            char cursorXletter = ' ';
            string cursorXstr = "";
            string cursorYstr = "";

            Color cursor = new Color(0, 200, 0, 128);

        //Images
            Image cursorImg = Raylib.LoadImage(@"Textures/CursorRed.png");

            Image backGroundImg = Raylib.LoadImage(@"Textures/Back2.png");
            Image backGroundMenuImg = Raylib.LoadImage(@"Textures/BackMenu.png");

            Color gridCol = new Color(200,200,200,255);
            Image bg0 = Raylib.LoadImage(@"Textures/grid/Used/bg0.png");
            Image bg1 = Raylib.LoadImage(@"Textures/grid/Used/bg1.png");
            Image bg2 = Raylib.LoadImage(@"Textures/grid/Used/bg2.png");
            Image bg3 = Raylib.LoadImage(@"Textures/grid/Used/bg3.png");
            Image bg4 = Raylib.LoadImage(@"Textures/grid/Used/bg4.png");
            Image bg5 = Raylib.LoadImage(@"Textures/grid/Used/bg5.png");
            Image bg6 = Raylib.LoadImage(@"Textures/grid/Used/bg6.png");
            Image bg7 = Raylib.LoadImage(@"Textures/grid/Used/bg7.png");
            Image bg8 = Raylib.LoadImage(@"Textures/grid/Used/bg8.png");
            Image bg9 = Raylib.LoadImage(@"Textures/grid/Used/bg9.png");
            Image bg10 = Raylib.LoadImage(@"Textures/grid/Used/bg10.png");
            Image bg11 = Raylib.LoadImage(@"Textures/grid/Used/bg11.png");

            //Buttons
            Image butMenHovImg = Raylib.LoadImage(@"Textures/buttons/butMenHov.png");     //Menu button
            Image butMenOrgImg = Raylib.LoadImage(@"Textures/buttons/butMenOrg.png");
            Image butMenPreImg = Raylib.LoadImage(@"Textures/buttons/butMenPre.png");

            Image butSinHovImg = Raylib.LoadImage(@"Textures/buttons/butSinHov.png");     //Single Player button
            Image butSinOrgImg = Raylib.LoadImage(@"Textures/buttons/butSinOrg.png");
            Image butSinPreImg = Raylib.LoadImage(@"Textures/buttons/butSinPre.png");

            Image butMulHovImg = Raylib.LoadImage(@"Textures/buttons/butMulHov.png");     //Multi Player button
            Image butMulOrgImg = Raylib.LoadImage(@"Textures/buttons/butMulOrg.png");
            Image butMulPreImg = Raylib.LoadImage(@"Textures/buttons/butMulPre.png");

            Image butOptHovImg = Raylib.LoadImage(@"Textures/buttons/butOptHov.png");     //Options button
            Image butOptOrgImg = Raylib.LoadImage(@"Textures/buttons/butOptOrg.png");
            Image butOptPreImg = Raylib.LoadImage(@"Textures/buttons/butOptPre.png");

            Image butCreHovImg = Raylib.LoadImage(@"Textures/buttons/butCreHov.png");     //Credits button
            Image butCreOrgImg = Raylib.LoadImage(@"Textures/buttons/butCreOrg.png");
            Image butCrePreImg = Raylib.LoadImage(@"Textures/buttons/butCrePre.png");

            Image butYesHovImg = Raylib.LoadImage(@"Textures/buttons/butYesHov.png");     //Yes button
            Image butYesOrgImg = Raylib.LoadImage(@"Textures/buttons/butYesOrg.png");
            Image butYesPreImg = Raylib.LoadImage(@"Textures/buttons/butYesPre.png");

            Image butNoHovImg = Raylib.LoadImage(@"Textures/buttons/butNoHov.png");     //No button
            Image butNoOrgImg = Raylib.LoadImage(@"Textures/buttons/butNoOrg.png");
            Image butNoPreImg = Raylib.LoadImage(@"Textures/buttons/butNoPre.png");
            

            //Player 1 Ships
            Image player1_1x3Img = Raylib.LoadImage(@"Textures/ships/Spaceship_01_RED.png");
            Image player1_1x2Img = Raylib.LoadImage(@"Textures/ships/Spaceship_02_RED.png");
            Image player1_Test3Img = Raylib.LoadImage(@"Textures/ships/Spaceship_03_RED.png");
            Image player1_1x5Img = Raylib.LoadImage(@"Textures/ships/Spaceship_04_RED.png");
            Image player1_1x4Img = Raylib.LoadImage(@"Textures/ships/Spaceship_05_RED.png");
            Image player1_Test6Img = Raylib.LoadImage(@"Textures/ships/Spaceship_06_RED.png");

            //Player 2 Ships
            Image player2_1x3Img = Raylib.LoadImage(@"Textures/ships/Spaceship_01_GREEN.png");
            Image player2_1x2Img = Raylib.LoadImage(@"Textures/ships/Spaceship_02_GREEN.png");
            Image player2_Test3Img = Raylib.LoadImage(@"Textures/ships/Spaceship_03_GREEN.png");
            Image player2_1x5Img = Raylib.LoadImage(@"Textures/ships/Spaceship_04_GREEN.png");
            Image player2_1x4Img = Raylib.LoadImage(@"Textures/ships/Spaceship_05_GREEN.png");
            Image player2_Test6Img = Raylib.LoadImage(@"Textures/ships/Spaceship_06_GREEN.png");

        //Start Program
            Raylib.SetTargetFPS(60);        //Prefereble FPS
            Raylib.InitWindow(width+width/2, height, "Battleships");    //Makeing the window

        //Audio
            Raylib.InitAudioDevice();
            Music music = Raylib.LoadMusicStream(@"Sound/background/spaceEmergency.mp3");
            
            Raylib.PlayMusicStream(music);
            Raylib.SetMusicVolume(music, 0.5f);

        //Load Textures
            Texture2D cursorTex = Raylib.LoadTextureFromImage(cursorImg);
            Texture2D backGroundTex = Raylib.LoadTextureFromImage(backGroundImg);
            Texture2D backGroundMenuTex = Raylib.LoadTextureFromImage(backGroundMenuImg);
            
            Texture2D butMenHovTex = Raylib.LoadTextureFromImage(butMenHovImg);
            Texture2D butMenOrgTex = Raylib.LoadTextureFromImage(butMenOrgImg);
            Texture2D butMenPreTex = Raylib.LoadTextureFromImage(butMenPreImg);

            Texture2D butSinHovTex = Raylib.LoadTextureFromImage(butSinHovImg);
            Texture2D butSinOrgTex = Raylib.LoadTextureFromImage(butSinOrgImg);
            Texture2D butSinPreTex = Raylib.LoadTextureFromImage(butSinPreImg);

            Texture2D butMulHovTex = Raylib.LoadTextureFromImage(butMulHovImg);
            Texture2D butMulOrgTex = Raylib.LoadTextureFromImage(butMulOrgImg);
            Texture2D butMulPreTex = Raylib.LoadTextureFromImage(butMulPreImg);

            Texture2D butOptHovTex = Raylib.LoadTextureFromImage(butOptHovImg);
            Texture2D butOptOrgTex = Raylib.LoadTextureFromImage(butOptOrgImg);
            Texture2D butOptPreTex = Raylib.LoadTextureFromImage(butOptPreImg);

            Texture2D butCreHovTex = Raylib.LoadTextureFromImage(butCreHovImg);
            Texture2D butCreOrgTex = Raylib.LoadTextureFromImage(butCreOrgImg);
            Texture2D butCrePreTex = Raylib.LoadTextureFromImage(butCrePreImg);

            Texture2D butYesHovTex = Raylib.LoadTextureFromImage(butYesHovImg);
            Texture2D butYesOrgTex = Raylib.LoadTextureFromImage(butYesOrgImg);
            Texture2D butYesPreTex = Raylib.LoadTextureFromImage(butYesPreImg);

            Texture2D butNoHovTex = Raylib.LoadTextureFromImage(butNoHovImg);
            Texture2D butNoOrgTex = Raylib.LoadTextureFromImage(butNoOrgImg);
            Texture2D butNoPreTex = Raylib.LoadTextureFromImage(butNoPreImg);

            Texture2D bgTex0 = Raylib.LoadTextureFromImage(bg0);
            Texture2D bgTex1 = Raylib.LoadTextureFromImage(bg1);
            Texture2D bgTex2 = Raylib.LoadTextureFromImage(bg2);
            Texture2D bgTex3 = Raylib.LoadTextureFromImage(bg3);
            Texture2D bgTex4 = Raylib.LoadTextureFromImage(bg4);
            Texture2D bgTex5 = Raylib.LoadTextureFromImage(bg5);
            Texture2D bgTex6 = Raylib.LoadTextureFromImage(bg6);
            Texture2D bgTex7 = Raylib.LoadTextureFromImage(bg7);
            Texture2D bgTex8 = Raylib.LoadTextureFromImage(bg8);
            Texture2D bgTex9 = Raylib.LoadTextureFromImage(bg9);     
            Texture2D bgTex10 = Raylib.LoadTextureFromImage(bg10);
            Texture2D bgTex11 = Raylib.LoadTextureFromImage(bg11);

            Texture2D player1_1x3Tex = Raylib.LoadTextureFromImage(player1_1x3Img);
            Texture2D player1_1x2Tex = Raylib.LoadTextureFromImage(player1_1x2Img);
            Texture2D player1_Test3Tex = Raylib.LoadTextureFromImage(player1_Test3Img);
            Texture2D player1_1x5Tex = Raylib.LoadTextureFromImage(player1_1x5Img);
            Texture2D player1_1x4Tex = Raylib.LoadTextureFromImage(player1_1x4Img);
            Texture2D player1_Test6Tex = Raylib.LoadTextureFromImage(player1_Test6Img);

            Texture2D player2_1x3Tex = Raylib.LoadTextureFromImage(player2_1x3Img);
            Texture2D player2_1x2Tex = Raylib.LoadTextureFromImage(player2_1x2Img);
            Texture2D player2_Test3Tex = Raylib.LoadTextureFromImage(player2_Test3Img);
            Texture2D player2_1x5Tex = Raylib.LoadTextureFromImage(player2_1x5Img);
            Texture2D player2_1x4Tex = Raylib.LoadTextureFromImage(player2_1x4Img);
            Texture2D player2_Test6Tex = Raylib.LoadTextureFromImage(player2_Test6Img);
            WindowResize();

            while(!Raylib.WindowShouldClose())                          //Game Loop
            {
                Raylib.SetExitKey(0);       //Dont exit with Esc
                Raylib.BeginDrawing();      //Start drawing (Including everything to be able to draw at any point)
                Raylib.ClearBackground(Color.BLACK);    //Basic Background
                FPS = Raylib.GetFPS();      //The FPS
                Raylib.DrawText($"FPS: {FPS}", width-80, height-18, 16, Color.WHITE);   //Draws FPS, remove?
                MousePos = Raylib.GetMousePosition();
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                {
                    gameStage = 0;
                }
                if(gameStage == 0)
                {
                    menu();
                }
                else if(gameStage == 1) //Game
                {
                    game();
                    Sound();
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
                Raylib.DrawTexture(backGroundMenuTex,0,0,Color.WHITE);
                //Sin button
                if((MousePos.X > butSinVec.X && MousePos.Y > butSinVec.Y) && (MousePos.X < butSinVec.X+normalButWidth*menButScale && MousePos.Y < butSinVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Raylib.DrawTextureEx(butSinPreTex, butSinVec, 0, menButScale, Color.WHITE);
                }
                else if((MousePos.X > butSinVec.X && MousePos.Y > butSinVec.Y) && (MousePos.X < butSinVec.X+normalButWidth*menButScale && MousePos.Y < butSinVec.Y+normalButHeight*menButScale))
                {
                    Raylib.DrawTextureEx(butSinHovTex, butSinVec, 0, menButScale, Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureEx(butSinOrgTex, butSinVec, 0, menButScale, Color.WHITE);
                }
                if((MousePos.X > butSinVec.X && MousePos.Y > butSinVec.Y) && (MousePos.X < butSinVec.X+normalButWidth*menButScale && MousePos.Y < butSinVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    gameStage = 1;
                    System.Console.WriteLine("INFO : Singleplayer Activated (I Hope)");
                }

                if((MousePos.X > butMulVec.X && MousePos.Y > butMulVec.Y) && (MousePos.X < butMulVec.X+normalButWidth*menButScale && MousePos.Y < butMulVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Raylib.DrawTextureEx(butMulPreTex, butMulVec, 0, menButScale, Color.WHITE);
                }
                else if((MousePos.X > butMulVec.X && MousePos.Y > butMulVec.Y) && (MousePos.X < butMulVec.X+normalButWidth*menButScale && MousePos.Y < butMulVec.Y+normalButHeight*menButScale))
                {
                    Raylib.DrawTextureEx(butMulHovTex, butMulVec, 0, menButScale, Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureEx(butMulOrgTex, butMulVec, 0, menButScale, Color.WHITE);
                }
                if((MousePos.X > butMulVec.X && MousePos.Y > butMulVec.Y) && (MousePos.X < butMulVec.X+normalButWidth*menButScale && MousePos.Y < butMulVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    gameStage = 1;
                    System.Console.WriteLine("INFO : Multiplayer Activated");
                }

                if((MousePos.X > butOptVec.X && MousePos.Y > butOptVec.Y) && (MousePos.X < butOptVec.X+normalButWidth*menButScale && MousePos.Y < butOptVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Raylib.DrawTextureEx(butOptPreTex, butOptVec, 0, menButScale, Color.WHITE);
                }
                else if((MousePos.X > butOptVec.X && MousePos.Y > butOptVec.Y) && (MousePos.X < butOptVec.X+normalButWidth*menButScale && MousePos.Y < butOptVec.Y+normalButHeight*menButScale))
                {
                    Raylib.DrawTextureEx(butOptHovTex, butOptVec, 0, menButScale, Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureEx(butOptOrgTex, butOptVec, 0, menButScale, Color.WHITE);
                }
                if((MousePos.X > butOptVec.X && MousePos.Y > butOptVec.Y) && (MousePos.X < butOptVec.X+normalButWidth*menButScale && MousePos.Y < butOptVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    gameStage = 2;
                    System.Console.WriteLine("INFO : Options Activated");
                }

                if((MousePos.X > butCreVec.X && MousePos.Y > butCreVec.Y) && (MousePos.X < butCreVec.X+normalButWidth*menButScale && MousePos.Y < butCreVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Raylib.DrawTextureEx(butCrePreTex, butCreVec, 0, menButScale, Color.WHITE);
                }
                else if((MousePos.X > butCreVec.X && MousePos.Y > butCreVec.Y) && (MousePos.X < butCreVec.X+normalButWidth*menButScale && MousePos.Y < butCreVec.Y+normalButHeight*menButScale))
                {
                    Raylib.DrawTextureEx(butCreHovTex, butCreVec, 0, menButScale, Color.WHITE);
                }
                else
                {
                    Raylib.DrawTextureEx(butCreOrgTex, butCreVec, 0, menButScale, Color.WHITE);
                }
                if((MousePos.X > butCreVec.X && MousePos.Y > butCreVec.Y) && (MousePos.X < butCreVec.X+normalButWidth*menButScale && MousePos.Y < butCreVec.Y+normalButHeight*menButScale) && Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    gameStage = 3;
                    System.Console.WriteLine("INFO : Credits Activated");
                }
            }
            
            void game()
            {
                if(activePlayer == 0)
                {
                    Raylib.ClearBackground(p1Color);
                    Raylib.DrawTexture(backGroundTex,0,0,Color.WHITE);
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        activePlayer = 1;
                        if(gridSize % 2 == 0)
                        {
                            cursorPos.X = generator.Next(gridSize/2-1, gridSize/2+1);
                            cursorPos.Y = generator.Next(gridSize/2-1, gridSize/2+1);
                        }
                        else
                        {
                            cursorPos.X = gridSize/2;
                            cursorPos.Y = gridSize/2;
                        }
                    }
                }
                else if(activePlayer == 1)
                {
                    Raylib.ClearBackground(p2Color);
                    Raylib.DrawTexture(backGroundTex, 0, 0, Color.WHITE);
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        activePlayer = 0;
                        if(gridSize % 2 == 0)
                        {
                            cursorPos.X = generator.Next(gridSize/2-1, gridSize/2+1);
                            cursorPos.Y = generator.Next(gridSize/2-1, gridSize/2+1);
                        }
                        else
                        {
                            cursorPos.X = gridSize/2;
                            cursorPos.Y = gridSize/2;
                        }
                    }
                }
                else
                {
                   Raylib.ClearBackground(Color.BLACK); 
                }

                Grid();

                Cursor();
                
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
                        Raylib.DrawText($"Swedish Captain: Set out your {ship}", border/10, border/10, (border/10)*9, Color.BLACK);
                    }
                }
            }

            void Cursor()
            {
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_UP) || Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    if(cursorPos.Y > 0)
                    {
                        cursorPos.Y--;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT) || Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    if(cursorPos.X > 0)
                    {
                        cursorPos.X--;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) || Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    if(cursorPos.Y < gridSize-1)
                    {
                        cursorPos.Y++;
                    }
                }
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                {
                    if(cursorPos.X < gridSize-1)
                    {
                        cursorPos.X++;
                    }
                }
                
                if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A) || 
                Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W))
                    {
                        if(cursorPos.Y > 0 && cursorTime >= (FPS/3)*2)
                        {
                            cursorPos.Y--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A))
                    {
                        if(cursorPos.X > 0 && cursorTime >= (FPS/3)*2)
                        {
                            cursorPos.X--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S))
                    {
                        if(cursorPos.Y < gridSize-1 && cursorTime >= (FPS/3)*2)
                        {
                            cursorPos.Y++;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                    {
                        if(cursorPos.X < gridSize-1 && cursorTime >= (FPS/3)*2)
                        {
                            cursorPos.X++;
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
                
                float tryLength = gridSize/26f;
                for (int i = 0; i < Math.Ceiling(tryLength); i++)   //Bad Way because a large amount of breaking of try so lower FPS (break improves for earlier part of 'i')
                {
                    try
                    {
                        if(cursorPos.X >= (i*26) && cursorPos.X < (i*26)+26)    //Search the correct set, only the correct 'A'X or 'B'X or 'C'X
                        {
                            cursorXstr+=alphabet[i];
                            cursorXletter = alphabet[(int)cursorPos.X-26*(i)];   //Find the exact letter of alphabet to add
                            cursorXstr+=cursorXletter;                  //Adds it to the printed string
                            break;          //Extra Safty by stoping loop if correct point found
                        }
                    }
                    catch{      //If something happens, shouldent but to be safe
                        System.Console.WriteLine($"Error: cursorX out of index{cursorPos.X}");  //If the X position is somehow wrong, Error Messege
                    }
                }

                if(cursorPos.Y > gridSize - 10) //To add 0 before for a more systemiced Grid system (Y)
                {
                    cursorYstr += "0";
                }
                Raylib.DrawTexture(player1_1x2Tex, 2*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border, Color.WHITE);        //Test Ship Textures
                Raylib.DrawTexture(player1_1x3Tex, 3*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border, Color.WHITE);        //Test Ship Textures
                Raylib.DrawTexture(player1_1x4Tex, 4*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border , Color.WHITE);       //Test Ship Textures
                Raylib.DrawTexture(player1_1x5Tex, 5*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border , Color.WHITE);       //Test Ship Textures
                Raylib.DrawTexture(player1_Test3Tex, 0*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border, Color.WHITE);      //Test Ship Textures

                Raylib.DrawTexture(player2_1x2Tex, 17*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border, Color.WHITE);        //Test Ship Textures
                Raylib.DrawTexture(player2_1x3Tex, 16*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border, Color.WHITE);        //Test Ship Textures
                Raylib.DrawTexture(player2_1x4Tex, 15*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border , Color.WHITE);       //Test Ship Textures
                Raylib.DrawTexture(player2_1x5Tex, 14*((width-(border*2))/gridSize)+border, 0*((width-(border*2))/gridSize)+border , Color.WHITE);       //Test Ship Textures

                cursorYstr += (gridSize-cursorPos.Y);           //To show position count starting from bottom left for player and not top left
                Raylib.DrawTexture(cursorTex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);  //Cursor Position
                Raylib.DrawText($"{cursorXstr} {cursorYstr}", border/10+width, border/10 + height/2, (border/10)*18, Color.BLACK);  //Actully shows Position
            }
            
            void Sound()
            {
                Raylib.UpdateMusicStream(music);
            }
        
            void Grid()
            {
                for(int a = 0; a < gridSize; a++)
                {
                    for(int b = 0; b < gridSize; b++)   //To improve FPS these are calculated outside of loop then multiplied by 'a' and 'b' inside
                    {
                        //Raylib.DrawTexture(waterRecTex, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, Color.WHITE);                             //The GamePlane Texture
                        if(backgroundTexArray[a,b]==0)
                        {
                            Raylib.DrawTexture(bgTex0, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==1)
                        {
                            Raylib.DrawTexture(bgTex1, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==2)
                        {
                            Raylib.DrawTexture(bgTex2, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==3)
                        {
                            Raylib.DrawTexture(bgTex3, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==4)
                        {
                            Raylib.DrawTexture(bgTex4, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==5)
                        {
                            Raylib.DrawTexture(bgTex5, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==6)
                        {
                            Raylib.DrawTexture(bgTex6, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==7)
                        {
                            Raylib.DrawTexture(bgTex7, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==8)
                        {
                            Raylib.DrawTexture(bgTex8, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==9)
                        {
                            Raylib.DrawTexture(bgTex9, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else if(backgroundTexArray[a,b]==10)
                        {
                            Raylib.DrawTexture(bgTex10, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }
                        else
                        {
                            Raylib.DrawTexture(bgTex11, border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, gridCol);
                        }

                        if(backgroundTexArray[a,b]==0)
                        {
                            Raylib.DrawTextureEx(bgTex0,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==1)
                        {
                            Raylib.DrawTextureEx(bgTex1,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==2)
                        {
                            Raylib.DrawTextureEx(bgTex2,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==3)
                        {
                            Raylib.DrawTextureEx(bgTex3,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==4)
                        {
                            Raylib.DrawTextureEx(bgTex4,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==5)
                        {
                            Raylib.DrawTextureEx(bgTex5,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==6)
                        {
                            Raylib.DrawTextureEx(bgTex6,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==7)
                        {
                            Raylib.DrawTextureEx(bgTex7,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==8)
                        {
                            Raylib.DrawTextureEx(bgTex8,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==9)
                        {
                            Raylib.DrawTextureEx(bgTex9,gridSidePos,0,0.5f,gridCol);
                        }
                        else if(backgroundTexArray[a,b]==10)
                        {
                            Raylib.DrawTextureEx(bgTex10,gridSidePos,0,0.5f,gridCol);
                        }
                        else
                        {
                            Raylib.DrawTextureEx(bgTex11,gridSidePos,0,0.5f,gridCol);
                        }
                        Raylib.DrawRectangleLines(border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, (int)gridMainSize.X, (int)gridMainSize.Y, waterLines);  //Grid
                        gridSidePos.X = a*((width-(border*2))/gridSize)/2 + width;
                        gridSidePos.Y = b*((height-(border*2))/gridSize)/2;
                        Raylib.DrawTextureEx(bgTex0,gridSidePos,0,0.5f,gridCol);
                        //Raylib.DrawRectangle((int)gridSidePos.X , (int)gridSidePos.Y , (int)gridMainSize.X/2 , (int)gridMainSize.Y/2 , water);     //Side gamePlane
                        Raylib.DrawRectangleLines((int)gridSidePos.X , (int)gridSidePos.Y , (int)gridMainSize.X/2 , (int)gridMainSize.Y/2 , waterLines);   //^ Grid

                    }
                }
            }
        
            void Options(){}    //Future

            void WindowResize() //Because when the window is resized textures need to be recalculated   //Future
            {
            //Change Values
                gridMainPos = new Vector2(((width-(border*2))/gridSize), ((height-(border*2))/gridSize));   //The position to calculate the correct spot for each squre
                gridMainSize = new Vector2((width/gridSize)-((border*2)/gridSize), (height/gridSize)-((border*2)/gridSize));    //The Size of the squres

                gridSidePos = new Vector2(((width-(border*2))/gridSize)/2, ((height-(border*2))/gridSize)/2);   //The position to calculate the side squres
            
            //Image Resizeing
                Raylib.ImageResize(ref cursorImg, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref backGroundImg, width+ width/2 ,height);
                Raylib.ImageResize(ref backGroundImg, width+ width/2 ,height);

                Raylib.ImageResize(ref bg0, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg1, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg2, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg3, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg4, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg5, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg6, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg7, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg8, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg9, (int)gridMainSize.X, (int)gridMainSize.Y); 
                Raylib.ImageResize(ref bg10, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref bg11, (int)gridMainSize.X, (int)gridMainSize.Y);

            //Player 1 Ships
                Raylib.ImageResize(ref player1_1x3Img, (int)gridMainSize.X, (int)gridMainSize.Y*3);
                Raylib.ImageResize(ref player1_1x2Img, (int)gridMainSize.X, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player1_Test3Img, (int)gridMainSize.X*2, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player1_1x5Img, (int)gridMainSize.X, (int)gridMainSize.Y*5);
                Raylib.ImageResize(ref player1_1x4Img, (int)gridMainSize.X, (int)gridMainSize.Y*4);
                Raylib.ImageResize(ref player1_Test6Img, (int)gridMainSize.X, (int)gridMainSize.Y);

            //Player 2 Ships
                Raylib.ImageResize(ref player2_1x3Img, (int)gridMainSize.X, (int)gridMainSize.Y*3);
                Raylib.ImageResize(ref player2_1x2Img, (int)gridMainSize.X, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player2_Test3Img, (int)gridMainSize.X, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player2_1x5Img, (int)gridMainSize.X, (int)gridMainSize.Y*5);
                Raylib.ImageResize(ref player2_1x4Img, (int)gridMainSize.X, (int)gridMainSize.Y*4);
                Raylib.ImageResize(ref player2_Test6Img, (int)gridMainSize.X, (int)gridMainSize.Y);
            
            //Textures Reload
                cursorTex = Raylib.LoadTextureFromImage(cursorImg);
                backGroundTex = Raylib.LoadTextureFromImage(backGroundImg);
                backGroundMenuTex = Raylib.LoadTextureFromImage(backGroundMenuImg);

                butMenHovTex = Raylib.LoadTextureFromImage(butMenHovImg);
                butMenOrgTex = Raylib.LoadTextureFromImage(butMenOrgImg);
                butMenPreTex = Raylib.LoadTextureFromImage(butMenPreImg);

                butSinHovTex = Raylib.LoadTextureFromImage(butSinHovImg);
                butSinOrgTex = Raylib.LoadTextureFromImage(butSinOrgImg);
                butSinPreTex = Raylib.LoadTextureFromImage(butSinPreImg);

                butMulHovTex = Raylib.LoadTextureFromImage(butMulHovImg);
                butMulOrgTex = Raylib.LoadTextureFromImage(butMulOrgImg);
                butMulPreTex = Raylib.LoadTextureFromImage(butMulPreImg);

                butOptHovTex = Raylib.LoadTextureFromImage(butOptHovImg);
                butOptOrgTex = Raylib.LoadTextureFromImage(butOptOrgImg);
                butOptPreTex = Raylib.LoadTextureFromImage(butOptPreImg);

                butCreHovTex = Raylib.LoadTextureFromImage(butCreHovImg);
                butCreOrgTex = Raylib.LoadTextureFromImage(butCreOrgImg);
                butCrePreTex = Raylib.LoadTextureFromImage(butCrePreImg);

                butYesHovTex = Raylib.LoadTextureFromImage(butYesHovImg);
                butYesOrgTex = Raylib.LoadTextureFromImage(butYesOrgImg);
                butYesPreTex = Raylib.LoadTextureFromImage(butYesPreImg);

                butNoHovTex = Raylib.LoadTextureFromImage(butNoHovImg);
                butNoOrgTex = Raylib.LoadTextureFromImage(butNoOrgImg);
                butNoPreTex = Raylib.LoadTextureFromImage(butNoPreImg);

                bgTex0 = Raylib.LoadTextureFromImage(bg0);
                bgTex1 = Raylib.LoadTextureFromImage(bg1);
                bgTex2 = Raylib.LoadTextureFromImage(bg2);
                bgTex3 = Raylib.LoadTextureFromImage(bg3);
                bgTex4 = Raylib.LoadTextureFromImage(bg4);
                bgTex5 = Raylib.LoadTextureFromImage(bg5);
                bgTex6 = Raylib.LoadTextureFromImage(bg6);
                bgTex7 = Raylib.LoadTextureFromImage(bg7);
                bgTex8 = Raylib.LoadTextureFromImage(bg8);
                bgTex9 = Raylib.LoadTextureFromImage(bg9);     
                bgTex10 = Raylib.LoadTextureFromImage(bg10);
                bgTex11 = Raylib.LoadTextureFromImage(bg11);

                player1_1x3Tex = Raylib.LoadTextureFromImage(player1_1x3Img);
                player1_1x2Tex = Raylib.LoadTextureFromImage(player1_1x2Img);
                player1_Test3Tex = Raylib.LoadTextureFromImage(player1_Test3Img);
                player1_1x5Tex = Raylib.LoadTextureFromImage(player1_1x5Img);
                player1_1x4Tex = Raylib.LoadTextureFromImage(player1_1x4Img);
                player1_Test6Tex = Raylib.LoadTextureFromImage(player1_Test6Img);

                player2_1x3Tex = Raylib.LoadTextureFromImage(player2_1x3Img);
                player2_1x2Tex = Raylib.LoadTextureFromImage(player2_1x2Img);
                player2_Test3Tex = Raylib.LoadTextureFromImage(player2_Test3Img);
                player2_1x5Tex = Raylib.LoadTextureFromImage(player2_1x5Img);
                player2_1x4Tex = Raylib.LoadTextureFromImage(player2_1x4Img);
                player2_Test6Tex = Raylib.LoadTextureFromImage(player2_Test6Img);
            }
        }
    }
}

//FIX
//Change reset point so random for 4 middle points if even gridsize
//Size of window can be changed during game.. So it should be changeable
//Raylib.DisableCursor(); so while ingame no cursor (only if fullscreen?) //Dont do fullscreen, we will all die
//Ship texture rotated?
//Diffrent Cursor Depending on player
//Use Mouse?
//Recordar hela striden och sen ställer up sidorna mot varandra och ser deras attacker?
//Meny Knappar, nedre vänstra hörnet

//freakyNews to be used when waiting for next player?
