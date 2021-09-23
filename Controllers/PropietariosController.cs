using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    [Authorize]
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
            try
            {
                var p = repo.ById(id);
                if (p.Id > 0)
                {
                    return View(p);
                }
                else
                {
                    TempData["msg"] = "No se encontró el propietario. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PropietariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario p)
        {
            //Propietario p = new Propietario();
            //p.Id = id;
            //p.Nombre = collection["Nombre"].ToString();
            //p.Dni = collection["Dni"].ToString();
            //p.FechaN = DateTime.Parse(collection["FechaN"].ToString());
            //p.Direccion = collection["Direccion"].ToString();
            //p.Email = collection["Email"].ToString();
            //p.Telefono = collection["Telefono"].ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    int res = repo.Edit(p);
                    if (res > 0)
                    {
                        TempData["msg"] = "Cambios guardados.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                    else
                    {
                        TempData["msg"] = "No se guardaron los cambios. Intente Nuevamente.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }

            }
            catch
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Edit), new { id = id });
            }
        }

        // GET: PropietariosController/Delete/5
        public ActionResult Delete(int id)
        {

            try
            {
                var p = repo.ById(id);

                if (p.Id > 0)
                {
                    return View(p);
                }
                else
                {
                    TempData["msg"] = "No se encontró el propietario. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: PropietariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = repo.Delete(id);
                if (res > 0)
                {
                    TempData["msg"] = "Propietario borrado";
                    return RedirectToAction(nameof(Index));
                }
                else if (res == 0)
                {
                    TempData["msg"] = "No se pudo borrar propietario. Intente Nuevamente.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
                else
                {
                    TempData["msg"] = "No se puede borrar un propietario con inmuebles registrados";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch (Exception e)
            {
                TempData["msg"] = e.Message;
                return View();
            }
        }
    }
}
