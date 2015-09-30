/*******************************************************
 * Copyright (C) 2015-2011 Marco Carettoni
 * 
 * This file is part of AutoCasterPro.
 * 
 ******************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using System.Reflection;

namespace com.marcocarettoni.AutoCasterPro
{
    public partial class Form1 : Form
    {

        Boolean stop = false;
        private ArrayList listaA;
        private int currentIndex = 0;
        private int timebetweencycle = 0;

        public Form1()
        {
            InitializeComponent();
            //checkVersionToolStripMenuItem_Click(null, null);
            LoadConfig();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start();
        }
        
        private void start()
        {
            listaA = new ArrayList();
            
            if (checkBox1.Checked)
                listaA.Add(new Spell(btnMana1.Text, btnButton1.Text, chkWaitFullMana1.Checked));

            if (checkBox2.Checked)
                listaA.Add(new Spell(btnMana2.Text, btnButton2.Text, chkWaitFullMana2.Checked));
            
            if (checkBox3.Checked)
                listaA.Add(new Spell(btnMana3.Text, btnButton3.Text, chkWaitFullMana3.Checked));

            if (checkBox4.Checked)
                listaA.Add(new Spell(btnMana4.Text, btnButton4.Text, chkWaitFullMana4.Checked));

            if (checkBox5.Checked)
                listaA.Add(new Spell(btnMana5.Text, btnButton5.Text, chkWaitFullMana5.Checked));

            int max_mana = Int32.Parse(btnManaMax.Text);
            decimal mana_sec = Decimal.Parse(btnManaSecond.Text);
            if (radioButton1.Checked)
            {
                int currentMana = max_mana;
                int targetMana = 0;
                for (int i = 0; i < listaA.Count; i++)
                {
                    Spell current = ((Spell)listaA[i]);

                    if (current.WAIT_FULL_MANA)
                    {
                        targetMana = max_mana - currentMana;
                    }
                    else
                    {
                        targetMana = current.MANA_USE - currentMana;
                    }

                    decimal timeoutMs = 0;
                    if (targetMana > 0)
                    {
                        timeoutMs = (targetMana * 1000) / (mana_sec);
                        currentMana -= current.MANA_USE - targetMana;
                    }
                    else
                    {
                        currentMana -= current.MANA_USE;
                    }
                    current.DELAY_MS = ((int)timeoutMs) + 10;
                }

                Spell starting = ((Spell)listaA[0]);
                timebetweencycle = ((int)(((max_mana - currentMana) * 1000) / mana_sec)) + 10;
            }

            if (radioButton2.Checked)
            {
                Control[] contr = null;
                for (int i = 1; i < listaA.Count; i++)
                {
                    contr = this.Controls.Find(String.Concat("txtDelay", i), true);
                    if (contr != null && contr.Length > 0 && contr[0] is TextBox)
                    {
                        ((Spell)listaA[i]).DELAY_MS = Int32.Parse(((TextBox)contr[0]).Text);
                       
                    }
                }

                timebetweencycle = 3000; // safe initializer

                contr = this.Controls.Find("txtDelay" + (listaA.Count), true);                
                if (contr != null && contr.Length > 0 && contr[0] is TextBox)
                {
                    timebetweencycle = Int32.Parse(((TextBox)contr[0]).Text);
                }
                ((Spell)listaA[0]).DELAY_MS = timebetweencycle;
            }

            currentIndex = 0;
            stop = false;
            button1.Enabled = false;
            button2.Enabled = true;
                
            Timer tmr1 = new Timer();
            tmr1.Interval = Int32.Parse(btnStartingDelay.Text);
            tmr1.Tick += sendTimerHandler;
            tmr1.Start();
        }


        private void sendTimer()
        {
            int delay = 0;
            
            if (currentIndex >= listaA.Count)
            {
                currentIndex = 0;
                delay = timebetweencycle;
            }
            else
            {
                delay = ((Spell)listaA[currentIndex]).DELAY_MS;
            }

            Timer tmr1 = new Timer();
            tmr1.Interval = (delay+1);
            tmr1.Tick += sendTimerHandler;
            tmr1.Start();           
        }

        private void sendTimerHandler(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            String btn = ((Spell)listaA[currentIndex]).BUTTON;
            SendKeys.Send(btn);

            if (!stop)
            {
                currentIndex++;
                sendTimer();
            }
            else
            {
                button1.Enabled = true;
            }
        }
         
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            stop = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            btnMana1.Enabled = checkBox1.Checked;
            btnButton1.Enabled = checkBox1.Checked;
            chkWaitFullMana1.Enabled = checkBox1.Checked;

            txtDelay1.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            btnMana2.Enabled = checkBox2.Checked;
            btnButton2.Enabled = checkBox2.Checked;
            chkWaitFullMana2.Enabled = checkBox2.Checked;

            txtDelay2.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            btnMana3.Enabled = checkBox3.Checked;
            btnButton3.Enabled = checkBox3.Checked;
            chkWaitFullMana3.Enabled = checkBox3.Checked;

            txtDelay3.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            btnMana4.Enabled = checkBox4.Checked;
            btnButton4.Enabled = checkBox4.Checked;
            chkWaitFullMana4.Enabled = checkBox4.Checked;

            txtDelay4.Enabled = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            btnMana5.Enabled = checkBox5.Checked;
            btnButton5.Enabled = checkBox5.Checked;
            chkWaitFullMana5.Enabled = checkBox5.Checked;

            txtDelay5.Enabled = checkBox5.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = radioButton1.Checked;
            panel2.Enabled = !radioButton1.Checked;
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This application is free and open source.\nIf you would like to support the development of this project please Donate!\n\nWould you like to donate?", "Donate", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://paypal.me/MarcoCarettoni");
            }
            
        }

        private void checkVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string urlAddress = "http://www.marcocarettoni.it/realmgrinder/version";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();
                    if (data != null && data.Length > 0)
                    {
                        if (data != Application.ProductVersion)
                        {
                            if (MessageBox.Show("Download latest version?", "New Version Available", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start("http://www.marcocarettoni.it/realmgrinder/AutoCasterPro.exe");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Update Available", "Check new Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception err) { }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AutoCaster PRO\nCurrent Version: " + Application.ProductVersion + "\n\nCopyright \u00a9 2015 Marco Carettoni\nhttp://www.marcocarettoni.it", "AutoCaster PRO");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        public void checkConfigExists()
        {
            string loc = String.Concat(Assembly.GetEntryAssembly().Location, ".config");
            if (!File.Exists(loc))
            {
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sb.AppendLine("<configuration>");
                sb.AppendLine("<appSettings>");

                sb.AppendLine("<add key=\"CheckForUpdateOnStartup\" value=\"0\" />");
                sb.AppendLine("<add key=\"startingDelay\" value=\"3000\" />");
                sb.AppendLine("<add key=\"ManaSec\" value=\"10\" />");
                sb.AppendLine("<add key=\"ManaMax\" value=\"10\" />");
                sb.AppendLine("<add key=\"AutomaticChk\" value=\"1\" />");
                sb.AppendLine("<add key=\"Skill1\" value=\"\" />");
                sb.AppendLine("<add key=\"Button1\" value=\"\" />");
                sb.AppendLine("<add key=\"Mana1\" value=\"\" />");
                sb.AppendLine("<add key=\"Wait1\" value=\"\" />");
                sb.AppendLine("<add key=\"Dalay1\" value=\"\" />");
                sb.AppendLine("<add key=\"Skill2\" value=\"\" />");
                sb.AppendLine("<add key=\"Button2\" value=\"\" />");
                sb.AppendLine("<add key=\"Mana2\" value=\"\" />");
                sb.AppendLine("<add key=\"Wait2\" value=\"\" />");
                sb.AppendLine("<add key=\"Dalay2\" value=\"\" />");
                sb.AppendLine("<add key=\"Skill3\" value=\"\" />");
                sb.AppendLine("<add key=\"Button3\" value=\"\" />");
                sb.AppendLine("<add key=\"Mana3\" value=\"\" />");
                sb.AppendLine("<add key=\"Wait3\" value=\"\" />");
                sb.AppendLine("<add key=\"Dalay3\" value=\"\" />");
                sb.AppendLine("<add key=\"Skill4\" value=\"\" />");
                sb.AppendLine("<add key=\"Button4\" value=\"\" />");
                sb.AppendLine("<add key=\"Mana4\" value=\"\" />");
                sb.AppendLine("<add key=\"Wait4\" value=\"\" />");
                sb.AppendLine("<add key=\"Dalay4\" value=\"\" />");
                sb.AppendLine("<add key=\"Skill5\" value=\"\" />");
                sb.AppendLine("<add key=\"Button5\" value=\"\" />");
                sb.AppendLine("<add key=\"Mana5\" value=\"\" />");
                sb.AppendLine("<add key=\"Wait5\" value=\"\" />");
                sb.AppendLine("<add key=\"Dalay5\" value=\"\" />");


                sb.AppendLine("</appSettings>");
                sb.AppendLine("</configuration>");

                File.WriteAllText(loc, sb.ToString());

                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public void LoadConfig()
        {
            checkConfigExists();


            chkCheckUpdate.Checked = (ConfigurationManager.AppSettings["CheckForUpdateOnStartup"] == "1");

            btnStartingDelay.Text = ConfigurationManager.AppSettings["startingDelay"];
            btnManaSecond.Text = ConfigurationManager.AppSettings["ManaSec"];
            btnManaMax.Text = ConfigurationManager.AppSettings["ManaMax"];

            radioButton1.Checked = (ConfigurationManager.AppSettings["AutomaticChk"] == "1");
            radioButton2.Checked = !radioButton1.Checked;

            checkBox1.Checked = (ConfigurationManager.AppSettings["Skill1"] == "1");
            btnButton1.Text = ConfigurationManager.AppSettings["Button1"];
            btnMana1.Text = ConfigurationManager.AppSettings["Mana1"];
            chkWaitFullMana1.Checked = (ConfigurationManager.AppSettings["Wait1"] == "1");
            txtDelay1.Text = ConfigurationManager.AppSettings["Dalay1"];

            checkBox2.Checked = (ConfigurationManager.AppSettings["Skill2"] == "1");
            btnButton2.Text = ConfigurationManager.AppSettings["Button2"];
            btnMana2.Text = ConfigurationManager.AppSettings["Mana2"];
            chkWaitFullMana2.Checked = (ConfigurationManager.AppSettings["Wait2"] == "1");
            txtDelay2.Text = ConfigurationManager.AppSettings["Dalay2"];

            checkBox3.Checked = (ConfigurationManager.AppSettings["Skill3"] == "1");
            btnButton3.Text = ConfigurationManager.AppSettings["Button3"];
            btnMana3.Text = ConfigurationManager.AppSettings["Mana3"];
            chkWaitFullMana3.Checked = (ConfigurationManager.AppSettings["Wait3"] == "1");
            txtDelay3.Text = ConfigurationManager.AppSettings["Dalay3"];

            checkBox4.Checked = (ConfigurationManager.AppSettings["Skill4"] == "1");
            btnButton4.Text = ConfigurationManager.AppSettings["Button4"];
            btnMana4.Text = ConfigurationManager.AppSettings["Mana4"];
            chkWaitFullMana4.Checked = (ConfigurationManager.AppSettings["Wait4"] == "1");
            txtDelay4.Text = ConfigurationManager.AppSettings["Dalay4"];

            checkBox5.Checked = (ConfigurationManager.AppSettings["Skill5"] == "1");
            btnButton5.Text = ConfigurationManager.AppSettings["Button5"];
            btnMana5.Text = ConfigurationManager.AppSettings["Mana5"];
            chkWaitFullMana5.Checked = (ConfigurationManager.AppSettings["Wait5"] == "1");
            txtDelay5.Text = ConfigurationManager.AppSettings["Dalay5"];

            checkBox1_CheckedChanged(null, null);
            checkBox2_CheckedChanged(null, null);
            checkBox3_CheckedChanged(null, null);
            checkBox4_CheckedChanged(null, null);
            checkBox5_CheckedChanged(null, null);
            radioButton1_CheckedChanged(null, null);

            if (chkCheckUpdate.Checked)
            {
                checkVersionToolStripMenuItem_Click(null, null);
            }
        }

        public void SaveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            
            config.AppSettings.Settings["CheckForUpdateOnStartup"].Value = (chkCheckUpdate.Checked ? "1" : "0");
            config.AppSettings.Settings["startingDelay"].Value = btnStartingDelay.Text;
            config.AppSettings.Settings["ManaSec"].Value = btnManaSecond.Text;
            config.AppSettings.Settings["ManaMax"].Value = btnManaMax.Text;
            config.AppSettings.Settings["AutomaticChk"].Value = (radioButton1.Checked ? "1" : "0");
            config.AppSettings.Settings["Skill1"].Value = (checkBox1.Checked ? "1" : "0");
            config.AppSettings.Settings["Button1"].Value = btnButton1.Text;
            config.AppSettings.Settings["Mana1"].Value = btnMana1.Text;
            config.AppSettings.Settings["Wait1"].Value = (chkWaitFullMana1.Checked ? "1" : "0");
            config.AppSettings.Settings["Dalay1"].Value = txtDelay1.Text;
            config.AppSettings.Settings["Skill2"].Value = (checkBox2.Checked ? "1" : "0");
            config.AppSettings.Settings["Button2"].Value = btnButton2.Text;
            config.AppSettings.Settings["Mana2"].Value = btnMana2.Text;
            config.AppSettings.Settings["Wait2"].Value = (chkWaitFullMana2.Checked ? "1" : "0");
            config.AppSettings.Settings["Dalay2"].Value = txtDelay2.Text;
            config.AppSettings.Settings["Skill3"].Value = (checkBox3.Checked ? "1" : "0");
            config.AppSettings.Settings["Button3"].Value = btnButton3.Text;
            config.AppSettings.Settings["Mana3"].Value = btnMana3.Text;
            config.AppSettings.Settings["Wait3"].Value = (chkWaitFullMana3.Checked ? "1" : "0");
            config.AppSettings.Settings["Dalay3"].Value = txtDelay3.Text;
            config.AppSettings.Settings["Skill4"].Value = (checkBox4.Checked ? "1" : "0");
            config.AppSettings.Settings["Button4"].Value = btnButton4.Text;
            config.AppSettings.Settings["Mana4"].Value = btnMana4.Text;
            config.AppSettings.Settings["Wait4"].Value = (chkWaitFullMana4.Checked ? "1" : "0");
            config.AppSettings.Settings["Dalay4"].Value = txtDelay4.Text;
            config.AppSettings.Settings["Skill5"].Value = (checkBox5.Checked ? "1" : "0");
            config.AppSettings.Settings["Button5"].Value = btnButton5.Text;
            config.AppSettings.Settings["Mana5"].Value = btnMana5.Text;
            config.AppSettings.Settings["Wait5"].Value = (chkWaitFullMana5.Checked ? "1" : "0");
            config.AppSettings.Settings["Dalay5"].Value = txtDelay5.Text;
            
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Save completed.");
        }
    }
}
