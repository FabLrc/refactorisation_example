// ============================================================================
// EXERCICE DE REFACTORING - CODE SMELL DETECTOR
// ============================================================================
// Ce code contient TOUS les code smells suivants :
// - Fonction longue (> 20-30 lignes)
// - Classe géante (trop de responsabilités)
// - Liste de paramètres longue (> 3 paramètres)
// - Commentaires excessifs (code peu clair)
// - Code dupliqué (copier-coller)
// - Feature Envy (méthode utilise trop une autre classe)
// - Data Clumps (données toujours ensemble)
// - Switch statements (switch/if-else répétés)
//
// OBJECTIF : Refactoriser ce code en appliquant les principes Clean Code
// ============================================================================

using System;
using System.Collections.Generic;

namespace ExerciceRefactoring
{
    // Cette classe fait TOUT : gestion des employés, calcul des salaires,
    // génération de rapports, envoi d'emails, gestion des congés...
    public class GestionnaireEntreprise
    {
        public List<object[]> employes = new List<object[]>();

        // Ajoute un employé avec tous ses paramètres
        // prenom : le prénom de l'employé
        // nom : le nom de l'employé
        // age : l'âge de l'employé
        // rue : la rue de l'adresse
        // ville : la ville de l'adresse
        // codePostal : le code postal
        // type : le type d'employé (CDI, CDD, Stagiaire, Freelance)
        // salaire : le salaire de base
        // departement : le département
        public void AjouterEmploye(string prenom, string nom, int age,
            string rue, string ville, string codePostal,
            string type, double salaire, string departement,
            string email, string telephone)
        {
            // On stocke tout dans un tableau d'objets
            object[] emp = new object[11];
            emp[0] = prenom;
            emp[1] = nom;
            emp[2] = age;
            emp[3] = rue;
            emp[4] = ville;
            emp[5] = codePostal;
            emp[6] = type;
            emp[7] = salaire;
            emp[8] = departement;
            emp[9] = email;
            emp[10] = telephone;
            employes.Add(emp);
        }

        // Cette méthode calcule le salaire net d'un employé
        // Elle prend en compte le type d'employé, les primes, les taxes
        // Et plein d'autres trucs compliqués
        public double CalculerSalaireNet(string prenom, string nom, int mois, int annee,
            bool avecPrime, bool avecBonus, double tauxHoraire, int heuresSupp)
        {
            // D'abord on cherche l'employé
            object[] employe = null;
            // On parcourt la liste pour trouver l'employé
            for (int i = 0; i < employes.Count; i++)
            {
                // On vérifie si c'est le bon employé
                if ((string)employes[i][0] == prenom && (string)employes[i][1] == nom)
                {
                    // C'est lui !
                    employe = employes[i];
                    break;
                }
            }

            // Si on n'a pas trouvé l'employé
            if (employe == null)
            {
                // On retourne 0
                return 0;
            }

            // On récupère le salaire de base
            double salaireBase = (double)employe[7];
            // On récupère le type
            string type = (string)employe[6];

            // Variable pour stocker le résultat
            double resultat = 0;

            // On calcule selon le type d'employé
            if (type == "CDI")
            {
                // Pour les CDI, on applique les charges patronales de 23%
                resultat = salaireBase;
                // On retire les charges
                resultat = resultat - (resultat * 0.23);
                // On ajoute les heures supp si il y en a
                if (heuresSupp > 0)
                {
                    // Les heures supp sont majorées de 25%
                    resultat = resultat + (heuresSupp * tauxHoraire * 1.25);
                }
                // On ajoute la prime si demandé
                if (avecPrime)
                {
                    // Prime de 10% du salaire
                    resultat = resultat + (salaireBase * 0.10);
                }
                // On ajoute le bonus si demandé
                if (avecBonus)
                {
                    // Bonus fixe de 500€
                    resultat = resultat + 500;
                }
            }
            else if (type == "CDD")
            {
                // Pour les CDD, charges de 20%
                resultat = salaireBase;
                // On retire les charges
                resultat = resultat - (resultat * 0.20);
                // On ajoute les heures supp si il y en a
                if (heuresSupp > 0)
                {
                    // Les heures supp sont majorées de 25%
                    resultat = resultat + (heuresSupp * tauxHoraire * 1.25);
                }
                // On ajoute la prime si demandé
                if (avecPrime)
                {
                    // Prime de 8% du salaire pour les CDD
                    resultat = resultat + (salaireBase * 0.08);
                }
                // Pas de bonus pour les CDD
            }
            else if (type == "Stagiaire")
            {
                // Les stagiaires ont une gratification fixe
                resultat = salaireBase;
                // Pas de charges pour les stagiaires
                // Pas d'heures supp
                // Pas de prime
                // Pas de bonus
            }
            else if (type == "Freelance")
            {
                // Les freelances sont payés à la journée
                resultat = salaireBase;
                // On ajoute les heures supp si il y en a
                if (heuresSupp > 0)
                {
                    // Pas de majoration pour les freelances
                    resultat = resultat + (heuresSupp * tauxHoraire);
                }
                // On ajoute le bonus si demandé
                if (avecBonus)
                {
                    // Bonus de 1000€ pour les freelances
                    resultat = resultat + 1000;
                }
            }

            // On retourne le résultat
            return resultat;
        }

