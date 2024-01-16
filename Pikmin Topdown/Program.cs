using Raylib_cs;
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

int pikminPosX = 500;
int pikminPosY = 500;
Vector2 pikminTruePos = new Vector2();
Vector2 pikminRelative = new Vector2();

bool pikminHeld = false;
bool pikminThrown = false;
int maxLength = 300;
Vector2 pikminThrowDist = new Vector2();
Vector2 pikminThrowDiff = new Vector2();

int whistlePosX;
int whistlePosY;
Vector2 whistlePos = new Vector2();
Vector2 pikminWhistleDiff = new Vector2();
int pikminWhistleLength = (int)pikminWhistleDiff.Length();

Vector2 throwPosition = new Vector2();
Vector2 indicatorPoint = new Vector2();

int whistleRange = 150;
int whistleRangeTrue = 170;


int mousePositionX = Raylib.GetMouseX();
int mousePositionY = Raylib.GetMouseY();
Vector2 mousePosition = new Vector2(mousePositionX, mousePositionY);

bool pikminDestReached = false;

Texture2D Louie = Raylib.LoadTexture("Louie.png");
Rectangle rectLouie = new Rectangle(0, 0, Louie.Width, Louie.Height);

int centerLouieWidth = Louie.Width / 2;
int centerLouieHeight = Louie.Height / 2;
Vector2 positionLouie = new Vector2(centerLouieWidth, centerLouieHeight);
Vector2 centeredLouie = new Vector2();

Vector2 diff = new Vector2();
int throwVector = (int)diff.Length();
int pikminDiffLength = (int)pikminThrowDiff.Length();

while (!Raylib.WindowShouldClose())
{

    if (scene == "startScreen")
    {
        Raylib.ClearBackground(Color.WHITE);
        Raylib.BeginDrawing();
        Raylib.DrawText(start, screenSizeX / 6, screenSizeY / 3, 20, Color.DARKBLUE);
        Raylib.EndDrawing();

        //Startar spelet
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
        {
            scene = "game";

        }
    }

    if (scene == "game")
    {
        centeredLouie = new Vector2(centerLouieHeight + positionLouie.X, centerLouieHeight + positionLouie.Y);
        LouieMovement();

        Raylib.ClearBackground(Color.WHITE);
        Raylib.BeginDrawing();
        Raylib.DrawTextureRec(Louie, rectLouie, positionLouie, Color.WHITE);
        Raylib.DrawCircle(pikminPosX, pikminPosY, 20, Color.BLACK);




        //Normala Pikmin walk
        if (!pikminHeld)
        {
            pikminRelative = centeredLouie - pikminTruePos;


            if ((int)Math.Ceiling(pikminRelative.Length()) > 100)
            {
                pikminTruePos = new Vector2(pikminPosX, pikminPosY);
                pikminRelative = Vector2.Normalize(pikminRelative) * 2;
                pikminPosX += (int)Math.Ceiling(pikminRelative.X);
                pikminPosY += (int)Math.Ceiling(pikminRelative.Y);
            }
        }



        //Throwing indicator
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
        {
            mousePositionX = Raylib.GetMouseX();
            mousePositionY = Raylib.GetMouseY();

            //Räknar ut hur linjen ska se ut 
            mousePosition = new Vector2(mousePositionX, mousePositionY);
            diff = mousePosition - centeredLouie;
            throwVector = (int)diff.Length();


            indicatorPoint = Vector2.Normalize(diff) * maxLength;

            if (throwVector < maxLength)
            {
                Raylib.DrawLine((int)centeredLouie.X, (int)centeredLouie.Y, (int)mousePosition.X, (int)mousePosition.Y, Color.BLACK);
            }

            if (throwVector > maxLength)
            {
                Raylib.DrawLine((int)centeredLouie.X, (int)centeredLouie.Y, (int)centeredLouie.X + (int)indicatorPoint.X, (int)centeredLouie.Y + (int)indicatorPoint.Y, Color.BLACK);
            }

            // var pikmin1 = new Pikmin() {startPosX = mousePositionX, startPosY = mousePositionY};

        }



        //Throwing Pikmin
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON) && !pikminThrown)
        {
            pikminHeld = true;
            pikminPosX = (int)centeredLouie.X;
            pikminPosY = (int)centeredLouie.Y;
        }




        if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
        {
            pikminThrown = true;
            throwPosition = new Vector2(mousePositionX, mousePositionY);
            if (throwVector > maxLength)
            {

                throwPosition.X = (int)centeredLouie.X + (int)indicatorPoint.X;
                throwPosition.Y = (int)centeredLouie.Y + (int)indicatorPoint.Y;
            }
        }





        //Throwing Calculations
        if (pikminThrown && !pikminDestReached)
        {
            pikminTruePos = new Vector2(pikminPosX, pikminPosY);

            pikminThrowDist = throwPosition - pikminTruePos;
            pikminThrowDiff = Vector2.Normalize(pikminThrowDist) * 3;

            pikminPosX += (int)Math.Round(pikminThrowDiff.X);
            pikminPosY += (int)Math.Round(pikminThrowDiff.Y);

            if ((int)Math.Ceiling(pikminThrowDist.Length()) < 2)
            {
                pikminPosX = (int)throwPosition.X;
                pikminPosY = (int)throwPosition.Y;
                pikminDestReached = true;
            }
        }




        //Tar tillbaka Pikmin
        if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
        {
            whistlePosX = Raylib.GetMouseX();
            whistlePosY = Raylib.GetMouseY();
            whistlePos = new Vector2(whistlePosX, whistlePosY);
            pikminTruePos = new Vector2(pikminPosX, pikminPosY);


            pikminWhistleDiff = pikminTruePos - whistlePos;
            pikminWhistleLength = (int)pikminWhistleDiff.Length();

            Raylib.DrawCircleLines(whistlePosX, whistlePosY, whistleRange, Color.BLUE);

            if (pikminWhistleLength < whistleRangeTrue)
            {
                pikminHeld = false;
                pikminThrown = false;
                pikminDestReached = false;
            }
        }

        Raylib.EndDrawing();
    }
}





//Louies movement
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