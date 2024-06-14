using Isekai.UI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Isekai.UI.ViewModels
{
    public class ViewModel:IViewModel
    {
        public ViewModel()
        {

        }

        public event PropertyValueChangedEventHandler PropertyValueChanged;
        protected bool ChangePropertyAndNotify<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (newValue == null && currentValue == null)
            {
                return false;
            }

            if (newValue != null && newValue.Equals(currentValue))
            {
                return false;
            }

            currentValue = newValue;

            RaisePropertyChanged(propertyName, newValue);

            return true;
        }
        protected bool ChangePropertyAndNotify<T>(T currentValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (newValue == null && currentValue == null)
            {
                return false;
            }

            if (newValue != null && newValue.Equals(currentValue))
            {
                return false;
            }

            currentValue = newValue;

            RaisePropertyChanged(propertyName, newValue);

            return true;
        }

        protected virtual void RaisePropertyChanged(string propertyName, object value = null)
        {
            PropertyValueChanged?.Invoke(this, new PropertyValueChangedEventArgs(propertyName, value));
        }
    }

}
