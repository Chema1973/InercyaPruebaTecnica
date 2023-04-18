
namespace RandomDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> possible = Enumerable.Range(1, 10000000).ToList();

            var rand = new Random();
            /*
            List<int> result = possible.OrderBy(a => rand.Next()).ToList();
            // --> Con el OrderBy de forma aleatoria se cumple hacerlo en menos de 10 segundos
            using (TextWriter tw = new StreamWriter(@"C:\RandomNumbers.txt"))
            {
                foreach (int s in result)
                {
                    tw.WriteLine(s.ToString());
                }

                tw.Close();
            }
            // --> Entre 5 y 10 segundos
            */

            int n = possible.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                int value = possible[k];
                possible[k] = possible[n];
                possible[n] = value;
            }

            using (TextWriter tw = new StreamWriter(@"C:\RandomNumbers.txt"))
            {
                foreach (int s in possible)
                {
                    tw.WriteLine(s.ToString());
                }

                tw.Close();
            }
            // --> Menos de 5 segundos
        }
    }
}