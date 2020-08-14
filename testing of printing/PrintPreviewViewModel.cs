using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing_of_printing
{
    public class PrintPreviewViewModel : INotifyPropertyChanged
    {
        public PrintPreviewViewModel(PersonDetails personDetails)
        {
            _personDetails = personDetails;

            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(Age));
        }

        #region Properties

        public string Date => _personDetails.Date.ToString("dd/MM/yyyy");

        public string FirstName => _personDetails.FirstName;

        public string LastName => _personDetails.LastName;

        public int Age => _personDetails.Age;

        #endregion Properties


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged

        PersonDetails _personDetails;
    }
}
