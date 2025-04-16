using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestaoInventario.Controllers;
using GestaoInventario.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GestaoInventario.Views
{
    public partial class InventoryFormView : Form, IInventoryView
    {
        private readonly InventoryController _controller;

        public InventoryFormView(InventoryController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        public void DisplayInventory(List<Item> items)
        {
            itemsGrid.DataSource = null;
            itemsGrid.DataSource = items;
        }

        public void DisplayItemDetails(Item item)
        {
            nameTextBox.Text = item.Name;
            descriptionTextBox.Text = item.Description;
            quantityNumeric.Value = item.Quantity;
            priceNumeric.Value = item.Price;
            categoryComboBox.Text = item.Category;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void RefreshView()
        {
            DisplayInventory(_controller.GetInventory());
        }

        private void AddButton_Click(object sender, EventArgs e)
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
            }
            else
            {
                ShowMessage("Erro ao adicionar o produto.");
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (itemsGrid.SelectedRows.Count > 0)
            {
                string id = itemsGrid.SelectedRows[0].Cells["Id"].Value.ToString();

                if (_controller.DeleteItem(id))
                {
                    ShowMessage("Removido com sucesso.");
                    RefreshView();
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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string query = searchTextBox.Text.Trim();
            var results = _controller.SearchInventory(query);
            DisplayInventory(results);
        }

        private void LowStockButton_Click(object sender, EventArgs e)
        {
            var lowStock = _controller.GetLowStockAlert(5);
            DisplayInventory(lowStock);
        }

        private void InitializeComponent()
        {
            nameTextBox = new System.Windows.Forms.TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            quantityNumeric = new NumericUpDown();
            priceNumeric = new NumericUpDown();
            categoryComboBox = new System.Windows.Forms.ComboBox();
            descriptionTextBox = new System.Windows.Forms.TextBox();
            idTextBox = new System.Windows.Forms.TextBox();
            addButton = new System.Windows.Forms.Button();
            updateButton = new System.Windows.Forms.Button();
            deleteButton = new System.Windows.Forms.Button();
            itemsGrid = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            searchTextBox = new System.Windows.Forms.TextBox();
            label7 = new Label();
            searchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)quantityNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)priceNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)itemsGrid).BeginInit();
            SuspendLayout();
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(142, 12);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(258, 27);
            nameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 19);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 1;
            label1.Text = "Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 57);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 2;
            label2.Text = "Descrição:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 97);
            label3.Name = "label3";
            label3.Size = new Size(27, 20);
            label3.TabIndex = 3;
            label3.Text = "ID:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(28, 138);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 4;
            label4.Text = "Quantidade:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(28, 180);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 5;
            label5.Text = "Preço:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(28, 223);
            label6.Name = "label6";
            label6.Size = new Size(77, 20);
            label6.TabIndex = 6;
            label6.Text = "Categoria:";
            // 
            // quantityNumeric
            // 
            quantityNumeric.Location = new Point(142, 131);
            quantityNumeric.Name = "quantityNumeric";
            quantityNumeric.Size = new Size(182, 27);
            quantityNumeric.TabIndex = 7;
            // 
            // priceNumeric
            // 
            priceNumeric.Location = new Point(142, 173);
            priceNumeric.Name = "priceNumeric";
            priceNumeric.Size = new Size(182, 27);
            priceNumeric.TabIndex = 8;
            // 
            // categoryComboBox
            // 
            categoryComboBox.FormattingEnabled = true;
            categoryComboBox.Location = new Point(142, 215);
            categoryComboBox.Name = "categoryComboBox";
            categoryComboBox.Size = new Size(182, 28);
            categoryComboBox.TabIndex = 9;
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(142, 50);
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(258, 27);
            descriptionTextBox.TabIndex = 11;
            // 
            // idTextBox
            // 
            idTextBox.Location = new Point(142, 90);
            idTextBox.Name = "idTextBox";
            idTextBox.Size = new Size(182, 27);
            idTextBox.TabIndex = 12;
            // 
            // addButton
            // 
            addButton.Location = new Point(573, 9);
            addButton.Name = "addButton";
            addButton.Size = new Size(133, 29);
            addButton.TabIndex = 13;
            addButton.Text = "Adicionar";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click_1;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(573, 47);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(133, 29);
            updateButton.TabIndex = 14;
            updateButton.Text = "Atualizar";
            updateButton.UseVisualStyleBackColor = true;
            updateButton.Click += updateButton_Click_1;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(573, 87);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(133, 29);
            deleteButton.TabIndex = 15;
            deleteButton.Text = "Remover";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click_1;
            // 
            // itemsGrid
            // 
            itemsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            itemsGrid.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            itemsGrid.Location = new Point(28, 257);
            itemsGrid.Name = "itemsGrid";
            itemsGrid.RowHeadersWidth = 51;
            itemsGrid.Size = new Size(678, 197);
            itemsGrid.TabIndex = 16;
            // 
            // Column1
            // 
            Column1.HeaderText = "Nome";
            Column1.MinimumWidth = 6;
            Column1.Name = "Column1";
            Column1.Width = 125;
            // 
            // Column2
            // 
            Column2.HeaderText = "Descrição";
            Column2.MinimumWidth = 6;
            Column2.Name = "Column2";
            Column2.Width = 125;
            // 
            // Column3
            // 
            Column3.HeaderText = "ID";
            Column3.MinimumWidth = 6;
            Column3.Name = "Column3";
            Column3.Width = 125;
            // 
            // Column4
            // 
            Column4.HeaderText = "Quantidade";
            Column4.MinimumWidth = 6;
            Column4.Name = "Column4";
            Column4.Width = 125;
            // 
            // Column5
            // 
            Column5.HeaderText = "Preço";
            Column5.MinimumWidth = 6;
            Column5.Name = "Column5";
            Column5.Width = 125;
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(408, 181);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(298, 27);
            searchTextBox.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(408, 159);
            label7.Name = "label7";
            label7.Size = new Size(146, 20);
            label7.TabIndex = 18;
            label7.Text = "Texto para pesquisar";
            // 
            // searchButton
            // 
            searchButton.Location = new Point(573, 214);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(133, 29);
            searchButton.TabIndex = 19;
            searchButton.Text = "Pesquisar";
            searchButton.UseVisualStyleBackColor = true;
            searchButton.Click += searchButton_Click_1;
            // 
            // InventoryFormView
            // 
            ClientSize = new Size(1025, 466);
            Controls.Add(searchButton);
            Controls.Add(label7);
            Controls.Add(searchTextBox);
            Controls.Add(itemsGrid);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(addButton);
            Controls.Add(idTextBox);
            Controls.Add(descriptionTextBox);
            Controls.Add(categoryComboBox);
            Controls.Add(priceNumeric);
            Controls.Add(quantityNumeric);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(nameTextBox);
            Name = "InventoryFormView";
            ((System.ComponentModel.ISupportInitialize)quantityNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)priceNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)itemsGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private NumericUpDown quantityNumeric;
        private NumericUpDown priceNumeric;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button deleteButton;
        private DataGridView itemsGrid;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.TextBox searchTextBox;
        private Label label7;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox nameTextBox;

        private void addButton_Click_1(object sender, EventArgs e)
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
            }
            else
            {
                ShowMessage("Erro ao adicionar o produto.");
            }
        }

        private void updateButton_Click_1(object sender, EventArgs e)
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

        private void deleteButton_Click_1(object sender, EventArgs e)
        {
            if (itemsGrid.SelectedRows.Count > 0)
            {
                string id = itemsGrid.SelectedRows[0].Cells["Id"].Value.ToString();

                if (_controller.DeleteItem(id))
                {
                    ShowMessage("Removido com sucesso.");
                    RefreshView();
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

        private void searchButton_Click_1(object sender, EventArgs e)
        {
            string query = searchTextBox.Text.Trim();
            var results = _controller.SearchInventory(query);
            DisplayInventory(results);
        }
    }
}
