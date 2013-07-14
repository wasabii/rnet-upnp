﻿using System.ComponentModel;

namespace Rnet
{

    /// <summary>
    /// Base class for RNET model objects.
    /// </summary>
    public abstract class RnetModelObject : INotifyPropertyChanged
    {

        /// <summary>
        /// Raised when the value of a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}