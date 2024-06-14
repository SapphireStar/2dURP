using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.Views.Widgets
{
    public class LoadingIconWidget : MonoBehaviour
    {
        [SerializeField]
        private float m_rotateSpeed = 200;
        void Update()
        {
            transform.Rotate(new Vector3(0, 0, 1), -m_rotateSpeed * Time.deltaTime);
        }
    }
}