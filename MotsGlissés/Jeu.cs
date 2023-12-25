using System.Reflection;
using System.Text;

namespace MOTS_GLISSES
{

    internal sealed class Jeu
    {

        private static void Main()
        {
            _ = new Jeu();
        }
        /// La liste des joueurs en jeu
        List<Joueur> joueurs = new();
        // Le plateau de jeu
        Plateau plateau = new();
        PropriétésDeJeu pdj = new();

        PropertyInfo[]? pinfo = null;


        #region PDJ


        private static int CurrentPlayerTurnTime
        {
            get => PropriétésDeJeu.CurrentPlayerTurnTime;
            set => PropriétésDeJeu.CurrentPlayerTurnTime = value;
        }

        private static int MaxPlayerTurnTime
        {
            get => PropriétésDeJeu.MaxPlayerTurnTime;
            set => PropriétésDeJeu.MaxPlayerTurnTime = value;
        }

        private static int TempsRestant
        {
            get => PropriétésDeJeu.TempsRestant;
            set => PropriétésDeJeu.TempsRestant = value;
        }

        private static int TempsBase
        {
            get => PropriétésDeJeu.TempsBase;
            set => PropriétésDeJeu.TempsBase = value;
        }

        private static string GameMode
        {
            get => PropriétésDeJeu.GameMode;
            set => PropriétésDeJeu.GameMode = value;
        }

        private static bool Debug
        {
            get => PropriétésDeJeu.Debug;
            set => PropriétésDeJeu.Debug = value;
        }

        private static bool AutoriserDiagonales
        {
            get => PropriétésDeJeu.AutoriserDiagonales;
            set => PropriétésDeJeu.AutoriserDiagonales = value;
        }

        private static int AQuiLeTour
        {
            get => PropriétésDeJeu.AQuiLeTour;
            set => PropriétésDeJeu.AQuiLeTour = value;
        }

        private static int? Gagnant
        {
            get => PropriétésDeJeu.Gagnant;
            set => PropriétésDeJeu.Gagnant = value;
        }

        private static string? RaisonDeGagnage
        {
            get => PropriétésDeJeu.RaisonDeGagnage;
            set => PropriétésDeJeu.RaisonDeGagnage = value;
        }

        private static List<int> PlayersThatGaveUp
        {
            get => PropriétésDeJeu.PlayersThatGaveUp;
            set => PropriétésDeJeu.PlayersThatGaveUp = value;
        }

        private static List<int> PlayersThatPassed
        {
            get => PropriétésDeJeu.PlayersThatPassed;
            set => PropriétésDeJeu.PlayersThatPassed = value;
        }

        private static Dictionary<string, string> Helper
        {
            get => PropriétésDeJeu.helper;
            set => PropriétésDeJeu.helper = value;
        }
        #endregion PDJ


        // Stockage des variables d'erreur et de statut du jeu, Peut-être sous forme "clé:valeur" dans la liste ? "winner:1" ou "score1:69" ou "err:pasdeplages"
        // le truc c'est que si on prend un dictoinnaire (la structure plus adaptée pour clé:valeur) on ne peut pas avoir plusieurs clefs avec le même nom

        Dictionary<string, string> gameState = new();
        //List<string> gameState = new();





        public Jeu(List<Joueur>? joueurs = null, Plateau? plateau = null)
        {


            if (joueurs != null && joueurs.Count != 0)
            { this.joueurs = joueurs; }

            if (plateau != null)
            {
                this.plateau = plateau;
            }
            this.plateau.CreateNewGame();

            Task backgroundTask = Task.Run(() => Dictionnaire.Init());

            Console.CancelKeyPress += (sender, e) =>
        {
            // Your code to send a message can go here
            Console.WriteLine("\nAu revoir !");
            Console.WriteLine();

        };

            //backgroundTask.Wait();
            try
            {
                MenuPrincipal();
            }
            catch (System.Exception problem)
            {
                Console.WriteLine("Aïe, un problème est survenu: ");
                Console.WriteLine(problem);
                Console.WriteLine("Vous pouvez reporter l'erreur en ouvrant une issue sur le github");

            }

        }


