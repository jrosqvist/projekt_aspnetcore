using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asp_projekt.Models
{
    public class HotelBooking : IValidatableObject
    {
        public HotelBooking()
        {
        }

        [Required]
        public int Id { get; set; }

        [Display(Name = "Incheckning")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public DateTime CheckIn { get; set; }

        [Display(Name = "Utcheckning")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public DateTime CheckOut { get; set; }

        [Display(Name = "Antal personer")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1, 100, ErrorMessage = "Minst 1 och max 100 gäster tack!")]
        public int NoOfPersons { get; set; }

        [Display(Name = "Rum")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Display(Name = "Förnamn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string LastName { get; set; }

        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage ="5-20 siffror tack!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-postadress")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [EmailAddress(ErrorMessage = "Felaktig e-postadress")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Email { get; set; }

        [Display(Name = "Övriga uppgifter (frivillig)")]
        [MaxLength(1024, ErrorMessage = "Max 1024 tecken!")]
        public string ExtraInformation { get; set; }

        // Kontrollerar att datum för utcheckning är efter incheckning
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckOut < CheckIn)
            {
                yield return
                  new ValidationResult(errorMessage: "Du kan inte checka ut tidigare än incheckningsdatumet!",
                                       memberNames: new[] { "CheckOut" });
            }
        }

    }
}
