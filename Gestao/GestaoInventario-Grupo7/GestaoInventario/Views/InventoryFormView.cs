using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestaoInventario.Controllers;
using GestaoInventario.Models;

namespace GestaoInventario.Views
{
    public partial class Form1 : Form
    {
        private readonly InventoryController _controller;
        private bool bloquearEventos = false;

        public Form1(InventoryController controller)
        {
            InitializeComponent();
            _controller = controller;

            itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsGrid.MultiSelect = false;

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

            categoryComboBox.Items.AddRange(new string[]
            {
                "Guloseimas", "Bebidas", "Higiene", "Papelaria", "Eletrónica", "Jardim", "Outros"
            });

            // Desliga o evento antes de carregar
            itemsGrid.SelectionChanged -= itemsGrid_SelectionChanged;

            RefreshView(); // Atualizar a grid e limpar

            // Volta a ligar o evento
            itemsGrid.SelectionChanged += itemsGrid_SelectionChanged;

            // Quando o formulário for exibido
            this.Shown += Form1_Shown;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string name = nameTextBox.Text;
            string description = descriptionTextBox.Text;
            int quantity = (int)quantityNumeric.Value;
            decimal price = priceNumeric.Value;
            string category = categoryComboBox.Text;

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
                string id = itemsGrid.SelectedRows[0].Cells["ID"].Value?.ToString() ?? "";

                if (_controller.DeleteItem(id))
                {
                    ShowMessage("Removido com sucesso.");
                    RefreshView();
                    LimparCampos();
                }
                else
                {
                    ShowMessage("Erro ao remover.");
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

        public void DisplayInventory(List<Item> items)
        {
            itemsGrid.Rows.Clear();

            foreach (var item in items)
            {
                itemsGrid.Rows.Add(
                    item.Name,
                    item.Category,
                    item.Description,
                    item.Id,
                    item.Quantity,
                    item.Price
                );
            }
        }

        public void RefreshView()
        {
            bloquearEventos = true;

            DisplayInventory(_controller.GetInventory());

            if (itemsGrid.Rows.Count > 0)
            {
                itemsGrid.ClearSelection();
                itemsGrid.CurrentCell = null;
            }

            LimparCampos();

            bloquearEventos = false;
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
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            LimparCampos();             // Limpar todos os campos
            itemsGrid.ClearSelection();  // Limpar seleção da grid
            itemsGrid.CurrentCell = null; // Nenhuma célula ativa
        }
    }
}
