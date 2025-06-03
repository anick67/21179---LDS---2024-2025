using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using GestaoInventario.Interfaces;
using GestaoInventario.Models;
using System.Windows.Forms;

namespace GestaoInventario.Services
{
    // Exporta uma coleção de itens do inventário para um ficheiro Excel
    public class ExportadorExcel : IExportadorInventario
    {
        public void Exportar(IEnumerable<Item> items, IDestinoExportacao destino)
        {
            if (items == null)
            {
                MessageBox.Show("Não há dados para exportar.", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inventário");

                    // Cabeçalhos da tabela
                    string[] headers = { "Nome", "Categoria", "Descrição", "ID", "Quantidade", "Preço" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        var cell = worksheet.Cell(1, i + 1);
                        cell.Value = headers[i];
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Preenche os dados dos itens
                    int row = 2;
                    foreach (var item in items)
                    {
                        worksheet.Cell(row, 1).Value = item.Name;
                        worksheet.Cell(row, 2).Value = item.Category;
                        worksheet.Cell(row, 3).Value = item.Description;
                        worksheet.Cell(row, 4).Value = item.Id;
                        worksheet.Cell(row, 5).Value = item.Quantity;
                        worksheet.Cell(row, 6).Value = item.Price;
                        worksheet.Cell(row, 6).Style.NumberFormat.Format = "#,##0.00 €";
                        row++;
                    }

                    worksheet.Columns().AdjustToContents(); // Ajusta largura das colunas

                    // Pede o caminho ao utilizador
                    string? caminho = destino.ObterCaminho();
                    if (string.IsNullOrWhiteSpace(caminho))
                        return;

                    // Guarda o ficheiro Excel
                    workbook.SaveAs(caminho);
                    MessageBox.Show("Exportação concluída com sucesso!", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ioEx) when ((ioEx.HResult & 0xFFFF) == 32)
            {
                // Ficheiro já está aberto noutro programa
                MessageBox.Show("O ficheiro está aberto noutra aplicação. Por favor, feche o ficheiro antes de exportar novamente.",
                                "Ficheiro em uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}