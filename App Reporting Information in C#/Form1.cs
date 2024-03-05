using App_Reporting_Information_in_C_.Class;
using System;
using System.IO;
using System.Windows.Forms;

namespace App_Reporting_Information_in_C_
{
    public partial class Form1 : Form
    {
        public string filePath;

        public Form1()
        {
            InitializeComponent();
        }

        public bool OpenDialog(ref string filePath, string filter)
        {
            ListFilesData.Rows.Clear();
            ListFilesData.Columns.Clear();

            var fileDialog = new OpenFileDialog
            {
                Filter = filter
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return true;
            }

            filePath = fileDialog.FileName;

            NombreArchivoLBL.Text = $"Archivo: {fileDialog.SafeFileName}";

            return false;
        }

        public bool SaveDialog(ref string filePath, string filter)
        {
            if (ListFilesData.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }

            var saveFileTem = new SaveFileDialog
            {
                Filter = filter
            };

            if (saveFileTem.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show($"Datos no guardados desde el archivo {Path.GetFileName(saveFileTem.FileName)} incorrectamente.", "Operación cancelada", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                return true;
            }

            filePath = saveFileTem.FileName;

            NombreArchivoLBL.Text = $"Archivo: {Path.GetFileName(saveFileTem.FileName)}";

            return false;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenDialog(ref filePath, "Archivos CSV (*.csv)|*.csv|Archivos XML (*.xml)|*.xml|Archivos JSON (*.json)|*.json"))
            {
                return;
            }

            Open_Save.GetFileExtension(true, ref filePath, ListFilesData);

            Charts.ConfigurationPieForYear(PieYear,ListFilesData);
            Charts.ConfigurationPie("Consola",PieConsole, ListFilesData);
            Charts.ConfigurationPie("Género", PieGenere, ListFilesData);
            Charts.ConfigurationPie("Región", PieRegion, ListFilesData);
            Charts.ConfigurationPie("Generación", PieGeneration, ListFilesData);
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (SaveDialog(ref filePath, "Archivos CSV (*.csv)|*.csv|Archivos XML (*.xml)|*.xml|Archivos JSON (*.json)|*.json"))
                {
                    return;
                }

                Open_Save.GetFileExtension(false, ref filePath, ListFilesData);
            }
            else
            {
                Open_Save.GetFileExtension(false, ref filePath, ListFilesData);
            }
        }

        private void SaveHowMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveDialog(ref filePath, "Archivos CSV (*.csv)|*.csv|Archivos XML (*.xml)|*.xml|Archivos JSON (*.json)|*.json"))
            {
                return;
            }

            Open_Save.GetFileExtension(false, ref filePath, ListFilesData);
        }

        private void FileMenuItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = false;
        }

        private void AñoMenuSubItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = true;
            PanelGraphicsConsolas.Visible = false;
        }

        private void ConsolasMenuSubItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = true;
            PanelGraphicsConsolas.Visible = true;
            PanelGenero.Visible = false;
        }

        private void GeneroMenuSubItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = true;
            PanelGraphicsConsolas.Visible = true;
            PanelGenero.Visible = true;
            PanelRegion.Visible = false;
        }

        private void RegionMenuSubItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = true;
            PanelGraphicsConsolas.Visible = true;
            PanelGenero.Visible = true;
            PanelRegion.Visible = true;
            PanelGeneracion.Visible = false;
        }

        private void GeneraciónMenuSubItem_Click(object sender, EventArgs e)
        {
            PanelGraphicsAño.Visible = true;
            PanelGraphicsConsolas.Visible = true;
            PanelGenero.Visible = true;
            PanelRegion.Visible = true;
            PanelGeneracion.Visible = true;
        }
    }
}
