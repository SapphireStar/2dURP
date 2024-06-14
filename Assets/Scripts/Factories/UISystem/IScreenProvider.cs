using Cysharp.Threading.Tasks;
using Isekai.Managers;
using Isekai.UI;
using Isekai.UI.Views.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Factories
{
    public interface IScreenProvider
    {
        UniTask<UIElement> GetScreen(EScreenType screenType, ELayerType layer);
    }

}
