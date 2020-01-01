using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiBackEnd.Services
{
    public class NotImplementedMultipartFormDataModelBinderService : IModelBinderService
    {
        public Task<object> BuildModelAsync(Type propertyType, object value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}