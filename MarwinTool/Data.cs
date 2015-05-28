using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarwinTool
{
    public class Data : INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyChanged)
        {
            if (propertyChanged != null)
            {
                //PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
            }
        }
        #endregion

        #region Properties
        private DateTime _LastOnlineTime;
        public DateTime LastOnlineTime
        {
            get { return _LastOnlineTime; }
            set
            {
                _LastOnlineTime = value;
                OnPropertyChanged("LastOnlineTime");
            }
        }
        #endregion

        #region Static
        public static string GetVersion()
        {
            return "0.1";
        }
        #endregion
    }
}
