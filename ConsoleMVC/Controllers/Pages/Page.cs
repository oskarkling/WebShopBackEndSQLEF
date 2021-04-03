using System;
using System.Diagnostics;
using System.Collections;

namespace Pages
{
    internal class Page
    {
        internal PageType Type {get; set; }
        internal string Message {get; set; }

        internal int NrOfMenuOptions {get; set; }
    }
}