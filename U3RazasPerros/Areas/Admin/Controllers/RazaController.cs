using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using U3RazasPerros.Areas.Admin.Models;
using U3RazasPerros.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace U3RazasPerros.Controllers
{
    [Area("Admin")]
    public class RazaController : Controller
    {
        public perrosContext Context { get; }
        public IWebHostEnvironment Hostito { get; }

        public RazaController(perrosContext context, IWebHostEnvironment hostito)
        {
            Context = context;
            Hostito = hostito;
        }

        
        public IActionResult Index()
        {
            var razas = Context.Razas.OrderBy(x => x.Nombre);

            return View(razas);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            AgregarRazaViewModel vm = new AgregarRazaViewModel();
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);

            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarRazaViewModel vm, IFormFile subimg)
        {
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);

            if (vm.Razas.Caracteristicasfisicas.Id != vm.Razas.Id)
            {
                ModelState.AddModelError("", "Error en el sistema... el sistema fue hackeado... el sistema fue hackeado..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre)) 
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Descripcion))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.OtrosNombres))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Cola))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Color))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Hocico))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Patas))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Pelo))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.AlturaMax == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.AlturaMin == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.PesoMax == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.PesoMin == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.EsperanzaVida == 0)
            {
                ModelState.AddModelError("", "Como va a tener una esperanza de vida de 0 años? Ni que fuera mosca..."); return View(vm);
            }
            //if (vm.Razas.IdPais == 0 || vm.Razas.IdPais == null)
            //{
            //    ModelState.AddModelError("", "Selecciona un pais que sea valido... menos Marte... es un planeta(?)...."); return View(vm);
            //}
            if(vm.Razas.AlturaMin > vm.Razas.AlturaMax)
            {
                ModelState.AddModelError("", "La altura minima no puede ser mayor que la altura maxima"); return View(vm);
            }
            if (vm.Razas.AlturaMax < vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura maxima no puede ser menor que la altura minima"); return View(vm);
            }
            if (vm.Razas.PesoMin > vm.Razas.PesoMax)
            {
                ModelState.AddModelError("", "El peso minimo no puede ser mayor que el peso maximo"); return View(vm);
            }
            if (vm.Razas.PesoMax < vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso maximo no puede ser menor que el peso minimo"); return View(vm);
            }
            else
            {
                var car = vm.Razas.Caracteristicasfisicas;
                vm.Razas.Caracteristicasfisicas = null;
                Context.Razas.Add(vm.Razas);
                Context.SaveChanges();
                //
                var razita = Context.Razas.FirstOrDefault(x => x.Nombre == vm.Razas.Nombre);
                car.Id = razita.Id;
                Context.Caracteristicasfisicas.Add(car);
                Context.SaveChanges();

            }

            if (subimg != null)
            { 
                if (subimg.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Tiene que ser jpeg prro");
                    return View(vm);
                }
                if (subimg.Length > 1024 * 1024 * 10)
                {
                ModelState.AddModelError("", "La vas a matar prro");
                return View(vm);
                }
                else
                {
                    var razita = Context.Razas.FirstOrDefault(x => x.Nombre == vm.Razas.Nombre);

                    string name = razita.Nombre;

                    var path = Hostito.WebRootPath + "/imgs_perros/" + razita.Id + "_0" + ".jpg";

                    FileStream fs = new FileStream(path, FileMode.Create);
                    subimg.CopyTo(fs);
                    fs.Close();
                }
            }

            if (subimg == null)
            {
                var path = Hostito.WebRootPath + "/imgs_perros/NoPhoto.jpg";

                var razita2 = Context.Razas.FirstOrDefault(x => x.Nombre == vm.Razas.Nombre);

                var path2 = Hostito.WebRootPath + "/imgs_perros/" + razita2.Id + "_0" + ".jpg";

                FileStream fack = new FileStream(path, FileMode.Open);

                FileStream fos = new FileStream(path2, FileMode.Create);
                fack.CopyTo(fos);
                fos.Close();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            AgregarRazaViewModel vm = new AgregarRazaViewModel();

            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);

            var razita = Context.Razas.FirstOrDefault(x => x.Id == id);
            var carazita = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == razita.Id);
            //var parazita = Context.Paises.FirstOrDefault(x => x.Id == id);

            if (razita == null)
            {
                return RedirectToAction("Index");
            }
            vm.Razas = razita;
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);


            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(AgregarRazaViewModel vm, IFormFile subimg)
        {
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);

            if (vm.Razas.Caracteristicasfisicas.Id != vm.Razas.Id)
            {
                ModelState.AddModelError("", "Error en el sistema... el sistema fue hackeado... el sistema fue hackeado..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Descripcion))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.OtrosNombres))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Cola))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Color))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Hocico))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Patas))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (string.IsNullOrWhiteSpace(vm.Razas.Caracteristicasfisicas.Pelo))
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.AlturaMax == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.AlturaMin == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.PesoMax == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.PesoMin == 0)
            {
                ModelState.AddModelError("", "Completa todos los campos, que no quede ninguno en blanco..."); return View(vm);
            }
            if (vm.Razas.EsperanzaVida == 0)
            {
                ModelState.AddModelError("", "Como va a tener una esperanza de vida de 0 años? Ni que fuera mosca..."); return View(vm);
            }
            //if (vm.Razas.IdPais == 0 || vm.Razas.IdPais == null)
            //{
            //    ModelState.AddModelError("", "Selecciona un pais que sea valido... menos Marte... es un planeta(?)...."); return View(vm);
            //}
            if (vm.Razas.AlturaMin > vm.Razas.AlturaMax)
            {
                ModelState.AddModelError("", "La altura minima no puede ser mayor que la altura maxima"); return View(vm);
            }
            if (vm.Razas.AlturaMax < vm.Razas.AlturaMin)
            {
                ModelState.AddModelError("", "La altura maxima no puede ser menor que la altura minima"); return View(vm);
            }
            if (vm.Razas.PesoMin > vm.Razas.PesoMax)
            {
                ModelState.AddModelError("", "El peso minimo no puede ser mayor que el peso maximo"); return View(vm);
            }
            if (vm.Razas.PesoMax < vm.Razas.PesoMin)
            {
                ModelState.AddModelError("", "El peso maximo no puede ser menor que el peso minimo"); return View(vm);
            }
            else
            {
                //var razita = Context.Razas.FirstOrDefault(x => (x.Id == vm.Razas.Id && x.Caracteristicasfisicas.Id == vm.Razas.Caracteristicasfisicas.Id));

                var razita = Context.Razas.FirstOrDefault(x => (x.Id == vm.Razas.Id));
                var carazita = Context.Caracteristicasfisicas.FirstOrDefault(x => (x.Id == razita.Id));

                razita.Nombre = vm.Razas.Nombre;
                razita.Descripcion = vm.Razas.Descripcion;
                razita.OtrosNombres = vm.Razas.OtrosNombres;
                razita.PesoMax = vm.Razas.PesoMax;
                razita.PesoMin = vm.Razas.PesoMin;
                razita.AlturaMax = vm.Razas.AlturaMax;
                razita.AlturaMin = vm.Razas.AlturaMin;
                razita.EsperanzaVida = vm.Razas.EsperanzaVida;
                razita.IdPais = vm.Razas.IdPais;

                carazita.Patas = vm.Razas.Caracteristicasfisicas.Patas;
                carazita.Pelo = vm.Razas.Caracteristicasfisicas.Pelo;
                carazita.Color = vm.Razas.Caracteristicasfisicas.Color;
                carazita.Hocico = vm.Razas.Caracteristicasfisicas.Hocico;
                carazita.Cola = vm.Razas.Caracteristicasfisicas.Cola;


                Context.Update(razita);
                Context.Update(carazita);
                Context.SaveChanges();
            }

            if (subimg != null)
            {
                if (subimg.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Tiene que ser jpeg prro");
                    return View(vm);
                }
                if (subimg.Length > 1024 * 1024 * 10)
                {
                    ModelState.AddModelError("", "La vas a matar prro");
                    return View(vm);
                }
                else
                {
                    var razita = Context.Razas.FirstOrDefault(x => x.Nombre == vm.Razas.Nombre);

                    string name = razita.Nombre;

                    var path = Hostito.WebRootPath + "/imgs_perros/" + razita.Id + "_0" + ".jpg";

                    FileInfo hihi = new FileInfo(path);

                    FileStream fs = new FileStream(path, FileMode.Create);
                    subimg.CopyTo(fs);
                    fs.Close();
                }
            }

            //if (subimg == null)
            //{
            //    var path = Hostito.WebRootPath + "/imgs_perros/NoPhoto.jpg";

            //    var razita2 = Context.Razas.FirstOrDefault(x => x.Nombre == vm.Razas.Nombre);

            //    var path2 = Hostito.WebRootPath + "/imgs_perros/" + razita2.Id + "_0" + ".jpg";

            //    FileStream fack = new FileStream(path, FileMode.Open);

            //    FileStream fos = new FileStream(path2, FileMode.Create);
            //    fack.CopyTo(fos);
            //    fos.Close();
            //}

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            AgregarRazaViewModel vm = new AgregarRazaViewModel();

            var razita = Context.Razas.FirstOrDefault(x => x.Id == id);
            var carazita = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == razita.Id);
            //var parazita = Context.Paises.FirstOrDefault(x => x.Id == id);

            if (razita == null)
            {
                return RedirectToAction("Index");
            }
            vm.Razas = razita;
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);


            return View(vm);
        }

        [HttpPost]
        public IActionResult Eliminar(AgregarRazaViewModel vm)
        {

            var razita = Context.Razas.FirstOrDefault(x => x.Id == vm.Razas.Id);
            var carazita = Context.Caracteristicasfisicas.FirstOrDefault(x => x.Id == razita.Id);

            Context.Remove(carazita);
            Context.Remove(razita);
            Context.SaveChanges();

            var path2 = Hostito.WebRootPath + "/imgs_perros/" + razita.Id + "_0" + ".jpg";

            FileInfo hihi = new FileInfo(path2);

            hihi.Delete();

            return RedirectToAction("Index");
        }

    }
}
