using ApplicationWebTreeView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml.Linq;

namespace ApplicationWebTreeView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            string[] files = Directory.GetFiles("files");

            var fileItems = files.Where(a => a.Contains("Items")).Single();

            string readText = System.IO.File.ReadAllText(fileItems);
            JArray convert = JArray.Parse(readText);
            var div =  new XElement("div");
            recursive(convert, div);

            return Content(div.ToString(), "text/html");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void recursive(JToken convert, XElement node)
        {
            var list = new XElement("ul");

            for (var a = 0; a < convert.Count(); a++)
            {
                var item = new XElement("li");

                item.Value = (string)convert[a]["Name"];

                if (convert[a]["Children"].HasValues) {
                    recursive(convert[a]["Children"], item);
                }
                list.Add(item);
                
            }
            node.Add(list);
        }
    }
}