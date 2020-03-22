using System;
using System.ComponentModel.DataAnnotations;

namespace asp_projekt.Models
{
    public class Drink
    {
        public Drink()
        {
        }

        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int Id { get; set; }

        [Display(Name = "Typ")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public DrinkType Type { get; set; }

        [Display(Name = "Namn")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(128, ErrorMessage = "Max 128 tecken!")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        [MaxLength(256, ErrorMessage = "Max 256 tecken!")]
        public string Description { get; set; }

        [Display(Name = "Pris")]
        [Range(1, 10000, ErrorMessage = "Max 10 000")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Bara siffror tack!")]
        [Required(ErrorMessage = "Obligatoriskt fält!")]
        public int Price { get; set; }


    }
}
