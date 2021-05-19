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
        public static void Commander()
        {
            bool finCommande = false;
            List<string> panierPieces = new List<string>();
            List<string[]> panierVelos = new List<string[]>(); // [0] pour le modèle, [1] pour la grandeur
            int maxDelai = 0;
            decimal prixTotal = 0;
            while (!finCommande)
            {
                //Affichage panier
                if (panierVelos.Count() > 0)
                {
                    Console.WriteLine("Vélos commandés :");
                    for (int i = 0; i < panierVelos.Count(); i++) Console.WriteLine($"Modèle : {panierVelos[i][0]} , grandeur : {panierVelos[i][1]}");
                }
                if (panierPieces.Count() > 0)
                {
                    Console.WriteLine("\nPièces commandées :");
                    for (int i = 0; i < panierVelos.Count(); i++) Console.WriteLine($"Modèle commandé : {panierPieces[i]}");
                }
                Console.WriteLine($"\nPrix total : {prixTotal} euros");
                Console.WriteLine($"Délai de {maxDelai} jours avant expédition");
                // Choix action
                string choix = "0";
                while (choix != "1" && choix != "2" && choix != "3")
                {
                    Console.WriteLine("\nQue souhaitez vous commander ?\n(1) Un vélo\n(2) Une pièce\n(3) Conclure commande");
                    choix = Console.ReadLine();
                    if (choix != "1" && choix != "2" && choix != "3") Console.WriteLine("Veuillez entrer une valeur correcte");
                }
                string connexionString;
                MySqlConnection maConnexion;
                switch (choix)
                {
                    case "1": // Commande vélo
                        // Connection
                        connexionString = "SERVER=localhost;PORT=3306;" +
                                                     "DATABASE=VeloMax;" +
                                                     "UID=root;PASSWORD=root";
                        maConnexion = new MySqlConnection(connexionString);
                        maConnexion.Open();
                        string requete;
                        MySqlCommand command;
                        MySqlDataReader reader;
                        // Liste Code Modele velo + nom Modele
                        requete = "SELECT numeroModele, nom, prixUnitaire FROM VeloMax.modelevelo WHERE dateDiscontinuation is null or numeroModele in (select numeroModele FROM Velomax.Velo where vendu = false);";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        List<string> codeModeleVeloList = new List<string>();
                        List<string> nomModeleVeloList = new List<string>();
                        List<string> prixVeloList = new List<string>();
                        while (reader.Read())
                        {
                            codeModeleVeloList.Add(reader.GetValue(0).ToString());
                            nomModeleVeloList.Add(reader.GetValue(1).ToString());
                            prixVeloList.Add(reader.GetValue(2).ToString());
                        }
                        reader.Close();
                        command.Dispose();
                        // Liste Grandeurs
                        List<string> enStockList = new List<string>();
                        List<List<string>> grandeursModeleList = new List<List<string>>();
                        for (int i = 0; i < codeModeleVeloList.Count(); i++)
                        {
                            requete = $"select distinct mv.grandeur from modelevelo mv join velo v on mv.numeroModele = v.numeroModele where mv.numeroModele = {codeModeleVeloList[i]};";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            List<string> grandeurList = new List<string>();
                            while (reader.Read()) grandeurList.Add(reader.GetValue(0).ToString());
                            reader.Close();
                            command.Dispose();
                            grandeursModeleList.Add(grandeurList);
                            // Vérification en Stock / Date de livraison                            
                            for (int j = 0; j < grandeurList.Count(); j++)
                            {
                                requete = $"select count(*) from Velomax.velo where numeroModele = '{codeModeleVeloList[i]}' and grandeur = '{grandeurList[j]}' and vendu = false;";
                                command = maConnexion.CreateCommand();
                                command.CommandText = requete;
                                reader = command.ExecuteReader();
                                reader.Read();
                                int nbEnStock = Convert.ToInt32(reader.GetValue(0).ToString());
                                reader.Close();
                                command.Dispose();
                                if (nbEnStock > 0) enStockList.Add($"Modèle {codeModeleVeloList[i]} - {nomModeleVeloList[i]}, Grandeur {grandeurList[j]}, Prix {prixVeloList[i]} euros : En stock, expédition dans les 24h");
                                else
                                {
                                    bool assemblage = Velo.PossibilititéAssemblerVelo(codeModeleVeloList[i], grandeurList[j]);
                                    if (assemblage) enStockList.Add($"Modèle {codeModeleVeloList[i]} - {nomModeleVeloList[i]}, Grandeur {grandeurList[j]}, Prix {prixVeloList[i]} euros : En stock, expédition dans les 24h");
                                    else
                                    {
                                        string delai = Velo.TempsNecessairePieceManquanteAssemblageVelo(codeModeleVeloList[i], grandeurList[j]);
                                        enStockList.Add($"Modèle {codeModeleVeloList[i]} - {nomModeleVeloList[i]}, Grandeur {grandeurList[j]}, Prix {prixVeloList[i]} euros : Expedition possible au plus tôt dans {delai} jours");
                                    }
                                }
                            }
                        }
                        // Selection
                        Console.WriteLine("\n Liste des modèles et grandeurs disponibles :");
                        for (int i = 0; i < enStockList.Count(); i++) Console.WriteLine(enStockList[i]);                        
                        string selectionModele = "";
                        while (!codeModeleVeloList.Contains(selectionModele))
                        {
                            Console.WriteLine("\n Veuillez entrer le code du modèle que vous souhaitez commander");
                            selectionModele = Console.ReadLine();
                            if (!codeModeleVeloList.Contains(selectionModele)) Console.WriteLine("Le modèle n'est pas disponible");
                        }
                        string selectionGrandeur = "";
                        while (!grandeursModeleList[codeModeleVeloList.IndexOf(selectionModele)].Contains(selectionGrandeur))
                        {
                            Console.WriteLine("\n Veuillez entrer la grandeur que vous souhaitez commander pour ce modèle");
                            selectionGrandeur = Console.ReadLine();
                            if (!grandeursModeleList[codeModeleVeloList.IndexOf(selectionModele)].Contains(selectionGrandeur)) Console.WriteLine("Cette grandeur n'est pas disponible pour ce modèle");
                        }
                        // Enregistrement du changement
                        string[] selectionVelo = new string[2] { selectionModele, selectionGrandeur };
                        panierVelos.Add(selectionVelo);
                        requete = $"select prixUnitaire from Velomax.modelevelo where numeroModele = '{selectionModele}' and grandeur = '{selectionGrandeur}'";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Read();
                        prixTotal += Convert.ToDecimal(reader.GetValue(0).ToString());
                        reader.Close();
                        command.Dispose();
                        //Appliquer changement dans la base de donnée
                        requete = $"select count(*) from Velomax.velo where numeroModele = '{selectionModele}' and grandeur = '{selectionGrandeur}' and vendu = false;";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Read();
                        int nbEnStock2 = Convert.ToInt32(reader.GetValue(0).ToString());
                        reader.Close();
                        command.Dispose();
                        if (nbEnStock2 > 0)
                        {
                            requete = $"select numeroVelo from VeloMax.velo where numeroModele = '{selectionModele}' and grandeur = '{selectionGrandeur}' and vendu = false limit 1;";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Read();
                            string numeroVelo = reader.GetValue(0).ToString();
                            reader.Close();
                            command.Dispose();
                            requete = $"update velomax.velo set vendu = true where numeroVelo = {numeroVelo};";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            Velo.VendrePiecesVelo(numeroVelo, selectionModele, selectionGrandeur);
                        }
                        else
                        {
                            bool assemblage = Velo.PossibilititéAssemblerVelo(selectionModele, selectionGrandeur);
                            if (assemblage) Velo.AssemblerVelo(selectionModele, selectionGrandeur);
                            else
                            {
                                int delai = Convert.ToInt32(Velo.TempsNecessairePieceManquanteAssemblageVelo(selectionModele, selectionGrandeur));
                                if (delai > maxDelai) maxDelai = delai;
                            }
                        }
                        maConnexion.Close();
                        Console.WriteLine("\n");
                        break;

                    case "2": // Commande pièce
                        // Connection
                        connexionString = "SERVER=localhost;PORT=3306;" +
                                                     "DATABASE=VeloMax;" +
                                                     "UID=root;PASSWORD=root";
                        maConnexion = new MySqlConnection(connexionString);
                        maConnexion.Open();
                        // Liste Modeles pièces + description
                        requete = "SELECT distinct mp.codeModelePiece, mp.description, mp.prixVenteUnitaire FROM VeloMax.fournisseur_modelepiece fmp join Velomax.modelepiece mp on mp.codeModelePiece = fmp.codeModelePiece WHERE fmp.dateDiscontinuation is null or mp.codeModelePiece in (select codeModelePiece FROM Velomax.Piece where vendu = false and numeroVelo is null);";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        List<string> codeModelePieceList = new List<string>();
                        List<string> descriptionList = new List<string>();
                        List<string> prixPieceList = new List<string>();
                        while (reader.Read())
                        {
                            codeModelePieceList.Add(reader.GetValue(0).ToString());
                            descriptionList.Add(reader.GetValue(1).ToString());
                            prixPieceList.Add(reader.GetValue(2).ToString());
                        }
                        reader.Close();
                        command.Dispose();
                        // stock list
                        List<string> pieceEnStockList = new List<string>();
                        for (int i = 0; i < codeModelePieceList.Count(); i++)
                        {
                            requete = $"select count(*) from Velomax.piece where codeModelePiece = '{codeModelePieceList[i]}' and vendu = false and numeroVelo is null;";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Read();
                            int nbPiecesEnStock = Convert.ToInt32(reader.GetValue(0).ToString());
                            reader.Close();
                            command.Dispose();
                            if (nbPiecesEnStock > 0) pieceEnStockList.Add($"Modèle : {codeModelePieceList[i]} {descriptionList[i]}, Prix : {prixPieceList[i]} euros : En stock, expédition dans les 24h");
                            else
                            {
                                maConnexion = new MySqlConnection(connexionString);
                                maConnexion.Open();
                                requete = $"SELECT min(delaiJoursApprovisionnement) from velomax.fournisseur_modelepiece where codeModelePiece='{codeModelePieceList[i]}';";
                                command = maConnexion.CreateCommand();
                                command.CommandText = requete;
                                reader = command.ExecuteReader();
                                reader.Read();
                                string delai = reader.GetValue(0).ToString();
                                reader.Close();
                                command.Dispose();
                                pieceEnStockList.Add($"Modèle : {codeModelePieceList[i]} {descriptionList[i]}, Prix : {prixPieceList[i]} euros : Expedition possible au plus tôt dans {delai} jours");
                            }
                        }
                        // Selection
                        Console.WriteLine("\n Liste des modèles de pièces disponibles :");
                        for (int i = 0; i < pieceEnStockList.Count(); i++) Console.WriteLine(pieceEnStockList[i]);
                        string selectionModelePiece = "";
                        while (!codeModelePieceList.Contains(selectionModelePiece))
                        {
                            Console.WriteLine("\n Veuillez entrer le code du modèle de pièce que vous souhaitez commander");
                            selectionModelePiece = Console.ReadLine();
                            if (!codeModelePieceList.Contains(selectionModelePiece)) Console.WriteLine("Le modèle n'est pas disponible");
                        }
                        // Enregistrement du changement
                        panierPieces.Add(selectionModelePiece);
                        requete = $"select prixVenteUnitaire from Velomax.modelePiece where codeModelePiece = '{selectionModelePiece}';";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Read();
                        prixTotal += Convert.ToDecimal(reader.GetValue(0).ToString());
                        reader.Close();
                        command.Dispose();
                        //Appliquer changement dans la base de donnée
                        requete = $"select count(*) from Velomax.piece where codeModelePiece = '{selectionModelePiece}' and vendu = false and numeroVelo is null;";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Read();
                        int nbPiecesEnStock2 = Convert.ToInt32(reader.GetValue(0).ToString());
                        reader.Close();
                        command.Dispose();
                        if (nbPiecesEnStock2 > 0) //En stock
                        {
                            requete = $"select numeroSerie, numeroSiretFournisseur from VeloMax.piece where codeModelePiece = '{selectionModelePiece}' and vendu = false and numeroVelo is null limit 1;";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Read();
                            string[] piece = new string[3] {reader.GetValue(0).ToString(), selectionModelePiece, reader.GetValue(1).ToString()};
                            reader.Close();
                            command.Dispose();
                            requete = $"update velomax.piece set vendu = true where numeroSerie = '{piece[0]}' and codeModelePiece = '{piece[1]}' and numeroSiretFournisseur = '{piece[2]}';";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                        }
                        else // Pas en stock
                        {
                            maConnexion = new MySqlConnection(connexionString);
                            maConnexion.Open();
                            requete = $"SELECT min(delaiJoursApprovisionnement) from velomax.fournisseur_modelepiece where codeModelePiece='{selectionModelePiece}';";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Read();
                            int delai = Convert.ToInt32(reader.GetValue(0).ToString());
                            reader.Close();
                            command.Dispose();
                            if (delai > maxDelai) maxDelai = delai;
                        }
                        maConnexion.Close();
                        Console.WriteLine("\n");
                        break;

                    case "3": //Conclure commande
                        // Liste numéro client
                        connexionString = "SERVER=localhost;PORT=3306;" +
                                                     "DATABASE=VeloMax;" +
                                                     "UID=root;PASSWORD=root";
                        maConnexion = new MySqlConnection(connexionString);
                        maConnexion.Open();
                        requete = "SELECT numeroClient from velomax.client order by numeroClient;";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        List<string> numeroClientList = new List<string>();
                        while (reader.Read()) numeroClientList.Add(reader.GetValue(0).ToString());
                        reader.Close();
                        command.Dispose();
                        // Selection numéro client
                        Console.WriteLine("\nListe des numéros clients");
                        for (int i = 0; i < numeroClientList.Count(); i++) Console.WriteLine(numeroClientList[i]);
                        string numeroClient = "";
                        while (!numeroClientList.Contains(numeroClient))
                        {
                            Console.WriteLine("\n (Entrez le numéro du client)");
                            numeroClient = Console.ReadLine();
                            if (!numeroClientList.Contains(numeroClient)) Console.WriteLine("Erreur dans le numéro client");
                        }
                        Console.WriteLine("\n Entrez votre adresse (rue et numéro de rue)");
                        string rue = Console.ReadLine();
                        Console.WriteLine("\n Entrez votre code Postal");
                        string codePostal = Console.ReadLine();
                        Console.WriteLine("\n Entrez votre ville");
                        string ville = Console.ReadLine();
                        DateTime _date = DateTime.Now;
                        string date = _date.ToString("yyyy-MM-dd");
                        _date.AddDays(maxDelai);
                        string dateExpedition = _date.ToString("yyyy-MM-dd");
                        requete = $"INSERT INTO velomax.commande (date, dateLivraison, rue, ville, codePostal, numeroClient) VALUES ('{date}','{dateExpedition}','{rue}','{ville}','{codePostal}','{numeroClient}');";
                        command = maConnexion.CreateCommand();
                        command.CommandText = requete;
                        reader = command.ExecuteReader();
                        reader.Close();
                        command.Dispose();
                        finCommande = true;
                        maConnexion.Close();
                        Console.WriteLine("Commande réussie\n\n");
                        break;
                }                
            }
        }
        public static void LireDataCommmande()
        {
            //Connexion
           string connexionString = "SERVER=localhost;PORT=3306;" +
                                                     "DATABASE=VeloMax;" +
                                                     "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            // Liste des numéros de commande
            string requete = $"select numeroCommande, numeroClient, date from velomax.commande order by numeroCommande;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> numeroCommandeList = new List<string>();
            while (reader.Read())
            {
                numeroCommandeList.Add(reader.GetValue(0).ToString());
                Console.WriteLine($"Numéro de commande : {reader.GetValue(0)}, Numéro Client : {reader.GetValue(1)}, Date : {reader.GetValue(2)}");
            }
            reader.Close();
            command.Dispose();
            //Selection numéro de commande
            string numeroCommande = "";
            while (!numeroCommandeList.Contains(numeroCommande))
            {
                Console.WriteLine("Veuillez entrer le numéro de la commande que vous souhaitez consulter");
                numeroCommande = Console.ReadLine();
                if (!numeroCommandeList.Contains(numeroCommande)) Console.WriteLine("Le numéro indiqué est incorrect");
            }
            // Détails de la commande
            Console.WriteLine("\nDétails de commande :");
            requete = $"select * from velomax.commande where numeroCommande = '{numeroCommande}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> commandeData = new List<string>();
            reader.Read();
            for (int i = 0; i < reader.FieldCount; i++) commandeData.Add(reader.GetValue(i).ToString());
            reader.Close();
            command.Dispose();
            Console.WriteLine($"Numero commande : {commandeData[0]}");
            Console.WriteLine($"Date de la commande : {commandeData[1]}");
            Console.WriteLine($"Date de livraison : {commandeData[2]}");
            Console.WriteLine($"Adresse de rue : {commandeData[3]}");
            Console.WriteLine($"Ville : {commandeData[4]}");
            Console.WriteLine($"Code postal : {commandeData[5]}");
            Console.WriteLine($"Numéro du client associé : {commandeData[6]}");
            // Détails des produits achetés
            Console.WriteLine("\n Liste des vélos commandés");
            requete = $"select mv.numeroModele, mv.nom, mv.prixUnitaire from velomax.commande_modelevelo cmv join velomax.modeleVelo mv on mv.numeroModele = cmv.numeroModele and mv.grandeur = cmv.grandeur where cmv.numeroCommande = '{numeroCommande}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            decimal prixTotal = 0;
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetValue(0)} - {reader.GetValue(1)} - {reader.GetValue(2)} euros");
                prixTotal += Convert.ToDecimal(reader.GetValue(2).ToString());
            }
            for (int i = 0; i < reader.FieldCount; i++) commandeData.Add(reader.GetValue(i).ToString());
            reader.Close();
            command.Dispose();
            Console.WriteLine("\n Liste des pièces commandées");
            requete = $"select mp.codeModelePiece, mp.description, mp.prixVenteUnitaire from velomax.commande_piece cp join velomax.modelePiece mp on mp.codeModelePiece = cp.codeModelePiece where cp.numeroCommande = '{numeroCommande}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetValue(0)} - {reader.GetValue(1)} - {reader.GetValue(2)} euros");
                prixTotal += Convert.ToDecimal(reader.GetValue(2).ToString());
            }
            for (int i = 0; i < reader.FieldCount; i++) commandeData.Add(reader.GetValue(i).ToString());
            Console.WriteLine($"\nPrix total : {prixTotal} euros");
            reader.Close();
            command.Dispose();
            maConnexion.Close();
        }
        public static void SupprimerCommande()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                          "DATABASE=VeloMax;" +
                                          "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            maConnexion.Close();
        }
    }
}
