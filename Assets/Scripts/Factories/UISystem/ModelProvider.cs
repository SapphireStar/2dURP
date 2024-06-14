using Isekai.UI.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Isekai.Factories
{
    public class ModelProvider
    {
        private static Dictionary<Type, IModel> m_uniqueModels = new Dictionary<Type, IModel>();
        private static Dictionary<Type, Func<IModel>> m_uniqueModelFactoryFunctions = new Dictionary<Type, Func<IModel>>()
        {

        };
        public static T GetUniqueModel<T>() where T : IModel
        {
            Type type = typeof(T);
            if (!m_uniqueModels.ContainsKey(type))
            {
                m_uniqueModels[type] = CreateUniqueModel<T>();
            }
            return (T)m_uniqueModels[type];
        }

        public static T CreateUniqueModel<T>() where T : IModel
        {
            Type type = typeof(T);
            if (!m_uniqueModelFactoryFunctions.ContainsKey(type))
            {
                return default(T);
            }
            return (T)m_uniqueModelFactoryFunctions[type]();
        }

        public static void RegisterUniqueModelFactoryFunction<T>(Func<IModel> method) where T : IModel
        {
            Type type = typeof(T);
            m_uniqueModelFactoryFunctions[type] = method;
        }
    }

}
