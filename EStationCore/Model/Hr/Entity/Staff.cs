using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eStationCore.Model.Common.Entity;
using eStationCore.Model.Common.Enums;

namespace eStationCore.Model.Hr.Entity
{
    public class Staff
    {
        /// <summary>
        /// StaffGuid
        /// </summary>
        [Key]
        public Guid StaffGuid { get; set; }

        /// <summary>
        /// Guid de la personne Associer
        /// </summary>
        public Guid PersonGuid { get; set; }

        /// <summary>
        /// StaffId
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public string PositionPrincipale { get; set; }

        /// <summary>
        /// Departement
        /// </summary>
        public string DepartementPrincipale { get; set; }

        /// <summary>
        /// Division/Groupe
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Qualification
        /// </summary>
        public string Qualification { get; set; }

        /// <summary>
        /// Qualification
        /// </summary>
        public string Diploma { get; set; }

        /// <summary>
        /// NiveauDiplome
        /// </summary>
        public string DiplomaLevel { get; set; }

        /// <summary>
        /// Experience du Staff
        /// </summary>
        public int Experiences { get; set; }

        /// <summary>
        /// Ancien job
        /// </summary>
        public string FormerJob { get; set; }

        /// <summary>
        /// Senior
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// HiredDate
        /// </summary>
        public DateTime? HiredDate { get; set; }

        /// <summary>
        /// Suspendu, Regulier, Licencier
        /// </summary>
        public StaffStatus Statut { get; set; }




        /// <summary>
        /// La personne Associer
        /// </summary>
        [ForeignKey("PersonGuid")]
        public virtual Person Person { get; set; }



    }
}
