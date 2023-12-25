namespace MOTS_GLISSES
{
    public sealed class Dictionnaire
    {
        /// all_word va contenir tous les mots du dictionnaire dans l'ordre
        static string[] all_word = Array.Empty<string>();
        /// la longuer minimale pour qu'un mot soit valide
        static int longueurminmot = 2;
        /// la langue du dictionnaire
        static string langue = "Français";
        static Dictionary<int, int> nbdemotlongueur = new();

        static Dictionary<char, int> nbdemotparlettre = new();

        public static int Longueurminmot {
            get { return longueurminmot;}
            set {longueurminmot = value;}
        }
        public static string[] All_word
        {
            get { return all_word; }
            set { all_word = value; }
        }

        static public void Init()
        {
            if (all_word == Array.Empty<string>())
            {
                Load_Word();//S'occupe de charger tous les mots dans all_word
            }
            if (all_word == Array.Empty<string>() ||all_word.Length==0) return;
            if (nbdemotlongueur.Count == 0)
            {
                LoadDictionnairelongueur();//S'occupe de charger le nombre de mot en fonction de leur longueur
            }
            if(nbdemotparlettre.Count == 0)
            {
                LoadDictionnairechar();//S'occupe de charger le nombre de mot en fonction de la première lettre
            }
        }
        /// <summary>
        /// Renvoie les infos sur la classe bibliothèque
        /// </summary>
        /// <returns></returns>
        static public string toString()
        {
            Init();
            string dicolongueur = "";
            for(int i = Longueurminmot; i <= nbdemotlongueur.Keys.Max();i++)
            {
                dicolongueur += String.Format("Longueur : {0}, nombre : {1}, \n", i, nbdemotlongueur[i]);
            }
            dicolongueur = dicolongueur.Substring(0, dicolongueur.Length - 2);
            string dicochar = "";
            foreach (var entry in nbdemotparlettre)
            {
                dicochar += String.Format("Char : {0}, nombre : {1}, \n", entry.Key, entry.Value);
            }
            dicochar = dicochar.Substring(0, dicochar.Length - 2);
            return String.Format("Nombre de mots par lettre :\n{0} \n \nNombre de mots par char :\n{1}  \nLangue : {2}", dicolongueur,  dicochar, langue);
        }
        //S'occupe de charger le nombre de mot en fonction de la première lettre
        static private void LoadDictionnairechar()
        {
            for (int i = 0; i < all_word.Length; i++)
            {
                if(all_word[i]=="") { }
                else if (nbdemotparlettre.ContainsKey(all_word[i][0]))
                {
                    nbdemotparlettre[all_word[i][0]]++;
                }
                else
                {
                    nbdemotparlettre[all_word[i][0]] = 1;
                }
            }
        }
        //S'occupe de charger le nombre de mot en fonction de leur longueur
        static private void LoadDictionnairelongueur()
        {
            int itération = LongueurMaxMot();
            for(int i= Longueurminmot; i<=itération;i++)
            {
                nbdemotlongueur.Add(i, CompteXLetterMot(i));
            }
        }
        /// <summary>
        /// Recherche dans alml_word la longueur maximale qu'un mot possède
        /// </summary>
        /// <returns> la longueur maximale des mots dans le dictionnaire</returns>
        static private int LongueurMaxMot()
        {
            string max = all_word[0];
            for (int i = 1; i < all_word.Length; i++)
            {
                if (all_word[i].Length > max.Length)
                {
                    max = all_word[i];
                }
            }
            return max.Length;
        }
        /// <summary>
        /// Compte le nombre de fois qu'une longueur de mot apparait
        /// </summary>
        /// <param name="longueur"> la longueur recherchée</param>
        /// <returns> le nombre de mot de longueur X</returns>
        static private int CompteXLetterMot(int longueur)
        {
            int compteur = 0;
            for(int i=0; i<all_word.Length; i++)
            {
                if (all_word[i].Length == longueur)
                {
                    compteur++;
                }
            }
            return compteur;
        }

        /// <summary>
        /// L'initiateur de la recherche dicho, cela pour éviter d'avoir des instruction qui sont beaucoup appelés dans les fonctions
        /// </summary>
        /// <param name="searched_word"> le mot qui est recherché par l'utilisateur</param>
        /// <returns> un booléen pour savoir si le mot existe ou non</returns>
        public static bool AppelleRechercheDicho(string searched_word)
        {
            Init();
            if (all_word.Length == 0) return false;
            searched_word = searched_word.ToUpper();
            return RechDichoRecursif(searched_word, 0, all_word.Length -1);
        }
        /// <summary>
        /// la vrai fonction de recherche dichotomique
        /// </summary>
        /// <param name="searched_word">le mot recherché par l'utilisateur</param>
        /// <param name="beginning"> l'élement du début de la recherche</param>
        /// <param name="end">l'élement de la fin de la recherche</param>
        /// <returns></returns>
        public static bool RechDichoRecursif(string searched_word, int beginning, int end)
        {
            if (searched_word == all_word[(beginning + end) / 2])
            {
                return true;
            }
            if (beginning == end)
            {
                return false;
            }
            ///Ce cas est pris en compte car si a=1 et b=2 et que le mot est au niveau de b soit 2 alors (a+b)//2 = 1 donc ça stack overflow
            if (beginning == end - 1 && searched_word != all_word[end])
            {
                return false;
            }
            if (searched_word.CompareTo(all_word[(beginning + end) / 2]) == 1)
            {
                beginning = (beginning + end) / 2;
                return RechDichoRecursif(searched_word, beginning, end);
            }
            end = (beginning + end) / 2;
            return RechDichoRecursif(searched_word, beginning, end);
        }
        /// <summary>
        /// méthode qui s'occupe de charger les mots dans all_word
        /// </summary>
        private static void Load_Word()
        {
            string dirToSearchIn = "../../../";
            if (!File.Exists(dirToSearchIn+"Mots_Français.txt")) dirToSearchIn = "./";
            if (!File.Exists(dirToSearchIn + "Mots_Français.txt")) {Console.WriteLine("Could not find Mots_Français.txt"); return;}
            StreamReader word_reader = new(dirToSearchIn+"Mots_Français.txt");
            string word = word_reader.ReadToEnd();
            word += " ";
            word_reader.Close();
            string[] unsortedwords = word.Split(' ');
            all_word = Tri_Fusion(unsortedwords);
        }
        /// <summary>
        /// Méthode de trifusion
        /// </summary>
        /// <param name="array">tous les mots dans le dictionnaire non triés</param>
        /// <returns></returns>
        public static string[] Tri_Fusion(string[] array)//2^n>n -> O(log(n))
        {
            string[] temp;
            string[] temp2;
            if (array.Length == 1) return array;
            else if (array.Length % 2 == 1)
            {
                temp = new string[array.Length / 2];
                temp2 = new string[array.Length / 2 + 1];
                for (int i = 0; i < array.Length / 2; i++)
                {
                    temp[i] = array[i];
                    temp2[i] = array[i + array.Length / 2];
                }
                temp2[array.Length / 2] = array[^1];
            }
            else
            {
                temp = new string[array.Length / 2];
                temp2 = new string[array.Length / 2];
                for (int i = 0; i < array.Length / 2; i++)
                {
                    temp[i] = array[i];
                    temp2[i] = array[i + array.Length / 2];
                }
            }
            return Merge(Tri_Fusion(temp), Tri_Fusion(temp2));
        }
        /// <summary>
        /// méthode de merge pour le trifusion pour 2 tableux
        /// </summary>
        /// <param name="array">1er tableu</param>
        /// <param name="array1">2ème taleau</param>
        /// <returns>retourne un tableau</returns>
        private static string[] Merge(string[] array, string[] array1)// O(n)
        {
            int compteur = 0;
            int compteur1 = 0;
            string[] temp = new string[array.Length + array1.Length];
            while ((compteur != array.Length) || (compteur1 != array1.Length))
            {
                if (compteur == array.Length)
                {
                    temp[compteur + compteur1] = array1[compteur1];
                    compteur1++;
                }
                else if (compteur1 == array1.Length)
                {
                    temp[compteur + compteur1] = array[compteur];
                    compteur++;
                }
                else if (array[compteur].CompareTo(array1[compteur1]) == -1)
                {
                    temp[compteur + compteur1] = array[compteur];
                    compteur++;
                }
                else
                {
                    temp[compteur + compteur1] = array1[compteur1];
                    compteur1++;
                }
            }
            return temp;
        }
    }
}
