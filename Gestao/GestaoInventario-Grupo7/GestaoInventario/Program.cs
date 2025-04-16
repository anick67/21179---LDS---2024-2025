using System;
using System.IO;
using System.Windows.Forms;
using GestaoInventario.Controllers;
using GestaoInventario.Models;
using GestaoInventario.Views;

namespace GestaoInventario
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
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
            InventoryFormView view = new InventoryFormView(controller);

            // Protege o arranque com try/catch
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
