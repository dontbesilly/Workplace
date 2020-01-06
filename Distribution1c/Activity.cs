using System;
using System.Collections.Generic;
using System.Text;

namespace Workplace1c.Distribution1c
{
    class Activity
    {
        public string Name { get; set; }
        public object Parameters { get; set; }
    }

    class ActivityName
    {
        public const string UnloadBase = "UnloadBase";
        public const string DownloadBase = "DownloadBase";
    }
}
