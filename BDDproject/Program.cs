using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("BONJOUR");
            Console.WriteLine("BIENVENUE SUR PROJET VELOMAX");
            Console.WriteLine();
            Console.WriteLine("Entrer 'bozo' puis 'bozo' pour voir le demonstrateur");
            Console.WriteLine("Entrer 'root' puis 'root' pour voir l'application");
            Console.WriteLine("Entrer nom utilisateur");
            string nomUtilisateur = Console.ReadLine();
            Console.WriteLine("Entrer mot de passe");
            string mdp = Console.ReadLine();
            if (mdp == "root" && nomUtilisateur == "root")
            {
                Console.Clear();
                main();
            }
            else
            {
                Console.Clear();
                presentateur();
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void main()
        {
            bool quitter= true;
            while(quitter==true)
            {
                Console.WriteLine("Projet VeloMax");
                Console.WriteLine("MENU");
                Console.WriteLine("1: Gestion des pieces de rechange\n" +
               "2: Gestions des Velos\n" +
               "3: Gestion des clients\n" +
               "4: Gestion des fournisseurs \n" +
               "5: Gestion des commandes\n" +
               "6: Gestion du stock\n" +
               "7: Module statistique\n" +
               "8: Exports");

                int choix = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (choix)
                {
                    case 1:
                        Console.WriteLine("Gestion pièces de rechange");
                        Console.WriteLine("1: Creation pieces\n" +
                            "2: Modification pièce\n"+
                        "3: Suppression pieces\n");
                        int choix2= Convert.ToInt32(Console.ReadLine());
                        switch(choix2)
                        {
                            case 1:
                                Piece.AjouterPiece();
                                break;
                            case 2:
                                Piece.ModifierPiece();
                                break;
                            case 3:
                                Piece.SupprimerPiece();
                                break;
                            default:
                                break;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Gestion des velos");
                        Console.WriteLine("1: Creation velo\n" +
                        "2: Suppression velo\n" +
                        "3: Mise a jour velo\n" +
                        "4: Lire data velos");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Velo.CreerVelo();
                                break;
                            case 2:
                                Velo.SupprimerVelo();
                                break;
                            case 3:
                                Velo.ModifierVelo();
                                break;
                            case 4:
                                Velo.LireDataVelo();
                                break;
                            default:
                                break;
                        }
                        break;
                        

                    case 3:
                        Console.WriteLine("Gestion des clients");
                        Console.WriteLine("1: Creation client\n" +
                        "2: Suppression client\n" +
                        "3: Mise a jour client\n" +
                        "4: Lecture clients");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Client.CreerClient();
                                break;
                            case 2:
                                Client.SupprimerClient();
                                break;
                            case 3:
                                Client.ModifierClient();
                                break;
                            case 4:
                                Client.LireDataClient();
                                break;
                            default:
                                break;
                        }
                        break;

                    case 4:
                        Console.WriteLine("Gestion des fournisseurs");
                        Console.WriteLine("1: Creation fournisseur\n" +
                        "2: Suppression fournisseur\n" +
                        "3: Mise a jour fournisseur\n" +
                        "4: Lire Data fournisseur\n");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Fournisseur.CreerFournisseur();
                                break;
                            case 2:
                                Fournisseur.SupprimerFournisseur();
                                break;
                            case 3:
                                Fournisseur.ModifDataFournisseur();
                                break;
                            case 4:
                                Fournisseur.LireDataFournisseur();
                                break;
                            default:
                                break;
                        }
                        break;

                    case 5:
                        Console.WriteLine("Gestion des commandes");
                        Console.WriteLine("1: Creation commande\n" +
                            "2: Lire data commande");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Commande.Commander();
                                break;
                            case 2:
                                Commande.LireDataCommmande();
                                break;
                            default:
                                break;

                        }
                        break;

                    case 6:
                        Console.WriteLine("Gestion du stock");
                        Console.WriteLine("1: par pièce\n" +
                        "2: par fournisseur\n" +
                        "3: par velo\n" +
                        "4: par categorie de velo\n");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Stock.StockPieces();
                                break;
                            case 2:
                                Stock.StockPiecesFournisseur();
                                break;
                            case 3:
                                Stock.StockVelos();
                                break;
                            case 4:
                                Stock.StockVeloCategorie();
                                break;
                            default:
                                break;
                        }
                        break;

                    case 7:
                        
                        Console.WriteLine("Module statistique");
                        Console.WriteLine("1: rapport statistique sur pieces et velos vendus\n" +
                        "2: Liste des membres pour chaque programme d'adhesion\n" +
                        "3: Date d'expiration des adhesions\n" +
                        "4: Meilleur(s) client(s)\n" +
                        "5: Analyse des commandes\n");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Statistique.QtesVendues();
                                break;
                            case 2:
                                Statistique.ListeMembreProgrammeAdhesion();
                                break;
                            case 3:
                                Statistique.DateExpirationProgrammesFideliteParClient();
                                break;
                            case 4:
                                Statistique.MeilleurClientEuros();
                                break;
                            case 5:
                                Statistique.MoyenneMontantCommande();
                                Statistique.MoyenneNombrePiecesParCommande();
                                Statistique.MoyenneNombreVelosParCommande();
                                break;
                            default:
                                break;
                        }
                        break;

                    case 8:
                        Console.WriteLine("Exports:");
                        Console.WriteLine("1: Export en XML\n" +
                        "2: Export en JSON\n");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Export.ExportXML();
                                break;
                            case 2:
                                Export.ExportJSON();
                                break;
                            default:
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Numero invalide");
                        break;
                }

                string reponse = "";
                while (reponse!="oui" && reponse!="non")
                {

                    Console.WriteLine("Souhaitez vous quitter l'application ? oui ou non");
                    reponse = Console.ReadLine();
                    if (reponse == "oui")
                    {
                        quitter = false;
                    }
                    Console.Clear();
                }
               
            }
            Console.WriteLine("Merci d'avoir utilisé le projet VeloMax");
            Console.WriteLine("Aurevoir");
           
        }



        /// <summary>
        /// 
        /// </summary>
        public static void presentateur()
        {
            Console.WriteLine("Bonjour");
            Console.WriteLine("Bienvenue dans le presentateur du projet VeloMax");
            Console.ReadKey();
            Console.Clear();

            // nombre de clients
            Console.WriteLine("Nombre de clients dans la base de données");
            Presentateur.NbClients();
            Console.ReadKey();
            Console.Clear();

            // client+cumul de leurs achats
            Console.WriteLine("Clients avec cumul de leurs achats");
            Presentateur.ClientEtCumulDesachats(); ;
            Console.ReadKey();
            Console.Clear();

            // liste des produits ayant une quantite en stock inferieur ou egale à 2
            Console.WriteLine("STOCK DES PIECES");
            Console.WriteLine();
            Console.WriteLine("Liste des pieces ayant du stock en quantité suffisante (>2 pieces)");
            Presentateur.ListeDesProduitsQteInf2(); ;
            Console.ReadKey();
            Console.Clear();

            // Nombre de pièces et/ou velos fournis par le fournisseur
            Console.WriteLine("Pieces fournies par chaque fournisseur");
            Presentateur.NombrePiecesParFournisseur();
            Console.ReadKey();
            Console.Clear();

            // Export en XML
            Console.WriteLine("Export");
            Presentateur.ExportTableModelePiece();
            Export.ExportJSON();
            Console.ReadKey();

        }
    }
}
