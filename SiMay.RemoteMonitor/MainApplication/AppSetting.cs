using SiMay.Basic;
using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Entitys;
using SiMay.RemoteMonitor.Extensions;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.MainApplication
{

    public partial class AppSetting : Form
    {

        public AppSetting()
        {
            InitializeComponent();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (ip.Text == "" || port.Text == "" || connectLimitCount.Text == "" || conPwd.Text == "")
            {
                MessageBoxHelper.ShowBoxExclamation("请正确完整填写设置,否则可能导致客户端上线失败!");
                return;
            }

            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().IPAddress = ip.Text;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().Port = int.Parse(port.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MaxConnectCount = int.Parse(connectLimitCount.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ValidatePassWord = conPwd.Text;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().DbClickViewExc = (funComboBox.Items[funComboBox.SelectedIndex] as KeyValueItem).Value;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().WindowMaximize = maximizeCheckBox.Checked;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().UnLockeCredential = pwdTextBox.Text;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AccessKey = long.Parse(accessKey.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().SessionMode = sessionModeList.SelectedIndex;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MiddlerProxyIPAddress = txtservice_address.Text;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MiddlerProxyPort = int.Parse(txtservice_port.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AccessId = long.Parse(txtAccessId.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().EnabledAnonyMous = enableAnonymous.Checked;
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().Flush();

            DialogResult result = MessageBox.Show("设置完成，是否重启生效设置?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            if (result == DialogResult.OK)
            {
                System.Windows.Forms.Application.Restart();
            }
        }

        private void SetForm_Load(object sender, EventArgs e)
        {
            SysUtil.ApplicationTypes.ForEach(c =>
            {
                var type = c.ApplicationType;
                funComboBox.Items.Add(new KeyValueItem()
                {
                    Key = type.GetApplicationName(),
                    Value = c.ApplicationName
                });
                if (c.ApplicationName == AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().DbClickViewExc)
                    funComboBox.Text = type.GetApplicationName();
            });

            if (funComboBox.SelectedIndex == -1)
                funComboBox.Text = SysUtil.ApplicationTypes.First().ApplicationType.GetApplicationName();

            ip.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().IPAddress;
            conPwd.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().ValidatePassWord;
            port.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().Port.ToString();
            connectLimitCount.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MaxConnectCount.ToString();
            pwdTextBox.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().UnLockeCredential;
            accessKey.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AccessKey.ToString();
            txtservice_address.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MiddlerProxyIPAddress;
            txtservice_port.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().MiddlerProxyPort.ToString();
            txtAccessId.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AccessId.ToString();

            int index = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().SessionMode;
            sessionModeList.SelectedIndex = index;

            maximizeCheckBox.Checked = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().WindowMaximize;

            enableAnonymous.Checked = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().EnabledAnonyMous;
            txtAccessId.Enabled = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().EnabledAnonyMous;

            txtAccessId.Enabled = enableAnonymous.Checked ? false : true;
        }

        private void conPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void enableAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            txtAccessId.Enabled = enableAnonymous.Checked ? false : true;
        }
    }
}