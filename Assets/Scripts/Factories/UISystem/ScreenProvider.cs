using Cysharp.Threading.Tasks;
using Isekai.Managers;
using Isekai.UI;
using Isekai.UI.ViewModels;
using Isekai.UI.Views;
using Isekai.UI.Views.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Isekai.Factories
{
    public class ScreenProvider :  IScreenProvider
    {
        public ScreenProvider()
        {

        }
        public void Initialize()
        {

        }
        public async UniTask<UIElement> GetScreen(EScreenType screenType, ELayerType layer)
        {
            var prefab = await ResourceManager.Instance.LoadResourceAsync<GameObject>(screenType.ToString());
            GameObject screen = ResourceManager.Instance.InstantiateGO(prefab, LayerManager.Instance.GetLayer(layer));

            return screen.GetComponent<UIElement>();
        }
        public async UniTask<Screen<TViewModel>> GetScreen<TViewModel>(EScreenType screenType, ELayerType layer) where TViewModel:IViewModel
        {
            var prefab = await ResourceManager.Instance.LoadResourceAsync<GameObject>(screenType.ToString());
            GameObject screen = ResourceManager.Instance.InstantiateGO(prefab, LayerManager.Instance.GetLayer(layer));

            return screen.GetComponent<Screen<TViewModel>>();
        }
    }

}
