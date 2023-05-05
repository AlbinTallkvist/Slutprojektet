using System;
using System.Numerics;
using Raylib_cs;

// Skapar två konstanter "Skärmhöjd" & Bredd med respektvie värde som sedan kan placeras in senare i koden.
const int SkärmBredd = 800;
const int SkärmHöjd = 600;

// Sätter "mainspel" till den scenen som visas när man startar spelet
string currentScene = "mainspel";

// Standard inställningar, hur stor spel-skärmen ska vara och fps
Raylib.InitWindow(SkärmBredd, SkärmHöjd, "Hoppa Mot Himlen");
Raylib.SetTargetFPS(60);

// Metod för att färga bakgrunden helt röd
static void ClearBackground()
{
    Raylib.ClearBackground(Color.RED);
}

// Skapar rektangel som spelaren kan kontrollera med en bredd på 40, höjd 80 
// och placerar den i mitten av skärmen i x-led och vid botten av skärmen i y-led.
Rectangle Spelare = new Rectangle(SkärmBredd / 2 - 20, SkärmHöjd - 250, 40, 80);

// Skapar en ny vector2 och sätter dess x och y till värdet 0, denna kod representerar spelarens hastighet i x & y-riktning.
Vector2 SpelareHastighet = new Vector2(0, 0);

// Sätter så att man i början när spelet startas kan hoppa och att score är 0 (bool sparar ett värde som sannt eller falskt)
bool KanHoppa = true;
int score = 0;

// ----------------------------------------------------------------------------------------------------->
//                  KEYBOARD-CONTROLS                  //

while (!Raylib.WindowShouldClose())
{
    // Skapar en array med 20 nya rektanglar (platformar) med slumpmässiga x & y-koordinater med bredd 80, höjd 20.
    // Bättre skapa en array här för då kan man kontrollera/hantera alla platformar i en loop istället för indivduellt
    // Gör det också lättare att ändra egenskaperna på platformerna och minskar mängden onödig kod.
    Rectangle[] platformer = new Rectangle[20];
    for (int i = 0; i < platformer.Length; i++)
    {
        platformer[i] = new Rectangle(Raylib.GetRandomValue(0, SkärmBredd - 80), Raylib.GetRandomValue(0, SkärmHöjd - 20), 80, 20);
    }                                    //gör att rektangeln hamnar inom ramen av spelet och inte går utanför den.
                                                            //rektangelns bredd är 80


    while (!Raylib.WindowShouldClose())
    {
        // Om spelaren klickar på vänsterpiltangenten flyttas spelaren 5 pixlar vänster
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            Spelare.x -= 5;
        }

        // Om spelaren klickar på högerpiltangenten flyttas spelaren 5 pixlar höger
        if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            Spelare.x += 5;
        }

        // Om spelaren klickar på spacebar och kan hoppa då, sätt spelarens hastighet uppåt(y) -15 (får den att hoppa)
        // KanHoppa sätts till falskt för att hindra spelaren från att hoppa igen tills de landar på marken.
        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && KanHoppa)
        {
            SpelareHastighet.Y = -15;
            KanHoppa = false;
        }

        // Om spelaren klickar på Q så byts scenen till "gameover" och spelet avslutas.
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
        {
            currentScene = "gameover";
        }

        // Öka spelarens hastighet uppåt(y) med 0.5
        SpelareHastighet.Y += 0.5f;

        // Spara spelarens nuvarande position i en variabel och uppdatera sen spelarens position genom att
        // lägga till den nya hastigheten i y-riktning.
        Rectangle SpelareInnan = Spelare;
        Spelare.y += SpelareHastighet.Y;


    // Kontrollera om Spelaren går utanför skärmens gränser och förhindra det.
    if (Spelare.x < 0)
    {
        Spelare.x = 0;
    }

    if (Spelare.x + Spelare.width > SkärmBredd)
    {
        Spelare.x = SkärmBredd - Spelare.width;
    }

        // ENTER för att gå från "gameover" skärmen" till att stänga av programmet/spelet.
        if (currentScene == "gameover")
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER))
            {
                System.Environment.Exit(1);
            }
        }

