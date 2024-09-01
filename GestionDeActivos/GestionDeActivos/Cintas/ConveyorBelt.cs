using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeActivos.Cintas
{
    class ConveyorBelt : INotifyPropertyChanged
    {

        private int idConveyorBelt;
        private string desConveyorBelt;
        private string certificte;
        private string technicalSheet;

        public ConveyorBelt() { }

        public ConveyorBelt(int idConveyorBelt, string desConveyorBelt, string certificte, string technicalSheet)
        {
            IdConveyorBelt = idConveyorBelt;
            DesConveyorBelt = desConveyorBelt;
            Certificte = certificte;
            TechnicalSheet = technicalSheet;
        }

        public int IdConveyorBelt
        {
            get => idConveyorBelt;
            set
            {
                if (idConveyorBelt != value)
                {
                    idConveyorBelt = value;
                    OnPropertyChanged(nameof(IdConveyorBelt));
                }
            }
        }

        public string DesConveyorBelt
        {
            get => desConveyorBelt;
            set
            {
                if (desConveyorBelt != value)
                {
                    desConveyorBelt = value;
                    OnPropertyChanged(nameof(DesConveyorBelt));
                }
            }
        }

        public string Certificte
        {
            get => certificte;
            set
            {
                if (certificte != value)
                {
                    certificte = value;
                    OnPropertyChanged(nameof(Certificte));
                }
            }
        }

        public string TechnicalSheet
        {
            get => technicalSheet;
            set
            {
                if (technicalSheet != value)
                {
                    technicalSheet = value;
                    OnPropertyChanged(nameof(TechnicalSheet));
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
