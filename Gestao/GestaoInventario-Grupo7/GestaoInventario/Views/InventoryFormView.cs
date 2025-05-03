using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;
using GestaoInventario.Controllers;
using GestaoInventario.Models;

namespace GestaoInventario.Views
{
    public partial class Form1 : Form
    {
        private readonly InventoryController _controller;
        private bool bloquearEventos = false;
        private bool temStockBaixoParaAlertar = false;
        private bool alertaMostrado = false;

        public Form1(InventoryController controller)
        {
            InitializeComponent();
            _controller = controller;

            // Impede casas decimais na quantidade e colar texto
            quantityNumeric.DecimalPlaces = 0;
            quantityNumeric.Minimum = 0;
            quantityNumeric.Maximum = 99999;
            quantityNumeric.KeyPress += QuantityNumeric_KeyPress;
            quantityNumeric.Controls[1].KeyDown += QuantityNumeric_KeyDown;

            // Impede colar texto no campo preço
            priceNumeric.Controls[1].KeyDown += PriceNumeric_KeyDown;

            itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsGrid.MultiSelect = false;
            itemsGrid.ShowCellToolTips = true;

            if (itemsGrid.Columns.Count > 0)
            {
                itemsGrid.Columns[0].Width = 225;
                itemsGrid.Columns[1].Width = 110;
                itemsGrid.Columns[2].Width = 260;
                itemsGrid.Columns[3].Width = 63;
                itemsGrid.Columns[4].Width = 80;
                itemsGrid.Columns[5].Width = 85;
                itemsGrid.Columns[5].DefaultCellStyle.Format = "C2";
            }

            categoryComboBox.SelectedIndexChanged += categoryComboBox_SelectedIndexChanged;

            categoryComboBox.Items.AddRange(new string[]
            {
            "Nova Categoria...", "Guloseimas", "Bebidas", "Higiene", "Papelaria", "Eletrónica", "Jardim", "Outros"
            });

            // Desliga o evento antes de carregar
            itemsGrid.SelectionChanged -= itemsGrid_SelectionChanged;

            // Volta a ligar o evento
            itemsGrid.SelectionChanged += itemsGrid_SelectionChanged;

            // Quando o formulário for exibido
            this.Shown += Form1_Shown;

            // Cabeçalho da grelha personalizado
            itemsGrid.EnableHeadersVisualStyles = false;
            itemsGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            itemsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            itemsGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9);
        }

