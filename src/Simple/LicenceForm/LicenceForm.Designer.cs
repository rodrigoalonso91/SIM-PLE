﻿namespace Simple.LicenceForm
{
    partial class LicenceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Txtbox_Licence = new System.Windows.Forms.TextBox();
            this.Lbl_Licence = new System.Windows.Forms.Label();
            this.Btn_Licence = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // Txtbox_Licence
            // 
            this.Txtbox_Licence.Location = new System.Drawing.Point(171, 82);
            this.Txtbox_Licence.Name = "Txtbox_Licence";
            this.Txtbox_Licence.Size = new System.Drawing.Size(252, 20);
            this.Txtbox_Licence.TabIndex = 0;
            // 
            // Lbl_Licence
            // 
            this.Lbl_Licence.AutoSize = true;
            this.Lbl_Licence.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Licence.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Lbl_Licence.Location = new System.Drawing.Point(166, 34);
            this.Lbl_Licence.Name = "Lbl_Licence";
            this.Lbl_Licence.Size = new System.Drawing.Size(198, 28);
            this.Lbl_Licence.TabIndex = 1;
            this.Lbl_Licence.Text = "Ingrese su licencia";
            // 
            // Btn_Licence
            // 
            this.Btn_Licence.IconChar = FontAwesome.Sharp.IconChar.None;
            this.Btn_Licence.IconColor = System.Drawing.Color.Black;
            this.Btn_Licence.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.Btn_Licence.Location = new System.Drawing.Point(244, 132);
            this.Btn_Licence.Name = "Btn_Licence";
            this.Btn_Licence.Size = new System.Drawing.Size(75, 23);
            this.Btn_Licence.TabIndex = 2;
            this.Btn_Licence.Text = "iconButton1";
            this.Btn_Licence.UseVisualStyleBackColor = true;
            // 
            // LicenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(553, 224);
            this.Controls.Add(this.Btn_Licence);
            this.Controls.Add(this.Lbl_Licence);
            this.Controls.Add(this.Txtbox_Licence);
            this.Name = "LicenceForm";
            this.Text = "SIM-PLE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Txtbox_Licence;
        private System.Windows.Forms.Label Lbl_Licence;
        private FontAwesome.Sharp.IconButton Btn_Licence;
    }
}