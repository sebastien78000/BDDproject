using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public class Commande
    {

        public static void Commander() // à compléter
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                                     "DATABASE=VeloMax;" +
                                                     "UID=root;PASSWORD=root";
            string numeroCommande = "";
            bool commandeCree = false;
            bool finCommande = false;
            while(finCommande==false)
            {
                
                Console.WriteLine("Que voulez vous commander ? (1) un velo (2) une piece");
                int choix = Convert.ToInt32(Console.ReadLine());
                switch (choix)
                {
                    case 1:

                        //demander modele et grandeur
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


                        // verifier disponibilité velo
                        requete = $"SELECT count(*) FROM VeloMax.velo where numeroModele = '{codeModeleVelo}' and grandeur='{grandeur}' and vendu=false;";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        string valueString2 = "";
                        while (reader.Read())
                        {
                            valueString2 = reader.GetValue(0).ToString();
                        }
                        reader.Close();
                        command.Dispose();
                        

                        if (Convert.ToInt32(valueString2) == 0)
                        {
                            // voir si montage possible
                            bool assemblage = Velo.PossibilititéAssemblerVelo(codeModeleVelo, grandeur);

                            if (assemblage==true)
                            {
                                // creer velo
                                Velo.AssemblerVelo(codeModeleVelo, grandeur);
                                //creer commande si non existente
                                //ajouter à commande_modelevelo 
                            }
                            else
                            {
                                // donner delai pour obtenir piece
                                Console.WriteLine($"Piece manquante, temps nécessaire pour obtenir pièce: {Velo.TempsNecessairePieceManquanteAssemblageVelo(codeModeleVelo,grandeur)}");
                            }
                        }
                        else
                        {
                            // si disponibilité: ajouter à commande_modelevelo + passer vendu à true
                        }

                        break;

                    case 2:
                        // piece desirée
                        // recuperer liste des pieces
                        List<string> pieceExistante = new List<string>();
                        pieceExistante=Piece.PieceExistante();
                        string piece = "";
                        while (!pieceExistante.Contains(piece))
                        {
                            Console.WriteLine("Quelle piece voulez vous commander ?");
                            piece = Console.ReadLine();
                            if (!pieceExistante.Contains(piece)) Console.WriteLine("La pièce que vous souhaitez n'existe pas.\n");
                        }

                        // verifier disponibilité pièce
                        List<string> pieceDispo = new List<string>();
                        pieceDispo = Stock.PiecesDisponibles();
                        if(pieceDispo.Contains(piece))
                        {
                            // creer commande si commande non existente puis ajouter à commande_piece puis bloquer piece 
                        }
                        else
                        {

                            //donner delai pour la piece
                            maConnexion = new MySqlConnection(connexionString);
                            maConnexion.Open();
                            requete = $"SELECT min(delaiJoursApprovisionnement) from velomax.fournisseur_modelepiece where codeModelePiece='{piece}';";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            valueString = "";
                            while (reader.Read())
                            {
                                valueString = reader.GetValue(0).ToString();
                            }
                            reader.Close();
                            command.Dispose();
                            Console.WriteLine($"Nombre de jours minimals: {valueString}");
                        }
                        break;
                }




                Console.WriteLine("Avez vous fini vos achats ? Si oui taper O Sinon taper N");
                string fin = Console.ReadLine();
                if (fin == "O") finCommande = true;

            }

        }


        public static string CreerCommande()
        {
            string numeroCommande="";
            string date = DateTime.Now.ToString();

            return numeroCommande;
        }
    }
}
