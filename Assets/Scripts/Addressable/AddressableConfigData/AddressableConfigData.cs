using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Isekai.Util;
using System.IO;

namespace Isekai.Resource
{
    [CreateAssetMenu(fileName = "AddressableConfigData", menuName = "Data/Addressable/AddressableConfigData",order =3)]
    public class AddressableConfigData : ScriptableObject
    {
        public UnityEngine.Object[] Entries;
    }
}

