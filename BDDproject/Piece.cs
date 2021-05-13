using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public static class Piece
    { 
        public static void AjouterPiece()
        {
            // Connection
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele
            string requete = "SELECT codeModelePiece FROM VeloMax.fournisseur_modelePiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModelePieceList = new List<string>();
            string valueString;
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                codeModelePieceList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine("Modèles de pièce référencés par les fournisseurs:");
            for (int i = 0; i < codeModelePieceList.Count; i++)
            {
                Console.WriteLine(codeModelePieceList[i]);
            }

            string codeModelePiece = "";
            while (!codeModelePieceList.Contains(codeModelePiece))
            {
                Console.WriteLine("CodeModelePiece ?");
                codeModelePiece = Console.ReadLine();
                if (!codeModelePieceList.Contains(codeModelePiece)) Console.WriteLine("Modele pas dans la liste.\n");
            }
            // Siret
            requete = $"SELECT numeroSiretFournisseur FROM VeloMax.fournisseur_modelePiece where codeModelePiece = '{codeModelePiece}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> siretList = new List<string>();
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                siretList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine("Numéros Siret des fournisseurs pour la pièce sélectionnée:");
            for (int i = 0; i < siretList.Count; i++)
            {
                Console.WriteLine(siretList[i]);
            }

            string siret = "";
            while (!siretList.Contains(siret))
            {
                Console.WriteLine("Siret ?");
                siret = Console.ReadLine();
                if (!siretList.Contains(siret)) Console.WriteLine("Le fournisseur ne fournit pas le modèle.\n");
            }
            // Numéro Série
            requete = $"SELECT numeroSerie FROM VeloMax.piece where codeModelePiece = '{codeModelePiece}' and numeroSiretFournisseur = '{siret}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> noSerieList = new List<string>();
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                noSerieList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine("Numéros série déjà utilisés pour ce modèle et ce fournisseur :");
            for (int i = 0; i < noSerieList.Count; i++)
            {
                Console.WriteLine(noSerieList[i]);
            }

            string noSerie = "";
            while (noSerieList.Contains(noSerie) || noSerie == "")
            {
                Console.WriteLine("noSerie ?");
                noSerie = Console.ReadLine();
                if (siretList.Contains(siret)) Console.WriteLine("Ce numéro est déjà utilisé, veuillez en sélectionner un autre\n");
            }
            // Ajouter pièce
            string donnees = $"'{noSerie}','{codeModelePiece}','{siret}'";
            requete = $"INSERT INTO VeloMax.piece (numeroSerie, codeModelePiece, numeroSiretFournisseur) VALUES({donnees});";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }

        public static void AjouterPieceSansChoixCodeModele(string codeModelePiece)
        {
            Console.WriteLine("Piece: " + codeModelePiece);
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Siret
            string requete = $"SELECT numeroSiretFournisseur,libelle FROM VeloMax.fournisseur_modelePiece join VeloMax.fournisseur on fournisseur.numeroSiret=fournisseur_modelepiece.numeroSiretFournisseur where codeModelePiece = '{codeModelePiece}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;            
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuesString = new string[reader.FieldCount];
            List<string[]> siretList = new List<string[]>();
            while (reader.Read())
            {
                valuesString = new string[reader.FieldCount];
                for (int i=0;i<reader.FieldCount;i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    
                }
                siretList.Add(valuesString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine("Numéros Siret des fournisseurs et libelle du fournisseur pour la pièce sélectionnée:");
            for (int i = 0; i < siretList.Count; i++)
            {
                Console.WriteLine(siretList[i][0] +" "+ siretList[i][1]);
            }
            string siret = "";
            
            bool test = false;
            while(test==false)
            {
                Console.WriteLine("Siret ?");
                siret = Console.ReadLine();
                for (int i = 0; i < siretList.Count; i++)
                {
                    if (siretList[i][0] == siret) test = true;
                }
                if (test == false) Console.WriteLine("Siret incorrect");
            }
            
            // Numéro Série
            requete = $"SELECT numeroSerie FROM VeloMax.piece where codeModelePiece = '{codeModelePiece}' and numeroSiretFournisseur = '{siret}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            string valueString = "";
            List<string> noSerieList = new List<string>();
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                noSerieList.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            Console.WriteLine("Numéros série déjà utilisés pour ce modèle et ce fournisseur :");
            for (int i = 0; i < noSerieList.Count; i++)
            {
                Console.WriteLine(noSerieList[i]);
            }

            string noSerie = "";
            while (noSerieList.Contains(noSerie) || noSerie == "")
            {
                Console.WriteLine("noSerie ?");
                noSerie = Console.ReadLine();
                if (noSerieList.Contains(noSerie)) Console.WriteLine("Ce numéro est déjà utilisé, veuillez en sélectionner un autre\n");
            }
            // Ajouter pièce
            string donnees = $"'{noSerie}','{codeModelePiece}','{siret}'";
            requete = $"INSERT INTO VeloMax.piece (numeroSerie, codeModelePiece, numeroSiretFournisseur) VALUES({donnees});";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }

        public static void SupprimerPiece()
        {
            // Connection
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
        }

        public static void LireDataPiece()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"select * from veloMax.piece;";
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


        public static List<string> PieceExistante() //à tester
        // retourne la liste des pieces existantes dans le catalogue
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = "SELECT codeModelePiece FROM VeloMax.modelepiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> pieceListPossible = new List<string>();
            string valueString = "";
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                pieceListPossible.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            return pieceListPossible;
        }

        


        public static void LireDataPieceDispo()
        // renvoie le code modele piece des pieces disponiblers en stock
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"SELECT codeModelePiece FROM VeloMax.piece WHERE numeroVelo is null and piece.vendu=false;";
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
    }
}
