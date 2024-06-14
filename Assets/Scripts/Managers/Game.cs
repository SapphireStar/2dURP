using Cysharp.Threading.Tasks;
using Isekai.UI.ViewModels.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Managers
{
    public class Game : MonoSingleton<Game>
    {
        async void Start()
        {
            await InitializeManagers();

            MainMenuViewModel viewmodel = new MainMenuViewModel();
            await ScreenManager.Instance.TransitionToInstant(UI.EScreenType.MainMenuScreen, ELayerType.DefaultLayer, viewmodel);

            SoundManager.Instance.PlayMusic(SoundDefine.Music_CottonPuzzle);
        }

        public async UniTask InitializeManagers()
        {
            await LayerManager.Instance.Initialize();
            ScreenManager.Instance.Initialize();
            ResourceManager.Instance.Initialize();
            await SettingsManager.Instance.Initialize();
            SoundManager.Instance.Initialize();
            LevelManager.Instance.Initialize();
        }
        private void Update()
        {

        }

    }

}
