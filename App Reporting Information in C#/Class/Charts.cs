using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace App_Reporting_Information_in_C_.Class
{
    static class Charts
    {
        static public void ConfigurationPieForYear(Chart PieYear, DataGridView ListFilesData)
        {
            PieYear.Series.Clear();
            PieYear.Series.Add(new Series("Año"));
            PieYear.Series["Año"].ChartType = SeriesChartType.Pie;

            ShowPieDataYear(PieYear, ListFilesData);
        }

        static public void ConfigurationPie(string NameColumn,Chart PieConsole, DataGridView ListFilesData)
        {
            PieConsole.Series.Clear();
            PieConsole.Series.Add(new Series(NameColumn));
            PieConsole.Series[NameColumn].ChartType = SeriesChartType.Pie;

            ShowColumn(NameColumn, PieConsole, ListFilesData);
        }

        static private void ShowPieDataYear(Chart PieYear, DataGridView ListFilesData)
        {
            PieYear.Series["Año"].Points.Clear();

            var periodoContador = new Dictionary<string, int>();

            foreach (DataGridViewRow row in ListFilesData.Rows)
            {
                object valorCelda = row.Cells[1].Value;
                if (valorCelda != null && valorCelda != "")
                {
                    int año = Convert.ToInt32(valorCelda);

                    int intervalo = (año / 10) * 10;

                    string clave = $"{intervalo} - {intervalo + 9}";
                    if (!periodoContador.ContainsKey(clave))
                    {
                        periodoContador[clave] = 1;
                    }
                    else
                    {
                        periodoContador[clave]++;
                    }
                }
            }

            foreach (var kvp in periodoContador)
            {
                PieYear.Series["Año"].Points.AddXY(kvp.Key, kvp.Value);
            }
        }

        static private void ShowColumn(string NameColumn, Chart PieConsole, DataGridView ListFilesData)
        {
            PieConsole.Series[NameColumn].Points.Clear();

            var consolaContador = new Dictionary<string, int>();

            foreach (DataGridViewRow row in ListFilesData.Rows)
            {
                object valorCelda = row.Cells[NameColumn].Value;
                if (valorCelda != null)
                {
                    string consola = valorCelda.ToString();

                    if (!consolaContador.ContainsKey(consola))
                    {
                        consolaContador[consola] = 1;
                    }
                    else
                    {
                        consolaContador[consola]++;
                    }
                }
            }

            foreach (var kvp in consolaContador)
            {
                PieConsole.Series[NameColumn].Points.AddXY(kvp.Key, kvp.Value);
            }
        }
    }
}