        // Génère un rapport pour un employé
        public string GenererRapport(string prenom, string nom, string typeRapport,
            bool inclureAdresse, bool inclureSalaire, bool inclureConges)
        {
            // D'abord on cherche l'employé
            object[] employe = null;
            // On parcourt la liste pour trouver l'employé
            for (int i = 0; i < employes.Count; i++)
            {
                // On vérifie si c'est le bon employé
                if ((string)employes[i][0] == prenom && (string)employes[i][1] == nom)
                {
                    // C'est lui !
                    employe = employes[i];
                    break;
                }
            }

            // Si on n'a pas trouvé
            if (employe == null)
            {
                return "Employé non trouvé";
            }

            string rapport = "";

            // Selon le type de rapport
            if (typeRapport == "PDF")
            {
                rapport = "=== RAPPORT PDF ===\n";
                rapport = rapport + "Nom: " + (string)employe[1] + "\n";
                rapport = rapport + "Prénom: " + (string)employe[0] + "\n";
                if (inclureAdresse)
                {
                    rapport = rapport + "Adresse: " + (string)employe[3] + ", ";
                    rapport = rapport + (string)employe[5] + " " + (string)employe[4] + "\n";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + "Salaire: " + (double)employe[7] + "€\n";
                }
                rapport = rapport + "===================\n";
            }
            else if (typeRapport == "HTML")
            {
                rapport = "<html><body>";
                rapport = rapport + "<h1>Rapport Employé</h1>";
                rapport = rapport + "<p>Nom: " + (string)employe[1] + "</p>";
                rapport = rapport + "<p>Prénom: " + (string)employe[0] + "</p>";
                if (inclureAdresse)
                {
                    rapport = rapport + "<p>Adresse: " + (string)employe[3] + ", ";
                    rapport = rapport + (string)employe[5] + " " + (string)employe[4] + "</p>";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + "<p>Salaire: " + (double)employe[7] + "€</p>";
                }
                rapport = rapport + "</body></html>";
            }
            else if (typeRapport == "CSV")
            {
                rapport = "Nom;Prénom";
                if (inclureAdresse) rapport = rapport + ";Rue;CP;Ville";
                if (inclureSalaire) rapport = rapport + ";Salaire";
                rapport = rapport + "\n";
                rapport = rapport + (string)employe[1] + ";" + (string)employe[0];
                if (inclureAdresse)
                {
                    rapport = rapport + ";" + (string)employe[3];
                    rapport = rapport + ";" + (string)employe[5];
                    rapport = rapport + ";" + (string)employe[4];
                }
                if (inclureSalaire)
                {
                    rapport = rapport + ";" + (double)employe[7];
                }
            }
            else if (typeRapport == "JSON")
            {
                rapport = "{\n";
                rapport = rapport + "  \"nom\": \"" + (string)employe[1] + "\",\n";
                rapport = rapport + "  \"prenom\": \"" + (string)employe[0] + "\"";
                if (inclureAdresse)
                {
                    rapport = rapport + ",\n  \"adresse\": {\n";
                    rapport = rapport + "    \"rue\": \"" + (string)employe[3] + "\",\n";
                    rapport = rapport + "    \"cp\": \"" + (string)employe[5] + "\",\n";
                    rapport = rapport + "    \"ville\": \"" + (string)employe[4] + "\"\n";
                    rapport = rapport + "  }";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + ",\n  \"salaire\": " + (double)employe[7];
                }
                rapport = rapport + "\n}";
            }

            return rapport;
        }

        // Envoie un email à l'employé
        public void EnvoyerEmail(string prenom, string nom, string sujet, string corps,
            bool avecCopie, string emailCopie, bool urgent, bool accuseReception)
        {
            // D'abord on cherche l'employé
            object[] employe = null;
            // On parcourt la liste pour trouver l'employé
            for (int i = 0; i < employes.Count; i++)
            {
                // On vérifie si c'est le bon employé
                if ((string)employes[i][0] == prenom && (string)employes[i][1] == nom)
                {
                    // C'est lui !
                    employe = employes[i];
                    break;
                }
            }

            if (employe == null)
            {
                Console.WriteLine("Employé non trouvé");
                return;
            }

            // On récupère l'email
            string email = (string)employe[9];

            // On construit l'email
            Console.WriteLine("Envoi email à: " + email);
            Console.WriteLine("Sujet: " + sujet);
            Console.WriteLine("Corps: " + corps);
            if (urgent)
            {
                Console.WriteLine("[URGENT]");
            }
            if (accuseReception)
            {
                Console.WriteLine("[ACCUSE DE RECEPTION DEMANDE]");
            }
            if (avecCopie)
            {
                Console.WriteLine("Copie à: " + emailCopie);
            }
        }

