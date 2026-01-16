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

        private IContratStrategy? _contrat;
        public IContratStrategy Contrat => _contrat ??= ContratFactory.CreerContrat(TypeContrat);
    }

    public class Adresse
    {
        public required string Rue { get; set; }
        public required string Ville { get; set; }
        public required string CodePostal { get; set; }
    }

    public interface IContratStrategy
    {
        double CalculerSalaireNet(double salaireBase, double tauxHoraire, int heuresSupp, bool avecPrime, bool avecBonus);
        int GetCongesAnnuels();
        string GetNomContrat();
    }

    public class ContratCDI : IContratStrategy
    {
        private const double Charges = 0.23;
        private const double TauxPrime = 0.10;
        private const double Bonus = 500;
        private const double MajorationHeuresSupp = 1.25;

        public double CalculerSalaireNet(double salaireBase, double tauxHoraire, int heuresSupp, bool avecPrime, bool avecBonus)
        {
            double resultat = salaireBase - (salaireBase * Charges);

            if (heuresSupp > 0)
            {
                resultat += heuresSupp * tauxHoraire * MajorationHeuresSupp;
            }
            if (avecPrime)
            {
                resultat += salaireBase * TauxPrime;
            }
            if (avecBonus)
            {
                resultat += Bonus;
            }

            return resultat;
        }

        public int GetCongesAnnuels() => 25;
        public string GetNomContrat() => "CDI";
    }

    public class ContratCDD : IContratStrategy
    {
        private const double Charges = 0.20;
        private const double TauxPrime = 0.08;
        private const double MajorationHeuresSupp = 1.25;

        public double CalculerSalaireNet(double salaireBase, double tauxHoraire, int heuresSupp, bool avecPrime, bool avecBonus)
        {
            double resultat = salaireBase - (salaireBase * Charges);

            if (heuresSupp > 0)
            {
                resultat += heuresSupp * tauxHoraire * MajorationHeuresSupp;
            }
            if (avecPrime)
            {
                resultat += salaireBase * TauxPrime;
            }

            return resultat;
        }

        public int GetCongesAnnuels() => 20;
        public string GetNomContrat() => "CDD";
    }

    public class ContratStagiaire : IContratStrategy
    {
        public double CalculerSalaireNet(double salaireBase, double tauxHoraire, int heuresSupp, bool avecPrime, bool avecBonus)
        {
            return salaireBase;
        }

        public int GetCongesAnnuels() => 0;
        public string GetNomContrat() => "Stagiaire";
    }

    public class ContratFreelance : IContratStrategy
    {
        private const double Bonus = 1000;

        public double CalculerSalaireNet(double salaireBase, double tauxHoraire, int heuresSupp, bool avecPrime, bool avecBonus)
        {
            double resultat = salaireBase;

            if (heuresSupp > 0)
            {
                resultat += heuresSupp * tauxHoraire;
            }
            if (avecBonus)
            {
                resultat += Bonus;
            }

            return resultat;
        }

        public int GetCongesAnnuels() => 0;
        public string GetNomContrat() => "Freelance";
    }

    // ========== FACTORY - Création des contrats ==========
    public static class ContratFactory
    {
        public static IContratStrategy CreerContrat(string typeContrat)
        {
            return typeContrat switch
            {
                "CDI" => new ContratCDI(),
                "CDD" => new ContratCDD(),
                "Stagiaire" => new ContratStagiaire(),
                "Freelance" => new ContratFreelance(),
                _ => throw new ArgumentException($"Type de contrat inconnu: {typeContrat}")
            };
        }
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
        public double CalculerSalaireNet(Employe employe, int mois, int annee,
            bool avecPrime, bool avecBonus, double tauxHoraire, int heuresSupp)
        {
            return employe.Contrat.CalculerSalaireNet(employe.Salaire, tauxHoraire, heuresSupp, avecPrime, avecBonus);
        }
    }

    public record OptionsRapport(bool InclureAdresse, bool InclureSalaire, bool InclureConges);

    public interface IRapportFormatter
    {
        string Formater(Employe employe, OptionsRapport options);
    }

    public class PdfFormatter : IRapportFormatter
    {
        public string Formater(Employe employe, OptionsRapport options)
        {
            string rapport = "=== RAPPORT PDF ===\n";
            rapport += $"Nom: {employe.Nom}\n";
            rapport += $"Prénom: {employe.Prenom}\n";

            if (options.InclureAdresse)
            {
                rapport += $"Adresse: {employe.Adresse.Rue}, {employe.Adresse.CodePostal} {employe.Adresse.Ville}\n";
            }
            if (options.InclureSalaire)
            {
                rapport += $"Salaire: {employe.Salaire}€\n";
            }

            rapport += "===================\n";
            return rapport;
        }
    }

    public class HtmlFormatter : IRapportFormatter
    {
        public string Formater(Employe employe, OptionsRapport options)
        {
            string rapport = "<html><body>";
            rapport += "<h1>Rapport Employé</h1>";
            rapport += $"<p>Nom: {employe.Nom}</p>";
            rapport += $"<p>Prénom: {employe.Prenom}</p>";

            if (options.InclureAdresse)
            {
                rapport += $"<p>Adresse: {employe.Adresse.Rue}, {employe.Adresse.CodePostal} {employe.Adresse.Ville}</p>";
            }
            if (options.InclureSalaire)
            {
                rapport += $"<p>Salaire: {employe.Salaire}€</p>";
            }

            rapport += "</body></html>";
            return rapport;
        }
    }

    public class CsvFormatter : IRapportFormatter
    {
        public string Formater(Employe employe, OptionsRapport options)
        {
            string entete = "Nom;Prénom";
            if (options.InclureAdresse) entete += ";Rue;CP;Ville";
            if (options.InclureSalaire) entete += ";Salaire";

            string donnees = $"{employe.Nom};{employe.Prenom}";
            if (options.InclureAdresse)
            {
                donnees += $";{employe.Adresse.Rue};{employe.Adresse.CodePostal};{employe.Adresse.Ville}";
            }
            if (options.InclureSalaire)
            {
                donnees += $";{employe.Salaire}";
            }

            return entete + "\n" + donnees;
        }
    }

    public class JsonFormatter : IRapportFormatter
    {
        public string Formater(Employe employe, OptionsRapport options)
        {
            string rapport = "{\n";
            rapport += $"  \"nom\": \"{employe.Nom}\",\n";
            rapport += $"  \"prenom\": \"{employe.Prenom}\"";

            if (options.InclureAdresse)
            {
                rapport += ",\n  \"adresse\": {\n";
                rapport += $"    \"rue\": \"{employe.Adresse.Rue}\",\n";
                rapport += $"    \"cp\": \"{employe.Adresse.CodePostal}\",\n";
                rapport += $"    \"ville\": \"{employe.Adresse.Ville}\"\n";
                rapport += "  }";
            }
            if (options.InclureSalaire)
            {
                rapport += $",\n  \"salaire\": {employe.Salaire}";
            }

            rapport += "\n}";
            return rapport;
        }
    }

    public static class RapportFormatterFactory
    {
        public static IRapportFormatter CreerFormatter(string typeRapport)
        {
            return typeRapport.ToUpper() switch
            {
                "PDF" => new PdfFormatter(),
                "HTML" => new HtmlFormatter(),
                "CSV" => new CsvFormatter(),
                "JSON" => new JsonFormatter(),
                _ => throw new ArgumentException($"Type de rapport inconnu: {typeRapport}")
            };
        }
    }

    public class RapportGenerateur
    {
        public string Generer(Employe employe, string typeRapport,
            bool inclureAdresse, bool inclureSalaire, bool inclureConges)
        {
            var options = new OptionsRapport(inclureAdresse, inclureSalaire, inclureConges);
            var formatter = RapportFormatterFactory.CreerFormatter(typeRapport);
            return formatter.Formater(employe, options);
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
            return employe.Contrat.GetCongesAnnuels();
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
