using Cysharp.Threading.Tasks;
using Isekai.Managers;
using Isekai.UI.ViewModels.Screens;
using Isekai.UI.Views.Widgets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Isekai.UI.Views.Screens
{
    public class LoadingScreen : Screen<LoadingViewModel>
    {
        [SerializeField]
        private ProgressBarWidget m_progressBar;
        [SerializeField]
        private Transform m_loadingAnim;
        [SerializeField]
        private TextMeshProUGUI m_loadingFinishNotify;

        private bool m_continue;
        public override void OnEnterScreen()
        {
            curValue = 0;
            Loading().Forget();
        }

        
        private void Update()
        {

        }
        float curValue = 0;
        async UniTaskVoid Loading()
        {
            while (curValue < 1)
            {
                //m_progressBar.SetFillValue(curValue);
                //curValue += Time.deltaTime * 0.4f;
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
            m_progressBar.gameObject.SetActive(false);
            m_loadingAnim.gameObject.SetActive(false);
            m_loadingFinishNotify.gameObject.SetActive(true);
            while (true)
            {
                if (Input.anyKeyDown)
                {
                    break;
                }
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
            ViewModel.LoadingComplete();
            
        }

        public override void HandleViewModelPropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.LoadingProgress):
                    curValue = (float)e.Value;
                    break;
                default:
                    break;
            }
        }

        public override void OnExitScreen()
        {
            Debug.Log("Exit Loading Screen");
        }

        public void OnAnyKeyDown()
        {

        }

    }
}

