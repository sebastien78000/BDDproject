using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    public static class Client
    {

        public static void CreerClient()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            Console.WriteLine("entreprise(2) ou particulier(1)");
            int choix = Convert.ToInt32(Console.ReadLine());
            switch (choix)
            {
                case 1:

                    Console.WriteLine("nom ?");
                    string nom = Console.ReadLine();
                    Console.WriteLine("prenom ?");
                    string prenom = Console.ReadLine();
                    Console.WriteLine("rue ?");
                    string rue = Console.ReadLine();
                    Console.WriteLine("ville ?");
                    string ville = Console.ReadLine();
                    Console.WriteLine("code postal ?");
                    string codePostal = Console.ReadLine();
                    Console.WriteLine("telephone ?");
                    string telephone = Console.ReadLine();
                    Console.WriteLine("email ?");
                    string email = Console.ReadLine();
                    Console.WriteLine("Programme de fidélité");
                    string pfidélité = Console.ReadLine();
                    
                    string donnees = $"'{nom}','{prenom}','{rue}','{ville}','{codePostal}','{telephone}','{email}','{pfidélité}'";

                    string requete = $"INSERT INTO `VeloMax`.`client` (`nom`, `prenom`, `rue`, `ville`, `codePostal`, `telephone`, `email`,`numeroProgramme`) VALUES({donnees});";
                    MySqlCommand command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;

                case 2:

                    Console.WriteLine("nom ?");
                    nom = Console.ReadLine();
                    Console.WriteLine("prenom ?");
                    prenom = Console.ReadLine();
                    Console.WriteLine("rue ?");
                    rue = Console.ReadLine();
                    Console.WriteLine("ville ?");
                    ville = Console.ReadLine();
                    Console.WriteLine("code postal ?");
                    codePostal = Console.ReadLine();
                    Console.WriteLine("telephone ?");
                    telephone = Console.ReadLine();
                    Console.WriteLine("email ?");
                    email = Console.ReadLine();
                    Console.WriteLine("nom Compagnie ?");
                    string nomCompagnie = Console.ReadLine();
                    Console.WriteLine("remise commerciale ?");
                    string remise = Console.ReadLine();

                    donnees = $"'{nom}','{prenom}','{rue}','{ville}','{codePostal}','{telephone}','{email}','{nomCompagnie}','{remise}'";

                    requete = $"INSERT INTO `VeloMax`.`client` (`nom`, `prenom`, `rue`, `ville`, `codePostal`, `telephone`, `email`,`nomCompagnie`,`remiseCommercialeCompagnie`) VALUES({donnees});";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;

            }

        }

        public static void SupprimerClient()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            LireDataClient();

            Console.WriteLine("Supprimer client grace à son numero client");
            string nbClient = Console.ReadLine();

            string requete = $"delete from VeloMax.client where client.numeroClient='{nbClient}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

        }

        public static void ModifierClient()
        {
            Console.WriteLine("numero Client ?");
            string nClient = Console.ReadLine();
            LireDataClient();

            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            Console.WriteLine("individu(1) ou entreprise(2) ?");
            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    Console.WriteLine("Que voulez vous modifier (sachant que le numero client ne peut pas etre modifié)\n" +
                        "nom (1)\n" +
                        "prenom (2)\n" +
                        "rue (3)\n" +
                        "ville (4)\n" +
                        "code postal (5)\n" +
                        "telephone (6)\n" +
                        "email (7)\n" +
                        "programme de fidélité (8)\n");
                    string modif = "";
                    int modification = Convert.ToInt32(Console.ReadLine());
                    switch (modification)
                    {
                        case 1:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            string requete = $"update VeloMax.client set client.nom='{modif}' where numeroClient='{nClient}'";
                            MySqlCommand command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 2:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.prenom='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 3:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.rue='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 4:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.ville='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 5:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.codePostal'{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 6:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.telephone='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 7:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.email='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 8:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.numeroProgramme='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;

                    }

                    break;

                case "2":
                    Console.WriteLine("Que voulez vous modifier (sachant que le numero client ne peut pas etre modifié)\n" +
                        "nom (1)\n" +
                        "prenom (2)\n" +
                        "rue (3)\n" +
                        "ville (4)\n" +
                        "code postal (5)\n" +
                        "telephone (6)\n" +
                        "email (7)\n" +
                        "programme de fidélité (8)\n");
                    modif = "";
                    modification = Convert.ToInt32(Console.ReadLine());
                    switch (modification)
                    {
                        case 1:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            string requete = $"update VeloMax.client set client.nom='{modif}' where numeroClient='{nClient}'";
                            MySqlCommand command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            MySqlDataReader reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 2:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.prenom='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 3:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.rue='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 4:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.ville='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 5:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.codePostal'{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 6:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.telephone='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 7:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.email='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 8:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.nomCompagnie='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;
                        case 9:
                            Console.WriteLine("entrer modification");
                            modif = Console.ReadLine();
                            requete = $"update VeloMax.client set client.remiseCommercialeCompagnie='{modif}' where numeroClient='{nClient}'";
                            command = maConnexion.CreateCommand();
                            command.CommandText = requete;
                            reader = command.ExecuteReader();
                            reader.Close();
                            command.Dispose();
                            break;

                    }
                    break;

                default:
                    break;
            }
            
            
        }

        public static void LireDataClient()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"select * from veloMax.client;";
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
