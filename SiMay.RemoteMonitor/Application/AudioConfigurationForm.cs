using SiMay.Core;
using SiMay.RemoteControls.Core;
using SiMay.RemoteMonitor.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiMay.RemoteMonitor.Application
{
    public partial class AudioConfigurationForm : Form
    {
        public AudioConfigurationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioSamplesPerSecond = short.Parse(nSamplesPerSec.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioBitsPerSample = short.Parse(wBitsPerSample.Text);
            AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioChannels = short.Parse(nChannels.Text);
            MessageBox.Show("设置保存成功,但设置需重新打开语音功能模块后才能生效。", "提示", 0, MessageBoxIcon.Exclamation);
            this.Close();
        }

        private void AudioConfigurationManager_Load(object sender, EventArgs e)
        {
            nSamplesPerSec.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioSamplesPerSecond.ToString();
            wBitsPerSample.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioBitsPerSample.ToString();
            nChannels.Text = AppConfiguration.GetApplicationConfiguration<SystemAppConfig>().AudioChannels.ToString();
        }
    }
}
