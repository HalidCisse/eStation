using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using eLib.Database.Interfaces;
using eStationCore.Model.Comm.Entity;
using eStationCore.Model.Common.Enums;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace eStationCore.Model.Common.Entity
{
    public class Person : Tracable, IValidable
    {
        /// <summary>
        /// Guid de la personne Associer
        /// </summary>
        [Key]
        public Guid PersonGuid { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public PersonTitles Title { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// PhotoIdentity
        /// </summary>
        public byte[] PhotoIdentity { get; set; }

        /// <summary>
        /// HealthState
        /// </summary>
        public HealthStates HealthState { get; set; }

        /// <summary>
        /// Nationality
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// IdentityNumber
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// BirthDate
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// BirthPlace
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// EmailAdress
        /// </summary>
        public string EmailAdress { get; set; }

        /// <summary>
        /// HomeAdress
        /// </summary>
        public string HomeAdress { get; set; }

        /// <summary>
        /// RegistrationDate
        /// </summary>
        public DateTime? RegistrationDate { get; set; } //todo deprecate

        /// <summary>
        /// FullName
        /// </summary>
        public string FullName => FirstName.Substring(0, 1).ToUpper() + FirstName.Substring(1).ToLower() + " " + LastName.Substring(0, 1).ToUpper() + LastName.Substring(1).ToLower();



        
        /// <summary>
        /// Les Documents de la personne
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; } = new HashSet<Document>();

        /// <summary>
        /// Les Documents de la personne
        /// </summary>
        public virtual ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();



        public async Task<List<ValidationFailure>> Validate()
        {
            var validationFailures = (await (new PersonValidator()).ValidateAsync(this)).Errors;
            return validationFailures != null
                ? validationFailures as List<ValidationFailure>
                : new List<ValidationFailure>();
        }

        public async void ValidateAndThrow() => await (new PersonValidator()).ValidateAndThrowAsync(this);
    }


    /// <summary>
    /// 
    /// </summary>
    public class PersonValidator : AbstractValidator<Person>
    {
        /// <summary>
        /// 
        /// </summary>
        public PersonValidator()
        {
            RuleFor(objt => objt.PersonGuid).NotEqual(Guid.Empty).WithMessage("Le Guid doit etre ne doit pas etre vide");
            RuleFor(objt => objt.FirstName).NotEmpty().WithMessage("Le prenom n'est pas valide");
            RuleFor(objt => objt.EmailAdress).SetValidator(new EmailValidator()).WithMessage("Adresse email non valide");
        }
    }
}
