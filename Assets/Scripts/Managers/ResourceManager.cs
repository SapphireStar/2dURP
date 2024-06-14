using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Isekai.Managers
{

    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public void Initialize()
        {

        }

        public async UniTask<T> LoadResourceAsync<T>(string name) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(name);
            await handle.Task;
            return handle.Result;
        }

        public GameObject InstantiateGO(GameObject gameobject, Transform parent)
        {
            return Instantiate(gameobject, parent);
        }
        public GameObject InstantiateGO(GameObject gameObject, Transform parent, Vector3 position, Quaternion quaternion)
        {
            return Instantiate(gameObject,position,quaternion,parent);
        }

    }
}

