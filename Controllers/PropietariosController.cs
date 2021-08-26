using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class PropietariosController : Controller
    {
        RepoPropietario repo;

        public PropietariosController(IConfiguration config)
        {
            repo = new RepoPropietario(config);
        }

        // GET: PropietariosController
        public ActionResult Index()
        {
            IList<Propietario> lista = repo.All();
            return View(lista);
        }

        // GET: PropietariosController/Details/5
        public ActionResult Details(int id)
        {
            Propietario p = repo.Details(id);

            if (p.Id != 0)
            {
                return View(p);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PropietariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                repo.Put(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietariosController/Edit/5
        public ActionResult Edit(int id)
        {
            var p = repo.ById(id);
            return View(p);
        }

        // POST: PropietariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = new Propietario();
            p.Id = id;
            p.Nombre = collection["Nombre"].ToString();
            p.Dni = collection["Dni"].ToString();
            p.FechaN = DateTime.Parse(collection["FechaN"].ToString());
            p.Direccion = collection["Direccion"].ToString();
            p.Email = collection["Email"].ToString();
            p.Telefono = collection["Telefono"].ToString();

            try
            {
                int res = repo.Edit(p);
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

        // GET: PropietariosController/Delete/5
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

        // POST: PropietariosController/Delete/5
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
