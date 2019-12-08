﻿using System;
using System.Collections.Generic;

namespace AuthExample.App.Model
{
    public partial class Klasse
    {
        public Klasse()
        {
            Schueler = new HashSet<Schueler>();
        }

        public string K_Name { get; set; }
        public string K_Abteilung { get; set; }

        public virtual ICollection<Schueler> Schueler { get; set; }
    }
}
