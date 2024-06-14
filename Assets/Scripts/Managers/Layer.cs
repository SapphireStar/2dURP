using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Isekai.Managers
{
    public class Layer : MonoBehaviour
    {
        public ELayerType LayerType;
        public Canvas CurCanvas;
        public CanvasGroup CurCanvasGroup;
        public CanvasScaler CurCanvasScaler;

        private void Start()
        {
            CurCanvas = GetComponent<Canvas>();
            CurCanvasGroup = GetComponent<CanvasGroup>();
            CurCanvasScaler = GetComponent<CanvasScaler>();
            if(LayerType == ELayerType.TransitionLayer)
            {
                CurCanvasGroup.interactable = false;
                CurCanvasGroup.blocksRaycasts = false;
            }
        }
    }

}
