﻿using Raylib_cs;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics;

string scene = "startScreen";
string start = "Press SPACE to start le PEAKMEN";
int screenSizeX = 1920;
int screenSizeY = 1080;
Raylib.InitWindow(1000, 1000, "PEAKMAN");

int maxLength = 300;

int pikminPosX = 500;
int pikminPosY = 500;

int pelletPosX = 200;
int pelletPosY = 200;

bool pikminHeld = false;
bool pikminThrown = false;


int mousePositionX = Raylib.GetMouseX();
int mousePositionY = Raylib.GetMouseY();

Texture2D Louie = Raylib.LoadTexture("Louie.png");
Rectangle rectLouie = new Rectangle(0, 0, Louie.Width, Louie.Height);

int centerLouieWidth = Louie.Width / 2;
int centerLouieHeight = Louie.Height / 2;

Vector2 positionLouie = new Vector2(centerLouieWidth, centerLouieHeight);
Vector2 centeredLouie = new Vector2();

Vector2 mousePosition = new Vector2(mousePositionX, mousePositionY);

Vector2 diff = new Vector2();

Vector2 pikminTruePos = new Vector2();

Vector2 pelletTruePos = new Vector2();

Vector2 pikminRelative = new Vector2();

int throwVector = (int)diff.Length();

while (!Raylib.WindowShouldClose())
{

    if (scene == "startScreen")
    {
        Raylib.ClearBackground(Color.WHITE);
        Raylib.BeginDrawing();
        Raylib.DrawText(start, screenSizeX / 6, screenSizeY / 3, 20, Color.DARKBLUE);
        Raylib.EndDrawing();
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            scene = "game";

        }
    }
    if (scene == "game")
    {
        centeredLouie = new Vector2(centerLouieHeight + positionLouie.X, centerLouieHeight + positionLouie.Y);
        Raylib.ClearBackground(Color.WHITE);
        LouieMovement();
        Raylib.BeginDrawing();
        Raylib.DrawTextureRec(Louie, rectLouie, positionLouie, Color.WHITE);
        //Raylib.DrawCircle((int)positionLouie.X,(int) positionLouie.Y, 300, Color.BEIGE);
        if(!pikminHeld)
        {
            pikminRelative = pikminTruePos - centeredLouie;
            if ((int)Math.Ceiling(pikminRelative.Length()) > 100)
            {
                pikminTruePos = new Vector2(pikminPosX, pikminPosY);
                pikminRelative = Vector2.Normalize(pikminRelative) * -2;
                pikminPosX += (int)Math.Ceiling(pikminRelative.X);
                pikminPosY += (int)Math.Ceiling(pikminRelative.Y);
            }
        }
        Raylib.DrawCircle(pikminPosX, pikminPosY, 20, Color.BLACK);

        Raylib.DrawCircle(pelletPosX, pelletPosY, 20, Color.RED);

        if (Raylib.CheckCollisionCircles(pikminTruePos, 20, pelletTruePos, 10))
        {

        }

        //Throwing indicator
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
        {
            mousePositionX = Raylib.GetMouseX();
            mousePositionY = Raylib.GetMouseY();

            mousePosition = new Vector2(mousePositionX, mousePositionY);
            Vector2 tempPos = new Vector2(positionLouie.X + Louie.Height / 2, positionLouie.Y + Louie.Width / 2);
            diff = mousePosition - tempPos;
            throwVector = (int)diff.Length();


            Vector2 tempVector = Vector2.Normalize(diff) * maxLength;

            if (throwVector < maxLength)
            {
                Raylib.DrawLine((int)positionLouie.X + Louie.Height / 2, (int)positionLouie.Y + Louie.Width / 2, (int)mousePosition.X, (int)mousePosition.Y, Color.BLACK);
            }

            if (throwVector > maxLength)
            {
                Raylib.DrawLine((int)positionLouie.X + Louie.Height / 2, (int)positionLouie.Y + Louie.Width / 2, (int)positionLouie.X + Louie.Height / 2 + (int)tempVector.X, (int)positionLouie.Y + Louie.Height / 2 + (int)tempVector.Y, Color.BLACK);
            }

            // var pikmin1 = new Pikmin() {startPosX = mousePositionX, startPosY = mousePositionY};

        }

        //Throwing Pikmin
        if(Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
        {
            pikminHeld = true;
            pikminPosX = (int)centeredLouie.X;
            pikminPosY = (int)centeredLouie.Y;
        }  


        if(Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
        {
            pikminThrown = true;
        }
        if(pikminThrown)
        {
            
        }

        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
        {
            pikminHeld = false;
            pikminThrown = false;
        }

        Raylib.EndDrawing();
    }
}



void LouieMovement()
{
    if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {
        positionLouie.X -= 1;
    }

    if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
        positionLouie.X += 1;
    }

    if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_W))
    {
        positionLouie.Y -= 1;

    }

    if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) || Raylib.IsKeyDown(KeyboardKey.KEY_S))
    {
        positionLouie.Y += 1;
    }
}

// public class Pikmin
// {

//     public int posX;
//     public int posY;
//     public int degreeTrow;

//     public Pikmin(int posX, int posY)
//     {
//         this.posX = posX;
//         this.posY = posY;
//     }


//     public void Move()
//     {
//         pikminRelative = pikminTruePos - centeredLouie;
//         if ((int)Math.Ceiling(pikminRelative.Length()) > 100)
//         {
//             pikminTruePos = new Vector2(pikminPosX, pikminPosY);
//             pikminRelative = Vector2.Normalize(pikminRelative) * -2;
//             pikminPosX += (int)Math.Ceiling(pikminRelative.X);
//             pikminPosY += (int)Math.Ceiling(pikminRelative.Y);
//         }


//     }
// }


