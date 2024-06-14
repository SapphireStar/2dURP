using Isekai.UI.ViewModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.Views.Screens
{
    public abstract class Screen<TViewModel> : View<TViewModel>, IScreen where TViewModel : IViewModel
    {
        [SerializeField]
        private bool m_canTraceBack;
        public bool CanTraceBack { get => m_canTraceBack; set=> m_canTraceBack = value; }

        public abstract void OnEnterScreen();

        public abstract void OnExitScreen();

        public virtual void Pop()
        {
            if (CanTraceBack)
            {
                gameObject.SetActive(false);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    }

}
