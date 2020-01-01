using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class GuidModelBinderService : IMultiPartFormDataModelBinderService
    {
        #region Methods

        public object BuildModel(PropertyInfo propertyInfo, object value)
        {
            throw new NotImplementedException();
        }

        public Task<object> BuildModelAsync(PropertyInfo propertyInfo, object value,
            CancellationToken cancellationToken = default)
        {
            if (value == null)
                throw new UnhandledParameterException();

            // Get property type.
            var propertyType = propertyInfo.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is GUID.
            if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
                return Task.FromResult((object) guid);

            if (underlyingType == typeof(Guid))
                if (Guid.TryParse(value.ToString(), out guid))
                    return Task.FromResult((object) guid);

            throw new UnhandledParameterException();
        }


        #endregion
    }
}