using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Exceptions;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiMultiPartFormData.Services.Implementations
{
    public class HttpFileModelBinderService : IModelBinderService
    {
        #region Methods

        public object BuildModel(PropertyInfo propertyInfo, object value)
        {
            throw new NotImplementedException();
        }

        public Task<object> BuildModelAsync(Type propertyType, object value,
            CancellationToken cancellationToken = default)
        {
            if (value == null)
                throw new UnhandledParameterException();

            if (!(value is HttpFileBase httpFileBase))
                throw new UnhandledParameterException();

            if (propertyType != typeof(HttpFile))
                throw new UnhandledParameterException();


            return Task.FromResult((object) new HttpFile(httpFileBase));
        }


        #endregion
    }
}