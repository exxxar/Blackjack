namespace Blackjack
{
    partial class AddPlayerForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.buttonAddPlayer = new System.Windows.Forms.Button();
            this.textBoxPlayerMoney = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxMoney = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoney)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.ForestGreen;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.buttonCancel.FlatAppearance.BorderSize = 2;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCancel.Location = new System.Drawing.Point(176, 211);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(145, 68);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.BackColor = System.Drawing.Color.Yellow;
            this.textBoxPlayerName.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerName.Location = new System.Drawing.Point(12, 134);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(145, 35);
            this.textBoxPlayerName.TabIndex = 1;
            // 
            // buttonAddPlayer
            // 
            this.buttonAddPlayer.BackColor = System.Drawing.Color.ForestGreen;
            this.buttonAddPlayer.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.buttonAddPlayer.FlatAppearance.BorderSize = 2;
            this.buttonAddPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddPlayer.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddPlayer.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAddPlayer.Location = new System.Drawing.Point(12, 211);
            this.buttonAddPlayer.Name = "buttonAddPlayer";
            this.buttonAddPlayer.Size = new System.Drawing.Size(145, 65);
            this.buttonAddPlayer.TabIndex = 3;
            this.buttonAddPlayer.Text = "ADD";
            this.buttonAddPlayer.UseVisualStyleBackColor = false;
            this.buttonAddPlayer.Click += new System.EventHandler(this.buttonAddPlayerClick);
            // 
            // textBoxPlayerMoney
            // 
            this.textBoxPlayerMoney.BackColor = System.Drawing.Color.Yellow;
            this.textBoxPlayerMoney.Font = new System.Drawing.Font("Baskerville Old Face", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerMoney.Location = new System.Drawing.Point(176, 134);
            this.textBoxPlayerMoney.Name = "textBoxPlayerMoney";
            this.textBoxPlayerMoney.Size = new System.Drawing.Size(145, 35);
            this.textBoxPlayerMoney.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Blackjack.Properties.Resources.player;
            this.pictureBox1.Location = new System.Drawing.Point(46, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 116);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxMoney
            // 
            this.pictureBoxMoney.Image = global::Blackjack.Properties.Resources.casino_chips_md;
            this.pictureBoxMoney.Location = new System.Drawing.Point(188, 12);
            this.pictureBoxMoney.Name = "pictureBoxMoney";
            this.pictureBoxMoney.Size = new System.Drawing.Size(123, 116);
            this.pictureBoxMoney.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMoney.TabIndex = 7;
            this.pictureBoxMoney.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(53, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 24);
            this.label1.TabIndex = 9;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Yellow;
            this.label2.Location = new System.Drawing.Point(218, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 24);
            this.label2.TabIndex = 10;
            this.label2.Text = "Money";
            // 
            // AddPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LimeGreen;
            this.ClientSize = new System.Drawing.Size(343, 319);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxMoney);
            this.Controls.Add(this.textBoxPlayerMoney);
            this.Controls.Add(this.buttonAddPlayer);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.buttonCancel);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "AddPlayerForm";
            this.Text = "New Player!";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMoney)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Button buttonAddPlayer;
        private System.Windows.Forms.TextBox textBoxPlayerMoney;
        private System.Windows.Forms.PictureBox pictureBoxMoney;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}