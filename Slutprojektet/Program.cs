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

// Spel Loop (Main one)
while (!Raylib.WindowShouldClose())
{
   
    // Spelarens Rectangel (kords)
    Rectangle player = new Rectangle(170, 330, 80, 80);
            
    // Spelbara karaktären rörelse hastighet
    int playerSpeed = 6;


// ----------------------------------------------------------------------------------------------------->
//                  KEYBOARD-CONTROLS                  //


    while (!Raylib.WindowShouldClose())
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

// ----------------------------------------------------------------------------------------------------->
//                  MAP-CUSTOMIZATION                  //

    // Initierar Raylib-Renderingssystemet
    Raylib.BeginDrawing();

        // Gör att bakgrunden är vit
        Raylib.ClearBackground(Color.WHITE);

        // Gör att spelarens karaktär (rektangeln) blir svart.
        Raylib.DrawRectangleRec(player, Color.BLACK);



        // Avslutar Raylib-Renderingssystemet
        Raylib.EndDrawing();
    }

    
}

// Stänger spelfönstret
Raylib.CloseWindow();