namespace FSPCB.FSC
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.listDevices = new System.Windows.Forms.ListBox();
            this.ctrlTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsConnectedDevices = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsSimConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listDevices
            // 
            this.listDevices.FormattingEnabled = true;
            this.listDevices.Location = new System.Drawing.Point(12, 29);
            this.listDevices.Name = "listDevices";
            this.listDevices.Size = new System.Drawing.Size(241, 199);
            this.listDevices.TabIndex = 4;
            // 
            // ctrlTimer
            // 
            this.ctrlTimer.Enabled = true;
            this.ctrlTimer.Interval = 2000;
            this.ctrlTimer.Tick += new System.EventHandler(this.ctrlTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsConnectedDevices,
            this.tsSimConnected});
            this.statusStrip1.Location = new System.Drawing.Point(0, 273);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(279, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsConnectedDevices
            // 
            this.tsConnectedDevices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.tsConnectedDevices.Name = "tsConnectedDevices";
            this.tsConnectedDevices.Size = new System.Drawing.Size(117, 17);
            this.tsConnectedDevices.Text = "0 Devices Connected";
            // 
            // tsSimConnected
            // 
            this.tsSimConnected.BackColor = System.Drawing.Color.LightCoral;
            this.tsSimConnected.Name = "tsSimConnected";
            this.tsSimConnected.Size = new System.Drawing.Size(146, 17);
            this.tsSimConnected.Text = "Simulator NOT Connected";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 295);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listDevices);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "FlightsimPCB Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listDevices;
        private System.Windows.Forms.Timer ctrlTimer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsConnectedDevices;
        private System.Windows.Forms.ToolStripStatusLabel tsSimConnected;
    }
}

