using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public static class Stock
    {
        public static void StockPieces()
        // pieces ni engagés dans un velo ni vendu
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele velo
            string requete = "SELECT codeModelePiece,count(codeModelePiece) FROM VeloMax.piece where piece.vendu=false and piece.numeroVelo is null group by codeModelePiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            string[] valueString = new string[reader.FieldCount];
            Console.WriteLine("Piece | Code modele piece");
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
            command.Dispose();
        }

        public static void StockPiecesFournisseur()
        // pieces libres tries par fournisseur
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // obtenir numero fournisseur
            string requete = "select distinct(numeroSiretFournisseur) from veloMax.piece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> listFournisseur = new List<string>();
            string valueString = "";
            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString = reader.GetValue(0).ToString();
                    listFournisseur.Add(valueString);
                }
            }
            reader.Close();
            command.Dispose();

            // lire pieces dispo par fournisseur
            List<string[]> pieces = new List<string[]>();
            for (int j=0;j<listFournisseur.Count();j++)
            {
                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
                requete = $"select codeModelePiece,count(codeModelePiece) from velomax.piece where vendu=false and numeroVelo is null and numeroSiretFournisseur='{listFournisseur[j]}' group by codeModelePiece;";
                command = maConnexion.CreateCommand();
                command.CommandText = requete;
                reader = command.ExecuteReader();
                string[] valuesString = new string[reader.FieldCount];
                Console.WriteLine($"Fournisseur: {listFournisseur[j]}");
                
                while (reader.Read())
                {

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        valuesString[i] = reader.GetValue(i).ToString();
                        Console.Write(valuesString[i]+ " ");
                    }
                    Console.WriteLine();
                    pieces.Add(valuesString);
                    
                }
                reader.Close();
                command.Dispose();
            }

        }

        public static void StockVelos()
        // velos deja assemblés mais pas encore vendus
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele velo
            string requete = "SELECT nom,modelevelo.grandeur FROM VeloMax.velo join VeloMax.modeleVelo on velo.numeroModele=modelevelo.numeroModele where velo.vendu=false;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
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
            command.Dispose();
        }
    }
}
