﻿using System;
using System.Windows.Forms;
using System.Threading;

using Server.Models;
using Server.Helpers;
using Server.Forms.Fragments;

namespace Server.Forms
{
    public partial class VisaulizationForm : Form
    {
        private CustomList<Testing> testings;
        private CustomList<Testing> Testings
        {
            set
            {
                testings = value;
                try { 
                    flowLayoutPanel1.BeginInvoke((MethodInvoker)(() =>
                    {
                        flowLayoutPanel1.Controls.Clear();
                    }));
                    foreach (var item in testings)
                    {
                        ClientFragment clientFragment = new ClientFragment(item);
                        flowLayoutPanel1.BeginInvoke((MethodInvoker)(() =>
                        {
                            flowLayoutPanel1.Controls.Add(clientFragment);
                        }));
                    }

                    label1.BeginInvoke((MethodInvoker)(() => {
                        label1.Text = $"Подключенные ПК ({testings.Count} шт.):";
                    }));
                }
                catch (InvalidOperationException)
                {
                    // TODO: не оставлять пустым
                }
            }
            get => testings;
        }

        public VisaulizationForm()
        {
            InitializeComponent();
        }

        private void VisaulizationForm_Load(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    Testings = ClientHandler.testings;
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
