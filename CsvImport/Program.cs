using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace CsvImport
{
   class Program
   {
      static void Main(string[] args)
      {
         string tableName = args[0];
         var input = Console.In;
         if (args.Length > 1)
            input = new StreamReader(args[1]);
         var sql = "Select top 0 * from " + tableName;
         var dbConnect = Environment.GetEnvironmentVariable("DBCONNECT") ?? "Integrated Security=SSPI;";
         
         using (var connection = new SqlConnection(dbConnect))
         {
            connection.Open();
            var fields = GetTableDef(connection, sql).ToArray();
            var bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.SqlRowsCopied += (s, dea) => Console.WriteLine(dea.RowsCopied + " rows copied.");
            var dataTable = new DataTable();
            foreach(var f in fields)
               dataTable.Columns.Add(f.Name, f.Type);

            var rdr = new CsvHelper.CsvReader(input);
            var fieldValues = new object[fields.Count()];
            for (int iteration = 0; rdr.Read(); iteration++ )
            {
               for (int f = 0; f < fieldValues.Length; f++)
               {
                  fieldValues[f] = fields[f].Parse(rdr.GetField(f));
               }
               dataTable.Rows.Add(fieldValues);
               if(iteration == 10000)
               {
                  iteration = 0;
                  bulkCopy.WriteToServer(dataTable);
                  dataTable.Clear();
               }
            }

            if(dataTable.Rows.Count > 0)
               bulkCopy.WriteToServer(dataTable);
            bulkCopy.Close();
         }
      }

      static IEnumerable<IFieldDefinition> GetTableDef(SqlConnection connection, string sql)
      {
         using (var command = connection.CreateCommand())
         {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            using (var rdr = command.ExecuteReader())
               return Enumerable.Range(0, rdr.FieldCount).Select(x => FieldDefinition.Create(rdr.GetFieldType(x), rdr.GetName(x))).ToList();
         }
      }
   }
}
