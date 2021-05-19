using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDproject
{
    public class Program
    {
        static void Main(string[] args)
        {

            Piece.LireDataPiece();
            Console.ReadKey();
        }

        public static void main()
        {
            bool quitter= true;
            while(quitter==true)
            {
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
                        Console.WriteLine("1: Creation pieces" +
                        "2: Suppression pieces\n");
                        int choix2= Convert.ToInt32(Console.ReadLine());
                        switch(choix2)
                        {
                            case 1:
                                Piece.AjouterPiece();
                                break;
                            case 2:
                                Piece.SupprimerPiece();
                                break;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Gestion des velos");
                        Console.WriteLine("1: Creation velo" +
                        "2: Suppression velo\n" +
                        "3: Mise a jour velo\n");
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
                        }
                        break;
                        

                    case 3:
                        Console.WriteLine("Gestion des clients");
                        Console.WriteLine("1: Creation client" +
                        "2: Suppression client\n" +
                        "3: Mise a jour client\n");
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
                        }
                        break;

                    case 4:
                        Console.WriteLine("Gestion des fournisseurs");
                        Console.WriteLine("1: Creation fournisseur" +
                        "2: Suppression fournisseur\n" +
                        "3: Mise a jour fournisseur\n");
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
                        }
                        break;

                    case 5:
                        Console.WriteLine("Gestion des commandes");
                        Console.WriteLine("1: Creation commande" +
                        "2: Suppression commande\n" +
                        "3: Mise a jour fournisseur\n");
                        choix2 = Convert.ToInt32(Console.ReadLine());
                        switch (choix2)
                        {
                            case 1:
                                Commande.Commander();
                                break;
                            case 2:
                                //Commande.SupprimerCommande();
                                break;
                            case 3:
                                break;
                        }
                        break;

                    case 6:
                        Console.WriteLine("Gestion du stock");
                        Console.WriteLine("1: par pièce" +
                        "2: par fournisseur\n" +
                        "3: par velo\n" +
                        "4: par categorie de velo");
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
                        }
                        break;

                    case 7:
                        
                        Console.WriteLine("Module statistique");
                        Console.WriteLine("1: rapport statistique sur pieces et velos vendus" +
                        "2: Liste des membres pour chaque programme d'adhesion\n" +
                        "3: Date d'expiration des adhesions\n" +
                        "4: Meilleur(s) client(s)\n" +
                        "5: Analyse des commandes");
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
                        }
                        break;

                    default:
                        Console.WriteLine("Numero invalide");
                        break;
                }

                Console.WriteLine("Souhaitez vous quitter l'application ? oui ou non");
                string reponse = "";
                while (reponse!="oui" || reponse!="nom")
                {
                    reponse = Console.ReadLine();
                    if (reponse == "oui")
                    {
                        quitter = false;
                    }
                }
               
            }
           
        }




        public static void presentateur()
        {

        }
    }
}
