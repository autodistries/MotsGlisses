namespace MOTS_GLISSES
{
    public class Joueur
    {
        private string nom;
        private string[]? mots_trouvés;
        private int score_actuel;
        private List<int> scores_plateaux;

        public Joueur(string nom)
        {
            this.nom = nom;
            this.score_actuel = 0;
            this.mots_trouvés = null;
            this.scores_plateaux = new();
        }


        public string Nom {
            get {return nom;}
            set {nom = value;}
        }
        public string[] Mots_trouvés
        {
            get {            mots_trouvés??=Array.Empty<string>();
 
                return mots_trouvés; }
            set { mots_trouvés = value; }
        }
        public int Score {
            get {return score_actuel;}
            set {score_actuel=value;}
        }

        public List<int> ScoresPassés {
            get {return scores_plateaux;}
        }

        public void AddScoreToHistory() {
            scores_plateaux.Add(score_actuel);
        }

        public string toString()
        {
            return String.Format("Nom : {0} \nScore Actuel : {1} \nMots trouvés : {2}",this.nom,this.score_actuel,this.Mots_trouvésString()).TrimEnd();
        }
        public string Mots_trouvésString()
        {
            mots_trouvés??=Array.Empty<string>();
            if (mots_trouvés.Length==0) return "";
            string ret = "";
            for (int i = 0; i < this.mots_trouvés.Length; i++)
            {
               ret+=this.mots_trouvés[i]+" | ";
            }
            ret= ret.Substring(0, ret.Length-2);
            return ret;
        }

        public void Add_Score(int val)
        {
            this.score_actuel += val;
        }

        public void Reset_Mots() {
            this.mots_trouvés = Array.Empty<string>();
        }
        public void Add_Mot(string mot)

        {
        

            if (!this.Contient(mot))
            {
                string[] new_mots = new string[mots_trouvés!.Length + 1];
                for (int i = 0; i < this.mots_trouvés.Length; i++)
                {
                    new_mots[i] = this.mots_trouvés[i];
                }
                new_mots[new_mots.Length - 1] = mot;
                mots_trouvés = new_mots;
            }
        }
        public bool Contient(string mot)
        {
                        mots_trouvés??=Array.Empty<string>();

            for (int i = 0; i < this.mots_trouvés.Length; i++)
            {
                if( this.mots_trouvés[i] == mot)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
