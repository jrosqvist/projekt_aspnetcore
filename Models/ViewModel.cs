using System;
using System.Collections.Generic;

namespace asp_projekt.Models
{
    // Denna används för att kunna skicka två modeller till samma vy
    public class ViewModel
    {
        public IEnumerable<Dish> Dish { get; set; }
        public IEnumerable<Drink> Drink { get; set; }
    }
}
