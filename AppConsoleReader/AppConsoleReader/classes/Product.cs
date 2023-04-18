namespace AppConsoleReader.classes
{
    public class Product
    {
        public Product() { }
        
        public Product(int id, int categoryId, string name, decimal price)
        {
            Id = id;
            CategoryId = categoryId;
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Convertimos una línea del csv en un objeto Product
        /// </summary>
        /// <param name="csvLine">Línea del csv</param>
        /// <returns></returns>
        public static Product FromCsv(string csvLine)
        {
            string[] csvValues = csvLine.Split(';');
            
            Product productValues = new Product(
                Convert.ToInt32(csvValues[0]),
                Convert.ToInt32(csvValues[1]),
                csvValues[2],
                Convert.ToDecimal(csvValues[3]));

            return productValues;
        }

        public int CategoryId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
