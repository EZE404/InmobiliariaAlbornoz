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
        private RepoInquilino repo;
        private RepoDev repoDev;

        public InquilinosController(IConfiguration config)
        {
            this.repo = new RepoInquilino(config);
            this.repoDev = new RepoDev(config);
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
            try
            {
                Inquilino i = repo.Details(id);

                if (i.Id != 0)
                {
                    return View(i);
                }
                else
                {
                    TempData["msg"] = "No se encontró Inquilino.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Details:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
                    var iAux = repo.ByDni(i.Dni);
                    if(iAux.Id > 0)
                    {
                        TempData["msg"] = "Ya existe un registro con el DNI " + i.Dni + ".";
                        return View();
                    }

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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Create:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Edit:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        // POST: InquilinosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var iAux = repo.ByDni(i.Dni);
                    if (iAux.Id > 0 && i.Id != iAux.Id)
                    {
                        TempData["msg"] = "Ya existe un registro con el DNI " + i.Dni + ".";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }

                    int res = repo.Edit(i);
                    if (res > 0)
                    {
                        TempData["msg"] = "Cambios guardados.";
                        return RedirectToAction(nameof(Edit), new { id = id });
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Edit:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // GET: InquilinosController/Delete/5
        [Authorize(Policy = "Administrador")]
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Delete:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // POST: InquilinosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Inquilinos", "Delete:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
