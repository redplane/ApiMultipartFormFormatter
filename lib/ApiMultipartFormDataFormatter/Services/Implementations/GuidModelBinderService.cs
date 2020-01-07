using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class GuidModelBinderService : IModelBinderService
    {
        #region Methods

        public Task<object> BuildModelAsync(Type propertyType, object value,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get property type.
            var underlyingType = Nullable.GetUnderlyingType(propertyType);

            // Property is GUID.
            if (propertyType == typeof(Guid) && Guid.TryParse(value.ToString(), out var guid))
                return Task.FromResult((object) guid);

            if (underlyingType == typeof(Guid))
            {
                if (Guid.TryParse(value?.ToString(), out guid))
                    return Task.FromResult((object)guid);

                return Task.FromResult(default(object));
            }

            throw new UnhandledParameterException();
        }


        #endregion
    }
}