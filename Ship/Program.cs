using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace Ship
{
    class Ship
    {
        public int type;    //1x2, 1x3, 1x4, 1x5, 2x2
        public bool align;  //false = vertical, true = horizontal   //Future
        public string name; //To show the player what ship it is //Future
        public Vector2 position;    //The position of the upper left corner of ship
        public Vector2 mainPositionDefined; //Future //To draw ships with Raylib.DrawTextureEx 
        public Vector2 sidePositionDefined; //To show player ships on the side panel during gameplay
        public static Vector2 currentPos;   //Temporary position calculated from Cursor Position
        //Hitboxes using Arrays means a problem if gridSize is changed during program is running, meaning the program needs to be restarted 
        public static bool[,] P1Hitbox;     //Where there are player 1 Ships
        public static bool[,] P2Hitbox;     //Where there are playere 2 Ships
        public static int activePlayer = 0;   //What player is currently playing
        
        public Ship(int _type, Vector2 _position)   //Calculates when a ship is placed
        {
            type = _type;
            position = _position;
        }
        public void Placement() //Called when ship placed
        {
           position = currentPos;   //Sets the position of the ship to the current position
            if(activePlayer == 0){  //If player 1 is playing
                if(align == false)  //If the ship is vertical   //Adds the positions to the hitbox 
                {
                    try{    //Try used if the player mangeges to have a ship out of borders
                        if(type == 0)
                        {
                            P1Hitbox[(int)position.X, (int)position.Y] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+1] = true;
                        }
                        if(type == 1)
                        {
                            P1Hitbox[(int)position.X, (int)position.Y] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+1] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+2] = true;
                        }
                        if(type ==2)
                        {
                            P1Hitbox[(int)position.X, (int)position.Y] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+1] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+2] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+3] = true;
                        }
                        if(type == 3)
                        {
                            P1Hitbox[(int)position.X, (int)position.Y] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+1] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+2] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+3] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+4] = true;
                        }
                        if(type == 4)
                        {
                            P1Hitbox[(int)position.X, (int)position.Y] = true;
                            P1Hitbox[(int)position.X, (int)position.Y+1] = true;
                            P1Hitbox[(int)position.X+1, (int)position.Y] = true;
                            P1Hitbox[(int)position.X+1, (int)position.Y+1] = true;
                        }
                    }
                    catch   //If try goes wrong (Ship will probely be drawn still but with no hitbox, having no effect)
                    {
                        System.Console.WriteLine("ERROR: Item out of Index");
                    }
                }
            }
            if(activePlayer == 1)   //Same as section above but player 2
            {
                if(align == false)
                {
                    if(type == 0)
                    {
                        P2Hitbox[(int)position.X, (int)position.Y] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+1] = true;
                    }
                    if(type == 1)
                    {
                        P2Hitbox[(int)position.X, (int)position.Y] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+1] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+2] = true;
                    }
                    if(type ==2)
                    {
                        P2Hitbox[(int)position.X, (int)position.Y] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+1] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+2] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+3] = true;
                    }
                    if(type == 3)
                    {
                        P2Hitbox[(int)position.X, (int)position.Y] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+1] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+2] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+3] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+4] = true;
                    }
                    if(type == 4)
                    {
                        P2Hitbox[(int)position.X, (int)position.Y] = true;
                        P2Hitbox[(int)position.X, (int)position.Y+1] = true;
                        P2Hitbox[(int)position.X+1, (int)position.Y] = true;
                        P2Hitbox[(int)position.X+1, (int)position.Y+1] = true;
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        //Basic Rules
            int width = 1000;   //The width of the window (Ish) actully Full width = width*1.5
            int FPS;            //How fast the game runs
            int gridSize = 15;  //Hard to use over arund 100   //May still be a problem with some numbers

            float audioMult = 1f; //To lower / increase sound ingame
            float sfxMult = 1f;
            float musicMult = 1f;
            try
            {
                string[] settingLoadString = File.ReadAllLines(@"Settings.txt");
                width = Int16.Parse(settingLoadString[0]);
                gridSize = Int16.Parse(settingLoadString[1]);
                audioMult = float.Parse(settingLoadString[2]);
                sfxMult = float.Parse(settingLoadString[3]);
                musicMult = float.Parse(settingLoadString[4]);

            }
            catch
            {
                System.Console.WriteLine("Settings File Missing or Corrupt");
            }
            int height = width;  //The height of the window
            int border = 20;    //Can be changed but pls dont
            int gameStage = 0;  //0 = Menu, 1 = Game
            int gamePhase = 0;  //0 = Build Ships, 1 = Play
            int activeShip = 0;  //What ship player is building
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //The english alphabet, simplest way, in use when converting numbers to letters
            Vector2 MousePos;   //Where the mouse Currently is
            Random generator = new Random();    //All random
            Ship.P1Hitbox = new bool[gridSize,gridSize];    //Sets the Size of Player 1 hitbox
            Ship.P2Hitbox = new bool[gridSize,gridSize];    //Sets the Size of Player 2 hitbox
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Ship.P1Hitbox[i,j] = false;
                    Ship.P2Hitbox[i,j] = false;
                }
            }

        //Grid System
            Vector2 gridMainPos = new Vector2(((width-(border*2))/gridSize), ((height-(border*2))/gridSize));   //The position to calculate the correct spot for each squre
            Vector2 gridMainSize = new Vector2((width/gridSize)-((border*2)/gridSize), (height/gridSize)-((border*2)/gridSize));    //The Size of the squres

            Vector2 gridSidePos = new Vector2(((width-(border*2))/gridSize)/2, ((height-(border*2))/gridSize)/2);   //The position to calculate the side squres

            Color water = new Color(0, 70, 170, 255);       //Background Color, Obselete
            Color gridLines = new Color(20, 20, 20, 100);  //Grid Line Color

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

        //Player 1
            Color p1Color = new Color(200,20,20,255);
            List<Ship> p1ShipList = new List<Ship>();
            
        //Player 2
            Color p2Color = new Color(190,190,0,255);
            List<Ship> p2ShipList = new List<Ship>();

        //Cursor
            Vector2 cursorPos = new Vector2(gridSize/2, gridSize/2);
            int cursorTime = 0;
            char cursorXletter = ' ';
            string cursorXstr = "";
            string cursorYstr = "";

            Color cursor = new Color(0, 200, 0, 128);

        //Options  //The prefix op stands for op-tions
            int optionBorder = width/50;
            Color optionBackColor = new Color(10,10,10,200);
            Color optionOrgColor = new Color(150, 150, 150, 255);
            Color optionHovColor = new Color(100, 100, 100, 255);
            Color optionPreColor = new Color(70, 70, 70, 255);
            Vector2 optionsPos = new Vector2(optionBorder*width/100, 0);
            Vector2 opContainerSize = new Vector2((width + width/2) - (optionsPos.X*2)-(optionBorder*2), height/6);
            
            Vector2 opGridPos = new Vector2(optionsPos.X + optionBorder, optionsPos.Y + optionBorder);
            Rectangle opGridRec = new Rectangle(optionBorder + opGridPos.X, 2*optionBorder + opGridPos.Y, opContainerSize.X - optionBorder*2, opContainerSize.Y - optionBorder*3);
            Color optionGridColor = optionOrgColor;

            Vector2 opScreenPos = new Vector2(optionsPos.X + optionBorder, optionsPos.Y + 2*optionBorder + height/6);
            Rectangle opScreenRec = new Rectangle(optionBorder + opScreenPos.X, 2*optionBorder + opScreenPos.Y, opContainerSize.X - optionBorder*2, opContainerSize.Y - optionBorder*3);
            Color optionScreenColor = optionOrgColor;

            Vector2 opAudioPos = new Vector2(optionsPos.X + optionBorder, optionsPos.Y + 3*optionBorder + 2*(height/6));
            Color optionAudioColor = optionOrgColor;

            Vector2 opMusicPos = new Vector2(optionsPos.X + optionBorder, optionsPos.Y + 4*optionBorder + 3*(height/6));
            Color optionMusicColor = optionOrgColor;

            Vector2 opSfxPos = new Vector2(optionsPos.X + optionBorder, optionsPos.Y + 5*optionBorder + 4*(height/6));
            Color optionSfxColor = optionOrgColor;
            
            int opGridInt = 0;
            int opScreenInt = 3;
            string opScreenString = $"Resulution : {width*1.5}x{width}";
            int opScreenStrWidth = 0;
            int opStrHeight = 0;


        //Images
            Image redCursorImg = Raylib.LoadImage(@"Textures/CursorRed.png");
            Image greenCursorImg = Raylib.LoadImage(@"Textures/CursorGreen.png");

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
            Image player1_2x2Img = Raylib.LoadImage(@"Textures/ships/Spaceship_03_RED.png");
            Image player1_1x5Img = Raylib.LoadImage(@"Textures/ships/Spaceship_04_RED.png");
            Image player1_1x4Img = Raylib.LoadImage(@"Textures/ships/Spaceship_05_RED.png");
            Image player1_Test6Img = Raylib.LoadImage(@"Textures/ships/Spaceship_06_RED.png");

            //Player 2 Ships
            Image player2_1x3Img = Raylib.LoadImage(@"Textures/ships/Spaceship_01_GREEN.png");
            Image player2_1x2Img = Raylib.LoadImage(@"Textures/ships/Spaceship_02_GREEN.png");
            Image player2_2x2Img = Raylib.LoadImage(@"Textures/ships/Spaceship_03_GREEN.png");
            Image player2_1x5Img = Raylib.LoadImage(@"Textures/ships/Spaceship_04_GREEN.png");
            Image player2_1x4Img = Raylib.LoadImage(@"Textures/ships/Spaceship_05_GREEN.png");
            Image player2_Test6Img = Raylib.LoadImage(@"Textures/ships/Spaceship_06_GREEN.png");

        //Start Program
            Raylib.SetTargetFPS(60);        //Prefereble FPS
            Raylib.InitWindow(width+width/2, height, "Battleships");    //Makeing the window

        //Audio
            Raylib.InitAudioDevice();
            Music musicTrack0 = Raylib.LoadMusicStream(@"Sound/background/spaceEmergency.mp3");
            Music musicTrack1 = Raylib.LoadMusicStream(@"Sound/background/puzzle.mp3");

            Music computerBeep = Raylib.LoadMusicStream(@"Sound/SFX/computerChirp.mp3");
            
            Raylib.PlayMusicStream(musicTrack0);
            float musicTrack0Mult = 0.5f;
            Raylib.SetMusicVolume(musicTrack0, musicTrack0Mult * musicMult * audioMult);

            Raylib.PlayMusicStream(musicTrack1);
            float musicTrack1Mult = 0.6f;
            Raylib.SetMusicVolume(musicTrack1, musicTrack1Mult * musicMult * audioMult);

            Raylib.PlayMusicStream(computerBeep);
            float computerBeepMult = 0.2f;
            Raylib.SetMusicVolume(computerBeep, computerBeepMult * musicMult * audioMult);

            Sound shot0 = Raylib.LoadSound(@"Sound/SFX/shot.mp3");
            float shot0Mult = 0.15f;
            Raylib.SetSoundVolume(shot0, shot0Mult * sfxMult * audioMult);

            Sound engineStart = Raylib.LoadSound(@"Sound/SFX/engineStartup.mp3");
            float engineStartMult = 0.3f;
            Raylib.SetSoundVolume(engineStart, engineStartMult * sfxMult * audioMult);

        //Load Textures
            Texture2D redCursorTex;     Texture2D greenCursorTex;
            Texture2D backGroundTex;    Texture2D backGroundMenuTex;
            
            Texture2D butSinHovTex; Texture2D butSinOrgTex; Texture2D butSinPreTex;
            Texture2D butMulHovTex; Texture2D butMulOrgTex; Texture2D butMulPreTex;
            Texture2D butOptHovTex; Texture2D butOptOrgTex; Texture2D butOptPreTex;
            Texture2D butCreHovTex; Texture2D butCreOrgTex; Texture2D butCrePreTex;

            Texture2D butMenHovTex; Texture2D butMenOrgTex; Texture2D butMenPreTex;
            Texture2D butYesHovTex; Texture2D butYesOrgTex; Texture2D butYesPreTex;
            Texture2D butNoHovTex;  Texture2D butNoOrgTex;  Texture2D butNoPreTex;

            Texture2D bgTex0;   Texture2D bgTex1;   Texture2D bgTex2;
            Texture2D bgTex3;   Texture2D bgTex4;   Texture2D bgTex5;
            Texture2D bgTex6;   Texture2D bgTex7;   Texture2D bgTex8;
            Texture2D bgTex9;   Texture2D bgTex10;  Texture2D bgTex11;

            Texture2D player1_1x3Tex;   Texture2D player1_1x2Tex;   Texture2D player1_2x2Tex;
            Texture2D player1_1x5Tex;   Texture2D player1_1x4Tex;   Texture2D player1_Test6Tex;

            Texture2D player2_1x3Tex;   Texture2D player2_1x2Tex;   Texture2D player2_2x2Tex;
            Texture2D player2_1x5Tex;   Texture2D player2_1x4Tex;   Texture2D player2_Test6Tex;
            WindowResize();
            while(!Raylib.WindowShouldClose())                          //Game Loop
            {
                try{
                    Raylib.SetExitKey(0);       //Dont exit with Esc
                    Raylib.BeginDrawing();      //Start drawing (Including everything to be able to draw at any point)
                    Raylib.ClearBackground(Color.BLACK);    //Basic Background
                    FPS = Raylib.GetFPS();      //The FPS
                    Raylib.DrawText($"FPS: {FPS}", width-80, height-18, 16, Color.WHITE);   //Draws FPS, remove?
                    MousePos = Raylib.GetMousePosition();
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
                    {
                        gameStage = 0;
                        System.Console.WriteLine("INFO : MENU Activated");
                    }
                    if(gameStage == 0)
                    {
                        menu();
                    }
                    else if(gameStage == 1) //Game
                    {
                        game();
                    }
                    else if(gameStage == 2) //Game
                    {
                        opScreenStrWidth = Raylib.MeasureText(opScreenString, width/11);
                        opStrHeight = (int)opContainerSize.Y - (width/11) - optionBorder;
                        Options();
                    }
                    else
                    {
                        gameStage = 0;
                        menu();
                    }
                }
                catch
                {
                    CritError();
                }
                Music();
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
                    Raylib.PlaySound(engineStart);
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
                    Raylib.PlaySound(engineStart);
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
                if(Ship.activePlayer == 0)
                {
                    Raylib.ClearBackground(p1Color);
                    Raylib.DrawTexture(backGroundTex,0,0,Color.WHITE);
                    Ship.currentPos = cursorPos;
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        if(gamePhase==0)
                        {
                            try{
                                if(activeShip == 0 && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p1ShipList.Add(shipInstances);
                                    p1ShipList[p1ShipList.Count-1].Placement();
                                    p1ShipList[p1ShipList.Count-1].mainPositionDefined = new Vector2(cursorPos.X*(width-(border/2)/gridSize), cursorPos.X*(width-(border/2)/gridSize));
                                    p1ShipList[p1ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 1;
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
                                
                                else if(activeShip == 1 && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p1ShipList.Add(shipInstances);
                                    p1ShipList[p1ShipList.Count-1].Placement();
                                    p1ShipList[p1ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 1;
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
                                
                                else if(activeShip == 2 && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+3] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p1ShipList.Add(shipInstances);
                                    p1ShipList[p1ShipList.Count-1].Placement();
                                    p1ShipList[p1ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 1;
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

                                else if(activeShip == 3 && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+3] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+4] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p1ShipList.Add(shipInstances);
                                    p1ShipList[p1ShipList.Count-1].Placement();
                                    p1ShipList[p1ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 1;
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
                                
                                else if(activeShip == 4 && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P1Hitbox[(int)Ship.currentPos.X+1, (int)Ship.currentPos.Y] == false && Ship.P1Hitbox[(int)Ship.currentPos.X+1, (int)Ship.currentPos.Y+1] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p1ShipList.Add(shipInstances);
                                    p1ShipList[p1ShipList.Count-1].Placement();
                                    p1ShipList[p1ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 1;
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
                                
                                else
                                {
                                    System.Console.WriteLine("ERROR : Position filled");
                                }
                            }
                            catch
                            {
                                System.Console.WriteLine("ERROR : Position outof Index");
                            }
                        }
                    }
                }
                
                else if(Ship.activePlayer == 1)
                {
                    Raylib.ClearBackground(p2Color);
                    Raylib.DrawTexture(backGroundTex, 0, 0, Color.WHITE);
                    Ship.currentPos = cursorPos;
                    if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        if(gamePhase==0)
                        {
                            try{
                                if(activeShip == 0 && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p2ShipList.Add(shipInstances);
                                    p2ShipList[p2ShipList.Count-1].Placement();
                                    p2ShipList[p2ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 0;
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
                                else if(activeShip == 1 && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p2ShipList.Add(shipInstances);
                                    p2ShipList[p2ShipList.Count-1].Placement();
                                    p2ShipList[p2ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 0;
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
                                else if(activeShip == 2 && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+3] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p2ShipList.Add(shipInstances);
                                    p2ShipList[p2ShipList.Count-1].Placement();
                                    p2ShipList[p2ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 0;
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
                                else if(activeShip == 3 && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+2] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+3] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+4] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p2ShipList.Add(shipInstances);
                                    p2ShipList[p2ShipList.Count-1].Placement();
                                    p2ShipList[p2ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 0;
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
                                else if(activeShip == 4 && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X, (int)Ship.currentPos.Y+1] == false && Ship.P2Hitbox[(int)Ship.currentPos.X+1, (int)Ship.currentPos.Y] == false && Ship.P2Hitbox[(int)Ship.currentPos.X+1, (int)Ship.currentPos.Y+1] == false)
                                {
                                    Ship shipInstances = new Ship(activeShip, cursorPos);
                                    p2ShipList.Add(shipInstances);
                                    p2ShipList[p2ShipList.Count-1].Placement();
                                    p2ShipList[p2ShipList.Count-1].sidePositionDefined = new Vector2((cursorPos.X*(width-(border*2))/gridSize)/2 + width, cursorPos.Y*((height-(border*2))/gridSize)/2);
                                    Ship.activePlayer = 0;
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
                                else
                                {
                                    System.Console.WriteLine("There is already a ship there");
                                }
                            }
                            catch
                            {
                                System.Console.WriteLine("That place is outside area");
                            }
                        }

                    }
                }
                
                else
                {
                   Raylib.ClearBackground(Color.BLACK); 
                }

                Grid();
                
                LoadShips();

                Cursor();
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
                    if(gamePhase == 0 && activeShip == 0)
                    {
                        if(cursorPos.Y < gridSize-2)
                        {
                            cursorPos.Y++;
                        }
                    }
                    else if(gamePhase == 0 && activeShip == 1)
                    {
                        if(cursorPos.Y < gridSize-3)
                        {
                            cursorPos.Y++;
                        }
                    }
                    else if(gamePhase == 0 && activeShip == 2)
                    {
                        if(cursorPos.Y < gridSize-4)
                        {
                            cursorPos.Y++;
                        }
                    }
                    else if(gamePhase == 0 && activeShip == 3)
                    {
                        if(cursorPos.Y < gridSize-5)
                        {
                            cursorPos.Y++;
                        }
                    }
                    else if(gamePhase == 0 && activeShip == 4)
                    {
                        if(cursorPos.Y < gridSize-2)
                        {
                            cursorPos.Y++;
                        }
                    }
                    else
                    {
                        if(cursorPos.Y < gridSize-1)
                        {
                            cursorPos.Y++;
                        }
                    }
                }
                
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                {
                    if(gamePhase == 0 && activeShip == 4)
                    {
                        if(cursorPos.X < gridSize-2)
                        {
                            cursorPos.X++;
                        }
                    }
                    else if(cursorPos.X < gridSize-1)
                    {
                        cursorPos.X++;
                    }
                }
                
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_Q))  //Test
                {
                    if(activeShip > 0)
                    {
                        activeShip--;
                    }
                }
                
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_E))  //Test
                {
                    if(activeShip<4)
                    {
                        activeShip++;
                    }
                }
                
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_X))  //Test
                {
                    if(gamePhase == 0)
                    {
                        gamePhase = 1;
                    }
                    else
                    {
                        gamePhase = 0;
                    }
                    Raylib.PlaySound(shot0);
                }

                if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A) || 
                Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W))
                    {
                        if(cursorPos.Y > 0 && cursorTime >= (FPS/3)*1.2f)
                        {
                            cursorPos.Y--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A))
                    {
                        if(cursorPos.X > 0 && cursorTime >= (FPS/3)*1.2f)
                        {
                            cursorPos.X--;
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S))
                    {
                        if(cursorTime >= (FPS/3)*1.2f)
                        {
                            if(activeShip == 0)
                            {
                                if(cursorPos.Y < gridSize-2)
                                {
                                    cursorPos.Y++;
                                }
                            }
                            else if(activeShip == 1)
                            {
                                if(cursorPos.Y < gridSize-3)
                                {
                                    cursorPos.Y++;
                                }
                            }
                            else if(activeShip == 2)
                            {
                                if(cursorPos.Y < gridSize-4)
                                {
                                    cursorPos.Y++;
                                }
                            }
                            else if(activeShip == 3)
                            {
                                if(cursorPos.Y < gridSize-5)
                                {
                                    cursorPos.Y++;
                                }
                            }
                            else if(activeShip == 4)
                            {
                                if(cursorPos.Y < gridSize-2)
                                {
                                    cursorPos.Y++;
                                }
                            }
                            else
                            {
                                if(cursorPos.Y < gridSize-1)
                                {
                                    cursorPos.Y++;
                                }
                            }
                        }
                    }
                    
                    if(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
                    {
                        if(cursorTime >= (FPS/3)*1.2f)
                        {
                            if(activeShip == 4)
                            {
                                if(cursorPos.X < gridSize-2)
                                {
                                    cursorPos.X++;
                                }
                            }
                            else if(cursorPos.X < gridSize-1)
                            {
                                cursorPos.X++;
                            }
                        }
                    }
                    if(cursorTime < (FPS/3)*1.2f)
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
                cursorYstr += gridSize-cursorPos.Y;
                Raylib.DrawText($"{cursorXstr} {cursorYstr}", border/10+width, border/10 + height/2, (border/10)*18, Color.BLACK);  //Actully shows Position
                cursorYstr += (gridSize-cursorPos.Y);           //To show position count starting from bottom left for player and not top left
                if(gamePhase == 0)
                {
                    if(activeShip == 0)
                    {
                        if(Ship.activePlayer == 0)
                        {
                            Raylib.DrawTexture(player1_1x2Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if(Ship.activePlayer == 1)
                        {
                            Raylib.DrawTexture(player2_1x2Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                    }
                    else if(activeShip == 1)
                    {
                        if(Ship.activePlayer == 0)
                        {
                            Raylib.DrawTexture(player1_1x3Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if(Ship.activePlayer == 1)
                        {
                            Raylib.DrawTexture(player2_1x3Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                    }
                    else if(activeShip == 2)
                    {
                        if(Ship.activePlayer == 0)
                        {
                             Raylib.DrawTexture(player1_1x4Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize) , Color.WHITE);
                        }
                        else if(Ship.activePlayer == 1)
                        {
                            Raylib.DrawTexture(player2_1x4Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize) , Color.WHITE);
                        }
                    }
                    else if(activeShip == 3)
                    {
                        if(Ship.activePlayer == 0)
                        {
                            Raylib.DrawTexture(player1_1x5Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize) , Color.WHITE);
                        }
                        else if(Ship.activePlayer == 1)
                        {
                            Raylib.DrawTexture(player2_1x5Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize) , Color.WHITE);
                        }
                    }
                    else if(activeShip == 4)
                    {
                        if(Ship.activePlayer == 0)
                        {
                            Raylib.DrawTexture(player1_2x2Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if(Ship.activePlayer == 1)
                        {
                            Raylib.DrawTexture(player2_2x2Tex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                    }
                    else if(activeShip == 5)
                    {
                        if(Ship.activePlayer == 0)
                        {

                        }
                        else if(Ship.activePlayer == 1)
                        {
                            
                        }
                    }
                    if(activeShip == 0 && cursorPos.Y > gridSize-2)
                    {
                        cursorPos.Y = gridSize-2;
                    }
                    if(activeShip == 1 && cursorPos.Y > gridSize-3)
                    {
                        cursorPos.Y = gridSize-3;
                    }
                    if(activeShip == 2 && cursorPos.Y > gridSize-4)
                    {
                        cursorPos.Y = gridSize-4;
                    }
                    if(activeShip == 3 && cursorPos.Y > gridSize-5)
                    {
                        cursorPos.Y = gridSize-5;
                    }
                    if(activeShip == 4 && cursorPos.Y > gridSize-2)
                    {
                        cursorPos.Y = gridSize-2;
                    }

                    if(activeShip == 4 && cursorPos.X > gridSize-2)
                    {
                        cursorPos.X = gridSize-2;
                    }
                }    

                if (gamePhase == 1)
                {
                    if(Ship.activePlayer == 0)
                    {
                        Raylib.DrawTexture(redCursorTex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);  //Cursor Position
                    }
                    else if(Ship.activePlayer == 1)
                    {
                        Raylib.DrawTexture(greenCursorTex, border+(int)cursorPos.X*((width-(border*2))/gridSize), border+(int)cursorPos.Y*((height-(border*2))/gridSize), Color.WHITE);  //Cursor Position
                    }
                }
            }
            
            void Music()
            {
                if(gameStage == 0)
                {
                    Raylib.UpdateMusicStream(musicTrack1);
                }

                if(gameStage == 1)
                {
                    Raylib.UpdateMusicStream(musicTrack0);
                }
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
                        Raylib.DrawRectangleLines(border+a*(int)gridMainPos.X, border+b*(int)gridMainPos.Y, (int)gridMainSize.X, (int)gridMainSize.Y, gridLines);  //Grid
                        Raylib.DrawRectangleLines((int)gridSidePos.X , (int)gridSidePos.Y , (int)gridMainSize.X/2 , (int)gridMainSize.Y/2 , gridLines);   //Side Grid
                        gridSidePos.X = a*((width-(border*2))/gridSize)/2 + width;
                        gridSidePos.Y = b*((height-(border*2))/gridSize)/2;                

                    }
                }
            }
        
            void Options()  //Menu for changeing options
            {   
                if(opGridInt == 0)    //Grid
                {

                }

                Raylib.DrawTexture(backGroundMenuTex,0,0,Color.WHITE);

                Raylib.DrawRectangle((int)opGridPos.X, (int)opGridPos.Y, (int)opContainerSize.X, (int)opContainerSize.Y, optionOrgColor);
                Raylib.DrawRectangleLinesEx(opGridRec, 5, Color.RED);

                Raylib.DrawRectangle((int)opScreenPos.X, (int)opScreenPos.Y, (int)opContainerSize.X, (int)opContainerSize.Y, optionOrgColor);
                Raylib.DrawRectangleLinesEx(opScreenRec, 5, Color.RED);
                Raylib.DrawText(opScreenString, (int)(width*0.75)-opScreenStrWidth/2, (int)opScreenPos.Y+opStrHeight, width/11, Color.WHITE);

                Raylib.DrawRectangle((int)opAudioPos.X, (int)opAudioPos.Y, (int)opContainerSize.X, (int)opContainerSize.Y, optionOrgColor);
                Raylib.DrawRectangle((int)opMusicPos.X, (int)opMusicPos.Y, (int)opContainerSize.X, (int)opContainerSize.Y, optionOrgColor);
                Raylib.DrawRectangle((int)opSfxPos.X, (int)opSfxPos.Y, (int)opContainerSize.X, (int)opContainerSize.Y, optionOrgColor);
                

                //Resulutions 
                //600 x 400 > width = 400
                //900 x 600 > width = 600
                //1200 x 800 > width = 800
                //1500 x 1000 > width = 1000

                //GridSizes > Restart Game
                //8, 10, 12, 15(Standard), 18, 20, 30, 40, 60

                //Audio multiplier
                
                if(Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    OptionSave();
                }
            }
            
            void ScreenString()
            {
                if(opScreenInt == 0)    //Resulutions
                {
                    opScreenString = "Resulution : 600x400";
                    opScreenStrWidth = Raylib.MeasureText(opScreenString, width/11);
                }
                else if(opScreenInt == 1)
                {
                    opScreenString = "Resulution : 900x600";
                    opScreenStrWidth = Raylib.MeasureText(opScreenString, width/11);
                }
                else if(opScreenInt == 2)
                {
                    opScreenString = "Resulution : 1200x800";
                    opScreenStrWidth = Raylib.MeasureText(opScreenString, width/11);
                }
                else if(opScreenInt == 3)
                {
                    opScreenString = "Resulution : 1500x1000";
                    opScreenStrWidth = Raylib.MeasureText(opScreenString, width/11);
                }
            }

            void OptionSave()   //Called to change the values and add the to Settings.txt
            {
                string[] settingStrings = new string[5];
                settingStrings[0] = width.ToString();
                settingStrings[1] = gridSize.ToString();
                settingStrings[2] = audioMult.ToString();
                settingStrings[3] = sfxMult.ToString();
                settingStrings[4] = musicMult.ToString();
                
                File.WriteAllLines(@"Settings.txt", settingStrings);
                Raylib.SetMusicVolume(musicTrack0, musicTrack0Mult*audioMult*musicMult);
                Raylib.SetMusicVolume(musicTrack1, musicTrack1Mult*audioMult*musicMult);
                Raylib.SetMusicVolume(computerBeep, computerBeepMult * musicMult * audioMult);
                Raylib.SetSoundVolume(shot0, shot0Mult*audioMult*sfxMult);
                Raylib.SetSoundVolume(engineStart, engineStartMult*audioMult*sfxMult);
                gameStage = 0;
            }

            void WindowResize() //Because when the window is resized textures need to be recalculated   //Future
            {
            //Change Values
                gridMainPos = new Vector2(((width-(border*2))/gridSize), ((height-(border*2))/gridSize));   //The position to calculate the correct spot for each squre
                gridMainSize = new Vector2((width/gridSize)-((border*2)/gridSize), (height/gridSize)-((border*2)/gridSize));    //The Size of the squres

                gridSidePos = new Vector2(((width-(border*2))/gridSize)/2, ((height-(border*2))/gridSize)/2);   //The position to calculate the side squres
            
            //Image Resizeing
                Raylib.ImageResize(ref redCursorImg, (int)gridMainSize.X, (int)gridMainSize.Y);
                Raylib.ImageResize(ref greenCursorImg, (int)gridMainSize.X, (int)gridMainSize.Y);
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
                Raylib.ImageResize(ref player1_2x2Img, (int)gridMainSize.X*2, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player1_1x5Img, (int)gridMainSize.X, (int)gridMainSize.Y*5);
                Raylib.ImageResize(ref player1_1x4Img, (int)gridMainSize.X, (int)gridMainSize.Y*4);
                Raylib.ImageResize(ref player1_Test6Img, (int)gridMainSize.X, (int)gridMainSize.Y);

            //Player 2 Ships
                Raylib.ImageResize(ref player2_1x3Img, (int)gridMainSize.X, (int)gridMainSize.Y*3);
                Raylib.ImageResize(ref player2_1x2Img, (int)gridMainSize.X, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player2_2x2Img, (int)gridMainSize.X*2, (int)gridMainSize.Y*2);
                Raylib.ImageResize(ref player2_1x5Img, (int)gridMainSize.X, (int)gridMainSize.Y*5);
                Raylib.ImageResize(ref player2_1x4Img, (int)gridMainSize.X, (int)gridMainSize.Y*4);
                Raylib.ImageResize(ref player2_Test6Img, (int)gridMainSize.X, (int)gridMainSize.Y);
            
            //Textures Reload
                redCursorTex = Raylib.LoadTextureFromImage(redCursorImg);
                greenCursorTex = Raylib.LoadTextureFromImage(greenCursorImg);
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
                player1_2x2Tex = Raylib.LoadTextureFromImage(player1_2x2Img);
                player1_1x5Tex = Raylib.LoadTextureFromImage(player1_1x5Img);
                player1_1x4Tex = Raylib.LoadTextureFromImage(player1_1x4Img);
                player1_Test6Tex = Raylib.LoadTextureFromImage(player1_Test6Img);

                player2_1x3Tex = Raylib.LoadTextureFromImage(player2_1x3Img);
                player2_1x2Tex = Raylib.LoadTextureFromImage(player2_1x2Img);
                player2_2x2Tex = Raylib.LoadTextureFromImage(player2_2x2Img);
                player2_1x5Tex = Raylib.LoadTextureFromImage(player2_1x5Img);
                player2_1x4Tex = Raylib.LoadTextureFromImage(player2_1x4Img);
                player2_Test6Tex = Raylib.LoadTextureFromImage(player2_Test6Img);
            }
        
            void LoadShips()
            {
                if(Ship.activePlayer == 0 && gamePhase == 0){
                    for(int i = 0; i < p1ShipList.Count; i++)
                    {
                        if (p1ShipList[i].type == 0)
                        {
                            Raylib.DrawTexture(player1_1x2Tex, border+(int)p1ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p1ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 1)
                        {
                            Raylib.DrawTexture(player1_1x3Tex, border+(int)p1ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p1ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 2)
                        {
                           Raylib.DrawTexture(player1_1x4Tex, border+(int)p1ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p1ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 3)
                        {
                            Raylib.DrawTexture(player1_1x5Tex, border+(int)p1ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p1ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 4)
                        {
                            Raylib.DrawTexture(player1_2x2Tex, border+(int)p1ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p1ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                    }
                }
                if(Ship.activePlayer == 1 && gamePhase == 0){
                    for(int i = 0; i < p2ShipList.Count; i++)
                    {
                        if (p2ShipList[i].type == 0)
                        {
                            Raylib.DrawTexture(player2_1x2Tex, border+(int)p2ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p2ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 1)
                        {
                            Raylib.DrawTexture(player2_1x3Tex, border+(int)p2ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p2ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 2)
                        {
                           Raylib.DrawTexture(player2_1x4Tex, border+(int)p2ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p2ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 3)
                        {
                            Raylib.DrawTexture(player2_1x5Tex, border+(int)p2ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p2ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 4)
                        {
                            Raylib.DrawTexture(player2_2x2Tex, border+(int)p2ShipList[i].position.X*((width-(border*2))/gridSize), border+(int)p2ShipList[i].position.Y*((height-(border*2))/gridSize), Color.WHITE);
                        }
                    }
                }

                if(Ship.activePlayer == 0 && gamePhase == 1){
                    for(int i = 0; i < p1ShipList.Count; i++)
                    {
                        if (p1ShipList[i].type == 0)
                        {
                            Raylib.DrawTextureEx(player1_1x2Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 1)
                        {
                            Raylib.DrawTextureEx(player1_1x3Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 2)
                        {
                            Raylib.DrawTextureEx(player1_1x4Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 3)
                        {
                            Raylib.DrawTextureEx(player1_1x5Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p1ShipList[i].type == 4)
                        {
                            Raylib.DrawTextureEx(player1_2x2Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                    }
                }
                if(Ship.activePlayer == 1 && gamePhase == 1){
                    for(int i = 0; i < p2ShipList.Count; i++)
                    {
                        if (p2ShipList[i].type == 0)
                        {
                            Raylib.DrawTextureEx(player2_1x2Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 1)
                        {
                            Raylib.DrawTextureEx(player2_1x3Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 2)
                        {
                            Raylib.DrawTextureEx(player2_1x4Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 3)
                        {
                            Raylib.DrawTextureEx(player2_1x5Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                        else if (p2ShipList[i].type == 4)
                        {
                            Raylib.DrawTextureEx(player2_2x2Tex, p1ShipList[i].sidePositionDefined, 0, 0.5f, Color.WHITE);
                        }
                    }
                }
            }         

            void Test()
            {
                
            }

            void CritError()
            {
                System.Console.WriteLine("ERROR : CRITICAL ERROR");
                Raylib.ClearBackground(Color.BLUE);
                Raylib.DrawText("THERE HAS BEEN A CRITICAL ERROR", border, border, width/20, Color.RED);
                Raylib.UpdateMusicStream(computerBeep);
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
//Meny Knappar, nedre Högra hörnet
//Shooting (Where you have shoot at oppenent and where opponent have shoot at you)
//How many ships you are Should use of each type
//Save The Option floats (They become 1 right now)

//freakyNews to be used when waiting for next player?
