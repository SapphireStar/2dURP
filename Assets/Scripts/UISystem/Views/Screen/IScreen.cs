using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.Views.Screens
{
    public interface IScreen
    {
        bool CanTraceBack { get; set; }
        void Show();
        void OnEnterScreen();
        void OnExitScreen();

        void Pop();
    }

}