        private void QuantityNumeric_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Só permite dígitos e teclas de controlo como backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void QuantityNumeric_KeyDown(object? sender, KeyEventArgs e)
        {
            // Impede colar com Ctrl+V
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void PriceNumeric_KeyDown(object? sender, KeyEventArgs e)
        {
            // Bloqueia colar com Ctrl+V
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string name = nameTextBox.Text;
            string description = descriptionTextBox.Text;
            int quantity = (int)quantityNumeric.Value;
            decimal price = priceNumeric.Value;
            string category = categoryComboBox.Text;

            // Alerta se quantidade ou preço forem zero
            if (quantity == 0 || price == 0)
            {
                DialogResult confirmacao = MessageBox.Show(
                    "Está a adicionar um produto com " +
                    (quantity == 0 && price == 0 ? "quantidade e preço" :
                     quantity == 0 ? "quantidade" :
                     "preço") +
                    " zero.\n\nDeseja continuar?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirmacao == DialogResult.No)
                {
                    return;
                }
            }

            if (_controller.AddNewItem(name, description, quantity, price, category))
            {
                ShowMessage("Produto adicionado com sucesso.");
                RefreshView();
                LimparCampos();
            }
            else
            {
                ShowMessage("Erro ao adicionar o produto.");
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string id = idTextBox.Text;
            int quantity = (int)quantityNumeric.Value;

            if (_controller.UpdateItemQuantity(id, quantity))
            {
                ShowMessage("Produto atualizado.");
                RefreshView();
            }
            else
            {
                ShowMessage("Erro ao atualizar.");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (itemsGrid.SelectedRows.Count > 0)
            {
                string nomeProduto = itemsGrid.SelectedRows[0].Cells["NameColumn"].Value?.ToString() ?? "(sem nome)";
                string descricao = itemsGrid.SelectedRows[0].Cells["Description"].Value?.ToString() ?? "(sem descrição)";
                string id = itemsGrid.SelectedRows[0].Cells["ID"].Value?.ToString() ?? "(sem ID)";

                DialogResult resultado = MessageBox.Show(
                    $"Tem a certeza que deseja remover o produto:\n\n" +
                    $"Nome: {nomeProduto}\n" +
                    $"Descrição: {descricao}\n" +
                    $"ID: {id}",
                    "Confirmação de Remoção",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resultado == DialogResult.Yes)
                {
                    if (_controller.DeleteItem(id))
                    {
                        ShowMessage("Produto removido com sucesso.");
                        RefreshView();
                        LimparCampos();
                    }
                    else
                    {
                        ShowMessage("Erro ao remover o produto.");
                    }
                }
            }
            else
            {
                ShowMessage("Seleciona um item para remover.");
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string query = searchTextBox.Text.Trim();
            var results = _controller.SearchInventory(query);
            DisplayInventory(results);

            itemsGrid.ClearSelection();
            itemsGrid.CurrentCell = null;
            LimparCampos();
        }

        private void categoryComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (categoryComboBox.SelectedItem?.ToString() == "Nova categoria...")
            {
                string novaCategoria = PromptInput("Nova Categoria", "Introduza o nome da nova categoria:");

                if (!string.IsNullOrWhiteSpace(novaCategoria))
                {
                    if (!categoryComboBox.Items.Contains(novaCategoria))
                    {
                        // Inserir antes de "Nova categoria..."
                        categoryComboBox.Items.Insert(categoryComboBox.Items.Count - 1, novaCategoria);
                    }

                    categoryComboBox.SelectedItem = novaCategoria;
                }
                else
                {
                    // Voltar atrás caso o utilizador cancele
                    categoryComboBox.SelectedIndex = -1;
                }
            }
        }

        private void itemsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (bloquearEventos) return;

            if (itemsGrid.SelectedRows.Count == 0)
            {
                LimparCampos();
                return;
            }

            var row = itemsGrid.SelectedRows[0];
            idTextBox.Text = row.Cells["ID"].Value?.ToString();
            nameTextBox.Text = row.Cells["NameColumn"].Value?.ToString();
            descriptionTextBox.Text = row.Cells["Description"].Value?.ToString();
            quantityNumeric.Value = Convert.ToInt32(row.Cells["Quantidade"].Value);
            priceNumeric.Value = Math.Round(Convert.ToDecimal(row.Cells["Price"].Value), 2);
            categoryComboBox.Text = row.Cells["Category"].Value?.ToString();
        }

        public bool DisplayInventory(List<Item> items)
        {
            itemsGrid.Rows.Clear();
            bool temStockBaixo = false;

            foreach (var item in items)
            {
                int rowIndex = itemsGrid.Rows.Add(
                    item.Name,
                    item.Category,
                    item.Description,
                    item.Id,
                    item.Quantity,
                    item.Price
                );

                if (item.Quantity < 5)
                {
                    itemsGrid.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Moccasin;
                    foreach (DataGridViewCell cell in itemsGrid.Rows[rowIndex].Cells)
                    {
                        cell.ToolTipText = "Stock crítico";
                    }
                    temStockBaixo = true;
                }
            }

            return temStockBaixo;
        }

        public void RefreshView()
        {
            bloquearEventos = true;

            temStockBaixoParaAlertar = DisplayInventory(_controller.GetInventory());

            if (itemsGrid.Rows.Count > 0)
            {
                itemsGrid.ClearSelection();
                itemsGrid.CurrentCell = null;
            }

            LimparCampos();

            bloquearEventos = false;

            this.Activate(); // Força nova ativação após dados carregados
        }

        private void LimparCampos()
        {
            nameTextBox.Text = "";
            descriptionTextBox.Text = "";
            quantityNumeric.Value = 0;
            priceNumeric.Value = 0;
            categoryComboBox.SelectedIndex = -1;
            idTextBox.Text = "";
        }

        private string PromptInput(string titulo, string mensagem)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                Text = titulo
            };

            Label lbl = new Label() { Left = 20, Top = 20, Text = mensagem, Width = 340 };
            TextBox txt = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmar = new Button() { Text = "OK", Left = 270, Width = 90, Top = 80, DialogResult = DialogResult.OK };

            prompt.Controls.Add(lbl);
            prompt.Controls.Add(txt);
            prompt.Controls.Add(confirmar);
            prompt.AcceptButton = confirmar;

            return prompt.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : "";
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                ShowMessage("O nome não pode estar vazio.");
                return false;
            }

            if (quantityNumeric.Value < 0)
            {
                ShowMessage("A quantidade não pode ser negativa.");
                return false;
            }

            if (priceNumeric.Value < 0)
            {
                ShowMessage("O preço não pode ser negativo.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(categoryComboBox.Text))
            {
                ShowMessage("A categoria é obrigatória.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                ShowMessage("A descrição é obrigatória.");
                return false;
            }

            return true;
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            bloquearEventos = true;

            itemsGrid.ClearSelection();
            itemsGrid.CurrentCell = null;
            LimparCampos();

            bloquearEventos = false;

            // Adia a chamada de RefreshView para garantir que o formulário já está visível
            BeginInvoke(new Action(() =>
            {
                RefreshView();

                if (temStockBaixoParaAlertar && !alertaMostrado)
                {
                    alertaMostrado = true;

                    MessageBox.Show("Atenção: Existem produtos com stock inferior a 5 unidades.",
                                    "Aviso de Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    temStockBaixoParaAlertar = false;

                    itemsGrid.ClearSelection();
                    itemsGrid.CurrentCell = null;
                }
            }));
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            LimparCampos();
            itemsGrid.ClearSelection();
            itemsGrid.CurrentCell = null;
        }

        private void btnExportarTodos_Click(object sender, EventArgs e)
        {
            var todos = _controller.GetInventory();
            ExportarParaExcel(todos, "Inventario_Completo.xlsx");

        }

        private void btnExportarStockBaixo_Click(object sender, EventArgs e)
        {

            var baixos = _controller.GetInventory().Where(i => i.Quantity < 5).ToList();
            ExportarParaExcel(baixos, "Inventario_Critico.xlsx");
        }

        private void ExportarParaExcel(List<Item> items, string tituloFicheiro)
        {
            if (items == null || items.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Ficheiros Excel (*.xlsx)|*.xlsx";
                saveDialog.FileName = tituloFicheiro;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("Inventário");

                            // Cabeçalhos
                            string[] headers = { "Nome", "Categoria", "Descrição", "ID", "Quantidade", "Preço" };
                            for (int i = 0; i < headers.Length; i++)
                            {
                                var cell = worksheet.Cell(1, i + 1);
                                cell.Value = headers[i];
                                cell.Style.Font.Bold = true;
                                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            // Dados
                            int row = 2;
                            foreach (var item in items)
                            {
                                worksheet.Cell(row, 1).Value = item.Name;
                                worksheet.Cell(row, 2).Value = item.Category;
                                worksheet.Cell(row, 3).Value = item.Description;
                                worksheet.Cell(row, 4).Value = item.Id;
                                worksheet.Cell(row, 5).Value = item.Quantity;
                                worksheet.Cell(row, 6).Value = item.Price;
                                worksheet.Cell(row, 6).Style.NumberFormat.Format = "€ #,##0.00"; // Formato moeda
                                row++;
                            }

                            worksheet.Columns().AdjustToContents(); // Largura automática
                            workbook.SaveAs(saveDialog.FileName);
                        }

                        MessageBox.Show("Exportação concluída com sucesso!", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (IOException ioEx) when ((ioEx.HResult & 0xFFFF) == 32)
                    {
                        MessageBox.Show("O ficheiro está aberto noutra aplicação. Por favor, feche o ficheiro antes de exportar novamente.",
                                        "Ficheiro em uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
