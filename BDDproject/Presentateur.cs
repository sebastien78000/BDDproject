using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Diagnostics;

namespace BDDproject
{
    public static class Presentateur
    {
        // nombre de clients
        // client+cumul de leurs achats
        // liste des produits ayant une quantite en stock inferieur ou egale à 2
        // Nombre de pièces et/ou velos fournis par le fournisseur
        // Export en XML
        
        /// <summary>
        /// Affiche le nombre de clients dans la base de données
        /// </summary>
        public static void NbClients()
        {
            
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=root;PASSWORD=root";
            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = "SELECT count(client.numeroClient) FROM VeloMax.client;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            string valueString = "";
            while (reader.Read())
            {
                valueString = reader.GetValue(0).ToString();

            }
            Console.WriteLine($"Notre base de données se compose de {valueString} clients");
            reader.Close();
            command.Dispose();
            maConnexion.Close();

            Console.WriteLine();
        }

        /// <summary>
        /// Affiche les clients ainsi que le cumul de tout leur argent
        /// </summary>
        public static void ClientEtCumulDesachats()
        {
            // somme des commandes de velos par client
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=VeloMax;" +
                                        "UID=bozo;PASSWORD=bozo";

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
            Console.WriteLine();
            Console.WriteLine("Nom de l'entreprise ou du particulier | dépenses cumulées");
            for(int i=0;i<final.Count();i++)
            {
                // verifier compagnie ou personne
                requete = $"select nomCompagnie from velomax.client where client.numeroClient={final[i][0]};";
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
                if (valueString2 == "")
                {
                    requete = $"select prenom,nom from velomax.client where client.numeroClient={final[i][0]};";
                    command = maConnexion.CreateCommand();
                    command.CommandText = requete;
                    reader = command.ExecuteReader();
                    string[] valuesString = new string[reader.FieldCount];
                    Console.Write("Particulier: ");
                    while (reader.Read())
                    {
                        valuesString = new string[reader.FieldCount];
                        for (int j = 0; j < reader.FieldCount; j++)
                        {
                            valuesString[j] = reader.GetValue(j).ToString();
                            Console.Write(valuesString[j]+" ");
                        }
                        Console.Write(" | "+final[i][1]+" euros");
                        Console.WriteLine();
                    }
                    reader.Close();
                    command.Dispose();
                }
                else
                {
                    Console.Write($"Entreprise: {valueString2} | ");
                    Console.Write(final[i][1]+" euros");
                    Console.WriteLine();

                }

            }

        }

        /// <summary>
        /// Affiche la liste des produits vendus ainsi que si le nombre de chaque produit est inferieur ou non à 2.
        /// </summary>
        public static void ListeDesProduitsQteInf2()
        {
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                       "DATABASE=VeloMax;" +
                                       "UID=bozo;PASSWORD=bozo";

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

            // modele piece existante
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

            // obtenir liste des pieces à commander
            List<string> pieceAcommander = new List<string>();
            for (int i = 0; i < pieceList.Count(); i++)
            {
                if (CodeModele.Contains(pieceList[i][0]) == true)
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
            for (int i = 0; i < CodeModele.Count(); i++)
            {
                bool test = false;
                for (int j = 0; j < pieceList.Count(); j++)
                {
                    if (CodeModele[i] == pieceList[j][0]) test = true;
                }
                if (test == false)
                {
                    pieceAcommander.Add(CodeModele[i]);
                }
            }

           

            pieceAcommander.OrderBy(name => name);
            // lecture de la liste des pieces à commander
            Console.WriteLine();
            Console.WriteLine("Pieces à commander (stock inferieur à 2):");
            for (int i = 0; i < pieceAcommander.Count(); i++)
            {
                Console.WriteLine(pieceAcommander[i]);
            }

            // velos en stock
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            requete = "SELECT numeroModele,grandeur,count(numeroModele) FROM VeloMax.velo where velo.vendu=false group by (numeroModele);";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string[]> veloList = new List<string[]>();
            valuesString = new string[reader.FieldCount];
            while (reader.Read())
            {
                valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                veloList.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            // modele velo existant
            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            requete = "select numeroModele,grandeur,nom from velomax.modelevelo;";
            command = maConnexion.CreateCommand();
            command.CommandText = requete;
            reader = command.ExecuteReader();
            List<string[]> ModeleVelo = new List<string[]>();
            valueString = "";
            while (reader.Read())
            {
                valuesString = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuesString[i] = reader.GetValue(i).ToString();
                }
                ModeleVelo.Add(valuesString);
            }
            reader.Close();
            command.Dispose();

            // obtenir liste des pieces à commander
            List<string[]> VeloAcommander = new List<string[]>();
            for (int i = 0; i < ModeleVelo.Count(); i++)
            {
                bool test = false;
                for(int j=0;j<veloList.Count();j++)
                {
                    if (ModeleVelo[i][0]==veloList[j][0] && ModeleVelo[i][1] == veloList[j][1])
                    {
                        test = true;
                        if(Convert.ToInt32(veloList[j][2])<2)
                        {
                            VeloAcommander.Add(ModeleVelo[i]);
                        }
                        else Console.WriteLine($"Quantité suffisante de {ModeleVelo[i][0]} : {ModeleVelo[i][2]} {ModeleVelo[i][1]} exite en stock");
                    }
                }
                if(test==false)
                {
                    VeloAcommander.Add(ModeleVelo[i]);
                }
            }

            // afficher velo
            Console.WriteLine();
            Console.WriteLine("Modeles de velos dont le stock assemblé est inferieur à 2:");
            for(int i=0;i<VeloAcommander.Count();i++)
            {
                Console.WriteLine(VeloAcommander[i][0]+" : "+ VeloAcommander[i][2] + " " + VeloAcommander[i][1]);
            }

        }
        
        /// <summary>
        /// Affiche les pieces fournies par chaque fournisseur
        /// </summary>
        public static void NombrePiecesParFournisseur()
        {
            Console.WriteLine("Siret   |   Nom entreprise   |   Pièce fournie");
            string connexionString = "SERVER=localhost;PORT=3306;" +
                             "DATABASE=VeloMax;" +
                             "UID=root;PASSWORD=root";

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();
            string requete = $"select numeroSiretFournisseur,nom,codeModelePiece from velomax.fournisseur_modelepiece join fournisseur on fournisseur.numeroSiret=fournisseur_modelepiece.numeroSiretFournisseur order by numeroSiretFournisseur;";
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
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
        /// Exporte la table fournisseur_modelepiece en xml
        /// </summary>
        public static void ExportTableModelePiece()
        {
            MySqlConnection maConnexion = null;
            string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=VeloMax;" +
                                         "UID=bozo;PASSWORD=bozo";

            maConnexion = new MySqlConnection(connexionString);
            maConnexion.Open();

            string requete = "SELECT fmp.codeModelePiece,fmp.numeroSiretFournisseur,fmp.prixAchat,fmp.delaiJoursApprovisionnement from velomax.fournisseur_modelepiece fmp ;";
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

            for (int i = 0; i < ListModelePiece.Count(); i++)
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

                XmlElement delai = docXml.CreateElement("delaiApprovisionnement");
                delai.InnerText = $"{ListModelePiece[i][3]}";
                piece.AppendChild(delai);

            }


            // enregistrement du document XML   ==> à retrouver dans le dossier bin\Debug de Visual Studio
            docXml.Save("./piece2.xml");
            Process.Start("piece2.xml");
            Console.WriteLine("fichier piece.xml créé");
        }
    }
}
