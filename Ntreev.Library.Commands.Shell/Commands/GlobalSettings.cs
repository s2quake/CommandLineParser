﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntreev.Library.Commands.Shell.Commands
{
    static class GlobalSettings
    {
        [CommandSwitch]
        public static string ID
        {
            get; set;
        }

        [CommandSwitch]
        public static string Password
        {
            get; set;
        }
    }
}