using System.ComponentModel;

namespace EStationCore.Model.Security.Entity
{
    /// <summary>
    /// Permissions d'utilisateurs
    /// </summary>
    public static class SecurityClearances
    {

        
        /// <summary>
        /// Administrateur
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string SuperUser = "SuperUser";

        /// <summary>
        /// Lire Un Autre Staff
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string StaffRead = "Staff";

        /// <summary>
        /// Ajouter/Modifier les informations d'un autre Staff
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string StaffWrite = "StaffWrite";

        /// <summary>
        /// Supprimer les informations d'un autre Staff
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string StaffDelete = "StaffDelete";

        /// <summary>
        /// Lire Un Autre Etudiant
        /// </summary>
        [Description("Peut Ajouter des Customer")]
        public const string CustomerRead = "CustomerRead";

        /// <summary>
        /// Ajouter/Modifier les informations d'un autre Etudiant
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string CustomerWrite = "CustomerWrite";

        /// <summary>
        /// Supprimer les informations d'un Etudiant
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string CustomerDelete = "CustomerDelete";

        /// <summary>
        /// Lire les transactions Financieres
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string FinanceRead = "FinanceRead";

        /// <summary>
        /// Peux Ajouter des Recues de Finance
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string FinanceWrite = "FinanceWrite";

        /// <summary>
        /// Recevoir de L'Argent
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string Treasurer = "Treasurer";

        /// <summary>
        /// Lire une matiere
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string MatiereRead = "MatiereRead";

        /// <summary>
        /// Ajouter/Modifier les informations d'une matiere
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string StudyWrite = "StudyWrite";

        /// <summary>
        /// Archiver les informations d'une matiere
        /// </summary>
        [Description("Peut Ajouter des Utilisateurs")]
        public const string StudyDelete = "StudyDelete";

        /// <summary>
        /// Corriger
        /// </summary>
        [Description("Saisir les Notes")]
        public const string Correcteur = "Correcteur";

        /// <summary>
        /// Presence
        /// </summary>
        [Description("Saisir la Presence")]
        public const string Superviseur = "Superviseur";

    }
}
