using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MultiPartFormData.Infrastructure.Extensions;
using MultiPartFormData.Models;

namespace MultiPartFormData.Analyzers
{
    public class MultipartFormFileAnalyzer
    {
        #region Properties

        /// <summary>
        ///     This instance stores input form data.
        /// </summary>
        private readonly MultipartFormData _formData;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initialize an instance with form data as input.
        /// </summary>
        /// <param name="sourceData"></param>
        public MultipartFormFileAnalyzer(MultipartFormData sourceData)
        {
            _formData = sourceData;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Convert an instance of type to an object.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Convert(Type type)
        {
            if (type == null)
                return null;

            if (type == typeof(MultipartFormData))
                return _formData;

            return InitializeInstanceFromType(type);
        }

        /// <summary>
        ///     Find the value with type and return the object created from buffer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private object InitializeInstanceFromType(Type type, string propertyName = "")
        {
            object buffer;
            if (AnalyzeFormData(type, propertyName, out buffer))
                return buffer;

            if (AnalyzeGenericDictionary(type, propertyName, out buffer))
                return buffer;

            if (AnalyzeGenericListOrArray(type, propertyName, out buffer))
                return buffer;

            if (AnalyzeCustomType(type, propertyName, out buffer))
                return buffer;

            return null;
        }

        /// <summary>
        ///     Analyze key-value from FormData instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private bool AnalyzeFormData(Type type, string propertyName, out object propertyValue)
        {
            propertyValue = null;

            // The type is HttpFile.
            if (type == typeof(HttpFile))
            {
                HttpFile httpFile;
                if (!_formData.TryGetValue(propertyName, out httpFile))
                    return false;

                // The result parameter will be the HttpFile instance.
                propertyValue = httpFile;

                return true;
            }

            // The property is not an instance of file.
            string value;
            if (!_formData.TryGetValue(propertyName, out value))
                return false;

            var typeConverter = type.GetFromStringConverter();
            if (typeConverter == null)
                return false;

            propertyValue = typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, value);
            return true;
        }

        /// <summary>
        ///     Analyze parameters as they're in a dictionary.
        /// </summary>
        /// <param name="destinitionType"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private bool AnalyzeGenericDictionary(Type destinitionType, string propertyName, out object propertyValue)
        {
            // By default, property value is null.
            propertyValue = null;

            // Initialize key-value types.
            Type keyType, valueType;

            // Check whether the destination type is a generic dictionary or not.
            var isGenericDictionary = IsGenericDictionary(destinitionType, out keyType, out valueType);

            // Instance is not a generic dictionary.
            if (!isGenericDictionary) return false;

            // Create a generic dictionary from key-value type.
            var dictionary = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);

            // Find the add method from dictionary for future use.
            var dictionaryMethodAdd = dictionary.GetMethod("Add");

            // Create a key-value pair from a dictionary.
            var keyValuePair = Activator.CreateInstance(dictionary);

            // By default, index of property name starts from 0.
            var index = 0;

            // Original property name before being changed.
            var originalPropertyName = propertyName;

            // Whether dictionary has been filled or not.
            var isFilled = false;

            while (true)
            {
                var key = $"{originalPropertyName}[{index}].Key";

                // Value which is retrieved from key.
                var propertyKey = InitializeInstanceFromType(keyType, key);

                // No key has been retrieved.
                if (propertyKey == null)
                    break;

                var propertyValueName = $"{originalPropertyName}[{index}].Value";
                var objValue = InitializeInstanceFromType(valueType, propertyValueName);

                if (objValue != null)
                {
                    dictionaryMethodAdd.Invoke(keyValuePair, new[] {propertyKey, objValue});
                    isFilled = true;
                }

                index++;
            }

            if (isFilled)
                propertyValue = keyValuePair;

            return true;
        }

