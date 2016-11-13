namespace ScreenRecord
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            //if (_writer.IsOpen)
            //{
            //    _streamVideo.SignalToStop();
            //    _writer.Close();
            //    _writer.Dispose();
            //}
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bt_start = new System.Windows.Forms.Button();
            this.bt_Save = new System.Windows.Forms.Button();
            this.lb_stopWatch = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bt_start
            // 
            this.bt_start.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bt_start.BackgroundImage")));
            this.bt_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.bt_start.ForeColor = System.Drawing.Color.Red;
            this.bt_start.Location = new System.Drawing.Point(-1, 26);
            this.bt_start.Margin = new System.Windows.Forms.Padding(2);
            this.bt_start.Name = "bt_start";
            this.bt_start.Size = new System.Drawing.Size(40, 34);
            this.bt_start.TabIndex = 1;
            this.bt_start.UseVisualStyleBackColor = true;
            this.bt_start.Click += new System.EventHandler(this.bt_start_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bt_Save.BackgroundImage")));
            this.bt_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.bt_Save.ForeColor = System.Drawing.Color.Red;
            this.bt_Save.Location = new System.Drawing.Point(43, 26);
            this.bt_Save.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.Size = new System.Drawing.Size(39, 34);
            this.bt_Save.TabIndex = 2;
            this.bt_Save.UseVisualStyleBackColor = true;
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // lb_stopWatch
            // 
            this.lb_stopWatch.AutoSize = true;
            this.lb_stopWatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lb_stopWatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lb_stopWatch.Location = new System.Drawing.Point(-6, -3);
            this.lb_stopWatch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_stopWatch.Name = "lb_stopWatch";
            this.lb_stopWatch.Size = new System.Drawing.Size(202, 26);
            this.lb_stopWatch.TabIndex = 13;
            this.lb_stopWatch.Text = "00:00:00.0000000";
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(87, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 34);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.bt_start;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 63);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb_stopWatch);
            this.Controls.Add(this.bt_Save);
            this.Controls.Add(this.bt_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Recorder";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_start;
        private System.Windows.Forms.Button bt_Save;
        private System.Windows.Forms.Label lb_stopWatch;
        private System.Windows.Forms.Button button1;
    }
}

