using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Isekai.UI.Views.Widgets
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField]
        private Image m_fillArea;
        [SerializeField]
        private Image m_background;

        private float m_maxValue = 1;
        public void Initialize(float initialValue = 0)
        {
            m_fillArea.fillAmount = initialValue;
        }
        

        public void SetValue(float value)
        {
            m_fillArea.fillAmount = value / m_maxValue;
        }
        public void SetMaxValue(float value)
        {
            m_maxValue = value;
        }
        public void SetBackgroundColor(Color color)
        {
            m_background.color = color;
        }
        public void SetFillAreaColor(Color color)
        {
            m_background.color = color;
        }
    }
}

