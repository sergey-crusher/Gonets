
namespace TimeSetting_v2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.TextBox();
            this.synchronization = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSynchronization = new System.Windows.Forms.Button();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.timeNow = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Текущее время:";
            // 
            // time
            // 
            this.time.Location = new System.Drawing.Point(268, 13);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(185, 31);
            this.time.TabIndex = 1;
            // 
            // synchronization
            // 
            this.synchronization.Location = new System.Drawing.Point(268, 63);
            this.synchronization.Name = "synchronization";
            this.synchronization.Size = new System.Drawing.Size(185, 31);
            this.synchronization.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Последняя синхронизация:";
            // 
            // buttonSynchronization
            // 
            this.buttonSynchronization.Location = new System.Drawing.Point(111, 118);
            this.buttonSynchronization.Name = "buttonSynchronization";
            this.buttonSynchronization.Size = new System.Drawing.Size(258, 73);
            this.buttonSynchronization.TabIndex = 4;
            this.buttonSynchronization.Text = "Принудительно синхронизировать";
            this.buttonSynchronization.UseVisualStyleBackColor = true;
            this.buttonSynchronization.Click += new System.EventHandler(this.buttonSynchronization_Click);
            // 
            // mainTimer
            // 
            this.mainTimer.Enabled = true;
            this.mainTimer.Interval = 14400000;
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // timeNow
            // 
            this.timeNow.Enabled = true;
            this.timeNow.Tick += new System.EventHandler(this.timeNow_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 215);
            this.Controls.Add(this.buttonSynchronization);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.synchronization);
            this.Controls.Add(this.time);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.8F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "TimeSetting_v2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox time;
        private System.Windows.Forms.TextBox synchronization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSynchronization;
        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.Timer timeNow;
    }
}

