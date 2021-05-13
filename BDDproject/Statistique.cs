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
        public static void  ListeMembreProgrammeAdhesionAvecChoix()
        // affiche les membres du programme de fidelite choisi
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


        public static void ListeMembreProgrammeAdhesion()
        // affiche tous les membres a
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

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

        public static void dateExpirationProgrammesFideliteParClient()
        // renvoie chaque membre du programme de fidelite avec son prenom,son nom, la date d'adhesion et la date d'expiration de son programme
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";


            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            Console.WriteLine("prenom | nom | date d'adhesion programme | date d'expiration programme");
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

    }
}
