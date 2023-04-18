using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CustomersDemo
{
    class Program
    {
        /// <summary>
        /// Asumimos que existe la BBDD, en este caso se ha llamado "CustomersDemo"
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var dt = new DataTable("Customers");
            dt.Columns.Add(new DataColumn("Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("City", typeof(string)));
            dt.Columns.Add(new DataColumn("Country", typeof(string)));
            dt.Columns.Add(new DataColumn("PostalCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone", typeof(string)));

            // Seleccionamos los ficheros de la carpeta "files"
            string[] files = Directory.GetFiles("../../../files");

            // Seleccionamos el fichero "Customers"
            var fileCustomers = files.Where(a => a.Contains("Customers")).Single();

            // Leemos el fichero para meterlo en nuestra DataTable
            // Tenemos en cuenta los acentos
            using (StreamReader sr = new StreamReader(fileCustomers, Encoding.Latin1))
            {
                // Excluimos la primera línea que nos indica el nombre de las columnas
                string headerLine = sr.ReadLine();
                string currentLine;

                while ((currentLine = sr.ReadLine()) != null)
                {
                    var data = currentLine.Split(";");
                    if (data.Length == 7)
                    {
                        dt.Rows.Add(data[0], data[1], data[2], data[3], data[4], data[5], data[6]);
                    }
                    else
                    {
                        // Cada línea debe tener 7 datos.
                        // En este caso todos los registros tienen los 7 datos pero habría que tener en
                        // cuenta si a una línea le falta algún dato y darle algún tratamiento especial
                        // Para este ejemplo escribiríamos una línea en la consola
                        Console.WriteLine("Línea sin datos completos: " + currentLine);
                    }
                }
            }

            // Conexión a la BBDD
            var sqlConnection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomersDemo;Integrated Security=True;TrustServerCertificate=True");
            sqlConnection.Open();

            // Comprobamos si existe la tabla
            var checkTableIfExistsCommand = new SqlCommand("IF EXISTS (SELECT 1 FROM sysobjects WHERE name =  '" + dt.TableName + "') SELECT 1 ELSE SELECT 0", sqlConnection);
            var exists = checkTableIfExistsCommand.ExecuteScalar().ToString().Equals("1");

            // La tabla no existe y la creamos de forma dinámica
            if (!exists)
            {
                var createTableBuilder = new StringBuilder("CREATE TABLE [" + dt.TableName + "]");
                createTableBuilder.AppendLine("(");

                foreach (DataColumn dc in dt.Columns)
                {
                    createTableBuilder.AppendLine("  [" + dc.ColumnName + "] VARCHAR(MAX),");
                }

                createTableBuilder.Remove(createTableBuilder.Length - 1, 1);
                createTableBuilder.AppendLine(")");

                var createTableCommand = new SqlCommand(createTableBuilder.ToString(), sqlConnection);
                createTableCommand.ExecuteNonQuery();
            }

            // En este punto tenemos la certeza que la tabla existe
            // y ya podemos copiar los datos con "SqlBulkCopy"
            using (var bulkCopy = new SqlBulkCopy(sqlConnection))
            {
                bulkCopy.DestinationTableName = dt.TableName;
                bulkCopy.WriteToServer(dt);
            }
        }
    }
}
