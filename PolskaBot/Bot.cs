﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using PolskaBot.Core;
using PolskaBot.Core.Darkorbit;
using PolskaBot.Core.Darkorbit.Commands.PostHandshake;
using Glide;
using MiscUtil.IO;

namespace PolskaBot
{
    
    public partial class Bot : Form
    {
        public int AccountsCount { get; set; }

        private List<BotPage> pages = new List<BotPage>();

        public Bot()
        {
            InitializeComponent();
            Load += (s, e) => Init();
        }

        private void Init()
        {
            startButton.Enabled = false;
            stopButton.Enabled = false;

            AcceptButton = loginButton;
            loginButton.Click += (s, e) =>
            {
                if(!usernameBox.Text.Equals("") || !passwordBox.Text.Equals(""))
                {
                    var botPage = new BotPage(usernameBox.Text, passwordBox.Text);
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
                } else
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
