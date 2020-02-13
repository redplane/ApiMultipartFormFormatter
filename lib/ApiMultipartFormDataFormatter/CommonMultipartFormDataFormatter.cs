using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Implementations;
using ApiMultiPartFormData.Services.Interfaces;

#if NETFRAMEWORK
using System.Net.Http.Formatting;
#elif NETCOREAPP
using Microsoft.AspNetCore.Mvc.Formatters;
#endif

namespace ApiMultiPartFormData
{
#if NETFRAMEWORK
    public partial class MultipartFormDataFormatter : MediaTypeFormatter
#else
    public partial class MultipartFormDataFormatter : IInputFormatter
#endif
    {
        #region Properties

        private readonly IModelBinderService[] _modelBinderServices;

        private const string SupportedMediaType = "multipart/form-data";

        /// <summary>
        ///     Interceptor for handling content disposition content name.
        /// </summary>
        public Func<string, List<string>> FindContentDispositionParametersInterceptor { get; set; }

        #endregion

        #region Constructor

        public MultipartFormDataFormatter(IEnumerable<IModelBinderService> modelBinderServices = null)
        {
            _modelBinderServices = modelBinderServices?.ToArray();

            if (_modelBinderServices == null || _modelBinderServices.Length < 1)
            {
                _modelBinderServices = new IModelBinderService[]
                {
                    new DefaultMultiPartFormDataModelBinderService(),
                    new GuidModelBinderService(),
                    new EnumModelBinderService(),
                    new HttpFileModelBinderService()
                };
            }

#if NETFRAMEWORK
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(SupportedMediaType));
#endif
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Read parameters list and bind information to model.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="value"></param>
        protected async Task BuildRequestModelAsync(object model, IList<string> parameters, object value)
        {
            // Initiate model pointer.
            var pointer = model;

            // Find the last key.
            //var lastKey = parameters[parameters.Count - 1];

            // Initiate property information.
            PropertyInfo propertyInfo = null;

            if (parameters == null || parameters.Count < 1)
                return;

            // Go through every part of parameters.
            // If the parameter name is : Items[0][list]. Parsed params will be : Items, 0, list.
            for (var index = 0; index < parameters.Count; index++)
            {
                // Find the next parameter index.
                // If the current parameter is : Item, the next param will be : 0
                var iNextIndex = index + 1;

                // Find parameter key.
                var key = parameters[index];

                // Numeric key is always about array.
                if (IsNumeric(key))
                {
                    // Invalid property info.
                    if (propertyInfo == null)
                        return;

                    // Current property information is not a list.
                    if (!IsList(propertyInfo.PropertyType))
                        return;

                    // Find the index of parameter.
                    if (!int.TryParse(key, out var iCollectionIndex))
                        iCollectionIndex = -1;

                    // This is the last key.
                    if (iNextIndex >= parameters.Count)
                    {
                        await AddCollectionItemAsync(pointer, iCollectionIndex, propertyInfo, value);
                        return;
                    }

                    var val = await AddCollectionItemAsync(pointer, iCollectionIndex, propertyInfo);
                    pointer = val;

                    // Find the property information of the next key.
                    var nextKey = parameters[iNextIndex];
                    propertyInfo = GetCaseInsensitiveProperty(pointer, nextKey);
                    continue;
                }

                // Find property of the current key.
                propertyInfo = GetCaseInsensitiveProperty(pointer, key);

                // Property doesn't exist.
                if (propertyInfo == null)
                    return;

                // This is the last parameter.
                if (iNextIndex >= parameters.Count)
                {
                    var modelValue =
                        await BuildParameterAsync(propertyInfo.PropertyType, value);

                    propertyInfo.SetValue(pointer, modelValue);
                    return;
                }

                // Find targeted value.
                var targetedValue = propertyInfo.GetValue(pointer);

                // Value doesn't exist.
                if (targetedValue == null)
                {
                    // Initiate property value.
                    targetedValue =
                        await BuildParameterAsync(propertyInfo.PropertyType,
                            Activator.CreateInstance(propertyInfo.PropertyType));

                    propertyInfo.SetValue(pointer, targetedValue);
                    pointer = targetedValue;
                    continue;
                }

                // Value is list.
                if (IsList(propertyInfo.PropertyType))
                {
                    pointer = propertyInfo.GetValue(pointer);
                    if (iNextIndex >= parameters.Count)
                        await AddCollectionItemAsync(pointer, -1, propertyInfo, value);
                    continue;
                }

                // Go to next key
                pointer = targetedValue;
            }
        }

        /// <summary>
        ///     Build model parameter asynchronously.
        /// </summary>
        /// <returns></returns>
        protected async Task<object> BuildParameterAsync(Type propertyType, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Output property value.
            var outputPropertyValue = value;

            if (propertyType == null)
                return outputPropertyValue;

            if (_modelBinderServices == null || _modelBinderServices.Length < 1)
                return outputPropertyValue;

            foreach (var availableService in _modelBinderServices)
            {
                if (availableService == null ||
                    !(availableService is IModelBinderService multiPartFormDataModelBinderService))
                    continue;

                try
                {
                    var builtModel =
                        await multiPartFormDataModelBinderService.BuildModelAsync(propertyType, value,
                            cancellationToken);

                    outputPropertyValue = builtModel;
                }
                catch (UnhandledParameterException)
                {
                }
            }

            return outputPropertyValue;
        }

        /// <summary>
        ///     Add or update member of array.
        /// </summary>
        /// <param name="pointer"></param>
        /// <param name="iCollectionIndex"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual async Task<object> AddCollectionItemAsync(object pointer, int iCollectionIndex, PropertyInfo propertyInfo,
            object value = null)
        {
            // Current member is an array, normally, it will have count property.
            var itemCountProperty = propertyInfo.PropertyType.GetProperty(nameof(Enumerable.Count));
            if (itemCountProperty == null)
                return null;

