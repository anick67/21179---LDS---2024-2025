namespace GestaoInventario.Views
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            nameTextBox = new TextBox();
            descriptionTextBox = new TextBox();
            idTextBox = new TextBox();
            quantityNumeric = new NumericUpDown();
            priceNumeric = new NumericUpDown();
            categoryComboBox = new ComboBox();
            searchTextBox = new TextBox();
            itemsGrid = new DataGridView();
            NameColumn = new DataGridViewTextBoxColumn();
            Category = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            ID = new DataGridViewTextBoxColumn();
            Quantidade = new DataGridViewTextBoxColumn();
            Price = new DataGridViewTextBoxColumn();
            addButton = new Button();
            updateButton = new Button();
            deleteButton = new Button();
            searchButton = new Button();
            pictureBox1 = new PictureBox();
            clearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)quantityNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)priceNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)itemsGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 23);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 19;
            label1.Text = "Nome:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 61);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 18;
            label2.Text = "Descrição:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 101);
            label3.Name = "label3";
            label3.Size = new Size(106, 20);
            label3.TabIndex = 17;
            label3.Text = "ID do Produto:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 142);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 16;
            label4.Text = "Quantidade:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 184);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 15;
            label5.Text = "Preço:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(27, 225);
            label6.Name = "label6";
            label6.Size = new Size(77, 20);
            label6.TabIndex = 14;
            label6.Text = "Categoria:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 7.8F);
            label7.Location = new Point(414, 200);
            label7.Name = "label7";
            label7.Size = new Size(131, 17);
            label7.TabIndex = 1;
            label7.Text = "Texto para pesquisar";
            // 
            // nameTextBox
            // 
            nameTextBox.Location = new Point(165, 20);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(380, 27);
            nameTextBox.TabIndex = 13;
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(165, 58);
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(380, 27);
            descriptionTextBox.TabIndex = 12;
            // 
            // idTextBox
            // 
            idTextBox.BackColor = SystemColors.ControlLight;
            idTextBox.Location = new Point(165, 98);
            idTextBox.Name = "idTextBox";
            idTextBox.ReadOnly = true;
            idTextBox.Size = new Size(120, 27);
            idTextBox.TabIndex = 11;
            // 
            // quantityNumeric
            // 
            quantityNumeric.Location = new Point(165, 140);
            quantityNumeric.Name = "quantityNumeric";
            quantityNumeric.Size = new Size(120, 27);
            quantityNumeric.TabIndex = 10;
            // 
            // priceNumeric
            // 
            priceNumeric.Location = new Point(165, 182);
            priceNumeric.Name = "priceNumeric";
            priceNumeric.Size = new Size(120, 27);
            priceNumeric.TabIndex = 9;
            // 
            // categoryComboBox
            // 
            categoryComboBox.Location = new Point(165, 222);
            categoryComboBox.Name = "categoryComboBox";
            categoryComboBox.Size = new Size(121, 28);
            categoryComboBox.TabIndex = 8;
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new Point(414, 223);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(457, 27);
            searchTextBox.TabIndex = 2;
            // 
            // itemsGrid
            // 
            itemsGrid.AllowUserToAddRows = false;
            itemsGrid.AllowUserToDeleteRows = false;
            itemsGrid.AllowUserToResizeRows = false;
            itemsGrid.ColumnHeadersHeight = 29;
            itemsGrid.Columns.AddRange(new DataGridViewColumn[] { NameColumn, Category, Description, ID, Quantidade, Price });
            itemsGrid.Location = new Point(27, 260);
            itemsGrid.MultiSelect = false;
            itemsGrid.Name = "itemsGrid";
            itemsGrid.ReadOnly = true;
            itemsGrid.RowHeadersVisible = false;
            itemsGrid.RowHeadersWidth = 51;
            itemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            itemsGrid.Size = new Size(944, 298);
            itemsGrid.TabIndex = 7;
            itemsGrid.SelectionChanged += itemsGrid_SelectionChanged;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Nome";
            NameColumn.MinimumWidth = 6;
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            NameColumn.Width = 210;
            // 
            // Category
            // 
            Category.HeaderText = "Categoria";
            Category.MinimumWidth = 6;
            Category.Name = "Category";
            Category.ReadOnly = true;
            Category.Width = 165;
            // 
            // Description
            // 
            Description.HeaderText = "Descrição";
            Description.MinimumWidth = 6;
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Width = 240;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.MinimumWidth = 6;
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Width = 85;
            // 
            // Quantidade
            // 
            Quantidade.HeaderText = "Quantidade";
            Quantidade.MinimumWidth = 6;
            Quantidade.Name = "Quantidade";
            Quantidade.ReadOnly = true;
            Quantidade.Width = 91;
            // 
            // Price
            // 
            dataGridViewCellStyle1.Format = "C2";
            Price.DefaultCellStyle = dataGridViewCellStyle1;
            Price.HeaderText = "Preço";
            Price.MinimumWidth = 6;
            Price.Name = "Price";
            Price.ReadOnly = true;
            Price.Width = 85;
            // 
            // addButton
            // 
            addButton.Location = new Point(620, 19);
            addButton.Name = "addButton";
            addButton.Size = new Size(125, 29);
            addButton.TabIndex = 6;
            addButton.Text = "Adicionar";
            addButton.Click += addButton_Click;
            // 
            // updateButton
            // 
            updateButton.Location = new Point(620, 57);
            updateButton.Name = "updateButton";
            updateButton.Size = new Size(125, 29);
            updateButton.TabIndex = 5;
            updateButton.Text = "Atualizar";
            updateButton.Click += updateButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(620, 95);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(125, 29);
            deleteButton.TabIndex = 4;
            deleteButton.Text = "Remover";
            deleteButton.Click += deleteButton_Click;
            // 
            // searchButton
            // 
            searchButton.Location = new Point(877, 222);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(94, 29);
            searchButton.TabIndex = 3;
            searchButton.Text = "Pesquisar";
            searchButton.Click += searchButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Logo_Grupo_7;
            pictureBox1.Location = new Point(824, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(147, 93);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(620, 146);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(125, 29);
            clearButton.TabIndex = 20;
            clearButton.Text = "Limpar";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(999, 570);
            Controls.Add(clearButton);
            Controls.Add(pictureBox1);
            Controls.Add(label7);
            Controls.Add(searchTextBox);
            Controls.Add(searchButton);
            Controls.Add(deleteButton);
            Controls.Add(updateButton);
            Controls.Add(addButton);
            Controls.Add(itemsGrid);
            Controls.Add(categoryComboBox);
            Controls.Add(priceNumeric);
            Controls.Add(quantityNumeric);
            Controls.Add(idTextBox);
            Controls.Add(descriptionTextBox);
            Controls.Add(nameTextBox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestão de Inventário";
            ((System.ComponentModel.ISupportInitialize)quantityNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)priceNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)itemsGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox nameTextBox;
        private TextBox descriptionTextBox;
        private TextBox idTextBox;
        private NumericUpDown quantityNumeric;
        private NumericUpDown priceNumeric;
        private ComboBox categoryComboBox;
        private DataGridView itemsGrid;
        private Button addButton;
        private Button updateButton;
        private Button deleteButton;
        private Button searchButton;
        private TextBox searchTextBox;
        private Label label7;
        private PictureBox pictureBox1;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn Category;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Quantidade;
        private DataGridViewTextBoxColumn Price;
        private Button clearButton;
    }
}
