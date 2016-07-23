﻿using clipr.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace clipr.Utils
{
    public static class EnumUtils
    {
        public static bool IsEnum(IValueStoreDefinition store)
        {
            return IsClrEnum(store) || IsStaticEnum(store);
        }

        private static bool IsClrEnum(IValueStoreDefinition store)
        {
            return store.Type.IsSubclassOf(typeof(Enum));
        }

        private static bool IsStaticEnum(IValueStoreDefinition store)
        {
            return (store.GetCustomAttribute<StaticEnumerationAttribute>() ??
                    store.Type.GetCustomAttribute<StaticEnumerationAttribute>()) != null;
        }

        public static string[] GetEnumValues(IValueStoreDefinition store)
        {
            if (IsClrEnum(store))
            {
                return Enum.GetNames(store.Type);
            }
            else if (IsStaticEnum(store))
            {
                return store.Type.GetFields(
                    BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsInitOnly &&
                            store.Type.IsAssignableFrom(f.FieldType))
                .Select(f => f.Name)
                .ToArray();
            }
            return new string[0];
        }
    }
}
