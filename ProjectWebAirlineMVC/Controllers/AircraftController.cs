using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Helpers;
using ProjectWebAirlineMVC.Models;

namespace ProjectWebAirlineMVC.Controllers
{
    public class AircraftController : Controller
    {
        private readonly DataContext _context;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;


        public AircraftController(DataContext context,
            IAircraftRepository aircraftRepository,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _context = context;
            _aircraftRepository = aircraftRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }



        // GET: Aircraft
        public async Task<IActionResult> Index()
        {
            return View(_aircraftRepository.GetAll().OrderBy(a => a.Name));
        }

        // GET: Aircraft/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return NotFound();
            }

            return View(aircraft);
        }

        // GET: Aircraft/Create
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
                var path = string.Empty;


                //Imagens
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "aircrafts");

                }

                var aircraft = _converterHelper.ToAircraft(model, path, true);

                //TODO: Modificar para o user que estiver logado
                aircraft.User = await _userHelper.GetUserByEmailAsync("diogovsky1904@gmail.com");
               await _aircraftRepository.CreateAsync(aircraft);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: Aircraft/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAircraftViewModel(aircraft);

            return View(model);
        }



        // POST: Aircraft/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AircraftViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "aircrafts");
                    }

                    var aircraft = _converterHelper.ToAircraft(model, path, true);


                    //TODO: Modificar para o user que estiver logado
                    aircraft.User = await _userHelper.GetUserByEmailAsync("diogovsky1904@gmail.com");
                    await _aircraftRepository.UpdateAsync(aircraft);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _aircraftRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aircraft = await _aircraftRepository.GetByIdAsync(id.Value);
            if (aircraft == null)
            {
                return NotFound();
            }

            return View(aircraft);
        }

        // POST: Aircraft/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aircraft =  await _aircraftRepository.GetByIdAsync(id);

            if(aircraft != null)
            {
                await _aircraftRepository.DeleteAsync(aircraft);
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
