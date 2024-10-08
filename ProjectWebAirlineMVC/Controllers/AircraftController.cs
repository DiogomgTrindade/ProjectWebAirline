using System;
using System.Collections.Generic;
using System.IO;
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
    public class AircraftController : Controller
    {
        private readonly DataContext _context;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;


        public AircraftController(DataContext context,
            IAircraftRepository aircraftRepository,
            ITicketRepository ticketRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _aircraftRepository = aircraftRepository;
            _ticketRepository = ticketRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }



        // GET: Aircraft
        public IActionResult Index()
        {
            return View(_aircraftRepository.GetAll().OrderBy(a => a.Name));
        }

        // GET: Aircraft/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            return View(aircraft);
        }



        // GET: Aircraft/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AircraftViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;


                //Imagens
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "aircrafts");

                }

                var aircraft = _converterHelper.ToAircraft(model, imageId, true);

                aircraft.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
               await _aircraftRepository.CreateAsync(aircraft);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: Aircraft/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }


            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            var model = _converterHelper.ToAircraftViewModel(aircraft);

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AircraftViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var aircraftToUpdate = await _aircraftRepository.GetByIdAsync(model.Id);
                    if(aircraftToUpdate == null)
                    {
                        return new NotFoundViewResult("AircraftNotFound");
                    }


                    Guid imageId = model.ImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "aircrafts");
                    }


                    aircraftToUpdate.Name = model.Name;
                    aircraftToUpdate.ImageId = imageId;
                    aircraftToUpdate.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);


                    if (model.Capacity != aircraftToUpdate.Capacity)
                    {
                        aircraftToUpdate.Capacity = model.Capacity;

                        await _aircraftRepository.UpdateAsync(aircraftToUpdate);

                        var associatedFlights = await _context.Flights
                                                              .Include(f => f.Aircraft)
                                                              .Where(f => f.AircraftId == model.Id)
                                                              .ToListAsync();

                        foreach (var flight in associatedFlights)
                        {
                            await _ticketRepository.UpdateTicketsFromFlightAsync(flight);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _aircraftRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("AircraftNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Aircraft/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            return View(aircraft);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aircraft =  await _aircraftRepository.GetByIdAsync(id);

            if(aircraft == null)
            {
                return new NotFoundViewResult("AircraftNotFound");
            }

            var associatedFlights = await _context.Flights.Where(f => f.AircraftId == id).ToListAsync();

            if (associatedFlights.Any())
            {
                ModelState.AddModelError(string.Empty, "The aircraft is already listed on a flight, please delete the flight first.");
                return View(aircraft);
            }

            try
            {
                await _aircraftRepository.DeleteAsync(aircraft);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Error trying to delete a country");
                return View(aircraft);
            }



            return RedirectToAction(nameof(Index));
        }

    }
}
