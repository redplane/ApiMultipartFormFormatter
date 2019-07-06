using System.Reflection;
using ApiMultiPartFormData.Services.Interfaces;

namespace ApiBackEnd.Services
{
    public class NotImplementedMultipartFormDataModelBinderService : IMultiPartFormDataModelBinderService
    {
        public virtual object BuildModel(PropertyInfo propertyInfo, object value)
        {
            throw new System.NotImplementedException();
        }
    }
}