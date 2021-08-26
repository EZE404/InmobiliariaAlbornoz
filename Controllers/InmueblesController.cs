using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaAlbornoz.Controllers
{
    public class InmueblesController : Controller
    {
        private RepoInmueble repo = new RepoInmueble();
        // GET: Inmueble
        public ActionResult Index()
        {
            var lista = repo.All();
            return View(lista);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var i = repo.ById(id);
                return View(i);
            }
            catch (Exception ex)
            {
                 // TODO
                Debug.WriteLine(ex.ToString());
                return View();
            }
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                // TODO: Add insert logic here
                var res = repo.Put(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var i = repo.ById(id);
                return View(i);            
            }
            catch (Exception ex)
            {
                 // TODO
                Debug.WriteLine(ex.ToString());
                return View();

            }
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                // TODO: Add update logic here
                i.Id = id;
                var res = repo.Edit(i);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}