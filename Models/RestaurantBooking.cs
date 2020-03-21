using System;
using System.ComponentModel.DataAnnotations;


namespace asp_projekt.Models
{
    public class RestaurantBooking
    {
        public RestaurantBooking()
        {
            
        }

        
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int Id { get; set; }

        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public DateTime Date { get; set; }

        [Display(Name = "Tid (Från 05:00 - 22:00")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        //[RegularExpression("^(0[5-9]|1[1-9]|2[1-2]):[0-0|3][0]$", ErrorMessage = "Väl hel- eller halvtimme tack (05-22)")]
        public DateTime Time { get; set; }

        [Display(Name = "Antal personer")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [Range(1, 20, ErrorMessage = "Minst 1 och max 20 personer tack!")]
        public int NoOfPersons { get; set; }

        [Display(Name = "Ditt förnamn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string FirstName { get; set; }

        [Display(Name = "Ditt efternamn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string LastName { get; set; }

        [Display(Name = "Ditt telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "5-20 siffror tack!")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Din e-postadress")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [EmailAddress(ErrorMessage = "Felaktig e-postadress")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Email { get; set; }

        [Display(Name = "Övriga uppgifter (frivillig)")]
        [MaxLength(1024, ErrorMessage = "Max 1024 tecken!")]
        public string ExtraInformation { get; set; }
    }
}
