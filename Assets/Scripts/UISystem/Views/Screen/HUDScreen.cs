using Isekai.UI.ViewModels.Screens;
using Isekai.UI.Views.Widgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.Views.Screens
{
    public class HUDScreen : Screen<HUDScreenViewModel>
    {
        [SerializeField]
        private ProgressBarWidget m_healthBar;
        [SerializeField]
        private ProgressBarWidget m_magicBar;

        private float curhealth;
        private float maxhealth;
        public override void OnEnterScreen()
        {
            
        }

        public override void OnExitScreen()
        {
            
        }
    }
}