        public string MenuPrincipal()
        {
            Console.Title = "Mots Glissés";
            Console.Clear();
            if (Console.WindowWidth < 87)
            {
                Console.WriteLine("Pour mieux profiter du jeu, élargissez assez la fenêtre pour que ce message tienne sur une ligne !");
            }
            Console.WriteLine(@"███╗   ███╗ ██████╗ ████████╗███████╗               
████╗ ████║██╔═══██╗╚══██╔══╝██╔════╝               
██╔████╔██║██║   ██║   ██║   ███████╗               
██║╚██╔╝██║██║   ██║   ██║   ╚════██║               
██║ ╚═╝ ██║╚██████╔╝   ██║   ███████║            
╚═╝     ╚═╝ ╚═════╝    ╚═╝   ╚══════╝  ▄█╗          
                                     ▄█╔═╝               
 ██████╗ ██╗     ██╗███████╗███████╗███████╗███████╗
██╔════╝ ██║     ██║██╔════╝██╔════╝██╔════╝██╔════╝
██║  ███╗██║     ██║███████╗███████╗█████╗  ███████╗
██║   ██║██║     ██║╚════██║╚════██║██╔══╝  ╚════██║
╚██████╔╝███████╗██║███████║███████║███████╗███████║
 ╚═════╝ ╚══════╝╚═╝╚══════╝╚══════╝╚══════╝╚══════╝");
            Console.WriteLine($"{magentaText("© autodistries, infarctus, 2023 - ")}{greenText("license")}{magentaText(" GNU GPLv3")}");

            Console.Write($"Bienvenue. Faites {greenText("help")} pour de l'aide, ou {greenText("qs")} pour sauter toute configuration.\nEntrez une commande: ");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() + "";
                input = input.Trim();
                string[] splitInput = input.Split(" ");
                // d'abodrd épandre la commande, puis traiter les one-liners, puis le reste sur un switch
                if (splitInput.Length == 0) splitInput[0] = "unknown";
                switch (splitInput[0].ToLower())
                { // normaliser les arguments d'entrée
                    case "a":
                    case "h":
                    case "aide":
                    case "help":
                        {
                            bool all = false; // Si 'all' est entré, afficher les alias en plus
                            if (!(splitInput.Length == 1) && (splitInput[1] == "all" || splitInput[1] == "a"))
                            {
                                all = true;
                            }

                            string res = "Commandes possibles:\n";


                            // qs command
                            res += $"  \u001b[32mqs[1-5]  {((all) ? "   " : "")} \u001b[0m : Quick start, crée entre 1 et 5 joueurs et démarre une partie\n";

                            // joueurs command
                            res += $"  \u001b[32mjoueurs {((all) ? "| j" : "")}  \u001b[0m : Liste les joueurs\n";
                            res += $"    ... \u001b[36majouter{((all) ? " | a" : "")}\u001b[0m <nom> : Ajoute un nouveau joueur\n";
                            res += $"    ... \u001b[36menlever{((all) ? " | e" : "")}\u001b[0m <nom> : Enlève un joueur existant\n";

                            // commencer command
                            res += $"  \u001b[32mcommencer{((all) ? "| c" : "")}\u001b[0m  : Débute la partie\n";

                            // plateau command
                            res += $"  \u001b[32mplateau {((all) ? "| p" : "")}\u001b[0m   : Affiche le plateau\n";
                            res += $"    ... \u001b[36mregen{((all) ? "  | r " : "")} \u001b[0m             : Regénère un nouveau plateau\n";
                            res += $"    ... \u001b[36mimporter{((all) ? "   | i" : "")}\u001b[0m  [chemin] : Importe un plateau existant depuis ./Plateau.csv ou [chemin]\n";
                            res += $"    ... \u001b[36mexporter{((all) ? "   | e" : "")} \u001b[0m [chemin] : Exporte le plateau actuel vers ./Plateau.csv ou [chemin]\n";




                            // paramètres command
                            res += $"  \u001b[32mparametres\u001b[0m {((all) ? "| param" : "")}: Affiche les paramètres du jeu\n";
                            res += $"    ... \u001b[36mget {((all) ? " | g" : "")}\u001b[0m <param>          : Obtient la valeur et des infos d'un paramètre\n";
                            res += $"    ... \u001b[36mset {((all) ? " | s" : "")}\u001b[0m <param> <newVal> : Modifie la valeur d'un paramètre\n";

                            // score command
                            res += $"  \u001b[32mscore   {((all) ? "   " : "")} \u001b[0m  : Affiche le tableau des scores\n";


                            // clear command
                            res += $"  \u001b[32mclear {((all) ? "| new" : "  ")} \u001b[0m  : Réinitialise le jeu avec un nouveau plateau et sans joueurs\n";
                            // règles command
                            res += $"  \u001b[32mrègles {((all) ? "| r" : "")}  \u001b[0m  : Affiche le but du jeu\n";
                            // license command
                            res += $"  \u001b[32mlicense   {((all) ? "   " : "")}\u001b[0m : Affiche les crédits du jeu (Non implémenté)\n";

                            // quitter command
                            res += $"  \u001b[32mquitter {((all) ? "| q" : "")} \u001b[0m  : Quitte le programme\n";

                            res += $"  \u001b[32mhelp {((all) ? "| h" : "")}\u001b[0m      : Vous êtes ici\n";
                            res += $"    ... \u001b[36mall {(all ? "| a" : "")}\u001b[0m      : Affiche des alias pour les commandes\n";




                            Console.WriteLine(res);
                            continue;
                        }

                    case "qs":
                    case "qs1":
                        {
                            AddPlayer(new Joueur("Joueur 1"));
                            StartGame();
                            continue;
                        }

                    case "qs2":
                        {
                            AddPlayer(new Joueur("Joueur 1"));
                            AddPlayer(new Joueur("Joueur 2"));
                            StartGame();
                            continue;
                        }
                    case "qs3":
                        {
                            AddPlayer(new Joueur("Joueur 1"));
                            AddPlayer(new Joueur("Joueur 2"));
                            AddPlayer(new Joueur("Joueur 3"));
                            StartGame();
                            continue;
                        }
                    case "qs4":
                        {
                            AddPlayer(new Joueur("Joueur 1"));
                            AddPlayer(new Joueur("Joueur 2"));
                            AddPlayer(new Joueur("Joueur 3"));
                            AddPlayer(new Joueur("Joueur 4"));
                            StartGame();
                            continue;
                        }
                    case "qs5":
                        {
                            AddPlayer(new Joueur("Joueur 1"));
                            AddPlayer(new Joueur("Joueur 2"));
                            AddPlayer(new Joueur("Joueur 3"));
                            AddPlayer(new Joueur("Joueur 4"));
                            AddPlayer(new Joueur("Joueur 5"));
                            StartGame();
                            continue;
                        }


                    case "j":
                    case "joueur":
                    case "joueurs":
                    case "players":
                    case "player":
                        {
                            if (splitInput.Length == 1 || splitInput[1] == "voir" || splitInput[1] == "v")
                            {
                                //affichage joueurs
                                if (joueurs.Count != 0)
                                {
                                    Console.Write("Joueurs en jeu :");
                                    foreach (Joueur them in joueurs)
                                    {
                                        Console.Write(them.Nom + "; ");
                                    }
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.WriteLine("Aucun joueur n'est dans la partie.\nFaites 'joueurs ajouter' pour en ajouter.");
                                }

                                continue;
                            }
                            if ((new string[] { "a", "ajouter", "add", "new", "n" }).Any((them) => them == splitInput[1]) && splitInput.Length > 2)
                            {

                                string jinput = string.Join(" ", splitInput.Skip(2));
                                string? userResult = AddPlayer(new Joueur(jinput));
                                if (userResult != null)
                                {
                                    Console.WriteLine("Nom du joueur modifié en " + userResult + " car déjà existant ou vide.");
                                }

                                Console.Write("Liste de joueurs mise à jour: ");
                                foreach (Joueur them in joueurs)
                                {
                                    Console.Write(them.Nom + "; ");
                                }
                                Console.WriteLine();


                            }
                            else if ((new string[] { "r", "remove", "enlever", "e" }).Any((them) => them == splitInput[1]) && splitInput.Length > 2)
                            {
                                string jinput = string.Join(" ", splitInput.Skip(2));
                                bool userResult = RemovePlayer(jinput);
                                if (userResult)
                                {
                                    Console.Write("Liste de joueurs mise à jour: ");
                                    foreach (Joueur them in joueurs)
                                    {
                                        Console.Write(them.Nom + "; ");
                                    }
                                    Console.WriteLine();
                                }
                                else Console.WriteLine("Pas de joueur correspondant trouvé");
                            }
                            else
                            {
                                Console.WriteLine($"Usage: {greenText("joueurs")} [{cyanText("<ajouter | enlever>")} <nom du joueur>]'");
                            }
                            continue;

                        }


                    case "c":
                    case "commencer":
                    case "start":
                    case "s":
                        {//start game
                            StartGame();
                            continue;
                        }

                    case "q":
                    case "quit":
                    case "quitter":
                    case "exit":
                    case "x":
                        {
                            Console.WriteLine("Au revoir !");
                            Environment.Exit(0);
                            continue;
                        }

                    case "r":
                    case "règles":
                    case "regles":
                    case "but":
                        {
                            Console.WriteLine(@$"Le jeu est très simple: chacun leur tour, chaque joueur doit trouver et entrer un mot valide dans la grille.
Un mot est considéré valide si il commence par la ligne du bas. Chacune des lettres du mot doit avoisinner la lettre précédente; les diagonales peuvent être désactivées avec {greenText("param")} {greenText("set")} AutoriserDiagonales false
Si vous trouvez un mot valide, toutes les lettres concernées disparaissent. Les lettres au dessus remplissent les trous, et vous gagnez des points.
Il y a deux modes de jeu;
Contre-la-montre (par défaut), où le jeu s'arrête lorsque tous les joueurs ne réussissent pas à jouer dans le temps imparti, ou si le temps attitré à la partie est dépassé. Le joueur ayant le plus de points gagne la partie !
Arcade, où le plateau est regénéré lorsque les joueurs le veulent. Ce mode n'est pas compétitif et ne se terminera pas automatiquement.
");
                            continue;
                        }



                    case "paramètres":
                    case "parametres":
                    case "param":
                        {
                            pinfo ??= typeof(PropriétésDeJeu).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                            // not internal bc we wont edit this. also this is best for handling input types
                            if (Debug == true) pinfo = typeof(PropriétésDeJeu).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

                            if (splitInput.Length == 1 || splitInput[1] == "voir" || splitInput[1] == "v" || splitInput[1] == "list" || splitInput[1] == "lister")
                            {
                                Console.WriteLine("Paramètres de jeu:");
                                var properties = pinfo;


                                foreach (var property in properties)
                                {
                                    Console.WriteLine($"{property.Name}: {property.GetValue(pdj)}");
                                }



                                continue;
                            } //là ca devient compliqué
                            else if ((new string[] { "get", "g", "r", "read" }).Any((them) => them == splitInput[1]) && splitInput.Length == 3)
                            {
                                if (pinfo.Any((ppt) =>
                                {
                                    if (ppt.Name == splitInput[2])
                                    {
                                        Console.WriteLine("Clef:" + splitInput[2] + ", valeur:" + ppt.GetValue(pdj));
                                        Console.WriteLine(Helper[splitInput[2]]);
                                        return true;
                                    }
                                    else return false;
                                })) { }
                                else
                                {
                                    Console.WriteLine("Clef pas trouvée, faites '" + splitInput[0] + "' pour afficher tous les paramètres disponibles.");
                                }
                            }
                            if ((new string[] { "set", "s", "c", "change" }).Any((them) => them == splitInput[1]) && splitInput.Length > 3)
                            {
                                if (pinfo.Any((ppt) =>
                                {
                                    if (ppt.Name == splitInput[2])
                                    {
                                        Console.WriteLine("Clef:" + splitInput[2] + ", valeur:" + ppt.GetValue(pdj) + "->" + splitInput[3]);
                                        object nval = ConvertToWhateverIsTheBest(splitInput[3] + "");
                                        if (nval.GetType() == ppt.PropertyType)
                                        {
                                            if (ppt.Name == "AutoriserDiagonales") { plateau.DiagonalesOk = (bool)nval; }
                                            ppt.SetValue(pdj, nval);
                                        }
                                        else Console.WriteLine($"Impossible de convertir {splitInput[3]} (de type {nval.GetType()}) en type {ppt.PropertyType}. {splitInput[2]} n'a pas été modifié.");
                                        return true;
                                    }
                                    else return false;
                                })) { }

                                else
                                {
                                    Console.WriteLine("Clef pas trouvée, faites '" + splitInput[0] + "' pour afficher tous les paramètres disponibles.");
                                }


                            }
                            // avec ou sans diagonales
                            //conditions de victoire: maxTUrnTIme, maxIgTime, pointsToWin
                            //paramètres
                            // debug mode, langue, timeout, taille de la grille
                            continue;
                        }

                    case "p":
                    case "plateau":
                    case "plt":
                        {
                            if (splitInput.Length == 1 || splitInput[1] == "voir" || splitInput[1] == "v")
                            {
                                Console.WriteLine("Futur plateau de jeu: ");
                                plateau.AfficherPlateau();
                            }
                            else if ((new string[] { "r", "regénérer", "regénerer", "regenérer", "n", "new", "regen" }).Any((them) => them == splitInput[1]) && splitInput.Length == 2)
                            {

                                plateau.CreateNewGame();
                                Console.WriteLine("Nouveau plateau de jeu: ");
                                plateau.AfficherPlateau();
                            }
                            else if ((new string[] { "i", "importer", "import" }).Any((them) => them == splitInput[1]))
                            {


  if (splitInput.Length == 2)
                                {
                                    if (File.Exists("./plateau.csv")) { plateau = new("./plateau.csv"); } else { plateau = new("../../../plateau.csv"); }

                                }
                               
                                else plateau = new(string.Join(" ", splitInput.Skip(2)));
                                Console.WriteLine("Nouveau plateau:");
                                plateau.AfficherPlateau();
                                continue;
                            }
                            else if ((new string[] { "e", "exporter", "export", "s", "save" }).Any((them) => them == splitInput[1]))
                            {
                               if (splitInput.Length == 2)
                                {
                                    if (File.Exists("./plateau.csv")) { plateau.ToFile("./plateau.csv"); } else { plateau.ToFile("../../../plateau.csv"); }

                                }
                                else plateau.ToFile(string.Join(" ", splitInput.Skip(2))); // fonctionne même avec des espaces !

                                continue;
                            }
                            else
                            {
                                Console.WriteLine($"Usage: {greenText("plateau")} {cyanText("[<regénérer | importer | exporter>]")}");
                            }
                            continue;

                        }

                    case "score":
                        {
                            AfficherTableauScores();
                            continue;
                        }



                    case "license":
                        {
                            string tp = "./gpl-3.0.txt";
                            if (!System.IO.File.Exists(tp))
                            {
                                tp = "../../../gpl-3.0.txt";
                                if (!System.IO.File.Exists(tp))
                                {
                                    Console.WriteLine("License pas trouvée; Récupérez une copie de la license à https://www.gnu.org/licenses/gpl-3.0.txt");
                                    break;
                                }
                            }
                            string[] fileData = File.ReadAllLines(tp);
                            foreach (string s in fileData)
                            {
                                Console.WriteLine(s);
                            }
                            break;


                        }

                    case "clear":
                    case "new":
                        {
                            joueurs = new();
                            plateau.CreateNewGame();
                            if (Debug) plateau.DebugAvailableCharacters();
                            Console.WriteLine("Plateau et joueurs réinitialisés.");
                            continue;
                        }



                    default: { Console.WriteLine("Commande non reconnue ! Essayez " + greenText("help") + " pour obtenir de l'aide."); continue; }
                }




            }
        }

        private static object ConvertToWhateverIsTheBest(string input)
        {

            if (bool.TryParse(input, out bool boolValue))
            {

                return boolValue;

            }

            else if (int.TryParse(input, out int outValue))
            {
                return outValue;
            }
            else
            {

                return input;

            }
        }

        private void AfficherTableauScores()
        {
            Console.WriteLine("Tableau des scores:");
            //tableau des scores
            int c1size = 4;
            int c2size = 6;
            int c3size = 6;

            foreach (Joueur j in joueurs)
            {
                if (j.Nom.Length > c1size)
                {
                    c1size = j.Nom.Length;
                }
                if (j.Score.ToString().Length > c2size)
                {
                    c2size = j.Score.ToString().Length;
                }
                if (j.ScoresPassés.Sum().ToString().Length > c3size)
                {
                    c3size = j.ScoresPassés.Sum().ToString().Length;
                }
            }

            string preres = "";
            string pretres = "Nom ";
            while (pretres.Length < c1size)
            {
                pretres += " ";
            }
            preres += pretres + "| ";
            pretres = "Score ";
            while (pretres.Length < c2size)
            {
                pretres += " ";
            }
            preres += pretres + "| ";
            pretres = "Total ";
            while (pretres.Length < c3size)
            {
                pretres += " ";
            }
            preres += pretres + "| ";
            preres += "Mots";
            Console.WriteLine(preres);


            foreach (Joueur j in joueurs.OrderByDescending((player) => player.Score))
            {
                string res = "";
                string tres = j.Nom;
                while (tres.Length < c1size)
                {
                    tres += " ";
                }
                res += tres + "| ";
                tres = j.Score.ToString();
                while (tres.Length < c2size)
                {
                    tres += " ";
                }
                res += tres + "| ";
                tres = j.ScoresPassés.Sum().ToString();
                while (tres.Length < c3size)
                {
                    tres += " ";
                }
                res += tres + "| ";
                res += j.Mots_trouvésString();
                Console.WriteLine(res);
            };
            //nom score mots trouvés
        }

        private bool RemovePlayer(string jinput)
        {
            int prevlen = joueurs.Count;
            joueurs.RemoveAll((joueur) => joueur.Nom == jinput);
            if (prevlen == joueurs.Count)
            {
                return false;
            }
            return true;
        }

        public void StartGame()
        {
            if (plateau.distrubutedcharacters == false)
            {
                Console.WriteLine(redText("Le plateau n'a pas pu être correctement généré ou importé.") + $"\nVeuillez éditer Lettre.txt et faites la commandes {greenText("p r")} ou importez un plateau correct.");
                return;
            }
            if (joueurs.Count == 0)
            {
                Console.WriteLine(redText("Impossible de démarrer le jeu sans joueurs.") + $" Faites {greenText("aide")} pour obtenir de l'aide.");
                return;
            }
            if (GameMode != "arcade" && GameMode != "temps")
            {
                Console.WriteLine(redText("Mode de jeu invalide.") + " Essayez 'arcade' ou 'temps'.");
                return;
            }
            if (MaxPlayerTurnTime <= 0 || TempsBase <= 0)
            {
                Console.WriteLine(redText($"Les paramètres de jeu liés au temps sont invalides.") + $" Faites {greenText("param")} {greenText("get")} <Nom de paramètre> pour obtenir des informations sur celui-ci.");
                return;

            }
            if (plateau.QtyOfEmptys(plateau.Tileschar) > 0.7)
            {
                Console.WriteLine("Le plateau pourrait être mal formé; quittez le jeusi besoin, vérifiez votre fichier d'import et/ou regénérez un plateau.");
            }
            TempsRestant = TempsBase;

            //initialisation des variables de jeu
            Console.Clear();
            Console.Write(joueurs[0].Nom);
            bool solo = true;
            foreach (Joueur j in joueurs.Skip(1))
            {
                Console.Write(" vs " + j.Nom);
                solo = false;
            }
            if (solo) Console.Write(" (solo)");
            Console.WriteLine(": que le jeu débute !");
            if (GameMode == "arcade") Console.WriteLine($@"{blueText("Mode arcade !")} Vous n'êtes pas limité{((joueurs.Count == 1) ? "" : "s")} par le temps.
Vous pouvez changer de plateau en écrivant {greenText("pass")}, ou terminer le jeu avec {greenText("ff")}. ");
            else Console.WriteLine($@"Mode contre la montre ! Chacun à {MaxPlayerTurnTime}s pour trouver un mot, ou il ne pourra plus jouer de la partie.
Vous pouvez abandonner en entrant {greenText("ff")}. Qui à le plus de points à la fin du temps gagne !
Attention, la partie se finira dans " + redText($"{TempsBase}s") + " !");
            Console.WriteLine();
            plateau.AfficherPlateau();



            /// main game loop
            while (Gagnant == null)
            {
                NextTurn();
            }

            if (RaisonDeGagnage == "pointstemps")
            {

                Console.WriteLine("Bravo à " + joueurs[(int)Gagnant].Nom + ", qui gagne " + ((joueurs.Count == 1) ? "sa" : "la") + " partie avec un score de " + joueurs[(int)Gagnant].Score);

            }
            else if (Gagnant != -1)
            {
                Console.WriteLine($"Bravo à {joueurs[(int)Gagnant].Nom} pour ton score de " + joueurs[(int)Gagnant].Score + " !");
            }

            foreach (Joueur j in joueurs)
            {
                j.AddScoreToHistory();
            }
            AfficherTableauScores();
            foreach (Joueur j in joueurs)
            {
                j.Score = 0; //reset running score;
                j.Reset_Mots();
            }


            // reset all game variables
            Gagnant = null;
            RaisonDeGagnage = null;
            PlayersThatGaveUp.Clear();
            PlayersThatPassed.Clear();
            CurrentPlayerTurnTime = -1;
            AQuiLeTour = 0;




            Console.WriteLine("Tapez sur n'importe quelle touche pour revenir au menu principal...");
            Task.Delay(500).Wait();
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine($"Menu Principal - faites {greenText("help")} pour de l'aide");
            plateau.CreateNewGame();
            if (Debug) DebugGameState();






        }

        // Ajoute un joueur à la liste.
        // Si le nom existe déja ou est vide, modification automatique et return du nouveau nom
        // Si tout va bien return null;
        public string? AddPlayer(Joueur joueur)
        {
            string? ret = null; ;
            if (joueur.Nom == "" || joueurs.Any((exjoueur) => exjoueur.Nom == joueur.Nom))
            {
                joueur.Nom = "Joueur " + (joueurs.Count + 1);
                ret = joueur.Nom;
            }
            joueurs!.Add(joueur);
            return ret;
        }

        /// <summary>
        /// return value for in case error so that while loop can end up escaping maybe
        /// if not return value can be voided
        /// </summary>
        /// <returns></returns>
        private bool NextTurn()
        {

            if (Debug) DebugGameState();


            if (PlayersThatPassed.Count == joueurs.Count)
            { //this can only happen in arcade mode

                Console.WriteLine("Tout le monde a passé son tour, on bascule vers un nouveau plateau !");
                plateau.CreateNewGame();
                AfficherTableauScores();
                AQuiLeTour = (AQuiLeTour + 1) % joueurs.Count;
                PlayersThatPassed.Clear();
                plateau.AfficherPlateau();
                return NextTurn();
            }




            if (PlayersThatGaveUp.Count == joueurs.Count)
            {
                if (GameMode == "arcade")
                {
                    Console.WriteLine("Tout le monde veut terminer la partie !");
                    Gagnant = -1;
                    return true;

                }
                else if (GameMode == "temps")
                {
                    Gagnant = GetWinnerFromPoints();
                    RaisonDeGagnage = "pointstemps";
                    return true;
                }
                //en mode arcade, continuer, regénérer un tableau sans interrompre
            }
            Joueur currentJoueur = joueurs[AQuiLeTour];
            if (PlayersThatGaveUp.Any(it => it == AQuiLeTour))
            {
                //pass turn
                AQuiLeTour = (AQuiLeTour + 1) % joueurs.Count;
                return NextTurn();
            }

            //check if all but we have surrendered only if last one did surrender


            Console.Title = "Mots Glissés | Temps restant : " + "";
            Console.Write(currentJoueur.Nom + ", à toi le tour !");
            if (GameMode == "temps") Console.Write($" Tu as " + redText($"{((CurrentPlayerTurnTime == -1) ? MaxPlayerTurnTime : CurrentPlayerTurnTime / 1000)}s") + " pour trouver un mot.");

            if (plateau.GetExistingLetters() < 20)
            {
                Console.Write($"\n(Tip: si tu es bloqué, entre {((GameMode == "arcade") ? $"{greenText("pass")} pour passer" : $"{greenText("f")} pour abandonner)")}");
            }
            Console.Write(" :");
            string userInput = GetUserWordInput();
            if (TempsRestant < 0)
            {
                Console.WriteLine(redText("\nLe temps alloué à la partie est écoulé !"));
                Gagnant = GetWinnerFromPoints();
                RaisonDeGagnage = "pointstemps";
                return true;
            }

            if (userInput == "ERRTIMEOUT")
            {
                Console.WriteLine($"Aïe, {redText("ton délai est dépassé")} ! Tu ne peux plus jouer sur ce plateau.");
                PlayersThatGaveUp.Add(AQuiLeTour);
                AQuiLeTour = (AQuiLeTour + 1) % joueurs.Count;
                return NextTurn();

            }
            if (Debug && (userInput == "CHEAT" || userInput == "TRICHE"))
            { //Pour gagner plus vite en debug
                Gagnant = AQuiLeTour;
                RaisonDeGagnage = "triche";
                return true;
            }
            if (userInput == "PASS")
            {
                if (GameMode == "arcade")
                {
                    PlayersThatPassed.Add(AQuiLeTour);
                    AQuiLeTour = (AQuiLeTour + 1) % joueurs.Count;
                    return NextTurn();
                }
                else
                {
                    userInput = "ERUIIRJ";
                }
            }

            if (userInput == "FF")
            {
                Console.WriteLine("Abandon de " + currentJoueur.Nom);
                if (joueurs.Count == 1)
                {
                    Gagnant = 0;
                    RaisonDeGagnage = "solo";
                    return true;
                }
                else
                {
                    PlayersThatGaveUp.Add(AQuiLeTour);
                }

                return NextTurn();
            }

            if (currentJoueur.Contient(userInput))
            {
                Console.WriteLine(redText("Impossible d'utiliser un mot que tu as déjà trouvé !'"));
                //peut etre a ne pas check en arcade

                return NextTurn(); //same player
            }
            if (!Dictionnaire.AppelleRechercheDicho(userInput)) //ne pas check en debug
            {
                Console.WriteLine(redText("Impossible de trouver ce mot dans le dictionnaire !"));

                return NextTurn(); //same player
            }
            (bool, List<int[]>?) existsInPlateau = plateau.Recherche_Mot(userInput);
            //Console.WriteLine("diags " + plateau.DiagonalesOk);
            if (!existsInPlateau.Item1)
            {
                Console.WriteLine(redText("Impossible de trouver ce mot dans le plateau !"));

                return NextTurn(); //same player
            }
            CurrentPlayerTurnTime = -1;



            // Console.WriteLine("debug userinput: " + userInput);
            plateau.EnleverCaractèresÀ(existsInPlateau.Item2!);
            currentJoueur.Add_Mot(userInput);
            int score1 = plateau.GetWordScore(userInput);
            currentJoueur.Add_Score(score1);
            Console.WriteLine(currentJoueur.Nom + ": " + greenText(userInput) + " +" + score1 + " points -> " + currentJoueur.Score + " points");




            AQuiLeTour = (AQuiLeTour + 1) % joueurs.Count;





            return true;
        }

        private int GetWinnerFromPoints()
        {
            return joueurs.IndexOf(joueurs.OrderByDescending((player) => player.Score).First());
        }



        /// <summary>
        /// prints ech value from gameState on a single line
        /// </summary>
        public void DebugGameState()
        {
            var pinfo = typeof(PropriétésDeJeu).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            var properties = pinfo;


            foreach (var property in properties)
            {
                Console.WriteLine($"{property.Name}: {property.GetValue(pdj)}");
            }





        }

        /// <summary>
        /// safe user input
        /// </summary>
        /// <returns></returns>
        private static string GetUserWordInput()
        {
            DateTime startWordInput = DateTime.Now;
            bool again = false;
            string userInput;
            //await user input
            do
            {
                if (again) Console.Write($"Aïe, {redText("ce mot est invalide")} ! Essaie encore :");
                var ui = (GameMode == "arcade") ? ReadLineWithTimeout(-1) : ReadLineWithTimeout(MaxPlayerTurnTime * 1000);

                string returnreadline = ui ?? "ERRTIMEOUT";
                userInput = (returnreadline + "").ToUpper();
                foreach (char c in userInput)
                {
                    if ((int)c < 65 || (int)c > 90)
                    {// characters are not between A and z
                        userInput = "";
                    }
                }
                if (userInput.Length < Dictionnaire.Longueurminmot)
                {
                    userInput = "";
                }
                again = true;
            } while (userInput == "");
            DateTime endWordInput = DateTime.Now;
            TimeSpan elapsedTime = endWordInput - startWordInput;
            //elapsedTime.TotalSeconds
            return userInput;
        }

        static string? ReadLineWithTimeout(int timeoutMilliseconds)
        {
            if (Debug) Console.WriteLine("User has " + ((CurrentPlayerTurnTime == -1) ? timeoutMilliseconds : CurrentPlayerTurnTime) + " ms to complete input");
            if (timeoutMilliseconds == -1 || GameMode == "arcade") timeoutMilliseconds = 50000000; //(almost) infinite time
            StringBuilder inputBuffer = new();
            int ttc = 0;

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < ((CurrentPlayerTurnTime == -1) ? timeoutMilliseconds : CurrentPlayerTurnTime) && TempsRestant >= 0)
            {


                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(intercept: false);

                    if (key.KeyChar == '\r' || key.KeyChar == '\n')
                    {
                        Console.WriteLine(); // skip la ligne
                        CurrentPlayerTurnTime = (int)(timeoutMilliseconds - (DateTime.Now - startTime).TotalMilliseconds); //save in case not ok
                        return inputBuffer.ToString();
                    }
                    else
                    {
                        if (key.Key != ConsoleKey.Backspace)
                        {

                            inputBuffer.Append(key.KeyChar);
                        }
                        else
                        {
                            if (inputBuffer.Length != 0)
                            {
                                inputBuffer.Remove(inputBuffer.Length - 1, 1);
                                Console.Write("\b \b");
                            }
                        }
                    }
                }

                Task.Delay(100).Wait(); // Sleep for a short duration to avoid high CPU usage
                ttc += 1;
                ttc %= 10;
                if (ttc == 0 && GameMode == "temps") TempsRestant -= 1;
            }
            CurrentPlayerTurnTime = -1;

            return null;
        }

        static string redText(string txt)
        {
            return "\u001b[31m" + txt + "\u001b[0m";
        }
        static string greenText(string txt)
        {
            return "\u001b[32m" + txt + "\u001b[0m";
        }
        static string yellowText(string txt)
        {
            return "\u001b[33m" + txt + "\u001b[0m";
        }
        static string blueText(string txt)
        {
            return "\u001b[34m" + txt + "\u001b[0m";
        }
        static string magentaText(string txt)
        {
            return "\u001b[35m" + txt + "\u001b[0m";
        }
        static string cyanText(string txt)
        {
            return "\u001b[36m" + txt + "\u001b[0m";
        }
    }
}
