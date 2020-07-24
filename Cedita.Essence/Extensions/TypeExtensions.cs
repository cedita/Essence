// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Reflection;

namespace Cedita.Essence.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get a Field from a Type (or, recursively its subtypes) of a given name.
        /// </summary>
        /// <param name="type">Type to work with.</param>
        /// <param name="fieldName">Name of Field.</param>
        /// <returns>Field found on type.</returns>
        public static FieldInfo GetField(this Type type, string fieldName)
        {
            if (type == null || string.IsNullOrEmpty(fieldName))
            {
                return default;
            }

            var currentType = type;
            do
            {
                var typeInfo = currentType.GetTypeInfo();
                var declaredField = typeInfo.GetDeclaredField(fieldName);
                if (declaredField != null)
                {
                    return declaredField;
                }

                currentType = typeInfo.BaseType;
            }
            while (currentType != null);

            return default;
        }

        /// <summary>
        /// Check if a Type is assignable from a generic type.
        /// </summary>
        /// <param name="type">Type to work with.</param>
        /// <param name="genericType">Generic type to attempt to match.</param>
        /// <returns>True if assignable, false otherwise.</returns>
        public static bool IsAssignableFromGenericType(this Type type, Type genericType)
        {
            if (type.IsAssignableFrom(genericType))
            {
                return true;
            }

            var interfaceTypes = type.GetInterfaces();
            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            return type.BaseType?.IsAssignableFromGenericType(genericType) ?? false;
        }
    }
}
