﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using asp_projekt.Models;
using Microsoft.AspNetCore.Authorization;
using asp_projekt.Data;
using Microsoft.EntityFrameworkCore;

namespace asp_projekt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hotel()
        {
            return View();
        }

       
        // Uppdaterad för att generera listor med maträtter och dricka
        public ActionResult Restaurant()
        {
            ViewModel mymodel = new ViewModel();
            mymodel.Dish = _context.Dish.ToList();
            mymodel.Drink = _context.Drink.ToList();
            return View(mymodel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
