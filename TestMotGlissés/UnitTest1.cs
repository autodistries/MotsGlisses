namespace MOTS_GLISSES
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestRechDichoRecursif()
        {
            // Arrange
            Dictionnaire.All_word = new string[] { "AA", "BB", "CC", "DD", "FF" };
            bool result1 = Dictionnaire.RechDichoRecursif("AB", 0, Dictionnaire.All_word.Length - 1);
            bool result2 = Dictionnaire.RechDichoRecursif("CC", 0, Dictionnaire.All_word.Length - 1);
            Assert.IsFalse(result1, "Un mot a été trouvé alors qu'il ne devrait pas.");
            Assert.IsTrue(result2, "Un mot contenu dans le tableau n'a pas été trouvé");
        }
        [TestMethod]
        public void TestRecherche_Mot()
        {
            Plateau a = new Plateau();
            a.Tileschar = new char[,] { { 'A','B' },{'C','D' },{ 'E', 'F' } };
            Assert.IsTrue(a.Recherche_Mot("eca").Item1, "Problème recherche1");
            Assert.IsTrue(a.Recherche_Mot("ecabdf").Item1, "Problème recherche2");
            Assert.IsFalse(a.Recherche_Mot("fef").Item1, "Problème répétition de lettre1");
            Assert.IsFalse(a.Recherche_Mot("fedcf").Item1, "Problème répétition de lettre2");
        }
        
        [TestMethod]
        public void TestJoueurAdd_Mot()
        {
            Joueur a = new Joueur("Test Unitaire");
            Assert.IsTrue(a.Mots_trouvés == Array.Empty<string>(),"Le tableau ne démarre pas vide");
            a.Add_Mot("UwU");
            Assert.IsTrue(a.Mots_trouvés.Length == 1 ,"N'a pas ajouté le mot correctement" );
            a.Add_Mot("UwU");
            Assert.IsTrue(a.Mots_trouvés.Length == 1, "A ajouté un mot déjà existant");
            a.Add_Mot("OwO");
            Assert.IsTrue(a.Mots_trouvés.Length == 2, "N'a pas ajouté le mot correctement");
        }
        [TestMethod]
        public void TestJoueurTostring()
        {
            Joueur a = new Joueur("Test Unitaire");
            Assert.IsTrue(a.toString() == "Nom : Test Unitaire \nScore Actuel : 0 \nMots trouvés :", "problème tostring");
            a.Add_Mot("UwU");
            Assert.IsTrue(a.toString() == "Nom : Test Unitaire \nScore Actuel : 0 \nMots trouvés : UwU", "problème affichage mot");
            a.Add_Score(1);
            Assert.IsTrue(a.toString() == "Nom : Test Unitaire \nScore Actuel : 1 \nMots trouvés : UwU", "problème score");
            a.Add_Mot("OwO");
            Assert.IsTrue(a.toString() == "Nom : Test Unitaire \nScore Actuel : 1 \nMots trouvés : UwU | OwO", "problème score");
            a.Add_Mot("OwO");
            Assert.IsTrue(a.toString() == "Nom : Test Unitaire \nScore Actuel : 1 \nMots trouvés : UwU | OwO", "problème affichage plusieurs mots");
        }
        [TestMethod]
        public void TestJoueurAdd_Score()
        {
            Joueur a = new Joueur("Test Unitaire");
            Assert.IsTrue(a.Score == 0, "Le score n'est pas initié à 0");
            a.Add_Score(1);
            Assert.IsTrue(a.Score == 1, "Le score n'est pas ajouté");
            a.Add_Score(100);
            Assert.IsTrue(a.Score == 101, "Le score n'est pas ajouté correctement");
            a.Add_Score(-100);
            Assert.IsTrue(a.Score == 1, "Le score n'est pas soustrait correctement");
        }
    }
}