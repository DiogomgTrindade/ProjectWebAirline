using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;
using NotFoundViewResult = ProjectWebAirlineMVC.Helpers.NotFoundViewResult;

namespace ProjectWebAirlineMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly DataContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public CountriesController(DataContext context ,ICountryRepository countryRepository, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _countryRepository = countryRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View( _countryRepository.GetAll().OrderBy(c => c.Name));
        }


        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool countryExists = await _countryRepository.CountryNameExistsAsync(model.Name);
                if (countryExists)
                {
                    ModelState.AddModelError(string.Empty, "Country already created.");
                    return View(model);
                }


                Guid imageId = Guid.Empty;


                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                }

                var country = _converterHelper.ToCountry(model, imageId, true);


                await _countryRepository.CreateAsync(country);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var model = _converterHelper.ToCountryViewModel(country);

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCountry = await _countryRepository.GetByIdAsync(model.Id);
                    if (existingCountry == null)
                    {
                        return new NotFoundViewResult("CountryNotFound");
                    }

                    if (!string.Equals(existingCountry.Name, model.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        bool countryExists = await _countryRepository.CountryNameExistsAsync(model.Name);
                        if (countryExists)
                        {
                            ModelState.AddModelError(string.Empty, "Country already created.");
                            return View(model);
                        }
                    }


                    Guid imageId = model.ImageId;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "countries");
                    }

                    var country = _converterHelper.ToCountry(model, imageId, false);


                    await _countryRepository.UpdateAsync(country);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _countryRepository.ExistAsync(model.Id)) 
                    {
                        return new NotFoundViewResult("CountryNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var country = await _countryRepository.GetByIdAsync(id.Value);
            if (country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            return View(country);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);
            if(country == null)
            {
                return new NotFoundViewResult("CountryNotFound");
            }

            var associatedFlights = await _context.Flights.AnyAsync(f => f.OriginCountryId == id || f.DestinationCountryId == id);

            if (associatedFlights)
            {
                ModelState.AddModelError(string.Empty, "The country is already listed on a flight, please delete the flight first.");
                return View(country);
            }

            try
            {
                await _countryRepository.DeleteAsync(country);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Error trying to delete a country");
                return View(country);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
