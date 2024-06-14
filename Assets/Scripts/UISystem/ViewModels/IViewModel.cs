using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.ViewModels
{
    public interface IViewModel
    {
        event PropertyValueChangedEventHandler PropertyValueChanged;
    }
}

