namespace SqlVersionService.Desktop
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblBaseUrl;
        private System.Windows.Forms.TextBox txtBaseUrl;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnOpenConnection;
        private System.Windows.Forms.Button btnGetVersion;
        private System.Windows.Forms.Button btnCloseConnection;
        private System.Windows.Forms.Label lblConnectionId;
        private System.Windows.Forms.TextBox txtConnectionId;
        private System.Windows.Forms.Label lblSqlVersion;
        private System.Windows.Forms.TextBox txtSqlVersion;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBaseUrl = new System.Windows.Forms.Label();
            this.txtBaseUrl = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnOpenConnection = new System.Windows.Forms.Button();
            this.btnGetVersion = new System.Windows.Forms.Button();
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.lblConnectionId = new System.Windows.Forms.Label();
            this.txtConnectionId = new System.Windows.Forms.TextBox();
            this.lblSqlVersion = new System.Windows.Forms.Label();
            this.txtSqlVersion = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            this.lblBaseUrl.AutoSize = true;
            this.lblBaseUrl.Location = new System.Drawing.Point(12, 15);
            this.lblBaseUrl.Name = "lblBaseUrl";
            this.lblBaseUrl.Size = new System.Drawing.Size(92, 13);
            this.lblBaseUrl.Text = "Service Base Url:";

            this.txtBaseUrl.Location = new System.Drawing.Point(140, 12);
            this.txtBaseUrl.Name = "txtBaseUrl";
            this.txtBaseUrl.Size = new System.Drawing.Size(520, 20);

            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(12, 45);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(95, 13);
            this.lblConnectionString.Text = "Connection String:";

            this.txtConnectionString.Location = new System.Drawing.Point(140, 42);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(520, 20);

            this.btnOpenConnection.Location = new System.Drawing.Point(15, 80);
            this.btnOpenConnection.Name = "btnOpenConnection";
            this.btnOpenConnection.Size = new System.Drawing.Size(140, 30);
            this.btnOpenConnection.Text = "Open Connection";
            this.btnOpenConnection.UseVisualStyleBackColor = true;
            this.btnOpenConnection.Click += new System.EventHandler(this.btnOpenConnection_Click);

            this.btnGetVersion.Location = new System.Drawing.Point(170, 80);
            this.btnGetVersion.Name = "btnGetVersion";
            this.btnGetVersion.Size = new System.Drawing.Size(140, 30);
            this.btnGetVersion.Text = "Get Version";
            this.btnGetVersion.UseVisualStyleBackColor = true;
            this.btnGetVersion.Click += new System.EventHandler(this.btnGetVersion_Click);

            this.btnCloseConnection.Location = new System.Drawing.Point(325, 80);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(140, 30);
            this.btnCloseConnection.Text = "Close Connection";
            this.btnCloseConnection.UseVisualStyleBackColor = true;
            this.btnCloseConnection.Click += new System.EventHandler(this.btnCloseConnection_Click);

            this.lblConnectionId.AutoSize = true;
            this.lblConnectionId.Location = new System.Drawing.Point(12, 130);
            this.lblConnectionId.Name = "lblConnectionId";
            this.lblConnectionId.Size = new System.Drawing.Size(77, 13);
            this.lblConnectionId.Text = "Connection Id:";

            this.txtConnectionId.Location = new System.Drawing.Point(140, 127);
            this.txtConnectionId.Name = "txtConnectionId";
            this.txtConnectionId.ReadOnly = true;
            this.txtConnectionId.Size = new System.Drawing.Size(520, 20);

            this.lblSqlVersion.AutoSize = true;
            this.lblSqlVersion.Location = new System.Drawing.Point(12, 160);
            this.lblSqlVersion.Name = "lblSqlVersion";
            this.lblSqlVersion.Size = new System.Drawing.Size(66, 13);
            this.lblSqlVersion.Text = "SQL Version:";

            this.txtSqlVersion.Location = new System.Drawing.Point(140, 157);
            this.txtSqlVersion.Multiline = true;
            this.txtSqlVersion.Name = "txtSqlVersion";
            this.txtSqlVersion.ReadOnly = true;
            this.txtSqlVersion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSqlVersion.Size = new System.Drawing.Size(520, 90);

            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 265);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(40, 13);
            this.lblStatus.Text = "Status:";

            this.txtStatus.Location = new System.Drawing.Point(140, 262);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(520, 140);

            this.ClientSize = new System.Drawing.Size(680, 420);
            this.Controls.Add(this.lblBaseUrl);
            this.Controls.Add(this.txtBaseUrl);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.btnOpenConnection);
            this.Controls.Add(this.btnGetVersion);
            this.Controls.Add(this.btnCloseConnection);
            this.Controls.Add(this.lblConnectionId);
            this.Controls.Add(this.txtConnectionId);
            this.Controls.Add(this.lblSqlVersion);
            this.Controls.Add(this.txtSqlVersion);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtStatus);
            this.Name = "MainForm";
            this.Text = "SQL Version Service Client";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}