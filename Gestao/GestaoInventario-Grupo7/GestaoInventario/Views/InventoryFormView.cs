using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;
using GestaoInventario.Controllers;
using GestaoInventario.Models;
using GestaoInventario.Interfaces;
using GestaoInventario.Services;

namespace GestaoInventario.Views
{
    public partial class Form1 : Form
    {
        private readonly InventoryController _controller;
        private readonly IExportadorInventario _exportador;
        private bool bloquearEventos = false;
        private bool temStockBaixoParaAlertar = false;
        private bool alertaMostrado = false;

        public event EventHandler<Item>? ProdutoAdicionado;
        public event EventHandler<Item>? ProdutoAtualizado;
        public event EventHandler<string>? ProdutoRemovido;
        public event EventHandler? StockCriticoDetectado;
        public Form1(InventoryController controller, IExportadorInventario exportador)
        {
            InitializeComponent();
            _controller = controller;
            _exportador = exportador;

            priceNumeric.DecimalPlaces = 2;

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
                itemsGrid.Columns[1].Width = 105;
                itemsGrid.Columns[2].Width = 255;
                itemsGrid.Columns[3].Width = 63;
                itemsGrid.Columns[4].Width = 78;
                itemsGrid.Columns[5].Width = 80;
                itemsGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                itemsGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                itemsGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                itemsGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                itemsGrid.Columns[5].DefaultCellStyle.Format = "C2";
                itemsGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                itemsGrid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

                // Evento disparado após adição bem-sucedida
                ProdutoAdicionado?.Invoke(this, new Item
                {
                    Name = name,
                    Description = description,
                    Quantity = quantity,
                    Price = price,
                    Category = category
                });
            }
            else
            {
                ShowMessage("Erro ao adicionar o produto.");
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string id = idTextBox.Text;
            string name = nameTextBox.Text;
            string description = descriptionTextBox.Text;
            int quantity = (int)quantityNumeric.Value;
            decimal price = priceNumeric.Value;
            string category = categoryComboBox.Text;

            if (_controller.UpdateItem(id, name, description, quantity, price, category))
            {
                ShowMessage("Produto atualizado.");

                var atualizado = _controller.GetItemById(id);
                if (atualizado != null)
                {
                    ProdutoAtualizado?.Invoke(this, atualizado);
                }

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

                        // Dispara o evento para informar que um produto foi removido
                        ProdutoRemovido?.Invoke(this, id);

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
            categoryComboBox.Text = "";
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
                    temStockBaixoParaAlertar = false;

                    // Dispara o evento em vez de mostrar a MessageBox diretamente
                    StockCriticoDetectado?.Invoke(this, EventArgs.Empty);

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
            var destino = new DestinoFicheiro("Inventario_Completo.xlsx");
            _exportador.Exportar(todos, destino);
        }

        private void btnExportarStockBaixo_Click(object sender, EventArgs e)
        {
            var baixos = _controller.GetInventory().Where(i => i.Quantity < 5).ToList();
            var destino = new DestinoFicheiro("Inventario_Critico.xlsx");
            _exportador.Exportar(baixos, destino);
        }
    }
}
