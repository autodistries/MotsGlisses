using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MOTS_GLISSES
{
    public sealed class Plateau
    {
        static int dimension = 8;

        bool diagonalesOk = true;

        // the object containing the current game
        //each entry of the dictionnary is a line, identified by it number
        //then the list contains letters as chars
        char[,] tileschar = new char[dimension, dimension];
        internal bool distrubutedcharacters { get; set; } = false;

        // each element of availableCharacters contains a key and a pair of ints like "A: [10; 0; ]"
        // A identifies the object. FIrst value is number of times it can be added; second is the weight of the letter when used
        Dictionary<char, List<int>> availableCharacters = new();

        /// <summary>
        /// Charge le plateau de jeux
        /// </summary>
        public Plateau(string? targetPath=null) {
            if (targetPath!=null)  ToRead(targetPath);
             string dirToSearchIn = "../../../";
            if (!File.Exists(dirToSearchIn+"Lettre.txt")) dirToSearchIn="./";
            availableCharacters = ReadAvailableCharacters(dirToSearchIn + "Lettre.txt")!;
            if (availableCharacters==null) distrubutedcharacters=false;

        } 

        
        public void CreateNewGame()
        {
            DistributeCharacters();
        }

        public char[,] Tileschar
        {
            get { return tileschar; }
            set { tileschar = value; }
        }

        public bool DiagonalesOk {
            get => diagonalesOk; 
            set => diagonalesOk=value;
        }

        /// <summary>
        /// S'occupe de charger et distribuer les lettres dans la matrice
        /// </summary>
        private void DistributeCharacters() // main function
        {
            string dirToSearchIn = "../../../";
            if (!File.Exists(dirToSearchIn+"Lettre.txt")) dirToSearchIn="./";
            availableCharacters = ReadAvailableCharacters(dirToSearchIn + "Lettre.txt")!;
            if (availableCharacters == null)
            {
                return;
            }
            //Console.WriteLine("Successfully read available chars");
            // DebugAvailableCharacters();
            Distributetileschar();
            //debugTiles();

        }
        /// <summary>
        /// Cherche le nombre de lettres qui restent dans le tableau
        /// </summary>
        /// <returns> le nombre de lettre restant dans le tableau</returns>
        public int GetExistingLetters()
        {
            int res = 0;
            for (int i = 0; i < tileschar.GetLength(0); i++)
            {
                for (int j = 0; j < tileschar.GetLength(1); j++)
                {
                    if (tileschar[i, j] != ' ')
                    {
                        res++;
                    }
                }

            }
            return res;
        }

        //possiblement inutile
        public bool IsLastLineLost()
        {
            int li = tileschar.GetLength(0);
            int ct = 0;
            for (int j = 0; j < tileschar.GetLength(1); j++)
            {
                if (tileschar[li, j] != ' ')
                {
                    ct++;
                }
                else ct = 0;
            }
            return ct < 2;
        }
        /// <summary>
        /// Affichage de la matrice
        /// </summary>
        public void AfficherPlateau()
        {
            string cadre = " --";
            for(int y =0;y< tileschar.GetLength(1); y++)
            {
                cadre += "-";
            }
            Console.WriteLine(cadre);
            for (int i = 0; i < tileschar.GetLength(0); i++)
            {
                if (i != tileschar.GetLength(0) - 1)
                {
                    Console.Write(" |");
                }
                else
                {
                    Console.Write("\u001b[32m->\u001b[0m");
                }
                for (int j = 0; j < tileschar.GetLength(1); j++)
                {
                    Console.Write(tileschar[i, j]);
                }
                if (i != tileschar.GetLength(0) - 1)
                {
                    Console.WriteLine("|");
                }
                else
                {
                    Console.WriteLine("\u001b[32m<-\u001b[0m");
                    
                }

            }
            Console.WriteLine(cadre);
        }
        /// <summary>
        /// s'occupe de faire la recherche du mot
        /// </summary>
        /// <param name="mot"> le mot recherché</param>
        /// <returns> booléen pour si il est trouvé, et une liste pour les coordonnées.</returns>
        public (bool, List<int[]>?) Recherche_Mot(string mot)
        {
            mot = mot.ToUpper();
            List<int[]> listposprem = RecherchePremiereligne(mot);
            if (listposprem == null || listposprem.Count == 0)
            {
                return (false, null);
            }
            foreach (int[] pos in listposprem)
            {
                List<int[]> retour = RechercheApprofondie(pos, mot)!;
                if (retour != null && retour.Count == mot.Length)
                {
                    return (true, retour);
                }
            }
            return (false, null);
        }
        /// <summary>
        /// Gère la logique de la liste et de faire la récursivité pour trouver le chemin pour le mot
        /// </summary>
        /// <param name="pos"> la position de la. dernière lettre trouvée</param>
        /// <param name="mot">le mot recherché</param>
        /// <param name="listpos"> la liste des positions utilisées par le mot</param>
        /// <param name="idx"> l'index de la lettre recherchée</param>
        /// <returns></returns>
        private List<int[]>? RechercheApprofondie(int[] pos, string mot, List<int[]>? listpos = null, int idx = 1)
        {
            listpos ??= new List<int[]>{pos};
            if (idx == mot.Length)
            {
                return listpos;
            }
            List<int[]> retourrecherche = RechercheAutour(pos, mot, listpos, idx);
            if (retourrecherche.Count == 0)
            {
                return null;
            }
            foreach (int[] posidx in retourrecherche)
            {
                listpos.Add(posidx);
                List<int[]> retour = RechercheApprofondie(posidx, mot, listpos, idx + 1)!;

                if (retour != null && retour.Count == mot.Length)
                {
                    return retour;
                }
                else if (retour == null)
                {
                    listpos.RemoveAt(listpos.Count - 1);
                }
            }
            return null;
        }
        /// <summary>
        /// S'occupe de retourner la liste des positions du char suivant dans le mot
        /// </summary>
        /// <param name="pos"> la position de la. dernière lettre trouvée</param>
        /// <param name="mot">le mot recherché</param>
        /// <param name="listposprec"> la liste des positions utilisées par le mot</param>
        /// <param name="idx"> l'index de la lettre recherchée</param>
        /// <returns>la liste des possiblités de position de la lettre cherchée</returns>
        private List<int[]> RechercheAutour(int[] pos, string mot, List<int[]> listposprec, int idx)
        {
            List<int[]> listpos = new List<int[]>();

            //déplacements_tab
            int[][] half = {
                new int[] {1, 0},
                new int[] {0, 1},
                new int[] {-1, 0},
                new int[] {0, -1},
            };
            
            int[][] full = {
                new int[] {1, 0},
                new int[] {0, 1},
                new int[] {-1, 0},
                new int[] {0, -1},
                new int[] {-1, -1},
                new int[] {-1, 1},
                new int[] {1, -1},
                new int[] {1, 1},
            };
            int[][] déplacements_tab = diagonalesOk ? full :half;

            
            foreach (int[] déplacement in déplacements_tab)
            {
                int newx = pos[0] + déplacement[0];
                int newy = pos[1] + déplacement[1];
                if (newx < 0 || newx > tileschar.GetLength(0) - 1)
                {
                    continue;
                }
                if (newy < 0 || newy > tileschar.GetLength(1) - 1)
                {
                    continue;
                }
                if (tileschar[newx, newy] == mot[idx])
                {
                    int[] charpos = { newx, newy };
                    if (!listposprec.Any(valeur => charpos.SequenceEqual(valeur)))
                    {
                        listpos.Add(charpos);
                    }
                }
            }
            return listpos;
        }
        /// <summary>
        /// Recherche toutes les occurences de la première lettre du mot recherché sur la première ligne
        /// </summary>
        /// <param name="mot"> le mot recherché</param>
        /// <returns> la liste des positions correspondant à la première lettre</returns>
        public List<int[]> RecherchePremiereligne(string mot)
        {
            List<int[]> listpos = new();
            for (int i = 0; i < tileschar.GetLength(1); i++)
            {
                if (tileschar[tileschar.GetLength(0) - 1, i] == mot[0])
                {
                    int[] pos = { tileschar.GetLength(0) - 1, i };
                    listpos.Add(pos);
                }
            }
            return listpos;
        }
        /// <summary>
        /// S'occupe de faire l'aléatoire et la distribution dans le tableau
        /// </summary>
        private void Distributetileschar()
        {
            int sum = 0;
            foreach (var item in availableCharacters)
            {
                sum+=item.Value[0];
            }
            if(sum < tileschar.GetLength(0) * tileschar.GetLength(1))
            {
                Console.WriteLine(" pas assez de lettre à distribuer dans Lettre.txt");
                distrubutedcharacters = false;
                return;
            }
            distrubutedcharacters = true;
            Random random = new();
            for (int i = tileschar.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = tileschar.GetLength(1) - 1; j >= 0; j--)
                {
                    bool notputted = true;
                    while (notputted)
                    {
                        const int ACharCode = 65;
                        int choice = random.Next() % 24;
                        if (availableCharacters[(char)(ACharCode + choice)][0] == 0)
                        {
                            // try again because that letter is not available
                        }
                        else
                        {
                            notputted = false;
                            tileschar[i, j] = (char)(ACharCode + choice);
                            availableCharacters[(char)(ACharCode + choice)][0]--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read what characters are available from an input file
        /// </summary>
        /// <param name="targetPath">the full or relative path of the Lettre.csv</param>
        /// <returns>a new to-be-assigned object of availableCharacters</returns>
        private static Dictionary<char, List<int>>? ReadAvailableCharacters(string targetPath)
        {
            if (!System.IO.Directory.Exists(Path.GetDirectoryName(targetPath)))
            {
                Console.WriteLine("Error while trying to read available lettres: " + targetPath + " cannot be accessed.");
                return null;
            }
            if (!File.Exists(targetPath))
            {
                Console.WriteLine("Could not find specified file at " + targetPath);
                return null;
            }
            string[] fileData = File.ReadAllLines(targetPath);
            Dictionary<char, List<int>> sortedData = new();
            foreach (string s in fileData)
            {
                string[] ss = s.Split(",");
                sortedData[ss[0][0]] = new List<int>() { Convert.ToInt16(ss[1]), Convert.ToInt16(ss[2]) };
            }
            return sortedData;
        }
        /// <summary>
        /// Pour le mode Debug permet d'afficher les lettres, leur nombre d'utilisation restante et le nombre de point attribué à chacune
        /// </summary>
        public void DebugAvailableCharacters()
        {
            if (availableCharacters == null)
            {
                Console.WriteLine("availableCharacters is null"); return;
            }
            foreach (KeyValuePair<char, List<int>> entry in availableCharacters)
            {
                string localr = "";
                localr += entry.Key + ": [";
                foreach (int it in entry.Value) { localr += it + "; "; }
                localr += "]";
                Console.WriteLine(localr);
            }
        }
        /// <summary>
        /// S'occupe de passer à travers toute la liste des positions pour enlever le mots de la matrice
        /// </summary>
        /// <param name="listpos"> la liste des positions du mot</param>
        internal void EnleverCaractèresÀ(List<int[]> listpos)
        {
            foreach (int[] coupleDeCoordonnées in listpos)
            {
                this.tileschar[coupleDeCoordonnées[0], coupleDeCoordonnées[1]] = ' ';
            }
            Maj_Plateau();
        }

        /// <summary>
        /// S'occupe de mettre à jour le tableau et faire tomber les lettre petit à petit
        /// </summary>
        private void Maj_Plateau()
        {// cest pour ici que ça aurait été plus pratique, des listes
            for (int i = tileschar.GetLength(1) - 1; i >= 0; i--)
            { //chaque colonne
                for (int j = tileschar.GetLength(0) - 1; j >= 0; j--)
                {
                    if (tileschar[i, j] == ' ' && i != 0)
                    {
                        int localc = 1;
                        while (tileschar[i - localc, j] == ' ' && localc != 0)
                        {
                            localc++;
                            if (i - localc < 0)
                            { // spaces till the top ! nothing to do
                                localc = 0;
                                //Console.WriteLine("skipped rest of col "+j);


                            }
                        }
                        if (!(localc == 0))
                        {
                            tileschar[i, j] = tileschar[i - localc, j];
                            tileschar[i - localc, j] = ' ';
                             Console.Clear();
                        AfficherPlateau();
                        Thread.Sleep(30);
                        }
                        //else i=-1;
                    }
                }
            }
        }
        /// <summary>
        /// S'occupe de calculer le score obtenu par un mot
        /// </summary>
        /// <param name="word"> le mot dont le score doit être calculé</param>
        /// <returns> le nombre de point à attribuer</returns>
        internal int GetWordScore(string word)
        {
            int r = 0;
            double mult = 1.0;
            foreach (char c in word)
            {
                r += availableCharacters[c][1]; //add character-specific score
                // vest with scores >=1
                mult += mult / 20.0;
            }
            r = (int)(r * mult); // give additional for length;
            return r;
        }

        /// <summary>
        /// S'occupe de exporter le tableau vers un fichier
        /// </summary>
        /// <param name="targetFile">le chemin du fichier cible</param>
        public void ToFile(string targetFile)
        { //export, or save; will always overwrite
            if (tileschar[0, 0] == new char())
            {
                Console.WriteLine("Trying to save an empty board ?");
                return;
            }
            try
            {
                string fullPath = System.IO.Path.GetFullPath(targetFile);
                List<string> entriesData = new();
                string ts;
                for (int i = 0; i < tileschar.GetLength(0); i++)
                {
                    ts = "";
                    for (int j = 0; j < tileschar.GetLength(1); j++)
                    {
                        ts += tileschar[i, j];
                        if (j != tileschar.GetLength(1) - 1) ts += ",";//separator

                    }
                    entriesData.Add(ts);
                }
                File.WriteAllLines(targetFile, entriesData);
                Console.WriteLine("Plateau enregistré sous " + fullPath);
            }
            catch
            {
                Console.WriteLine("Impossible de sauvgarder le plateau de jeu.");
                return;
            }
        }

        /// <summary>
        /// Charge un plateau à partir d'un fichier
        /// </summary>
        /// <param name="filename">le chemin du fichier cible</param>
        public void ToRead(string filename)
        { //import
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Console.WriteLine("Impossible de trouver le dossier en question.");
                return ;
            }
            if (!File.Exists(filename))
            {
                Console.WriteLine("Impossible de trouver le fichier en question.");
                return ;
            }
            try
            {
                string[] entries = File.ReadAllLines(filename);
                dimension = entries.Length;
                char[,] testPlt = new char[dimension, dimension];
                for (int i = 0; i < testPlt.GetLength(0); i++)
                {
                    string entry = entries[i].ToUpper();
                    string[] data = entry.Split(separator: ';');
                    if (data.Length == 1)
                    {
                        data = entry.Split(separator: ',');
                    }
                    if (data.Length!=entries.Length) {
                        throw new Exception("Inconsistences détéctées dans les dimensions.");
                    }

                    for (int j = 0; j < testPlt.GetLength(1); j++)
                    {
                        testPlt[i, j] = data[j][0];

                    }
                }
                distrubutedcharacters = true;
                Console.WriteLine("Plateau importé avec succès.");
                if (QtyOfEmptys(testPlt)>0.6) {
                    Console.WriteLine("Le plateau semble mal formé; vérifiez votre fichier d'import.");
                        throw new Exception("Inconsistences détéctées dans le contenu du tableau.");
                }
                tileschar = testPlt;
                return;

            }
            catch
            {
                Console.WriteLine("Le parsing du plateau a échoué. Vérifiez sa mise en forme et recommencez.");
                CreateNewGame();
                return;
            }
        }

        public double QtyOfEmptys(char[,] tbl) {

            int a = tbl.Length;
            if (tbl.Length<1) return 1;
            int b=0;
            for (int i = 0; i < tbl.GetLength(0); i++) {
                for (int j = 0; j < tbl.GetLength(1); j++) {
                    if (tbl[i, j]== ' ') {
                        b++;
                    }
                }
            }
            return b/a;
            

        }
    }
}