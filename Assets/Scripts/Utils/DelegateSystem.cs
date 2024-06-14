using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateSystem
{

}
public class PropertyValueChangedEventArgs : EventArgs
{
    public string PropertyName;
    public object Value;

    public PropertyValueChangedEventArgs(string propertyName, object value)
    {
        PropertyName = propertyName;
        Value = value;
    }
}

public delegate void PropertyValueChangedEventHandler(object sender, PropertyValueChangedEventArgs e);