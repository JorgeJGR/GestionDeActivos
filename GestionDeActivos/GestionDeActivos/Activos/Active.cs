﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Activos
{
    class Active : INotifyPropertyChanged
    {
        public enum Lines { Línea_01, Línea_02}

        public enum Zones { Obrador, EntreTúneles, Envasado, Anillo_TF}

        private int idActive;
        private string description;
        private string line;
        private string zone;
        private string image;

        public Active(int idActive, string description, string line, string zone, string image)
        {
           
            if (!Enum.IsDefined(typeof(Lines), line))
            {
                throw new ActiveException($"La línea '{line}' no es válida. Debe ser 'Línea_01' o 'Línea_02'.");
            }

            if (!Enum.IsDefined(typeof(Zones), zone))
            {
                throw new ActiveException($"La zona '{zone}' no es válida. Debe ser 'Obrador', 'EntreTúneles', 'Envasado' o 'Anillo_TF'.");
            }

            IdActive = idActive;
            Description = description;
            Line = line;
            Zone = zone;
            Image = image;
        }

        public Active() { }

        public int IdActive
        {
            get => idActive;
            set
            {
                if (idActive != value)
                {
                    idActive = value;
                    OnPropertyChanged(nameof(IdActive));
                }
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Line
        {
            get => line;
            set
            {
                if (line != value)
                {
                    line = value;
                    OnPropertyChanged(nameof(Line));
                }
            }
        }

        public string Zone
        {
            get => zone;
            set
            {
                if (zone != value)
                {
                    zone = value;
                    OnPropertyChanged(nameof(Zone));
                }
            }
        }

        public string Image
        {
            get => image;
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
