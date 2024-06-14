using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using UnityEditor.AddressableAssets.Settings;
using Isekai.Resource;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace Isekai.Util
{
    public class UpdateAddressableGroups : Editor
    {
        static AddressableAssetSettings settings;
        const string DEFAULT_GROUPNAME = "DefaultGroup";
        [MenuItem("Addressable/UpdateAddressableGroups", priority = 1)]
        public static void UpdateGroups()
        {
            if(settings == null)
            {
                settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(AddressableAssetSettingsDefaultObject.DefaultAssetPath);
            }
            settings.groups.Clear();
            var asset = (AddressableConfigData)AssetDatabase.LoadMainAssetAtPath("Assets/AddressableAssets/AddressableFolderConfig/AddressableConfigData.asset");
            
            foreach (var item in asset.Entries)
            {
                string path = AssetDatabase.GetAssetPath(item);
                if (AssetDatabase.IsValidFolder(path))
                {
                    AddressableAssetGroup group = settings.FindGroup(item.name);
                    if (!group)
                    {
                        group = settings.CreateGroup(item.name, false, false, false, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
                    }
                    AddFolderAssetsToAddressable(path, item.name, group);
                    EditorUtility.SetDirty(group);
                    EditorUtility.SetDirty(settings);
                }
                else
                {
                    AddressableAssetGroup group = settings.FindGroup(DEFAULT_GROUPNAME);
                    if (!group)
                    {
                        group = settings.CreateGroup(DEFAULT_GROUPNAME, false, false, false, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
                    }

                    AddSingleAssetToAddressable(item, DEFAULT_GROUPNAME, group);
                }
            }
        }

        static void AddFolderAssetsToAddressable(string path,string groupName, AddressableAssetGroup group)
        {
            List<string> paths = GetAllAssetsInFolder(path);

            foreach (var item in paths)
            {
                var asset = AssetDatabase.LoadMainAssetAtPath(item);
                string assetPath = AssetDatabase.GetAssetPath(asset);
                string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
                var assetEntry = settings.CreateOrMoveEntry(assetGUID, group, false, false);
                assetEntry.address = string.Format(asset.name);
            }
            
        }

        static void AddSingleAssetToAddressable(UnityEngine.Object asset, string groupName, AddressableAssetGroup group)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
            var assetEntry = settings.CreateOrMoveEntry(assetGUID, group, false, false);
            assetEntry.address = string.Format(asset.name);
        }

        public static List<string> GetAllAssetsInFolder(string path)
        {
            var guids = AssetDatabase.FindAssets("t:prefab,t:sprite,t:scriptableobject,t:audioclip", new string[] { path });

            List<string> result = new List<string>();
            foreach (var item in guids)
            {
                result.Add(AssetDatabase.GUIDToAssetPath(item));
            }
            return result;
        }

        #region Loop Through Folders

        [MenuItem("Addressable/GetAllFolders", priority = 2)]
        public static void GetAllFolders()
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("Assets/AddressableAssets");
            GetAllFoldersRecursively(queue, 0);
        }

        public static void GetAllFoldersRecursively(Queue<string> queue, int layer)
        {
            if (queue.Count <= 0)
            {
                return;
            }
            string curFolder = queue.Dequeue();
            GetAllAssetsInFolder(curFolder);

            //Debug.LogFormat("{0}{1}:", GetSpace(layer), curFolder);
            var folders = AssetDatabase.GetSubFolders(curFolder);
            layer++;
            foreach (var folder in folders)
            {
                queue.Enqueue(folder);
                GetAllFoldersRecursively(queue, layer);
            }
        }
        public static string GetSpace(int num)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append('-');
            }
            return sb.ToString();
        }

        #endregion
    }

}
