using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class DefaultCollectionBinderService : ICollectionBinderService
    {
        public Task<object> BuildModelAsync(PropertyInfo propertyInfo, object value, CancellationToken cancellationToken = default)
        {
            var propertyType = propertyInfo.PropertyType;
            if (!IsList(propertyType))
                throw new UnhandledParameterException();

            return Task.FromResult(Activator.CreateInstance(propertyType));
        }

        protected virtual bool IsList(Type propertyType)
        {
            return propertyType.IsGenericType
                          && (propertyType.GetGenericTypeDefinition() == typeof(IList<>) || propertyType.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}