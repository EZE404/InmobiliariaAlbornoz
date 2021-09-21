﻿using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class UsuariosController : Controller
    {
        RepoUsuario repo;
        IConfiguration configuration;
        IWebHostEnvironment environment;

        public UsuariosController(IConfiguration config, IWebHostEnvironment environment)
        {
            this.repo = new RepoUsuario(config);
            this.configuration = config;
            this.environment = environment;

        }

        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repo.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            var e = repo.ObtenerPorId(id);
            return View(e);
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                u.Clave = hashed;
                //u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repo.Alta(u);
                if (u.AvatarFile != null && res > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repo.Modificacion(u);
                }
                TempData["msg"] = "¡Usuario creado!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repo.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Edit", u);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repo.ObtenerPorId(id);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u)
        {
            var vista = nameof(Edit);//de que vista provengo
            try
            {
                if (!User.IsInRole("Administrador"))//no soy admin
                {
                    vista = nameof(Perfil);//solo puedo ver mi perfil
                    var usuarioActual = repo.ObtenerPorEmail(User.Identity.Name);
                    if (usuarioActual.Id != id)//si no es admin, solo puede modificarse él mismo
                        return View("Denied");
                        //return RedirectToAction(nameof(Index), "Home");
                }
                // TODO: Add update logic here

                return RedirectToAction(vista);
            }
            catch (Exception ex)
            {//colocar breakpoints en la siguiente línea por si algo falla
                throw;
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
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


        //[Authorize]
        //public IActionResult Avatar()
        //{
        //    var u = repo.ObtenerPorEmail(User.Identity.Name);
        //    string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
        //    string wwwPath = environment.WebRootPath;
        //    string path = Path.Combine(wwwPath, "Uploads");
        //    string pathCompleto = Path.Combine(path, fileName);

        //    //leer el archivo
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
        //    //devolverlo
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}


        // GET: Usuarios/Create
        //[Authorize]
        //public ActionResult Foto()
        //{
        //    try
        //    {
        //        var u = repo.ObtenerPorEmail(User.Identity.Name);
        //        var stream = System.IO.File.Open(
        //            Path.Combine(environment.WebRootPath, u.Avatar.Substring(1)),
        //            FileMode.Open,
        //            FileAccess.Read);
        //        var ext = Path.GetExtension(u.Avatar);
        //        return new FileStreamResult(stream, $"image/{ext.Substring(1)}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // GET: Usuarios/Create
        //[Authorize]
        //public ActionResult Datos()
        //{
        //    try
        //    {
        //        var u = repo.ObtenerPorEmail(User.Identity.Name);
        //        string buffer = "Nombre;Apellido;Email" + Environment.NewLine +
        //            $"{u.Nombre};{u.Apellido};{u.Email}";
        //        var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(buffer));
        //        var res = new FileStreamResult(stream, "text/plain");
        //        res.FileDownloadName = "Datos.csv";
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[AllowAnonymous]
        //// GET: Usuarios/Login/
        //public ActionResult LoginModal()
        //{
        //    return PartialView("_LoginModal", new LoginView());
        //}

        //[Authorize(Policy = "Administrador")]
        //public ActionResult TestLogin()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repo.ObtenerPorEmail(login.Usuario);

                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData.Remove("returnUrl");
                    //string retorno = HttpContext.Request.Headers["referer"].FirstOrDefault();
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "El email o la clave no son correctos");
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
