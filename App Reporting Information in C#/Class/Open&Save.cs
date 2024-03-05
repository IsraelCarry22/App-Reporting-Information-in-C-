using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace App_Reporting_Information_in_C_.Class
{
    static class Open_Save
    {
        static public void GetFileExtension(bool isOpen, ref string filePath, DataGridView ListFilesData)
        {
            if (isOpen)
            {
                switch (Path.GetExtension(filePath).ToLower())
                {
                    case ".csv":
                        OpenFileCSV(ref filePath, ListFilesData);
                        break;
                    case ".xml":
                        OpenFileXML(ref filePath,  ListFilesData);
                        break;
                    case ".json":
                        OpenFileJSON(ref filePath,  ListFilesData);
                        break;
                }
            }
            else
            {
                switch (Path.GetExtension(filePath).ToLower())
                {
                    case ".csv":
                        SaveFileCSV(ref filePath, ListFilesData);
                        break;
                    case ".xml":
                        SaveFileXML(ref filePath, ListFilesData);
                        break;
                    case ".json":
                        SaveFileJSON(ref filePath, ListFilesData);
                        break;
                }
            }

        }

        static public void OpenFileCSV(ref string filePath, DataGridView ListFilesData)
        {
            using (var CSVReader = new StreamReader(filePath))
            {
                string primeraLinea = CSVReader.ReadLine();

                if (primeraLinea == null)
                {
                    return;
                }

                string[] encabezados = primeraLinea.Split(',');

                ListFilesData.Columns.Clear();

                foreach (var encabezado in encabezados)
                {
                    ListFilesData.Columns.Add(encabezado, encabezado);
                }

                while (!CSVReader.EndOfStream)
                {
                    string line = CSVReader.ReadLine();
                    string[] propertyless = line.Split(',');

                    ListFilesData.Rows.Add(propertyless);
                }

                MessageBox.Show("Datos cargados desde el archivo CSV correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static public void OpenFileXML(ref string filePath, DataGridView ListFilesData)
        {
            var dataSet = new DataSet();

            dataSet.ReadXml(filePath);

            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[0];

                foreach (DataColumn column in dataTable.Columns)
                {
                    ListFilesData.Columns.Add(column.ColumnName, column.ColumnName);
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    ListFilesData.Rows.Add(row.ItemArray);
                }

                MessageBox.Show("Datos cargados desde el archivo XML correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static public void OpenFileJSON(ref string filePath, DataGridView ListFilesData)
        {
            string jsonDatos = File.ReadAllText(filePath);

            var listaObjetos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonDatos);

            if (listaObjetos != null && listaObjetos.Count > 0)
            {
                foreach (string clave in listaObjetos[0].Keys)
                {
                    ListFilesData.Columns.Add(clave, clave);
                }

                foreach (var objetoFila in listaObjetos)
                {
                    object[] valores = objetoFila.Values.ToArray();

                    ListFilesData.Rows.Add(valores);
                }

                MessageBox.Show("Datos cargados desde el archivo JSON correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        static public void SaveFileCSV(ref string filePath, DataGridView ListFilesData)
        {
            using (var CSVwriter = new StreamWriter(filePath))
            {
                for (int i = 0; i < ListFilesData.Columns.Count; i++)
                {
                    CSVwriter.Write(ListFilesData.Columns[i].HeaderText);

                    if (i < ListFilesData.Columns.Count - 1)
                    {
                        CSVwriter.Write(",");
                    }
                }

                CSVwriter.WriteLine();

                for (int i = 0; i < ListFilesData.Rows.Count; i++)
                {
                    for (int j = 0; j < ListFilesData.Columns.Count; j++)
                    {
                        CSVwriter.Write(ListFilesData.Rows[i].Cells[j].Value);

                        if (j < ListFilesData.Columns.Count - 1)
                        {
                            CSVwriter.Write(",");
                        }
                    }

                    CSVwriter.WriteLine();
                }
            }

            MessageBox.Show("Archivo CSV guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static public void SaveFileXML(ref string filePath, DataGridView ListFilesData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement rootElement = xmlDoc.CreateElement("Datos");
            xmlDoc.AppendChild(rootElement);

            for (int i = 0; i < ListFilesData.Rows.Count; i++)
            {
                XmlElement rowElement = xmlDoc.CreateElement("Fila");

                for (int j = 0; j < ListFilesData.Columns.Count; j++)
                {
                    string columnName = ListFilesData.Columns[j].HeaderText;

                    string xmlElementName = GetValidXmlElementName(columnName);

                    string cellValue = Convert.ToString(ListFilesData.Rows[i].Cells[j].Value);

                    XmlElement cellElement = xmlDoc.CreateElement(xmlElementName);
                    cellElement.InnerText = cellValue;

                    rowElement.AppendChild(cellElement);
                }

                rootElement.AppendChild(rowElement);
            }

            xmlDoc.Save(filePath);

            MessageBox.Show("Datos guardados como archivo XML correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static private string GetValidXmlElementName(string input)
        {
            string validName = input.Replace(' ', '_');

            validName = new string(validName.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            if (char.IsDigit(validName[0]))
            {
                validName = "_" + validName;
            }

            return validName;
        }

        static public void SaveFileJSON(ref string filePath, DataGridView ListFilesData)
        {
            var listaObjetos = new List<object>();

            foreach (DataGridViewRow fila in ListFilesData.Rows)
            {
                if (!fila.IsNewRow)
                {
                    var objetoFila = new Dictionary<string, object>();

                    foreach (DataGridViewCell celda in fila.Cells)
                    {
                        string nombreColumna = ListFilesData.Columns[celda.ColumnIndex].HeaderText;

                        objetoFila[nombreColumna] = celda.Value;
                    }

                    listaObjetos.Add(objetoFila);
                }
            }

            string jsonDatos = Newtonsoft.Json.JsonConvert.SerializeObject(listaObjetos, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(filePath, jsonDatos);

            MessageBox.Show("Datos guardados como archivo JSON correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
