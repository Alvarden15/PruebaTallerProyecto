using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaTallerProyecto.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;


namespace PruebaTallerProyecto.Controllers
{
    public class HomeController : Controller
    {
        //Como se llama a un metodo GET de una web API
        public IActionResult Index()
        {
            IEnumerable<String> lista=null;
            
            //Se utiliza un HttpClient
            using(var cliente= new HttpClient()){
                //Se llama a la URL de la API
                cliente.BaseAddress= new Uri("http://52.167.187.20/api/");
                
                //Se invoca el metodo Get, especificando de que controlador
                var task=cliente.GetAsync("values");
                //Se obtiene el resultado
                var resultado=task.Result;
                //Valida si tuvo exito o no la extracción
                if(resultado.IsSuccessStatusCode){
                    //Se lee el resultado como se indica
                    var read=resultado.Content.ReadAsAsync<List<String>>();
                    lista=read.Result;
                }else{
                    //Si ocurrió un error, llama a un mensaje de error
                ModelState.AddModelError(String.Empty,"No se pudo establecer contacto con el servidor. Por favor intente de nuevo");
                }
            }
            
            return View(lista);
        }

        public IActionResult Privacy()
        {
            Random rango= new Random();
            ViewData["numero"]=rango.Next(1,20);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
