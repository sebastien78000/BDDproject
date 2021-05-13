using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    // Les stocks pour les velos ne prennent en compte que les vélos deja assemblés !!!!!!!!!!!!
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
            // obtenir numeros fournisseur
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

        public static void StockVeloCategorie() 
        //nb velos en fonction de leur ligne produit
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Code Modele velo
            string requete = "SELECT ligneProduit,count(ligneProduit) FROM VeloMax.velo join VeloMax.modeleVelo on velo.numeroModele=modelevelo.numeroModele where velo.vendu=false group by ligneProduit;";
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

        public static void AnalyserStockEtCreerPieceFaibleQuantite()
        // cree des pieces lorsque le compte de pieces en stock est trop faible
        // considere pieces faibles quand nb pieces dispo est égale à 0 ou 1
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                       "DATABASE=VeloMax;" +
                                       "UID=root;PASSWORD=root";
            
            // Pieces en stock
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = "SELECT codeModelePiece,count(codeModelePiece) FROM VeloMax.piece where piece.vendu=false and piece.numeroVelo is null group by codeModelePiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> pieceList = new List<string[]>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                pieceList.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            //modele piece existante
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            requete = "SELECT codeModelePiece FROM VeloMax.modelepiece;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> CodeModele = new List<string>();
            string valueString = "";
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                CodeModele.Add(valueString);
            }
            reader.Close();
            command.Dispose();

            //obtenir liste des pieces à commander
            List<string> pieceAcommander = new List<string>();
            for(int i=0;i<pieceList.Count();i++)
            {
                if(CodeModele.Contains(pieceList[i][0])==true)
                {
                    if (Convert.ToInt32(pieceList[i][1]) == 1)
                    {
                        pieceAcommander.Add(pieceList[i][0]);
                    }
                    else
                    {
                        Console.WriteLine(pieceList[i][0] + " existe en quantité suffisante dans le stock");
                    }
                }
                
            }
            for(int i=0;i<CodeModele.Count();i++)
            {
                bool test = false;
                for(int j=0;j<pieceList.Count();j++)
                {
                    if (CodeModele[i] == pieceList[j][0]) test = true;
                }
                if (test==false)
                {
                    pieceAcommander.Add(CodeModele[i]);
                    pieceAcommander.Add(CodeModele[i]);
                }
            }


            //lecture de la liste des pieces à commander
            Console.WriteLine("Pieces à commander:");
            for(int i=0;i<pieceAcommander.Count();i++)
            {
                Console.WriteLine(pieceAcommander[i]);
            }

            

            // creer pieces manquantes
            for(int i=0;i<pieceAcommander.Count();i++)
            {
                Piece.AjouterPieceSansChoixCodeModele(pieceAcommander[i]);
            }

            

        }

        public static List<string> PiecesDisponibles()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // pieces dispo dans stock
            string requete = "SELECT codeModelePiece FROM VeloMax.piece where piece.vendu=false and piece.numeroVelo is null group by codeModelePiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> piecesDisponibles = new List<string>();
            string valueString = "";
            Console.WriteLine("Piece | Code modele piece");
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();
                piecesDisponibles.Add(valueString);
            }
            reader.Close();
            command.Dispose();
            return piecesDisponibles;
        }





    }
}
