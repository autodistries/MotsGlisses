namespace MOTS_GLISSES
{
    internal sealed class PropriétésDeJeu
    {
        internal static int CurrentPlayerTurnTime { get; set; } = -1;
        public static int MaxPlayerTurnTime { get; set; } = 15;
        internal static int TempsRestant { get; set; } = 120;
        public static int TempsBase { get; set; } = 120; // t en s
        public static string GameMode { get; set; } = "temps";
        public static bool Debug { get; set; } = false;
        public static bool AutoriserDiagonales { get; set; } = true;
        internal static int AQuiLeTour { get; set; } = 0;
        internal static int? Gagnant { get; set; } = null;
        internal static string? RaisonDeGagnage { get; set; } = null;
        internal static List<int> PlayersThatGaveUp { get; set; } = new List<int>();
        internal static List<int> PlayersThatPassed { get; set; } = new List<int>();
        internal static Dictionary<string, string> helper = new();

        public PropriétésDeJeu()
        {
            helper.Add("GameMode", "Définit le mode de jeu. 'temps' par défaut, valeurs possibles : 'arcade', 'temps'.");
            helper.Add("MaxPlayerTurnTime", "Temps maximum pour qu'un joueur joue en GameMode 'temps' en secondes. 30 par défaut.");
            helper.Add("TempsBase", "Temps après lequel la partie en mode 'temps' est interrompue en secondes. 120 par défaut.");
            helper.Add("Debug", "Définit le statut du mode débug. Affiche une flopée de statuts et de valeurs de débug.");
            helper.Add("AutoriserDiagonales", "Définit si atteindre des lettres en diagonale est autorisé. True par défaut.");
        }




    }}