// ----------------------------------------------------------------------------------------------------->
//                  Platformer                  //

        // Sätter så att man från början inte är på en platform eller faller ovanifrån (false).
        bool PåPlatform = false;
        bool FallOvanifrån = false;


        for (int i = 0; i < platformer.Length; i++)
        {

            // Kollar om spelarens karaktär/rektangel kolliderar med en av platformarna.
            if (Raylib.CheckCollisionRecs(Spelare, platformer[i]))
            {
                // Om den kolliderar och spelarens karaktär rör sig nerråt (positiv y) 
                // betyder det att spelaren har landat på toppen av en platform.
                if (SpelareHastighet.Y > 0 && SpelareInnan.y + SpelareInnan.height <= platformer[i].y)
                {                                           // Hamna ovanför eller precis på
                    // Spelarens position är inställd på att vara precis ovanför platform
                    Spelare.y = platformer[i].y - Spelare.height;
                    // Spelarens vertikala (upp & ner) hastighet blir 0 så den inte faller och stannar på platformen.
                    SpelareHastighet.Y = 0;
                    // Kanhoppa blir sannt så spelaren kan hoppa igen (för den inte nuvarande faller/är i luften)
                    KanHoppa = true;
                    // Påplatform blir sannt då spelaren står på mark/platform
                    PåPlatform = true;
                    // Break avslutar loopen direkt/tidigt eftersom vi inte behöver kolla alla platformar 
                    // när vi redan hittat en som spelaren kollider med.
                    break;
                }

            // Den här koden ser till så att när du ramlar ner på en platform och landar på den så,
            // stannar du på platformen och inte ramlar genom den samt du kan hoppa igen.
                // OM spelarens karaktär/rektangel kolliderar med en av platformar ovanifrån
                else if (SpelareHastighet.Y < 0 && SpelareInnan.y >= platformer[i].y + platformer[i].height)
                {
                    // Spelarens position är inställd på att vara precis ovanför platform
                    Spelare.y = platformer[i].y + platformer[i].height;
                    // Spelarens vertikala (upp & ner) hastighet blir 0 så den inte faller och stannar på platformen.
                    SpelareHastighet.Y = 0;
                    // FallOvanifrån blir sannt för att spelaren inte längre faller ovanifrån
                    // Gör så att när spelaren kolliderar med en platform ovanifrån slutar den falla nerråt.
                    FallOvanifrån = true;
                    break;
                }

                // "Else" körs om spelaren kolliderar med en platform men inte uppfyller nåt av de övre kraven
                // Detta gör så att spelarens hastighet vertikalt (upp & ner) blir 1 och börjar falla pga gravitation
                // Denna kod gör simpelt sakt att spelaren faller när den inte står på en platform
                else
                {
                    SpelareHastighet.Y = 1;
                }
            }
        }

        if (FallOvanifrån)
        {
            Spelare.y += 1;
        }

        // Kollar om spelaren har nått toppen av skärmen / Vunnit
        if (Spelare.y < 0)
        {
            // Respawnar spelaren längst ner på skärmen vid startpositon.
            Spelare.y = SkärmHöjd - Spelare.height;

            // Skapa nya platformar på slumpmässig positon på skärmen som tidigare.
            for (int i = 0; i < platformer.Length; i++)
            {
                platformer[i] = new Rectangle(Raylib.GetRandomValue(0, SkärmBredd - 80), Raylib.GetRandomValue(0, SkärmHöjd), 80, 20);
            }

            // Öka poängen på scoreboarden
            score++;
        }


        // Kollar om spelaren har nått botten av skärmen
        // Om spelarens y position är mer eller lika med höjden av skärmen minus spelarens höjd så är den på botten av skärmen.
        if (Spelare.y >= SkärmHöjd - Spelare.height)
        {
            // Spelarens y position är satt till höjden av skärmen minus spelarens höjd
            Spelare.y = SkärmHöjd - Spelare.height;
            // Kanhoppa blir sannt och gör att spelaren kan hoppa igen
            KanHoppa = true;
            // PåPlatform blir sannt vilket betyder att spelaren anses stå på mark och kunna hoppa igen.
            PåPlatform = true;
        }
        // Om spelaren inte står på en platform
        else if (!PåPlatform)
        {
            // Kanhoppa blir falskt och spelaren kan inte hoppa.
            // Detta är på grund av att spelaren måste vara på mark/platform för att hoppa annars 
            // skulle dom kunna hoppa i luften för alltid.
            KanHoppa = false;
        }