        /// <summary>
        ///     Analyze an input instance to as it is an instance of generic list or array.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private bool AnalyzeGenericListOrArray(Type type, string propertyName, out object propertyValue)
        {
            // By default, property value is always null.
            propertyValue = null;

            // Type of generic item.
            Type genericListItemType;

            // Check whether input instance is a generic list or not.
            var isGenericList = IsGenericListOrArray(type, out genericListItemType);

            // Instance is not a generic list.
            if (!isGenericList) return false;

            // Create a generic list from input instance.
            var listType = typeof(List<>).MakeGenericType(genericListItemType);

            // Find the add method for future invocation.
            var methodAdd = listType.GetMethod("Add");

            // Create an instance from type of list.
            var pValue = Activator.CreateInstance(listType);

            // By default, index of elements start from 0.
            var index = 0;

            // Conserve the property name, because all parsed data will be added into a list with 1 property name.
            var originalPropertyName = propertyName;

            // Whether list has been filled or not.
            var isFilled = false;

            while (true)
            {
                // Property name with index construction.
                propertyName = $"{originalPropertyName}[{index}]";

                // Initialize an instance from generic item using index.
                var initializedValue = InitializeInstanceFromType(genericListItemType, propertyName);

                // Invalid analyzed value.
                if (initializedValue == null)
                    break;

                // Add the analyzed instance to the list.
                methodAdd.Invoke(pValue, new[] {initializedValue});
                isFilled = true;

                index++;
            }

            // List hasn't been filled with any element.
            if (!isFilled) return true;

            // Input instance is an array.
            if (type.IsArray)
            {
                var toArrayMethod = listType.GetMethod("ToArray");
                propertyValue = toArrayMethod.Invoke(pValue, new object[0]);
            }
            else
                propertyValue = pValue;

            return true;
        }

        /// <summary>
        ///     Analyze the input instance as a custom type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private bool AnalyzeCustomType(Type type, string propertyName, out object propertyValue)
        {
            // By default, property value is null.
            propertyValue = null;

            // Check whether property is not a list.
            var isCustomNonEnumerableType = type.IsCustomNonEnumerableType();

            // Property is a list.
            if (!isCustomNonEnumerableType) return false;

            // This property name is unique.
            if (!string.IsNullOrWhiteSpace(propertyName) && !_formData.AllKeys()
                    .Any(m => m.StartsWith(propertyName + ".", StringComparison.CurrentCultureIgnoreCase)))
                return true;

            // Initialize an instance from source type.
            var instance = Activator.CreateInstance(type);

            // This property is used for checking whether property has been filled or not.
            var isFilled = false;

            foreach (var propertyInfo in type.GetPublicAccessibleProperties())
            {
                // Retrieve key-value from type.
                var analyzedPropertyName = (!string.IsNullOrEmpty(propertyName) ? propertyName + "." : "") +
                                           propertyInfo.Name;
                var analyzedValue = InitializeInstanceFromType(propertyInfo.PropertyType, analyzedPropertyName);

                // Invalid analyzed value.
                if (analyzedValue == null)
                    continue;

                propertyInfo.SetValue(instance, analyzedValue);
                isFilled = true;
            }

            // Property is valid. Set the output.
            if (isFilled)
                propertyValue = instance;

            return true;
        }

        /// <summary>
        ///     Analyze type from input type to check whether it is a generic dictionary or not.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        private bool IsGenericDictionary(Type type, out Type keyType, out Type valueType)
        {
            // From type to dictionary.
            var dictionary = type.GetInterface(typeof(IDictionary<,>).Name);

            // Get list of arguments.
            var arguments = dictionary?.GetGenericArguments();

            // Analyzed arguments number is 2. This is the dictionary.
            if (arguments?.Length == 2)
            {
                keyType = arguments[0];
                valueType = arguments[1];
                return true;
            }

            keyType = null;
            valueType = null;
            return false;
        }

        /// <summary>
        ///     Analyze a type to check whether it is a generic list or array.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private bool IsGenericListOrArray(Type type, out Type itemType)
        {
            if (type.GetInterface(typeof(IDictionary<,>).Name) == null) //not a dictionary
            {
                if (type.IsArray)
                {
                    itemType = type.GetElementType();
                    return true;
                }

                var iListType = type.GetInterface(typeof(ICollection<>).Name);
                var genericArguments = iListType?.GetGenericArguments();
                if (genericArguments?.Length == 1)
                {
                    itemType = genericArguments[0];
                    return true;
                }
            }

            itemType = null;
            return false;
        }

        #endregion
    }
}