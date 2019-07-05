using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaTallerProyecto.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Cassandra.Serialization;
using Cassandra;


namespace PruebaTallerProyecto.Controllers
{
    public class HomeController : Controller
    {
        //Como se llama a un metodo GET de una web API
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            IEnumerable<Int32> lista=null;
            
            //Se utiliza un HttpClient
            using(var cliente= new HttpClient()){
                //Se llama a la URL de la API (Si tiene URL, agrega el http:// o el https://. Si es solo un archivo, no agreges el prefijo)
                cliente.BaseAddress= new Uri("http://52.232.181.173/api/");
                
                //Se invoca el metodo Get, especificando de que controlador
                var task=cliente.GetAsync("values");
                //Se obtiene el resultado
                var resultado=task.Result;
                //Valida si tuvo exito o no la extracción
                if(resultado.IsSuccessStatusCode){
                    //Se lee el resultado como se indica
                    var read=resultado.Content.ReadAsAsync<List<Int32>>();
                    lista=read.Result;
                }else{
                    //Si ocurrió un error, llama a un mensaje de error
                    ModelState.AddModelError(String.Empty,"No se pudo establecer contacto con el servidor. Por favor intente de nuevo");
                }
            }
            
            return View(lista);;
        }

        public IActionResult Reportes(){
            List<Maceta> listado=new List<Maceta>();
            Maceta maseta= null;
            var cluster= Cluster.Builder()
                                .AddContactPoint("127.0.0.1")
                                .WithPort(9042)          
                                .Build();
            var session =cluster.Connect("espaciomaceta");
            var rs=session.Execute("SELECT * FROM espaciomaceta.Maseta;");
            foreach (var item in rs)
            {
               
                maseta= new Maceta();
                maseta.id=item.GetValue<Int32>("id");
                maseta.nombre=item.GetValue<string>("nombre");
                maseta.mensaje=item.GetValue<string>("mensaje");
                maseta.humedad=item.GetValue<double>("humedad");
                maseta.fecha=Convert.ToDateTime(item.GetValue<LocalDate>("fecha").ToString());
                maseta.hora=item.GetValue<LocalTime>("hora").ToString();
                listado.Add(maseta);
            }
            return View(listado.AsEnumerable().OrderByDescending(m=>m.fecha).Where(s=>s.fecha.Month>=DateTime.Now.Month-1));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
