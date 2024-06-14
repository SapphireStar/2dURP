using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Util
{
    public class UtilClass
    {
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 ScreenPosition, Camera mainCamera)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(ScreenPosition);
            return worldPos;
        }

    }
}

