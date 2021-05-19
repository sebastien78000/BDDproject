using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public class Velo
    {
        /// <summary>
        /// Permet d'ajouter un vélo dans la base de donnée
        /// </summary>
        public static void CreerVelo()
        {
            // verifié si modele velo et grandeur fourni existe


            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele velo
            string requete = "SELECT numeroModele FROM VeloMax.modelevelo;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            string valueString;
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                codeModeleVeloList.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            for (int i = 0; i < codeModeleVeloList.Count; i++)
            {
                Console.WriteLine(codeModeleVeloList[i]);
            }
            string codeModeleVelo = "";
            while (!codeModeleVeloList.Contains(codeModeleVelo))
            {
                Console.WriteLine("Numero Modele Velo ?");
                codeModeleVelo = Console.ReadLine();
                if (!codeModeleVeloList.Contains(codeModeleVelo)) Console.WriteLine("Numero pas dans la liste.\n");
            }

            // grandeur
            requete = $"SELECT grandeur FROM VeloMax.modeleVelo where numeroModele = '{codeModeleVelo}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> grandeurList = new List<string>();
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                grandeurList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine($"Grandeurs pour le numero de modele {codeModeleVelo} :");
            for (int i = 0; i < grandeurList.Count; i++)
            {
                Console.WriteLine(grandeurList[i]);
            }

            string grandeur = "";
            while (!grandeurList.Contains(grandeur))
            {
                Console.WriteLine("Grandeur ?");
                grandeur = Console.ReadLine();
                if (!grandeurList.Contains(grandeur)) Console.WriteLine("Cette grandeur n'existe pas.\n");
            }

            // obtenir liste des pieces du modele
            requete = $"SELECT * FROM VeloMax.listeassemblage WHERE numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> listePieceNecessaire = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    listePieceNecessaire.Add(valuesString[i]);
                }
            }
            reader.Close();
            command.Dispose();

            // obtenir liste des pieces disponibles en stock
            requete = $"SELECT codeModelePiece FROM VeloMax.piece WHERE numeroVelo is null and vendu=false;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> listePieceDispo = new List<string>();
            valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                listePieceDispo.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            // comparer les deux listes: verifier si l'assemblage est possible
            bool assemblage = true;
            for(int i=2;i<listePieceNecessaire.Count();i++)
            {
                if (listePieceNecessaire[i]!="")
                {
                    if (listePieceDispo.Contains(listePieceNecessaire[i]) == false)
                    {
                        Console.WriteLine("impossible d'assembler le velo: pieces non disponibles");
                        assemblage = false;
                        break;
                    }
                }
                
            }
            if(assemblage==true)
            {
                // creer le nouveau velo
                
                string donnees = $"{codeModeleVelo},'{grandeur}',false";
                requete = $"INSERT INTO VeloMax.velo (numeroModele,grandeur,vendu) VALUES({donnees});";
                command = maConnexion.CreateCommand();
                command.CommandText = requete;
                reader = command.ExecuteReader();
                reader.Close();
                command.Dispose();

                // recuperer nouveau numero de velo
                string numero = "";
                requete = $"SELECT max(numeroVelo) FROM VeloMax.velo;";
                command = maConnexion.CreateCommand();
                command.CommandText = requete;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    numero = reader.GetValue(0).ToString();
                }
                reader.Close();
                command.Dispose();

                // assigner aux pieces le numero du velo
                for(int i=0;i<listePieceNecessaire.Count();i++)
                {
                    if(listePieceNecessaire[i]!="")
                    {
                        requete = $"update VeloMax.piece set piece.numeroVelo='{numero}' where numeroVelo is null and vendu=false and piece.codeModelePiece='{listePieceNecessaire[i]}' limit 1;";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Close();
                        command.Dispose();
                    }
                    
                }               
            }


        }

        /// <summary>
        /// Permet de supprimer un vélo de la base de données en le désassemblant (les pièces deviennent libres)
        /// </summary>
        public static void SupprimerVelo()
        // demonte un velo en liberant les pieces (possible seulement si le velo n est pas dans une commande)
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Numero Velo
            string requete = "SELECT numeroVelo FROM VeloMax.velo;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeNumeroVeloList = new List<string>();
            string valueString;
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                codeNumeroVeloList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine();
            for (int i = 0; i < codeNumeroVeloList.Count; i++)
            {
                Console.WriteLine(codeNumeroVeloList[i]);
            }
            string NumeroVelo = "";
            while (!codeNumeroVeloList.Contains(NumeroVelo))
            {
                Console.WriteLine("Numero Velo ?");
                Console.WriteLine();
                NumeroVelo = Console.ReadLine();
                if (!codeNumeroVeloList.Contains(NumeroVelo)) Console.WriteLine("Numero pas dans la liste.\n");
            }

            // Demonter le velo en piece
            requete = $"update VeloMax.piece set piece.numeroVelo=null, piece.vendu=false where numeroVelo='{NumeroVelo}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

            //supprimer velo
            requete = $"delete from velomax.velo where numeroVelo='{NumeroVelo}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();



        }

        /// <summary>
        /// Permet d'afficher la liste d'assemblage d'un modèle
        /// </summary>
        public static void LireDataAssemblage()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"select * from veloMax.listeassemblage;";
            MySqlCommand command1 = maConnexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();

            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + " ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command1.Dispose();
        }


        /// <summary>
        /// Permet de modifier les informations d'un modèle de vélo
        /// </summary>
        public static void ModifierVelo()
        // modifier modele velo
        {
            // verifié si modele velo et grandeur fourni existe


            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele velo
            string requete = "SELECT numeroModele FROM VeloMax.modelevelo;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            string valueString;
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                codeModeleVeloList.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            for (int i = 0; i < codeModeleVeloList.Count; i++)
            {
                Console.WriteLine(codeModeleVeloList[i]);
            }
            string codeModeleVelo = "";
            while (!codeModeleVeloList.Contains(codeModeleVelo))
            {
                Console.WriteLine("Numero Modele Velo ?");
                codeModeleVelo = Console.ReadLine();
                if (!codeModeleVeloList.Contains(codeModeleVelo)) Console.WriteLine("Numero pas dans la liste.\n");
            }

            // grandeur
            requete = $"SELECT grandeur FROM VeloMax.modeleVelo where numeroModele = '{codeModeleVelo}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> grandeurList = new List<string>();
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                grandeurList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine($"Grandeurs pour le numero de modele {codeModeleVelo} :");
            for (int i = 0; i < grandeurList.Count; i++)
            {
                Console.WriteLine(grandeurList[i]);
            }

            string grandeur = "";
            while (!grandeurList.Contains(grandeur))
            {
                Console.WriteLine("Grandeur ?");
                grandeur = Console.ReadLine();
                if (!grandeurList.Contains(grandeur)) Console.WriteLine("Cette grandeur n'existe pas.\n");
            }

            Console.WriteLine("Que voulez vous modifier ?\n" +
                        "ligne produit (1)\n" +
                        "nom (2)\n" +
                        "prix unitaire (3)\n" +
                        "date Introduction (4)\n" +
                        "date Discontinuation (5)\n");
            string modif = "";
            int modification = Convert.ToInt32(Console.ReadLine());
            switch (modification)
            {
                case 1:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.modelevelo set modelevelo.ligneProduit='{modif}' where numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 2:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.modelevelo set modelevelo.nom='{modif}' where numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 3:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.modelevelo set modelevelo.prixUnitaire='{modif}' where numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 4:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.modelevelo set modelevelo.dateIntroduction='{modif}' where numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 5:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.modelevelo set modelevelo.dateDiscontinuation'{modif}' where numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                
            }
        }

        /// <summary>
        /// Permet d'afficher les infos des vélos assemblés stockés
        /// </summary>
        public static void LireDataVelo()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"select velo.numeroModele,modelevelo.nom,modelevelo.grandeur from veloMax.velo join velomax.modelevelo on velo.grandeur=modelevelo.grandeur and velo.numeroModele=modelevelo.numeroModele and velo.vendu=false;";
            MySqlCommand command1 = maConnexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();

            string[] valueString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + " ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command1.Dispose();
        }

        /// <summary>
        /// détermine si un certain modèle de vélo peut-être monté à l'aide des pièces disponibles en stock
        /// </summary>
        /// <param name="codeModeleVelo">code modèle du vélo qu'on aimerait assembler</param>
        /// <param name="grandeur">grandeur du vélo qu'on aimerait assembler</param>
        /// <returns></returns>
        public static bool PossibilititéAssemblerVelo(string codeModeleVelo, string grandeur)
        // renvoie true si le velo peut etre assemblé sinon renvoie faux
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"SELECT * FROM VeloMax.listeassemblage WHERE numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> listePieceNecessaire = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    listePieceNecessaire.Add(valuesString[i]);
                }
            }
            reader.Close();
            command.Dispose();

            // obtenir liste des pieces disponibles en stock
            requete = $"SELECT codeModelePiece FROM VeloMax.piece WHERE numeroVelo is null and vendu=false;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> listePieceDispo = new List<string>();
            string valueString = "";
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                listePieceDispo.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            // comparer les deux listes: verifier si l'assemblage est possible
            bool assemblage = true;
            for (int i = 2; i < listePieceNecessaire.Count(); i++)
            {
                if (listePieceNecessaire[i] != "")
                {
                    if (listePieceDispo.Contains(listePieceNecessaire[i]) == false)
                    {
                        //Console.WriteLine("impossible d'assembler le velo: pieces non disponibles");
                        assemblage = false;
                        break;
                    }
                }

            }
            return assemblage;

        }

        /// <summary>
        /// Assemble un vélo à partir des piècezs en stock. Associe le numéro du vélo aux pièces utilisées et marque le vélo comme vendu.
        /// </summary>
        /// <param name="codeModeleVelo">code du modèle de vélo à monter</param>
        /// <param name="grandeur">grandeur du vélo à monter</param>
        public static void AssemblerVelo(string codeModeleVelo,string grandeur)
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // creer le nouveau velo
            
            string donnees = $"{codeModeleVelo},'{grandeur}',true";
            string requete = $"INSERT INTO VeloMax.velo (numeroModele,grandeur,vendu) VALUES({donnees});";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

            // recuperer nouveau numero de velo
            string numero = "";
            requete = $"SELECT max(numeroVelo) FROM VeloMax.velo;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                numero = reader.GetValue(0).ToString();
            }
            reader.Close();
            command.Dispose();

            // obtenir liste des pieces du modele
            requete = $"SELECT * FROM VeloMax.listeassemblage WHERE numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> listePieceNecessaire = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    listePieceNecessaire.Add(valuesString[i]);
                }
            }
            reader.Close();
            command.Dispose();

            // assigner aux pieces le numero du velo
            for (int i = 0; i < listePieceNecessaire.Count(); i++)
            {
                if (listePieceNecessaire[i] != "")
                {
                    requete = $"update VeloMax.piece set piece.numeroVelo='{numero}', piece.vendu=true where numeroVelo is null and vendu=false and piece.codeModelePiece='{listePieceNecessaire[i]}' limit 1;";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                }

            }
        }


        /// <summary>
        /// Permet de marquer comme vendues les pièces contenues dans un vélo lorsque celui-ci est vendu.
        /// </summary>
        /// <param name="numeroVelo">numéro du vélo vendu</param>
        /// <param name="codeModeleVelo">code du modèle de vélo vendu</param>
        /// <param name="grandeur">grandeur du vélo vendu</param>
        public static void VendrePiecesVelo(string numeroVelo, string codeModeleVelo, string grandeur)
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            // obtenir liste des pieces du modele
            string requete = $"SELECT * FROM VeloMax.listeassemblage WHERE numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> listePieceNecessaire = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    listePieceNecessaire.Add(valuesString[i]);
                }
            }
            reader.Close();
            command.Dispose();

            // assigner les pièces du vélo 
            for (int i = 0; i < listePieceNecessaire.Count(); i++)
            {
                if (listePieceNecessaire[i] != "")
                {
                    requete = $"update VeloMax.piece set vendu=true where numeroVelo='{numeroVelo}';";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeModeleVelo"></param>
        /// <param name="grandeur"></param>
        /// <returns></returns>
        public static string TempsNecessairePieceManquanteAssemblageVelo(string codeModeleVelo,string grandeur)
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"SELECT * FROM VeloMax.listeassemblage WHERE numeroModele='{codeModeleVelo}' and grandeur='{grandeur}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> listePieceNecessaire = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    listePieceNecessaire.Add(valuesString[i]);
                }
            }
            reader.Close();
            command.Dispose();

            // obtenir liste des pieces disponibles en stock
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            requete = $"SELECT codeModelePiece FROM VeloMax.piece WHERE numeroVelo is null and vendu=false;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> listePieceDispo = new List<string>();
            string valueString = "";
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                listePieceDispo.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            // comparer les deux listes: obtenir pieces manquantes
            List<string> PieceManquante = new List<string>();
            for (int i = 2; i < listePieceNecessaire.Count(); i++)
            {
                if (listePieceNecessaire[i] != "")
                {
                    if (listePieceDispo.Contains(listePieceNecessaire[i]) == false)
                    {
                        PieceManquante.Add(listePieceNecessaire[i]);
                    }
                }

            }

            int tempsMin = 10000000;
            for (int i=0;i<PieceManquante.Count();i++)
            {
                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
                requete =  $"SELECT min(delaiJoursApprovisionnement) from velomax.fournisseur_modelepiece where codeModelePiece='{PieceManquante[i]}';"; ;
                command = maConnexion.CreateCommand();
                command.CommandText = requete;
                reader = command.ExecuteReader();
                valueString = "";
                while (reader.Read())
                {
                    valueString = reader.GetValue(0).ToString();
                }
                int temps = Convert.ToInt32(valueString);
                if (temps < tempsMin) tempsMin = temps;
                reader.Close();
                command.Dispose();
            }

            return Convert.ToString(tempsMin);


        }
    }
}