        // Calcule les congés restants
        public int CalculerCongesRestants(string prenom, string nom, int annee)
        {
            // D'abord on cherche l'employé
            object[] employe = null;
            // On parcourt la liste pour trouver l'employé
            for (int i = 0; i < employes.Count; i++)
            {
                // On vérifie si c'est le bon employé
                if ((string)employes[i][0] == prenom && (string)employes[i][1] == nom)
                {
                    // C'est lui !
                    employe = employes[i];
                    break;
                }
            }

            if (employe == null) return 0;

            string type = (string)employe[6];
            int conges = 0;

            // Selon le type
            if (type == "CDI")
            {
                conges = 25; // 25 jours pour les CDI
            }
            else if (type == "CDD")
            {
                conges = 20; // 20 jours pour les CDD
            }
            else if (type == "Stagiaire")
            {
                conges = 0; // Pas de congés pour les stagiaires
            }
            else if (type == "Freelance")
            {
                conges = 0; // Pas de congés pour les freelances
            }

            return conges;
        }

        // Affiche les infos d'un employé en utilisant trop les données d'autres objets
        public void AfficherInfosEmploye(AutreClasse autre)
        {
            // Feature Envy : on utilise énormément les données de AutreClasse
            Console.WriteLine("Nom entreprise: " + autre.nomEntreprise);
            Console.WriteLine("Adresse entreprise: " + autre.rueEntreprise + ", " +
                autre.cpEntreprise + " " + autre.villeEntreprise);
            Console.WriteLine("SIRET: " + autre.siret);
            Console.WriteLine("Capital: " + autre.capital);
            Console.WriteLine("Directeur: " + autre.prenomDirecteur + " " + autre.nomDirecteur);
            Console.WriteLine("Contact: " + autre.emailEntreprise + " / " + autre.telEntreprise);

            // Calcul fait avec les données de l'autre classe
            double tauxTVA = autre.capital > 1000000 ? 0.20 : 0.10;
            Console.WriteLine("Taux TVA applicable: " + (tauxTVA * 100) + "%");
        }
    }

    // Classe avec des Data Clumps évidents
    public class AutreClasse
    {
        // Données de l'entreprise qui devraient être regroupées
        public string nomEntreprise;
        public string rueEntreprise;
        public string cpEntreprise;
        public string villeEntreprise;
        public string siret;
        public double capital;

        // Données du directeur qui devraient être regroupées
        public string prenomDirecteur;
        public string nomDirecteur;
        public string emailEntreprise;
        public string telEntreprise;
    }

    // Classe de test
    public class Program
    {
        public static void Main()
        {
            GestionnaireEntreprise gestionnaire = new GestionnaireEntreprise();

            // Ajout d'employés avec trop de paramètres
            gestionnaire.AjouterEmploye("Jean", "Dupont", 35,
                "12 rue de la Paix", "Paris", "75001",
                "CDI", 3500.0, "Informatique",
                "jean.dupont@mail.com", "0612345678");

            gestionnaire.AjouterEmploye("Marie", "Martin", 28,
                "5 avenue des Champs", "Lyon", "69001",
                "CDD", 2800.0, "Marketing",
                "marie.martin@mail.com", "0698765432");

            // Calcul de salaire avec trop de paramètres
            double salaire = gestionnaire.CalculerSalaireNet("Jean", "Dupont",
                1, 2024, true, true, 25.0, 10);
            Console.WriteLine("Salaire net: " + salaire);

            // Génération de rapport
            string rapport = gestionnaire.GenererRapport("Jean", "Dupont",
                "JSON", true, true, false);
            Console.WriteLine(rapport);
        }
    }
}

// ============================================================================
// QUESTIONS DE REFACTORING :
// ============================================================================
// 1. Identifiez tous les code smells présents dans ce code
// 2. Proposez une architecture propre avec des classes séparées
// 3. Appliquez le polymorphisme pour éliminer les switch/if-else
// 4. Créez des objets pour regrouper les Data Clumps
// 5. Réduisez le nombre de paramètres des méthodes
// 6. Éliminez le code dupliqué
// 7. Supprimez les commentaires inutiles et rendez le code auto-documenté
// 8. Corrigez le Feature Envy
// ============================================================================
