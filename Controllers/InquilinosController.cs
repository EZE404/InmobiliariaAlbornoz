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
    public class InquilinosController : Controller
    {
        RepoInquilino repo;

        public InquilinosController(IConfiguration config)
        {
            repo = new RepoInquilino(config);
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
                if (ModelState.IsValid)
                {
                    var res = repo.Put(i);
                    if (res > 0)
                    {
                        TempData["msg"] = "Inquilino cargado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msg"] = "No se cargó Inquilino. Intente nuevamente.";
                        return RedirectToAction(nameof(Create));
                    }
                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(Create));
                }

            }
            catch
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return View();
            }
        }

        // GET: InquilinosController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var i = repo.ById(id);
                if (i.Id > 0)
                {
                    return View(i);
                }
                else
                {
                    TempData["msg"] = "No se encontró Inquilino. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: InquilinosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i)
        {
            //Inquilino i = new Inquilino();
            //i.Id = id;
            //i.Nombre = collection["Nombre"].ToString();
            //i.Dni = collection["Dni"].ToString();
            //i.FechaN = DateTime.Parse(collection["FechaN"].ToString());
            //i.DireccionTrabajo = collection["DireccionTrabajo"].ToString();
            //i.Email = collection["Email"].ToString();
            //i.Telefono = collection["Telefono"].ToString();

            try
            {
                if (ModelState.IsValid)
                {
                    int res = repo.Edit(i);
                    if (res > 0)
                    {
                        TempData["msg"] = "Cambios guardados.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msg"] = "No se guardaron cambios. Intente nuevamente.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }

            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Edit), new { id = id });
            }
        }

        // GET: InquilinosController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var i = repo.ById(id);
                if (i.Id > 0)
                {
                    return View(i);
                }
                else
                {
                    TempData["msg"] = "No se encontró Inquilino. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
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
                var res = repo.Delete(id);
                if (res > 0)
                {
                    TempData["msg"] = "Inquilino borrado.";
                    return RedirectToAction(nameof(Index));
                }
                else if (res == 0)
                {
                    TempData["msg"] = "No se pudo borrar Inquilino. Intente nuevamente.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
                else
                {
                    TempData["msg"] = "No se puede borrar un Inquilino con contratos asociados.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}