// ----------------------------------------------------------------------------------------------------->
//                      Raylib customization                     // 

        // Startar Raylib-skrivprocessen, rensar bakgrunden och gör den grå, ritar blå rektangel som representerar spelaren
        Raylib.BeginDrawing();
        ClearBackground();
        Raylib.DrawRectangleRec(Spelare, Color.BLUE);

        // Ritar en mörkgrå färgad rektangel på skärmen för varje platform i platformarrayen som spelaren kan stå på
        for (int i = 0; i < platformer.Length; i++)
        {
            Raylib.DrawRectangleRec(platformer[i], Color.DARKGRAY);
        }


        // Ritar spelarens nuvarande poäng som en string i en röd färg i högra hörnet
        string scoreText = "SCORE: " + score;
        int scoreTextBredd = Raylib.MeasureText(scoreText, 20);
        // Exakt den bit av koden som ritar ut score:n. 
        Raylib.DrawText(scoreText, SkärmBredd - scoreTextBredd - 10, 10, 20, Color.WHITE);


        // Ritar ut Instruktioner hur man spelar
        Raylib.DrawText("MAKE YOUR WAY TO THE TOP TO EARN A POINT", 5, 10, 15, Color.WHITE);
        Raylib.DrawText("POINTS CAN BE SEEN ON THE SCORE COUNTER", 5, 30, 15, Color.WHITE);
        Raylib.DrawText("HOW MANY POINTS CAN YOU GET?", 5, 50, 15, Color.WHITE);
        Raylib.DrawText("TIP: YOU CAN WALK THROUGH A PLATFORM FROM THE SIDE", 5, 70, 12, Color.WHITE);
        Raylib.DrawText("'ARROW KEYS' TO MOVE, 'SPACEBAR' TO JUMP, 'ESC' FOR NEW PLATFORMS AND 'Q' TO END GAME", 5, 90, 10, Color.WHITE);


        // Om man har tryckt Q och spelet har avslutats
        if (currentScene == "gameover")
        {
            // Ta bort från bakgrunden och färga den grå.
            ClearBackground();


            // Gör texten, spelarens karaktär och platformarna som fanns under spelets gång 
            // blir onsynliga för "gameover" scenen.
            for (int i = 0; i < platformer.Length; i++)
            {
                Raylib.DrawRectangleRec(platformer[i], Color.RED);
                Raylib.DrawRectangleRec(Spelare, Color.RED);
                Raylib.DrawText(scoreText, SkärmBredd - scoreTextBredd - 10, 10, 20, Color.RED);
                Raylib.DrawText("MAKE YOUR WAY TO THE TOP TO EARN A POINT", 5, 10, 15, Color.RED);
                Raylib.DrawText("POINTS CAN BE SEEN ON THE SCORE COUNTER", 5, 30, 15, Color.RED);
                Raylib.DrawText("HOW MANY POINTS CAN YOU GET?", 5, 50, 15, Color.RED);
                Raylib.DrawText("TIP: YOU CAN WALK THROUGH A PLATFORM FROM THE SIDE", 5, 70, 12, Color.RED);
                Raylib.DrawText("'ARROW KEYS' TO MOVE, 'SPACEBAR' TO JUMP, 'ESC' FOR NEW PLATFORMS AND 'Q' TO END GAME", 5, 90, 10, Color.RED);
            }


            // Ritar ut på skärmen text som säger "Game over".
            Raylib.DrawText("GAME OVER!", 110, 100, 90, Color.BLACK);
            // Ritar ut under det vilken "Score" man fick under spelets gång.
            Raylib.DrawText(scoreText, SkärmBredd - scoreTextBredd - 450, 300, 60, Color.BLACK);
            // Ritar ut instruktioner hur man stänger av spelet.
            Raylib.DrawText("\nENTER to exit", 285, 320, 32, Color.BLACK);
        }





        Raylib.EndDrawing();
    }
}

// ----------------------------------------------------------------------------------------------------->
//                  PSEUDO-KOD                  //

// Importera Raylib_cs
// Sätt SkärmBredden och höjden av spelet och startscenen till "mainspel"
// Starta spel-fönstret

// Skapa rektangel för spelaren och sätt spelbara karaktärens möjlighet att hoppa sannt 
// Skapa massvis med platformer och ge dom slumpmässiga positioner på skärmen.
// Sätt start-score till 0

// Spelaren rör sig med WASD
// Spelaren kan hoppa upp på platformar med Spacebar
// Spelaren faller nerråt om inte står på något

// När spelaren når toppen av skärmen, teleportera spelaren ner till botten igen och slumpa nya platformar
// Om spelaren trycker på Q så avslutas spelet och byter scen till "gameover"
// Om spelaren trycker på ENTER i "gameover" scen avslutas hela programmet och stängs av.