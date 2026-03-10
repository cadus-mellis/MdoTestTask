using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlVersionService.Desktop.Services;

namespace SqlVersionService.Desktop
{
    public partial class MainForm : Form
    {
        private Guid? _connectionId;

        public MainForm()
        {
            InitializeComponent();

            txtBaseUrl.Text = "http://localhost:5000/";
            txtConnectionString.Text = "Server=.;Database=master;Trusted_Connection=True;";
            UpdateUiState();
        }

        private SqlVersionServiceClient CreateClient()
        {
            return new SqlVersionServiceClient(txtBaseUrl.Text.Trim());
        }

        private void UpdateUiState()
        {
            txtConnectionId.Text = _connectionId?.ToString() ?? string.Empty;

            btnGetVersion.Enabled = _connectionId.HasValue;
            btnCloseConnection.Enabled = _connectionId.HasValue;
        }

        private void SetStatus(string message)
        {
            txtStatus.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private async void btnOpenConnection_Click(object sender, EventArgs e)
        {
            await ExecuteAsync(async () =>
            {
                var client = CreateClient();
                var response = await client.OpenConnectionAsync(txtConnectionString.Text);

                _connectionId = response.ConnectionId;
                txtSqlVersion.Text = string.Empty;

                UpdateUiState();
                SetStatus($"Connection opened. Id = {response.ConnectionId}");
            });
        }

        private async void btnGetVersion_Click(object sender, EventArgs e)
        {
            if (!_connectionId.HasValue)
            {
                SetStatus("No active connection.");
                return;
            }

            await ExecuteAsync(async () =>
            {
                var client = CreateClient();
                var response = await client.GetVersionAsync(_connectionId.Value);

                txtSqlVersion.Text = response.Version;
                SetStatus("SQL version received.");
            });
        }

        private async void btnCloseConnection_Click(object sender, EventArgs e)
        {
            if (!_connectionId.HasValue)
            {
                SetStatus("No active connection.");
                return;
            }

            await ExecuteAsync(async () =>
            {
                var client = CreateClient();
                var response = await client.CloseConnectionAsync(_connectionId.Value);

                SetStatus("Connection closed successfully.");

                _connectionId = null;
                txtSqlVersion.Text = string.Empty;

                UpdateUiState();
            });
        }

        private async Task ExecuteAsync(Func<Task> action)
        {
            try
            {
                ToggleBusy(true);
                await action();
            }
            catch (Exception ex)
            {
                SetStatus("Error: " + ex.Message);
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleBusy(false);
            }
        }

        private void ToggleBusy(bool isBusy)
        {
            btnOpenConnection.Enabled = !isBusy;
            btnGetVersion.Enabled = !isBusy && _connectionId.HasValue;
            btnCloseConnection.Enabled = !isBusy && _connectionId.HasValue;
            Cursor = isBusy ? Cursors.WaitCursor : Cursors.Default;
        }
    }
}