using System.ComponentModel;
using System;
using Raylib_cs;

// Spelets höjd och bredd som metod
const int skärmBredd = 1280;
const int skärmHöjd = 720;

// Standard inställningar, hur stor ska resolutionen vara och fps
Raylib.InitWindow(skärmBredd, skärmHöjd, "Slutprojektet (inge namn än)");
Raylib.SetTargetFPS(60);

// ----------------------------------------------------------------------------------------------------->

// Spelarens Rectangel (kords)
Rectangle player = new Rectangle(170, 330, 80, 80);
// Spelbara karaktären rörelse hastighet
int playerSpeed = 6;
// Sätter "welcomescreen" till den scenen som visas när man startar spelet
string currenctScene = "welcomescreen";

// Spel Loop (Main one)
while (!Raylib.WindowShouldClose())
{
    // KEYBOARD-CONTROLS                  
    if (currenctScene == "level1")
    {
        // Gör så att om spelaren trycker högerpil, rör rectangle höger
        if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            player.x += playerSpeed;
        }

        // Gör så att om spelaren trycker vänsterpil, rör rectangle vänster
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            player.x -= playerSpeed;
        }

        // Kontrollerar om "Spacebar" trycks och om spelarens kordinater är mer än y=0, 
        // Om det stämmer, uppdaterar den spelarens y kordinater genom att
        // subtrahera produkten av spelarens hastighet multiplicerat med 2
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && player.y >= 0)
        {
            player.y -= playerSpeed * 2;
        }

        // Kontrollerar om spelarens y kordinat plus spelarens höjd är mindre än skärmhöjden
        // Om det stämmer, uppdaterar spelarens y-koordinat genom att lägga till spelarens hastighet till y.
        // Denna kod tillåter spelaren att falla neråt när dom inte klickar på "spacebar" och hoppar
        if (player.y + player.height < skärmHöjd)
        {
            player.y += playerSpeed;
        }

        // Notes för mig: Gör så man inte kan hopppa för alltid
        // Cooldown?
    }
    else if (currenctScene == "welcomescreen")
    {
        // ENTER to transition from Welcome Screen to Game!
        if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
        {
            currenctScene = "level1";
        }
    }

    // MAP-CUSTOMIZATION                  
    // Initierar Raylib-Renderingssystemet
    Raylib.BeginDrawing();

   


        // Gör att bakgrunden är vit
        Raylib.ClearBackground(Color.WHITE);

        // Gör att spelarens karaktär (rektangeln) blir svart.
        Raylib.DrawRectangleRec(player, Color.BLACK);


        // Ritar text som säger man start, när programmet startas
   if (currenctScene == "welcomescreen")
  {
    Raylib.DrawText("Welcome To Slutprojektet", 300, 180, 50, Color.BLACK);
    Raylib.DrawText("\nMove using the arrow keys and jump with SPACE", 275, 230, 32, Color.BLACK);
    Raylib.DrawText("\nENTER to begin", 515, 195, 32, Color.BLACK);
  }

  else if (currenctScene == "level1")
  {   
    
  }

        // Avslutar Raylib-Renderingssystemet
        Raylib.EndDrawing();
    }

// Stänger spelfönstret
Raylib.CloseWindow();