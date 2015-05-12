using System.ComponentModel;

namespace EStationCore.Model.Security.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum AdminClearances
    {

        /// <summary>
        /// Administrateur
        /// </summary>
        [Description("Gerer les Utilisateurs/Roles")]
        SuperUser,

        /// <summary>
        /// Ajouter/Modifier les informations d'un autre Staff
        /// </summary>
        [Description("Ajouter des Staffs")]
        StaffWrite,

        /// <summary>
        /// Supprimer les informations d'un autre Staff
        /// </summary>
        [Description("Supprimer des Staffs")]
        StaffDelete,

        /// <summary>
        /// Ajouter/Modifier les informations d'un autre Etudiant
        /// </summary>
        [Description("Ajouter des Nouveaux Etudiant")]
        StudentWrite,

        /// <summary>
        /// Supprimer les informations d'un Etudiant
        /// </summary>
        [Description("Archiver des Etudiants")]
        StudentDelete,

        /// <summary>
        /// Peux Ajouter des Recues de Finance
        /// </summary>
        [Description("Ajouter des Salaires")]
        FinanceWrite,

        /// <summary>
        /// Recevoir de L'Argent
        /// </summary>
        [Description("Ajouter des Depenses/Recettes")]
        Treasurer,

        /// <summary>
        /// Ajouter/Modifier les informations d'une matiere
        /// </summary>
        [Description("Planifier des Cours")]
        StudyWrite,

        /// <summary>
        /// Archiver les informations d'une matiere
        /// </summary>
        [Description("Supprimer des Cours")]
        StudyDelete

    }
}
