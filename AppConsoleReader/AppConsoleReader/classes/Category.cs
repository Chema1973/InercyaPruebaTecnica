namespace AppConsoleReader.classes
{
    public class Category
    {
        public Category() { }
        public Category(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Products = new List<Product>();
        }

        /// <summary>
        /// Convertimos una línea del csv en un objeto Category
        /// </summary>
        /// <param name="csvLine">Línea del csv</param>
        /// <returns></returns>
        public static Category FromCsv(string csvLine)
        {
            string[] csvValues = csvLine.Split(';');
            Category categoryValues = new Category(
                Convert.ToInt32(csvValues[0]),
                csvValues[1],
                csvValues[2]);

            return categoryValues;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }
    }
}
