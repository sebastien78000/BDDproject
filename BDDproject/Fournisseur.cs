using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDproject
{
    class Fournisseur
    {


        public static void CreerFournisseur()
        {

            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            Console.WriteLine("numeroSiret ?");
            string siret = Console.ReadLine();
            Console.WriteLine("nom ?");
            string nom = Console.ReadLine();
            Console.WriteLine("telephone ?");
            string telephone = Console.ReadLine();
            Console.WriteLine("email ?");
            string email = Console.ReadLine();
            Console.WriteLine("rue ?");
            string rue = Console.ReadLine();
            Console.WriteLine("ville ?");
            string ville = Console.ReadLine();
            Console.WriteLine("code postal ?");
            string codePostal = Console.ReadLine();
            Console.WriteLine("libelle");
            string libelle = Console.ReadLine();

            string donnees = $"'{siret}','{nom}','{telephone}','{email}','{rue}','{ville}','{codePostal}','{libelle}'";

            string requete = $"INSERT INTO `VeloMax`.`client` (`siret`, `nom`, `telephone`, `email`, `rue`, `ville`, `codePostal`,'libelle') VALUES({donnees});";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

        }


        public static void SupprimerFournisseur()
        {
            LireDataFournisseur();
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            Console.WriteLine("Supprimer fournisseur");
            string numeroSiret = Console.ReadLine();

            string requete = $"Delete from VeloMax.fournisseur where fournissseur.numeroSiret='{numeroSiret}';";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

        }


        public static void LireDataFournisseur()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = $"select * from veloMax.fournisseur;";
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

        public static void ModifDataFournisseur()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root;";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            Console.WriteLine("siret ?");
            string siret = Console.ReadLine();

            Console.WriteLine("Que voulez vous modifier (sachant que le numero siret ne peut pas etre modifié)\n" +
                        "nom (1)\n" +
                        "telephone (2)\n" +
                        "email (3)\n" +
                        "rue (4)\n" +
                        "ville (5)\n" +
                        "code postal (6)\n" +
                        "libelle (7)\n");
            string modif = "";
            int modification = Convert.ToInt32(Console.ReadLine());
            switch (modification)
            {
                case 1:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    string requete = $"update VeloMax.fournisseur set client.nom='{modif}' where numeroSiret='{siret}'";
                    MySqlCommand command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 2:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.telephone='{modif}' where numeroSiret='{siret}";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 3:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.email='{modif}' where numeroSiret='{siret}'";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 4:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.rue='{modif}' where numeroSiret='{siret}'";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 5:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.ville'{modif}' where numeroSiret='{siret}'";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 6:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.codePostal='{modif}' where numeroSiret='{siret}";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;
                case 7:
                    Console.WriteLine("entrer modification");
                    modif = Console.ReadLine();
                    requete = $"update VeloMax.fournisseur set client.libelle='{modif}' where numeroSiret='{siret}";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    reader.Close();
                    command.Dispose();
                    break;


            }
        }
    }
}
