using Cysharp.Threading.Tasks;
using Isekai.Factories;
using Isekai.UI;
using Isekai.UI.ViewModels;
using Isekai.UI.ViewModels.Screens;
using Isekai.UI.Views;
using Isekai.UI.Views.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Managers
{
    public class ScreenManager : MonoSingleton<ScreenManager>
    {
        public TransitionScreen CurTransitionScreen;

        private List<GameObject> m_screens = new List<GameObject>();
        private Stack<IScreen> m_prevScreens = new Stack<IScreen>();
        private IScreen m_curScreen;

        private ScreenProvider m_provider = new ScreenProvider();
        
        private void Start()
        {

        }
        private void Update()
        {

        }
        public void Initialize()
        {

        }
        public void BackToPrevScreen()
        {
            BackToPrevScreenAsync().Forget();
        }
        public async UniTaskVoid BackToPrevScreenAsync()
        {
            LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, true);

            await CurTransitionScreen.TransitionEnter();

            if (m_prevScreens.Count == 0)
            {
                Debug.LogError("There is no prev screens");
                return;
            }
            if (m_curScreen != null)
            {
                m_curScreen.OnExitScreen();
                m_curScreen.Pop();
            }
            m_curScreen = m_prevScreens.Pop();
            m_curScreen.Show();
            
            await CurTransitionScreen.TransitionOut();

            m_curScreen.OnEnterScreen();
            LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, false);
        }

        public async UniTask TransitionToInstant<TViewModel>(EScreenType screenType, ELayerType layer, TViewModel viewModel) where TViewModel:IViewModel
        {
            if (m_curScreen != null)
            {
                if (m_curScreen.CanTraceBack)
                {
                    m_prevScreens.Push(m_curScreen);
                }
                m_curScreen.OnExitScreen();
                m_curScreen.Pop();
                
            }

            var newScreen = await m_provider.GetScreen<TViewModel>(screenType, layer) as Screen<TViewModel>;
            newScreen.AssignViewModel(viewModel);
            newScreen.OnEnterScreen();
            m_screens.Add(newScreen.gameObject);

            m_curScreen = newScreen;
        }
        public async UniTask TransitionTo<TViewModel>(EScreenType screenType, ELayerType layer, TViewModel viewModel) where TViewModel : IViewModel
        {
            LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, true);

            await CurTransitionScreen.TransitionEnter();

            if (m_curScreen != null)
            {
                if (m_curScreen.CanTraceBack)
                {
                    m_prevScreens.Push(m_curScreen);
                }
                m_curScreen.OnExitScreen();
                m_curScreen.Pop();
            }

            var newScreen = await m_provider.GetScreen<TViewModel>(screenType, layer) as Screen<TViewModel>;
            newScreen.AssignViewModel(viewModel);

            await CurTransitionScreen.TransitionOut();

            newScreen.OnEnterScreen();

            m_curScreen = newScreen;

            m_screens.Add(newScreen.gameObject);

            LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, false);
        }

        //public async UniTask TransitionToInstant(EScreenType screenType, ELayerType layer) 
        //{
        //    if (m_curScreen != null)
        //    {
        //        if (m_curScreen.CanTraceBack)
        //        {
        //            m_prevScreens.Push(m_curScreen);
        //        }
        //        m_curScreen.OnExitScreen();
        //        m_curScreen.Pop();
        //    }


        //    var newScreen = await m_provider.GetScreen(screenType, layer) as IScreen;
        //    newScreen.OnEnterScreen();
        //    m_curScreen = newScreen;

        //}
        //public async UniTask TransitionTo(EScreenType screenType, ELayerType layer)
        //{
        //    LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, true);

        //    await CurTransitionScreen.TransitionEnter();

        //    if (m_curScreen != null)
        //    {
        //        if (m_curScreen.CanTraceBack)
        //        {
        //            m_prevScreens.Push(m_curScreen);
        //        }
        //        m_curScreen.OnExitScreen();
        //        m_curScreen.Pop();
        //    }

        //    var newScreen = await m_provider.GetScreen(screenType, layer) as IScreen;
        //    newScreen.OnEnterScreen();
                
        //    m_curScreen = newScreen;

        //    LayerManager.Instance.SetLayerInteractable(ELayerType.TransitionLayer, false);
        //}

        public void PopAllScreenInstant()
        {
            m_curScreen?.OnExitScreen();
            m_curScreen?.Pop();
            m_curScreen = null;

            while (m_prevScreens.Count > 0)
            {
                IScreen screen = m_prevScreens.Pop();
                screen.OnExitScreen();
                screen.Pop();
                
            }
            foreach (var item in m_screens)
            {
                if (!item.Equals(null))
                {
                    DestroyImmediate(item);
                }
            }
            m_screens.Clear();
        }


        public async UniTask<View<TViewModel>> GetScreen<TViewModel>(EScreenType screenType, ELayerType layer = ELayerType.DefaultLayer) where TViewModel : IViewModel
        {
            var screen = await m_provider.GetScreen(screenType, layer);
            return screen as View<TViewModel>;
        }
    }
}

