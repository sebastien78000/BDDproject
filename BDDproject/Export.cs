using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MySql.Data.MySqlClient;
using System.IO;

namespace BDDproject
{
    public class Export
    {
        public static void ExportXML()
        // Export des stocks faibles avec fournisseurs pour command en XML 
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = "SELECT fmp.codeModelePiece,fmp.numeroSiretFournisseur,fmp.prixAchat from velomax.fournisseur_modelepiece fmp where fmp.codeModelePiece in (SELECT p.codeModelePiece from velomax.piece p join fournisseur_modelepiece fmp on p.codeModelePiece=fmp.codeModelePiece and p.numeroSiretFournisseur=fmp.numeroSiretFournisseur join velomax.fournisseur f on fmp.numeroSiretFournisseur=f.numeroSiret where p.vendu=false and numeroVelo is null group by p.codeModelePiece having count(p.codeModelePiece)<2) order by fmp.codeModelePiece;"; 
            MySqlCommand command1 = maConnexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();
            List<string[]> ListModelePiece = new List<string[]>();
            while (reader.Read())
            {
                string[] valueString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                }
                ListModelePiece.Add(valueString);
            }
            reader.Close();
            command1.Dispose();
            maConnexion.Close();




            XmlDocument docXml = new XmlDocument();

            // Création de l'élément racine... qu'on ajoute au document
            XmlElement racine = docXml.CreateElement("Pieces");
            docXml.AppendChild(racine);

            // création de l'en-tête XML (no <=> pas de DTD associée)
            XmlDeclaration xmldecl = docXml.CreateXmlDeclaration("1.0", "UTF-8", "no");
            docXml.InsertBefore(xmldecl, racine);

            for(int i=0; i<ListModelePiece.Count();i++)
            {
                XmlElement piece = docXml.CreateElement("piece");
                racine.AppendChild(piece);

                
                XmlElement codeModelePiece = docXml.CreateElement("codeModelePiece");
                codeModelePiece.InnerText = $"{ListModelePiece[i][0]}";
                piece.AppendChild(codeModelePiece);


                XmlElement fournisseur = docXml.CreateElement("fournisseur");
                fournisseur.InnerText = $"{ListModelePiece[i][1]}";
                piece.AppendChild(fournisseur);


                XmlElement prixAchat = docXml.CreateElement("prixAchat");
                prixAchat.InnerText = $"{ListModelePiece[i][2]}";
                piece.AppendChild(prixAchat);


            }

            



            // enregistrement du document XML   ==> à retrouver dans le dossier bin\Debug de Visual Studio
            docXml.Save("./piece.xml");
            Console.WriteLine("fichier piece.xml créé");
        }



        public static void ExportJSON()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = "select c.prenom,c.nom,c.email,c.dateAdhesion,p.description  from velomax.client c join velomax.programmefidelite p on p.numeroProgramme = c.numeroProgramme where c.numeroProgramme is not null and datediff(date_add(c.dateAdhesion, interval p.dureeMois month),curdate())> -60 and datediff(date_add(c.dateAdhesion, interval p.dureeMois month),curdate())< 0;";
            MySqlCommand command1 = maConnexion.CreateCommand();
            command1.CommandText = requete;

            MySqlDataReader reader = command1.ExecuteReader();
            List<string[]> ListClients = new List<string[]>();
            while (reader.Read())
            {
                string[] valueString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valueString[i] = reader.GetValue(i).ToString();
                }
                ListClients.Add(valueString);
            }
            reader.Close();
            command1.Dispose();
            maConnexion.Close();

            string json = "{"+'"'+"clients"+'"'+':'+'[';
            for (int i=0; i<ListClients.Count;i++)
            {

                string prenom = '"'+"prenom"+'"'+':'+'"'+ListClients[i][0]+'"';
                string nom = '"' + "nom" + '"' + ':' + '"' + ListClients[i][1] + '"';
                string email = '"' + "email" + '"' + ':' + '"' + ListClients[i][2] + '"';
                string dateAdhesion = '"' + "dateAdhesion" + '"' + ':' + '"' + ListClients[i][3] + '"';
                string programme = '"' + "programme" + '"' + ':' + '"' + ListClients[i][4] + '"';
                if (i!= ListClients.Count-1)
                {
                    json =json+ "{" + $"{prenom},{nom},{email},{dateAdhesion},{programme}" + "},";
                }
                else
                {
                    json = json + "{" + $"{prenom},{nom},{email},{dateAdhesion},{programme}" + "}]}";
                }
                
            }

            using (StreamWriter sw = new StreamWriter("./clients.json"))
            {               
                 sw.WriteLine(json);
            }
            Console.WriteLine("Fichier JSON créé");













        }
    }
}
