using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectStatsComponent : MonoBehaviour
{
    private bool isInTurn;
    public bool IsInTurn
    {
        get => isInTurn;
        set
        {
            ChangePropertyAndNotify<bool>(ref isInTurn, value);
        }
    }


    #region Event_Part
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

    protected virtual void RaisePropertyChanged(string propertyName, object value = null)
    {

        PropertyValueChanged?.Invoke(this, new PropertyValueChangedEventArgs(propertyName, value));
    }
    #endregion
}
