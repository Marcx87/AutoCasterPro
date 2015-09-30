/*******************************************************
 * Copyright (C) 2015-2011 Marco Carettoni
 * 
 * This file is part of AutoCasterPro.
 * 
 ******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace com.marcocarettoni.AutoCasterPro
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
