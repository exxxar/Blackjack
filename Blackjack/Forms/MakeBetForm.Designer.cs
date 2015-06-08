namespace Blackjack
{
    partial class MakeBetForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxMoney = new System.Windows.Forms.PictureBox();
            this.textBoxPlayerStake = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelPlayerName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoney)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(170, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "Please make your bet!";
            // 
            // pictureBoxMoney
            // 
            this.pictureBoxMoney.Image = global::Blackjack.Properties.Resources.casino_chips_md;
            this.pictureBoxMoney.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxMoney.Name = "pictureBoxMoney";
            this.pictureBoxMoney.Size = new System.Drawing.Size(138, 220);
            this.pictureBoxMoney.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMoney.TabIndex = 12;
            this.pictureBoxMoney.TabStop = false;
            // 
            // textBoxPlayerStake
            // 
            this.textBoxPlayerStake.BackColor = System.Drawing.Color.Yellow;
            this.textBoxPlayerStake.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerStake.Location = new System.Drawing.Point(174, 103);
            this.textBoxPlayerStake.Name = "textBoxPlayerStake";
            this.textBoxPlayerStake.Size = new System.Drawing.Size(192, 35);
            this.textBoxPlayerStake.TabIndex = 11;
            // 
            // buttonOK
            // 
            this.buttonOK.BackColor = System.Drawing.Color.ForestGreen;
            this.buttonOK.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.buttonOK.FlatAppearance.BorderSize = 2;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOK.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonOK.Location = new System.Drawing.Point(174, 175);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(192, 57);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = false;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelPlayerName
            // 
            this.labelPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPlayerName.ForeColor = System.Drawing.Color.Yellow;
            this.labelPlayerName.Location = new System.Drawing.Point(170, 32);
            this.labelPlayerName.Name = "labelPlayerName";
            this.labelPlayerName.Size = new System.Drawing.Size(196, 24);
            this.labelPlayerName.TabIndex = 15;
            this.labelPlayerName.Text = "Player";
            this.labelPlayerName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MakeBetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(378, 259);
            this.ControlBox = false;
            this.Controls.Add(this.labelPlayerName);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxMoney);
            this.Controls.Add(this.textBoxPlayerStake);
            this.DoubleBuffered = true;
            this.Name = "MakeBetForm";
            this.Text = "Bets";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoney)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxMoney;
        private System.Windows.Forms.TextBox textBoxPlayerStake;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelPlayerName;
    }
}