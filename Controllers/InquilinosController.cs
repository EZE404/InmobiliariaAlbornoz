using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class InquilinosController : Controller
    {
        RepoInquilino repo;

        public InquilinosController()
        {
            repo = new RepoInquilino();
        }

        // GET: InquilinosController
        public ActionResult Index()
        {
            IList<Inquilino> lista = repo.All();
            return View(lista);
        }

        // GET: InquilinosController/Details/5
        public ActionResult Details(int id)
        {
            Inquilino i = repo.Details(id);

            if (i.Id != 0)
            {
                return View(i);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InquilinosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
            try
            {
                repo.Put(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InquilinosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = new Inquilino();
            i.Id = id;
            i.Nombre = collection["Nombre"].ToString();
            i.Dni = collection["Dni"].ToString();
            i.FechaN = DateTime.Parse(collection["FechaN"].ToString());
            i.DireccionTrabajo = collection["DireccionTrabajo"].ToString();
            i.Email = collection["Email"].ToString();
            i.Telefono = collection["Telefono"].ToString();

            // Datos Garante
            i.DniGarante = collection["DniGarante"].ToString();
            i.NombreGarante = collection["NombreGarante"].ToString();
            i.TelefonoGarante = collection["TelefonoGarante"].ToString();
            i.EmailGarante = collection["EmailGarante"].ToString();

            try
            {
                int res = repo.Edit(i);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: InquilinosController/Delete/5
        public ActionResult Delete(int id)
        {
            int res = repo.Delete(id);
            if (res > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: InquilinosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
