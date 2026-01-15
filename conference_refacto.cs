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
    public class Employe
    {
        public required string Prenom { get; set; }
        public required string Nom { get; set; }
        public required int Age { get; set; }
        public required Adresse Adresse { get; set; }
        public required string TypeContrat { get; set; }
        public required double Salaire { get; set; }
        public required string Email { get; set; }
        public required string Telephone { get; set; }
        public required string Departement { get; set; }
    }

    public class Adresse
    {
        public required string Rue { get; set; }
        public required string Ville { get; set; }
        public required string CodePostal { get; set; }
    }

    public class GestionnaireEntreprise
    {
        public List<Employe> employes = new List<Employe>();

        public Employe? RechercherEmploye(string prenom, string nom)
        {
            for (int i = 0; i < employes.Count; i++)
            {
                if (employes[i].Prenom == prenom && employes[i].Nom == nom)
                {
                    return employes[i];
                }
            }

            return null;
        }

        // Ajoute un employé avec tous ses paramètres
        // prenom : le prénom de l'employé
        // nom : le nom de l'employé
        // age : l'âge de l'employé
        // rue : la rue de l'adresse
        // ville : la ville de l'adresse
        // codePostal : le code postal
        // typeContrat : le type d'employé (CDI, CDD, Stagiaire, Freelance)
        // salaire : le salaire de base
        // departement : le département
        public void AjouterEmploye(Employe employe)
        {
            employes.Add(employe);
        }

        // Cette méthode calcule le salaire net d'un employé
        // Elle prend en compte le type d'employé, les primes, les taxes
        // Et plein d'autres trucs compliqués
        public double CalculerSalaireNet(string prenom, string nom, int mois, int annee,
            bool avecPrime, bool avecBonus, double tauxHoraire, int heuresSupp)
        {
            
            Employe employe = RechercherEmploye(prenom, nom);

            // On récupère le salaire de base
            double salaireBase = employe.Salaire;
            // On récupère le type
            string typeContrat = employe.TypeContrat;

            // Variable pour stocker le résultat
            double resultat = 0;

            // On calcule selon le type d'employé
            if (typeContrat == "CDI")
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
            else if (typeContrat == "CDD")
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
            else if (typeContrat == "Stagiaire")
            {
                // Les stagiaires ont une gratification fixe
                resultat = salaireBase;
                // Pas de charges pour les stagiaires
                // Pas d'heures supp
                // Pas de prime
                // Pas de bonus
            }
            else if (typeContrat == "Freelance")
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
            Employe employe = RechercherEmploye(prenom, nom);

            if (employe == null) return "Employé non trouvé";

            string rapport = "";

            // Selon le type de rapport
            if (typeRapport == "PDF")
            {
                rapport = "=== RAPPORT PDF ===\n";
                rapport = rapport + "Nom: " + employe.Nom + "\n";
                rapport = rapport + "Prénom: " + employe.Prenom + "\n";
                if (inclureAdresse)
                {
                    rapport = rapport + "Adresse: " + employe.Adresse.Rue + ", ";
                    rapport = rapport + employe.Adresse.CodePostal + " " + employe.Adresse.Ville + "\n";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + "Salaire: " + employe.Salaire + "€\n";
                }
                rapport = rapport + "===================\n";
            }
            else if (typeRapport == "HTML")
            {
                rapport = "<html><body>";
                rapport = rapport + "<h1>Rapport Employé</h1>";
                rapport = rapport + "<p>Nom: " + employe.Nom + "</p>";
                rapport = rapport + "<p>Prénom: " + employe.Prenom + "</p>";
                if (inclureAdresse)
                {
                    rapport = rapport + "<p>Adresse: " + employe.Adresse.Rue + ", ";
                    rapport = rapport + employe.Adresse.CodePostal + " " + employe.Adresse.Ville + "</p>";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + "<p>Salaire: " + employe.Salaire + "€</p>";
                }
                rapport = rapport + "</body></html>";
            }
            else if (typeRapport == "CSV")
            {
                rapport = "Nom;Prénom";
                if (inclureAdresse) rapport = rapport + ";Rue;CP;Ville";
                if (inclureSalaire) rapport = rapport + ";Salaire";
                rapport = rapport + "\n";
                rapport = rapport + employe.Nom + ";" + employe.Prenom;
                if (inclureAdresse)
                {
                    rapport = rapport + ";" + employe.Adresse.Rue;
                    rapport = rapport + ";" + employe.Adresse.CodePostal;
                    rapport = rapport + ";" + employe.Adresse.Ville;
                }
                if (inclureSalaire)
                {
                    rapport = rapport + ";" + employe.Salaire;
                }
            }
            else if (typeRapport == "JSON")
            {
                rapport = "{\n";
                rapport = rapport + "  \"nom\": \"" + employe.Nom + "\",\n";
                rapport = rapport + "  \"prenom\": \"" + employe.Prenom + "\"";
                if (inclureAdresse)
                {
                    rapport = rapport + ",\n  \"adresse\": {\n";
                    rapport = rapport + "    \"rue\": \"" + employe.Adresse.Rue + "\",\n";
                    rapport = rapport + "    \"cp\": \"" + employe.Adresse.CodePostal + "\",\n";
                    rapport = rapport + "    \"ville\": \"" + employe.Adresse.Ville + "\"\n";
                    rapport = rapport + "  }";
                }
                if (inclureSalaire)
                {
                    rapport = rapport + ",\n  \"salaire\": " + employe.Salaire;
                }
                rapport = rapport + "\n}";
            }

            return rapport;
        }

        // Envoie un email à l'employé
        public void EnvoyerEmail(string prenom, string nom, string sujet, string corps,
            bool avecCopie, string emailCopie, bool urgent, bool accuseReception)
        {
            Employe employe = RechercherEmploye(prenom, nom);

            if (employe == null)
            {
                Console.WriteLine("Employé non trouvé");
                return;
            }

            // On récupère l'email
            string email = employe.Email;

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
            Employe employe = RechercherEmploye(prenom, nom);

            if (employe == null) return 0;

            string typeContrat = employe.TypeContrat;
            int conges = 0;

            // Selon le type
            if (typeContrat == "CDI")
            {
                conges = 25; // 25 jours pour les CDI
            }
            else if (typeContrat == "CDD")
            {
                conges = 20; // 20 jours pour les CDD
            }
            else if (typeContrat == "Stagiaire")
            {
                conges = 0; // Pas de congés pour les stagiaires
            }
            else if (typeContrat == "Freelance")
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
        public required string nomEntreprise;
        public required string rueEntreprise;
        public required string cpEntreprise;
        public required string villeEntreprise;
        public required string siret;
        public double capital;

        // Données du directeur qui devraient être regroupées
        public required string prenomDirecteur;
        public required string nomDirecteur;
        public required string emailEntreprise;
        public required string telEntreprise;
    }

    // Classe de test
    public class Program
    {
        public static void Main()
        {
            GestionnaireEntreprise gestionnaire = new GestionnaireEntreprise();

            // Ajout d'employés via objet Employe
            gestionnaire.AjouterEmploye(new Employe
            {
                Prenom = "Jean",
                Nom = "Dupont",
                Age = 35,
                Adresse = new Adresse { Rue = "12 rue de la Paix", Ville = "Paris", CodePostal = "75001" },
                TypeContrat = "CDI",
                Salaire = 3500.0,
                Departement = "Informatique",
                Email = "jean.dupont@mail.com",
                Telephone = "0612345678"
            });

            gestionnaire.AjouterEmploye(new Employe
            {
                Prenom = "Marie",
                Nom = "Martin",
                Age = 28,
                Adresse = new Adresse { Rue = "5 avenue des Champs", Ville = "Lyon", CodePostal = "69001" },
                TypeContrat = "CDD",
                Salaire = 2800.0,
                Departement = "Marketing",
                Email = "marie.martin@mail.com",
                Telephone = "0698765432"
            });

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
