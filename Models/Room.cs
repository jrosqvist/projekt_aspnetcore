using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace asp_projekt.Models
{
    public class Room
    {
        public Room()
        {
        }

        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string RoomName { get; set; }

        [Display(Name = "Antal")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1, 100, ErrorMessage = "Minst 1, max 100!")]
        public int NoOfRooms { get; set; }

        [Display(Name = "Pris per natt")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1, 50000, ErrorMessage = "Max 50 000 per natt!")]
        public int PricePerNight { get; set; }

        [Display(Name = "Max antal personer")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(1, 10, ErrorMessage = "Max 10 personer per rum!")]
        public int MaxNoOfPersons { get; set; }

        [Display(Name = "Kvadratmeter")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(8, 100, ErrorMessage = "8 - 10 kvadrat!")]
        public int SquareMeter { get; set; }

        [Display(Name = "Sängbredd i cm")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Range(90, 260, ErrorMessage = "90 - 260 cm tack!")]
        public int BedWidth { get; set; }

        [Display(Name = "Rum med utsikt?")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public Boolean View { get; set; }

        [Display(Name = "Ingår frukost?")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public Boolean BreakFast { get; set; }

        public ICollection<HotelBooking> HotelBookings { get; set; }

    }
}