            // Find items number in the list.
            var itemCount = (int)itemCountProperty.GetValue(pointer, null);

            // Get generic arguments from property.
            var genericArguments = propertyInfo.PropertyType.GetGenericArguments();

            // No generic argument has been found.
            if (genericArguments.Length < 1)
                return null;

            // Get the first argument.
            var genericArgument = genericArguments[0];

            // Build item to add to list.
            var listItem = await BuildParameterAsync(genericArgument, value);

            // Current index is invalid to the array, this means we will add a new item to the list.
            // For example, the current array has 1 element, and the iCollectionIndex is 1.
            // The item at the index is invalid, therefore, new item will be created.
            if (iCollectionIndex < 0 || iCollectionIndex > itemCount - 1)
            {
                // Find the add method.
                var addProperty = propertyInfo.PropertyType.GetMethod(nameof(IList.Add));
                if (addProperty != null)
                    addProperty.Invoke(pointer, new[] { listItem });
                return listItem;
            }

            // If the collection index is valid.
            // For example, list contains 2 element, and we are accessing the first element. This is ok, we can do it by searching for that element and set the property value.
            var elementAtMethod = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ElementAt));

            if (elementAtMethod != null)
            {
                var item = elementAtMethod.MakeGenericMethod(genericArguments);
                return item.Invoke(pointer, new[] { pointer, iCollectionIndex });
            }

            return null;
        }

        /// <summary>
        ///     Find property information of an instance by using property name.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected PropertyInfo GetCaseInsensitiveProperty(object instance, string name)
        {
            return
                instance.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///     Check whether text is only numeric or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool IsNumeric(string text)
        {
            var regexNumeric = new Regex("^[0-9]*$");
            return regexNumeric.IsMatch(text);
        }

        /// <summary>
        ///     Whether instance is a collection or not.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool IsList(Type type)
        {
            if (!type.IsGenericType)
                return false;

            return type.GetInterface(typeof(IEnumerable<>).FullName) != null;
        }

        /// <summary>
        ///     Find content disposition parameters
        /// </summary>
        /// <returns></returns>
        protected List<string> FindContentDispositionParameters(string contentDispositionName)
        {
            if (FindContentDispositionParametersInterceptor == null)
                return contentDispositionName.Replace("[", ",")
                    .Replace("]", ",")
                    .Replace(".", ",")
                    .Split(',')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

            return FindContentDispositionParametersInterceptor(contentDispositionName);
        }

        #endregion
    }
}