namespace Formularz
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_J_PL = new Button();
            button_J_Ang = new Button();
            DaneWnioska = new DataGridView();
            buttonApplyGrid = new Button();
            buttonApplyWniosek = new Button();
            buttonRemoweWniosek = new Button();
            buttonClearGrid = new Button();
            buttonConvertToSQL = new Button();
            buttonConvertFromSQL = new Button();
            richTextBoxWniosek = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)DaneWnioska).BeginInit();
            SuspendLayout();
            // 
            // button_J_PL
            // 
            button_J_PL.Location = new Point(213, 3);
            button_J_PL.Name = "button_J_PL";
            button_J_PL.Size = new Size(187, 64);
            button_J_PL.TabIndex = 0;
            button_J_PL.Text = "Wersja w j. polskim";
            button_J_PL.UseVisualStyleBackColor = true;
            button_J_PL.Click += button_J_PL_Click;
            // 
            // button_J_Ang
            // 
            button_J_Ang.Location = new Point(8, 3);
            button_J_Ang.Name = "button_J_Ang";
            button_J_Ang.Size = new Size(199, 64);
            button_J_Ang.TabIndex = 1;
            button_J_Ang.Text = "Wersja w j. angielskim";
            button_J_Ang.UseVisualStyleBackColor = true;
            button_J_Ang.Click += button_J_Ang_Click;
            // 
            // DaneWnioska
            // 
            DaneWnioska.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DaneWnioska.Location = new Point(8, 73);
            DaneWnioska.Name = "DaneWnioska";
            DaneWnioska.RowHeadersWidth = 62;
            DaneWnioska.Size = new Size(392, 292);
            DaneWnioska.TabIndex = 3;
            // 
            // buttonApplyGrid
            // 
            buttonApplyGrid.BackColor = Color.Chartreuse;
            buttonApplyGrid.Location = new Point(8, 371);
            buttonApplyGrid.Name = "buttonApplyGrid";
            buttonApplyGrid.Size = new Size(160, 48);
            buttonApplyGrid.TabIndex = 4;
            buttonApplyGrid.Text = "Odczytaj";
            buttonApplyGrid.UseVisualStyleBackColor = false;
            buttonApplyGrid.Click += buttonApplyGrid_Click;
            // 
            // buttonApplyWniosek
            // 
            buttonApplyWniosek.BackColor = Color.Chartreuse;
            buttonApplyWniosek.Location = new Point(406, 441);
            buttonApplyWniosek.Name = "buttonApplyWniosek";
            buttonApplyWniosek.Size = new Size(85, 51);
            buttonApplyWniosek.TabIndex = 5;
            buttonApplyWniosek.Text = "OK";
            buttonApplyWniosek.UseVisualStyleBackColor = false;
            buttonApplyWniosek.Click += buttonApplyWniosek_Click;
            // 
            // buttonRemoweWniosek
            // 
            buttonRemoweWniosek.BackColor = Color.Crimson;
            buttonRemoweWniosek.Location = new Point(925, 441);
            buttonRemoweWniosek.Name = "buttonRemoweWniosek";
            buttonRemoweWniosek.Size = new Size(173, 51);
            buttonRemoweWniosek.TabIndex = 6;
            buttonRemoweWniosek.Text = "Odzucenie";
            buttonRemoweWniosek.UseVisualStyleBackColor = false;
            buttonRemoweWniosek.Click += buttonRemoweWniosek_Click;
            // 
            // buttonClearGrid
            // 
            buttonClearGrid.BackColor = Color.Crimson;
            buttonClearGrid.Location = new Point(292, 371);
            buttonClearGrid.Name = "buttonClearGrid";
            buttonClearGrid.Size = new Size(108, 48);
            buttonClearGrid.TabIndex = 7;
            buttonClearGrid.Text = "Usuń";
            buttonClearGrid.UseVisualStyleBackColor = false;
            buttonClearGrid.Click += buttonClearGrid_Click;
            // 
            // buttonConvertToSQL
            // 
            buttonConvertToSQL.Location = new Point(8, 425);
            buttonConvertToSQL.Name = "buttonConvertToSQL";
            buttonConvertToSQL.Size = new Size(187, 67);
            buttonConvertToSQL.TabIndex = 8;
            buttonConvertToSQL.Text = "Przekonwertować na SQL";
            buttonConvertToSQL.UseVisualStyleBackColor = true;
            buttonConvertToSQL.Click += buttonConvertToSQL_Click;
            // 
            // buttonConvertFromSQL
            // 
            buttonConvertFromSQL.Location = new Point(201, 425);
            buttonConvertFromSQL.Name = "buttonConvertFromSQL";
            buttonConvertFromSQL.Size = new Size(199, 67);
            buttonConvertFromSQL.TabIndex = 9;
            buttonConvertFromSQL.Text = "Wczytać z SQL";
            buttonConvertFromSQL.UseVisualStyleBackColor = true;
            buttonConvertFromSQL.Click += buttonConvertFromSQL_Click;
            // 
            // richTextBoxWniosek
            // 
            richTextBoxWniosek.Location = new Point(406, 3);
            richTextBoxWniosek.Name = "richTextBoxWniosek";
            richTextBoxWniosek.Size = new Size(692, 437);
            richTextBoxWniosek.TabIndex = 10;
            richTextBoxWniosek.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1110, 496);
            Controls.Add(richTextBoxWniosek);
            Controls.Add(buttonConvertFromSQL);
            Controls.Add(buttonConvertToSQL);
            Controls.Add(buttonClearGrid);
            Controls.Add(buttonRemoweWniosek);
            Controls.Add(buttonApplyWniosek);
            Controls.Add(buttonApplyGrid);
            Controls.Add(DaneWnioska);
            Controls.Add(button_J_Ang);
            Controls.Add(button_J_PL);
            Name = "Form1";
            Text = "Wniosek";
            ((System.ComponentModel.ISupportInitialize)DaneWnioska).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button button_J_PL;
        private Button button_J_Ang;
        private DataGridView DaneWnioska;
        private Button buttonApplyGrid;
        private Button buttonApplyWniosek;
        private Button buttonRemoweWniosek;
        private Button buttonClearGrid;
        private Button buttonConvertToSQL;
        private Button buttonConvertFromSQL;
        private RichTextBox richTextBoxWniosek;
    }
}
