namespace Simple.Forms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenceForm));
            this.Txtbox_Licence = new System.Windows.Forms.TextBox();
            this.Lbl_Licence = new System.Windows.Forms.Label();
            this.Btn_ValidateLicense = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // Txtbox_Licence
            // 
            this.Txtbox_Licence.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txtbox_Licence.Location = new System.Drawing.Point(29, 83);
            this.Txtbox_Licence.Name = "Txtbox_Licence";
            this.Txtbox_Licence.Size = new System.Drawing.Size(334, 24);
            this.Txtbox_Licence.TabIndex = 0;
            // 
            // Lbl_Licence
            // 
            this.Lbl_Licence.AutoSize = true;
            this.Lbl_Licence.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Licence.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Lbl_Licence.Location = new System.Drawing.Point(97, 32);
            this.Lbl_Licence.Name = "Lbl_Licence";
            this.Lbl_Licence.Size = new System.Drawing.Size(198, 28);
            this.Lbl_Licence.TabIndex = 1;
            this.Lbl_Licence.Text = "Ingrese su licencia";
            // 
            // Btn_ValidateLicense
            // 
            this.Btn_ValidateLicense.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.Btn_ValidateLicense.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(85)))), ((int)(((byte)(168)))));
            this.Btn_ValidateLicense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ValidateLicense.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ValidateLicense.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Btn_ValidateLicense.IconChar = FontAwesome.Sharp.IconChar.CheckToSlot;
            this.Btn_ValidateLicense.IconColor = System.Drawing.Color.White;
            this.Btn_ValidateLicense.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.Btn_ValidateLicense.IconSize = 35;
            this.Btn_ValidateLicense.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Btn_ValidateLicense.Location = new System.Drawing.Point(135, 137);
            this.Btn_ValidateLicense.Name = "Btn_ValidateLicense";
            this.Btn_ValidateLicense.Padding = new System.Windows.Forms.Padding(1, 0, 20, 0);
            this.Btn_ValidateLicense.Size = new System.Drawing.Size(123, 40);
            this.Btn_ValidateLicense.TabIndex = 45;
            this.Btn_ValidateLicense.Text = "Validar";
            this.Btn_ValidateLicense.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Btn_ValidateLicense.UseVisualStyleBackColor = true;
            this.Btn_ValidateLicense.Click += new System.EventHandler(this.Btn_ValidateLicense_Click);
            // 
            // LicenceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(393, 204);
            this.Controls.Add(this.Btn_ValidateLicense);
            this.Controls.Add(this.Lbl_Licence);
            this.Controls.Add(this.Txtbox_Licence);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LicenceForm";
            this.Text = "SIM-PLE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Txtbox_Licence;
        private System.Windows.Forms.Label Lbl_Licence;
        private FontAwesome.Sharp.IconButton Btn_ValidateLicense;
    }
}