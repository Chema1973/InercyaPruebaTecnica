using AppConsoleReader.classes;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;

namespace CsvDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cogemos todos los ficheros necesarios para la tarea
            string[] files = Directory.GetFiles("../../../files");

            // Seleccionamos el fichero de "Productos"
            var fileProduct = files.Where(a => a.Contains("Products")).Single();

            // Convertimos los datos del fichero de productos en una lista de objetos
            // Tenemos en cuenta los acentos
            List<Product> products = File.ReadAllLines(fileProduct, Encoding.Latin1)
                                           .Skip(1)
                                           .Select(v => Product.FromCsv(v))
                                           .ToList();

            // Seleccionamos el fichero de "Categorías"
            var fileCategory = files.Where(a => a.Contains("Categories")).Single();

            // Convertimos los datos del fichero de categorías en una lista de objetos
            // Tenemos en cuenta los acentos
            List<Category> categories = File.ReadAllLines(fileCategory, Encoding.Latin1)
                                           .Skip(1)
                                           .Select(v => Category.FromCsv(v))
                                           .ToList();

            List<Category> catalog = new List<Category>();

            // A cada categoría le añadimos todos sus productos
            foreach (Category category in categories)
            {
                category.Products = products.FindAll(a => a.CategoryId == category.Id);
                catalog.Add(category);
            }

            // Convertimos los datos a json
            string json = JsonConvert.SerializeObject(catalog.ToArray());

            // Escribimos el fichero en C/
            File.WriteAllText(@"C:\Catalog.json", json);

            // Creamos un serializador para xml
            XmlSerializer serialiser = new XmlSerializer(catalog.GetType());

            // Creamos el text writer para poder escribir
            TextWriter filestream = new StreamWriter(@"C:\Catalog.xml");

            // Escribimos en el fichero
            serialiser.Serialize(filestream, catalog);

            // Cerramos el fichero
            filestream.Close();

        }
    }
}
