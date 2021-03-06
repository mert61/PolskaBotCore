﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PolskaBot.Fade;
using System.Net;
using System.Deployment.Application;

namespace PolskaBot
{

    public partial class Bot : Form
    {
        private string _ip;

        public int AccountsCount { get; set; }

        private List<BotPage> pages = new List<BotPage>();

        private FadeProxy proxy;

        public Bot()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Load += (s, e) => Init();
            FormClosed += (s, e) => {
                foreach (BotPage page in pages)
                {
                    page.Stop();
                }
            };
        }

        private void Init()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Text = $"PolskaBot {ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4)}";
            }

            string swfPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Fade.swf";

            Task serverTask = new Task(() =>
            {
                using (var client = new WebClient())
                {
                    flashEmbed.LoadMovie(0, swfPath);
                }
            });
            serverTask.Start();

            proxy = new FadeProxy(flashEmbed);
            proxy.Ready += (s, e) => loginButton.Enabled = true;

            AcceptButton = loginButton;
            loginButton.Click += (s, e) =>
            {
                if (!usernameBox.Text.Equals("") || !passwordBox.Text.Equals(""))
                {
                    var botPage = new BotPage(proxy.CreateClient(), usernameBox.Text, passwordBox.Text);
                    pages.Add(botPage);
                    botTabs.Controls.Add(botPage);
                    botTabs.SelectedIndex = ++AccountsCount;
                    usernameBox.Text = "";
                    passwordBox.Text = "";
                }
            };

            botTabs.SelectedIndexChanged += (s, e) =>
            {
                if (botTabs.SelectedIndex == 0)
                {
                    startButton.Enabled = false;
                    stopButton.Enabled = false;
                    closeButton.Enabled = false;
                    settingsButton.Enabled = false;
                }
                else
                {
                    startButton.Enabled = !pages[botTabs.SelectedIndex - 1].Running;
                    stopButton.Enabled = pages[botTabs.SelectedIndex - 1].Running;
                    closeButton.Enabled = true;
                    settingsButton.Enabled = true;
                }
            };

            startButton.Click += (s, e) =>
            {
                pages[botTabs.SelectedIndex - 1].Running = true;
                startButton.Enabled = !pages[botTabs.SelectedIndex - 1].Running;
                stopButton.Enabled = pages[botTabs.SelectedIndex - 1].Running;
            };

            stopButton.Click += (s, e) =>
            {
                pages[botTabs.SelectedIndex - 1].Running = false;
                startButton.Enabled = !pages[botTabs.SelectedIndex - 1].Running;
                stopButton.Enabled = pages[botTabs.SelectedIndex - 1].Running;
            };

            closeButton.Click += (s, e) =>
            {
                pages[botTabs.SelectedIndex - 1].Stop();
                pages.RemoveAt(botTabs.SelectedIndex - 1);
                botTabs.Controls.RemoveAt(botTabs.SelectedIndex);
                AccountsCount--;
            };

            settingsButton.Click += (s, e) =>
            {
                SettingsForm settingsForm = new SettingsForm(pages[botTabs.SelectedIndex - 1]);
                settingsForm.Show();
            };
        }
    }
}