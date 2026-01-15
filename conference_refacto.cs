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
//
// Notes personnelles :
// 
// Convention de nommage C# :
//      _nomVariablePrivée = champ privé
//      nomVariableLocale = variable locale
//      NomPropriete = propriété publique
//
// Architecture :
//
// GestionnaireEntreprise 
//    ├── EmployeRepository
//    ├── SalaireCalculateur
//    ├── RapportGenerateur
//    ├── EmailService
//    └── CongesService

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

    public class EmployeRepository
    {
        private readonly List<Employe> _employes = new List<Employe>();

        public void Ajouter(Employe employe)
        {
            _employes.Add(employe);
        }

        public Employe? Rechercher(string prenom, string nom)
        {
            for (int i = 0; i < _employes.Count; i++)
            {
                if (_employes[i].Prenom == prenom && _employes[i].Nom == nom)
                {
                    return _employes[i];
                }
            }
            return null;
        }

        public IReadOnlyList<Employe> GetAll() => _employes.AsReadOnly();
    }

    public class SalaireCalculateur
    {
        private const double ChargesCdi = 0.23;
        private const double ChargesCdd = 0.20;
        private const double PrimeCdi = 0.10;
        private const double PrimeCdd = 0.08;
        private const double BonusCdi = 500;
        private const double BonusFreelance = 1000;
        private const double MajorationHeuresSupp = 1.25;

        public double CalculerSalaireNet(Employe employe, int mois, int annee,
            bool avecPrime, bool avecBonus, double tauxHoraire, int heuresSupp)
        {
            double salaireBase = employe.Salaire;
            string typeContrat = employe.TypeContrat;
            double resultat = 0;

            if (typeContrat == "CDI")
            {
                resultat = salaireBase;
                resultat = resultat - (resultat * ChargesCdi);
                if (heuresSupp > 0)
                {
                    resultat = resultat + (heuresSupp * tauxHoraire * MajorationHeuresSupp);
                }
                if (avecPrime)
                {
                    resultat = resultat + (salaireBase * PrimeCdi);
                }
                if (avecBonus)
                {
                    resultat = resultat + BonusCdi;
                }
            }
            else if (typeContrat == "CDD")
            {
                resultat = salaireBase;
                resultat = resultat - (resultat * ChargesCdd);
                if (heuresSupp > 0)
                {
                    resultat = resultat + (heuresSupp * tauxHoraire * MajorationHeuresSupp);
                }
                if (avecPrime)
                {
                    resultat = resultat + (salaireBase * PrimeCdd);
                }
            }
            else if (typeContrat == "Stagiaire")
            {
                resultat = salaireBase;
            }
            else if (typeContrat == "Freelance")
            {
                resultat = salaireBase;
                if (heuresSupp > 0)
                {
                    resultat = resultat + (heuresSupp * tauxHoraire);
                }
                if (avecBonus)
                {
                    resultat = resultat + BonusFreelance;
                }
            }

            return resultat;
        }
    }

    public class RapportGenerateur
    {
        private const string RapportPdf = "PDF";
        private const string RapportHtml = "HTML";
        private const string RapportCsv = "CSV";
        private const string RapportJson = "JSON";

        public string Generer(Employe employe, string typeRapport,
            bool inclureAdresse, bool inclureSalaire, bool inclureConges)
        {
            string rapport = "";

            if (typeRapport == RapportPdf)
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
            else if (typeRapport == RapportHtml)
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
            else if (typeRapport == RapportCsv)
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
            else if (typeRapport == RapportJson)
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
    }

    public class EmailService
    {
        public void Envoyer(Employe employe, string sujet, string corps,
            bool avecCopie, string emailCopie, bool urgent, bool accuseReception)
        {
            Console.WriteLine("Envoi email à: " + employe.Email);
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
    }

    public class CongesService
    {
        public int CalculerCongesRestants(Employe employe, int annee)
        {
            string typeContrat = employe.TypeContrat;
            int conges = 0;

            if (typeContrat == "CDI")
            {
                conges = 25;
            }
            else if (typeContrat == "CDD")
            {
                conges = 20;
            }
            else if (typeContrat == "Stagiaire")
            {
                conges = 0;
            }
            else if (typeContrat == "Freelance")
            {
                conges = 0;
            }

            return conges;
        }
    }

    public class GestionnaireEntreprise
    {
        private readonly EmployeRepository _repository = new EmployeRepository();
        private readonly SalaireCalculateur _salaireCalculateur = new SalaireCalculateur();
        private readonly RapportGenerateur _rapportGenerateur = new RapportGenerateur();
        private readonly EmailService _emailService = new EmailService();
        private readonly CongesService _congesService = new CongesService();

        public void AjouterEmploye(Employe employe)
        {
            _repository.Ajouter(employe);
        }

        public Employe? RechercherEmploye(string prenom, string nom)
        {
            return _repository.Rechercher(prenom, nom);
        }

        public double CalculerSalaireNet(string prenom, string nom, int mois, int annee,
            bool avecPrime, bool avecBonus, double tauxHoraire, int heuresSupp)
        {
            var employe = RechercherEmploye(prenom, nom);
            if (employe == null) return 0;
            return _salaireCalculateur.CalculerSalaireNet(employe, mois, annee, avecPrime, avecBonus, tauxHoraire, heuresSupp);
        }

        public string GenererRapport(string prenom, string nom, string typeRapport,
            bool inclureAdresse, bool inclureSalaire, bool inclureConges)
        {
            var employe = RechercherEmploye(prenom, nom);
            if (employe == null) return "Employé non trouvé";
            return _rapportGenerateur.Generer(employe, typeRapport, inclureAdresse, inclureSalaire, inclureConges);
        }

        public void EnvoyerEmail(string prenom, string nom, string sujet, string corps,
            bool avecCopie, string emailCopie, bool urgent, bool accuseReception)
        {
            var employe = RechercherEmploye(prenom, nom);
            if (employe == null)
            {
                Console.WriteLine("Employé non trouvé");
                return;
            }
            _emailService.Envoyer(employe, sujet, corps, avecCopie, emailCopie, urgent, accuseReception);
        }

        public int CalculerCongesRestants(string prenom, string nom, int annee)
        {
            var employe = RechercherEmploye(prenom, nom);
            if (employe == null) return 0;
            return _congesService.CalculerCongesRestants(employe, annee);
        }

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
