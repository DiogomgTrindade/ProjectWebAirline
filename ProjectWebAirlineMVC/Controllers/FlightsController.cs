﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;

namespace ProjectWebAirlineMVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public FlightsController(IFlightRepository flightRepository, ICountryRepository countryRepository, IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _flightRepository = flightRepository;
            _countryRepository = countryRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            var flights = await _flightRepository.GetAllFlightsWithCountriesAsync();
            return View(flights);
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flights = await _flightRepository.GetAllFlightsWithCountriesAsync();
            var flight = flights.FirstOrDefault(f => f.Id == id.Value);
               
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var countries = await _countryRepository.GetCountryListAsync();
            var model = new FlightViewModel
            {
                Countries = countries.Select(country => new SelectListItem
                {
                    Value = country.Id.ToString(),
                    Text = country.Name
                }).ToList()

            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                var flight = _converterHelper.ToFlightAsync(model);

                await _flightRepository.CreateAsync(flight); 
                return RedirectToAction(nameof(Index));
            }

            var countries = await _countryRepository.GetCountryListAsync();
            model.Countries = countries.Select(country => new SelectListItem
            {
                Value = country.Id.ToString(),
                Text = country.Name
            }).ToList();


            return View(model);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToFlightViewModel(flight);

            var countries = await _countryRepository.GetCountryListAsync();
            model.Countries = countries.Select(country => new SelectListItem
            {
                Value = country.Id.ToString(),
                Text = country.Name
            });

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var flight =  _converterHelper.ToFlightAsync(model);

                    await _flightRepository.UpdateAsync(flight);
                }
                catch (DbUpdateConcurrencyException)
                {

                        return NotFound();

                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flights = await _flightRepository.GetAllFlightsWithCountriesAsync();
            var flight = flights.FirstOrDefault(f => f.Id == id.Value);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            await _flightRepository.DeleteAsync(flight);
            return RedirectToAction(nameof(Index));
        }

    }
}
