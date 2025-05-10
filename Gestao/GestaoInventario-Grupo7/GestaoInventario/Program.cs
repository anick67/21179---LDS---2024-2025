using System;
using System.IO;
using System.Windows.Forms;
using GestaoInventario.Controllers;
using GestaoInventario.Models;
using GestaoInventario.Views;
using System.Globalization;
using System.Threading;
using GestaoInventario.Interfaces;
using GestaoInventario.Services;

namespace GestaoInventario
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            CultureInfo euroCulture = new CultureInfo("pt-PT"); // Portugal, Euro
            Thread.CurrentThread.CurrentCulture = euroCulture;
            Thread.CurrentThread.CurrentUICulture = euroCulture;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string dataFilePath = "inventario.json";

            // Verifica se o ficheiro existe
            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, "[]"); // Cria ficheiro com lista vazia
            }

            InventoryModel model = new InventoryModel(dataFilePath);
            InventoryController controller = new InventoryController(model);

            // Instanciar o exportador concreto
            IExportadorInventario exportador = new ExportadorExcel();

            // Passar o exportador como dependência
            Form1 view = new Form1(controller, exportador);

            try
            {
                Application.Run(view);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocorreu um erro ao iniciar a aplicação:\n\n" + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}