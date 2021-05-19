using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public class Statistique
    {
        /// <summary>
        /// affiche les membres du programme de fidelite choisi
        /// </summary>
        public static void  ListeMembreProgrammeAdhesionAvecChoix()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            
            string requete = "SELECT numeroProgramme,description FROM  programmefidelite;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> listDescriptionModele = new List<string[]>();
            string[] valuesString =new string[reader.FieldCount];
            Console.WriteLine("Numero programme | description");
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    Console.Write(valuesString[i]+" ");
                }
                Console.WriteLine();
                listDescriptionModele.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            
            
            Console.WriteLine("Numero Programme ?");
            string nProgramme = Console.ReadLine();

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            
            requete = $"SELECT  prenom, nom,description FROM VeloMax.client join programmefidelite on client.numeroProgramme=programmefidelite.numeroProgramme where programmefidelite.numeroProgramme='{nProgramme}';";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            valuesString=new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    Console.Write(valuesString[i]+" ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// affiche les membres d'un programme de fidelité et le programme auquel ils ont souscrit
        /// </summary>
        public static void ListeMembreProgrammeAdhesion()
        // affiche tous les membres du programme de fidelite
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            Console.WriteLine("prenom | nom | programme d'adhesion");
            string requete = $"SELECT  prenom, nom,description FROM VeloMax.client join programmefidelite on client.numeroProgramme=programmefidelite.numeroProgramme order by programmefidelite.numeroProgramme;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            string [] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    Console.Write(valuesString[i] + " ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Affiche le nom, prenom, programme d'ahesion, la date d'adhesion et la date de fin d'adhesion des membres des programmes de fidelite
        /// </summary>
        public static void DateExpirationProgrammesFideliteParClient()
        // affiche chaque membre du programme de fidelite avec son prenom,son nom, la date d'adhesion et la date d'expiration de son programme
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";


            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            Console.WriteLine("prenom | nom | programme d'adhesion | date d'adhesion programme | date d'expiration programme");
            string requete = $"SELECT  prenom, nom,description,client.dateAdhesion,DATE_ADD(client.dateAdhesion,INTERVAL programmeFidelite.dureeMois MONTH ) FROM VeloMax.client join programmefidelite on client.numeroProgramme=programmefidelite.numeroProgramme order by programmefidelite.numeroProgramme;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string> codeModeleVeloList = new List<string>();
            string[] valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                    Console.Write(valuesString[i] + " ");
                }
                Console.WriteLine();
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Affiche les quantites vendues de chaque item qui se trouve dans l inventaire
        /// </summary>
        public static void QtesVendues()
        
        {
            // pieces vendues par categorie de piece
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select p.codeModelePiece,count(p.codeModelePiece) from velomax.piece p where p.vendu=true group by p.codeModelePiece;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> pieceList = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                pieceList.Add(valuesString);
            }
            reader.Close();
            command.Dispose();


            // pieces existantes en stock
            requete = $"SELECT codeModelePiece FROM VeloMax.modelepiece;";
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


            // croisement des deux listes
            List<string[]> finalList = new List<string[]>();
            for (int i = 0; i < listePieceDispo.Count(); i++)
            {
                bool test = true;
                for(int j=0;j<pieceList.Count();j++)
                {
                    if (listePieceDispo[i]==pieceList[j][0])
                    {
                        test = false;
                        finalList.Add(pieceList[j]);
                    }
                }
                if(test==true)
                {
                    string[] temp = new string[2];
                    temp[0] = listePieceDispo[i];
                    temp[1] = "0";
                    finalList.Add(temp);
                }
            }



            // velos vendus
            requete = $"select mv.nom,v.grandeur,count(v.numeroVelo) from velomax.velo v join velomax.modelevelo mv on  v.numeroModele=mv.numeroModele and v.grandeur=mv.grandeur where v.vendu=true  group by v.numeroModele;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string[]> VelosVendus = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                VelosVendus.Add(valuesString);
            }
            reader.Close();
            command.Dispose();



            // modele de velos existants
            requete = $"select mv1.nom,mv1.grandeur from velomax.modelevelo mv1,velomax.modelevelo mv2 where mv1.numeroModele!=mv2.numeroModele and mv1.grandeur!=mv2.grandeur and mv1.nom!=mv2.nom group by mv1.numeroModele;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string[]> VelosExistants = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                VelosExistants.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            // croisement des listes 
            List<string[]> finalListVelos = new List<string[]>();
            for (int i = 0; i < VelosExistants.Count(); i++)
            {
                bool test = true;
                for (int j = 0; j < VelosVendus.Count(); j++)
                {
                    if (VelosExistants[i][0] == VelosVendus[j][0] && VelosExistants[i][1] == VelosVendus[j][1])
                    {
                        test = false;
                        finalListVelos.Add(VelosVendus[j]);
                    }
                }
                if (test == true)
                {
                    string[] temp = new string[3];
                    temp[0] = VelosExistants[i][0];
                    temp[1] = VelosExistants[i][1];
                    temp[2] = "0";
                    finalListVelos.Add(temp);
                }
            }



            //lire listes
            Console.WriteLine("Pieces vendus:");
            for (int i=0;i<finalList.Count();i++)
            {
               Console.WriteLine(finalList[i][0]+" "+ finalList[i][1]);
            }
            Console.WriteLine();
            Console.WriteLine("Velos vendus:");
            for (int i = 0; i < finalListVelos.Count(); i++)
            {
                Console.WriteLine(finalListVelos[i][0] + " " + finalListVelos[i][1] + " " + finalListVelos[i][2]);
            }

        }

        /// <summary>
        /// Affiche le ou les meilleurs clients en fonction de la quantité cumulé d'argent des commandes
        /// </summary>
        public static void MeilleurClientEuros()
        {
            // somme des commandes de velos par client
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select co.numeroClient,sum(mv.prixUnitaire) from velomax.commande co join velomax.commande_modelevelo cmv on co.numeroCommande=cmv.numeroCommande join velomax.modelevelo mv on cmv.numeroModele = mv.numeroModele and cmv.grandeur = mv.grandeur group by co.numeroClient;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valueString = new string[reader.FieldCount];
            List<string[]> listeCommandeVelo = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                listeCommandeVelo.Add(valuesString);
            }

            reader.Close();
            command.Dispose();

            // somme des commandes de pieces par client
            requete = $"select co.numeroClient,sum(mp.prixVenteUnitaire) from  velomax.commande co join velomax.commande_piece cp on co.numeroCommande=cp.numeroCommande  join velomax.modelepiece mp on mp.codeModelePiece = cp.codeModelePiece  group by co.numeroClient;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            valueString = new string[reader.FieldCount];
            List<string[]> listeCommandePiece = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                listeCommandePiece.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            // obtenir tous les numeros de commande
            List<double[]> final = new List<double[]>();
            for (int i = 0; i < listeCommandeVelo.Count(); i++)
            {
                bool test = false; ;
                for (int j = 0; j < listeCommandePiece.Count; j++)
                {
                    if (listeCommandePiece[j][0] == listeCommandeVelo[i][0])
                    {
                        test = true;
                        double[] temp = new double[2];
                        temp[0] = Convert.ToDouble(listeCommandeVelo[j][0]);
                        temp[1] = Convert.ToDouble(listeCommandePiece[j][1]) + Convert.ToInt32(listeCommandeVelo[i][1]);
                        final.Add(temp);
                    }
                }
                if (test == false)
                {
                    double[] temp = new double[2];
                    temp[0] = Convert.ToDouble(listeCommandeVelo[i][0]);
                    temp[1] = Convert.ToDouble(listeCommandeVelo[i][1]);
                    final.Add(temp);
                }
            }
            for (int i = 0; i < listeCommandePiece.Count; i++)
            {
                bool test = false; ;
                for (int j = 0; j < listeCommandeVelo.Count(); j++)
                {
                    if (listeCommandePiece[i][0] == listeCommandeVelo[j][0])
                    {
                        test = true;
                    }
                }
                if (test == false)
                {
                    double[] temp = new double[2];
                    temp[0] = Convert.ToDouble(listeCommandePiece[i][0]);
                    temp[1] = Convert.ToDouble(listeCommandePiece[i][1]);
                    final.Add(temp);
                }
            }
            
            // obtenir les numeros clients du ou des plus grands acheteurs
            List<double> acheteurs = new List<double>();
            double max = 0;
            for(int i=0;i<final.Count()-1;i++)
            {
                if(final[i][1]>max)
                {
                    max = final[i][1];
                }
            }
            for (int i = 0; i < final.Count() - 1; i++)
            {
                if (final[i][1] == max)
                {
                    acheteurs.Add(final[i][0]);
                }
            }

            // retourner prenom, nom de la personne ou nom de la compagnie
            Console.WriteLine("Plus gros client(s):");
            for (int i=0;i<acheteurs.Count();i++)
            {
                // verifier compagnie ou personne
                requete = $"select nomCompagnie from velomax.client where client.numeroClient={acheteurs[i]};";
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
                if(valueString2=="")
                {
                    requete = $"select prenom,nom from velomax.client where client.numeroClient={acheteurs[i]};";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    string []valuesString = new string[reader.FieldCount];
                    Console.WriteLine("Client particulier: ");
                    while (reader.Read())
                    {
                        valuesString = new string[reader.FieldCount];
                        for (int j = 0; j < reader.FieldCount; j++)
                        {
                            valuesString[j] = reader.GetValue(j).ToString();
                            Console.Write(valuesString[j]+ " ");
                            
                        }
                        Console.WriteLine();
                        Console.WriteLine(max + " euros d'achat");
                    }
                    reader.Close();
                    command.Dispose();
                }
                else
                {
                    Console.WriteLine($"Entreprise: {valueString2}");
                    Console.WriteLine(max + " euros d'achat");
                }

            }
            


        }

        /// <summary>
        /// Affiche le nombre moyen de pieces commandeés par commande
        /// </summary>
        public static void MoyenneNombrePiecesParCommande()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select count(cp.numeroCommande) from velomax.commande_piece cp group by cp.numeroCommande;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            double somme = 0;
            double nb = 0;
            string valueString = "";
            while (reader.Read())
            {
                somme = somme + Convert.ToDouble(reader.GetValue(0).ToString());
                nb = nb + 1;
            }
            double moyenne = somme / nb;
            reader.Close();
            command.Dispose();
            Console.WriteLine($"Nombre de pieces moyennes par commande: {moyenne}");
        }

        /// <summary>
        /// Affiche le nombre moyen de pieces commandé par commande
        /// </summary>
        public static void MoyenneNombreVelosParCommande()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select count(cmv.numeroCommande) from velomax.commande_modelevelo cmv group by cmv.numeroCommande;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            double somme = 0;
            double nb = 0;
            string valueString = "";
            while (reader.Read())
            {
                somme = somme + Convert.ToDouble(reader.GetValue(0).ToString());
                nb = nb + 1;
            }
            double moyenne = somme / nb;
            reader.Close();
            command.Dispose();
            Console.WriteLine($"Nombre de velos moyens par commande: {moyenne}");
        }

        /// <summary>
        /// Affiche la moyene des montants des commandes
        /// </summary>
        public static void MoyenneMontantCommande()
        {
            // somme des commandes de velos par commande
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select cmv.numeroCommande,sum(mv.prixUnitaire) from velomax.commande_modelevelo cmv join velomax.modelevelo mv on cmv.numeroModele = mv.numeroModele and cmv.grandeur = mv.grandeur group by cmv.numeroCommande;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valueString = new string[reader.FieldCount];
            List<string[]> listeCommandeVelo= new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                listeCommandeVelo.Add(valuesString);
            }

            reader.Close();
            command.Dispose();

            // somme des commandes de pieces par commande
            requete = $"select cp.numeroCommande,sum(mp.prixVenteUnitaire) from velomax.commande_piece cp join velomax.modelepiece mp on mp.codeModelePiece = cp.codeModelePiece  group by cp.numeroCommande;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            valueString = new string[reader.FieldCount];
            List<string[]> listeCommandePiece = new List<string[]>();
            while (reader.Read())
            {
                string[] valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                listeCommandePiece.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            // regroupement des listes en fonction du numero de commande
            double somme = 0;
            double nombre = 0;
            for (int i = 0; i < listeCommandeVelo.Count(); i++)
            {
                bool test = false; ;
                for (int j = 0; j < listeCommandePiece.Count; j++)
                {
                    if (listeCommandePiece[j][0] == listeCommandeVelo[i][0])
                    {
                        test = true;
                        somme = somme + (Convert.ToInt32(listeCommandePiece[j][1]) + Convert.ToInt32(listeCommandeVelo[i][1])) / 2;
                        nombre = nombre + 1;
                    }
                }
                if (test == false)
                {
                    somme = somme + Convert.ToInt32(listeCommandeVelo[i][1]);
                    nombre = nombre + 1;
                }
            }
            for (int i = 0; i < listeCommandePiece.Count; i++)
            {
                bool test = false; ;
                for (int j = 0; j < listeCommandeVelo.Count(); j++)
                {
                    if (listeCommandePiece[i][0] == listeCommandeVelo[j][0])
                    {
                        test = true;
                    }
                }
                if (test == false)
                {
                    somme = somme + Convert.ToInt32(listeCommandePiece[i][1]);
                    nombre = nombre + 1;
                }
            }

            Console.WriteLine($"La moyenne des commandes est de {somme / nombre} euros");

        }

        

        

    }
}
