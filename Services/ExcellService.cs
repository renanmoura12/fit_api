using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace api_fit.Services
{
    public class ExcellService
    {
        public static DataTable CreateExcel(List<string> jsonList)
        {
            DataTable dataTable = CreateDataTable(jsonList);
            return dataTable;
        }

        public static DataTable CreateDataTable(List<string> jsonList)
        {
            DataTable dataTable = new ("Dados");

            foreach (var jsonString in jsonList)
            {
                dynamic dados = JsonConvert.DeserializeObject(jsonString);
                AddColumnsDynamically(dataTable, dados);
            }

            foreach (var jsonString in jsonList)
            {
                dynamic dados = JsonConvert.DeserializeObject(jsonString);
                FillRowsDynamically(dataTable, dados);
            }

            return dataTable;
        }

        private static DataTable CreateDataTable(dynamic dados)
        {
            DataTable dataTable = new ("Dados");
            AddColumnsDynamically(dataTable, dados);
            FillRowsDynamically(dataTable, dados);
            return dataTable;
        }

        private static void AddColumnsDynamically(DataTable dataTable, dynamic dados, string prefixo = "")
        {
            foreach (var propriedade in dados.Properties())
            {
                var chave = string.IsNullOrEmpty(prefixo) ? propriedade.Name : $"{prefixo}_{propriedade.Name}";

                if (propriedade.Value is JObject objetoAninhado)
                {
                    AddColumnsDynamically(dataTable, objetoAninhado, chave);
                }
                else if (propriedade.Value is JArray array)
                {
                    if (array.Count > 0 && array[0] is JObject)
                    {
                        AddColumnsDynamically(dataTable, array[0], chave);
                    }
                    else
                    {
                        AddColumnIfNotExists(dataTable, chave);
                    }
                }
                else
                {
                    AddColumnIfNotExists(dataTable, chave);
                }
            }
        }

        private static void AddColumnIfNotExists(DataTable dataTable, string columnName)
        {
            if (!dataTable.Columns.Contains(columnName))
            {
                dataTable.Columns.Add(new DataColumn(columnName, typeof(string)));
            }
        }

        private static void FillRowsDynamically(DataTable dataTable, dynamic dados, string prefixo = "")
        {
            DataRow row = dataTable.NewRow();
            FillRowsDynamically(row, dados);
            dataTable.Rows.Add(row);
        }

        private static void FillRowsDynamically(DataRow row, dynamic dados, string prefixo = "")
        {
            foreach (var propriedade in dados.Properties())
            {
                var chave = string.IsNullOrEmpty(prefixo) ? propriedade.Name : $"{prefixo}_{propriedade.Name}";

                if (propriedade.Value is JArray array)
                {
                    if (array.All(item => item is JValue))
                    {
                        row[chave] = string.Join(", ", array.Select(item => item.ToString()));
                    }
                    else
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            AddColumnIfNotExists(row.Table, $"{chave}_{i + 1}");
                            row[$"{chave}_{i + 1}"] = array[i].ToString();
                        }
                    }
                }
                else if (propriedade.Value is JObject objetoAninhado)
                {
                    FillRowsDynamically(row, objetoAninhado, chave);
                }
                else
                {
                    AddColumnIfNotExists(row.Table, chave);

                    if (row.Table.Columns.Contains(chave))
                    {
                        row[chave] = propriedade.Value.ToString();
                    }
                }
            }
        }
    }
}
