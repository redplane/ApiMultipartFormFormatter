#if NETCOREAPP
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using ApiMultiPartFormData.Models;
using ApiMultiPartFormData.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiMultiPartFormData
{
    public partial class MultipartFormDataFormatter : InputFormatter
    {
        public override bool CanRead(InputFormatterContext context)
        {
            var type = context.ModelType;
            if (type == null)
                throw new ArgumentException($"{nameof(context)}.{nameof(context.ModelType)} cannot be null.");

            return true;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var type = context.ModelType;

            // Type is invalid.
            if (type == null)
                throw new ArgumentNullException($"{nameof(context)}.{nameof(context.ModelType)} cannot be null.");

            try
            {
                // load multipart data into memory 
                var httpContents = await context.HttpContext.Request.ReadFormAsync();

                // Create an instance from specific type.
                var instance = Activator.CreateInstance(type);

                foreach (var httpContent in httpContents)
                {
                    // Find parameter from content deposition.
                    var contentParameter = httpContent.GetContentDispositionName();
                    var parameterParts = FindContentDispositionParameters(contentParameter);

                    // Content is a parameter, not a file.
                    var value = await httpContent.ReadAsStringAsync();
                    await BuildRequestModelAsync(instance, parameterParts, value);
                }

                foreach (var httpFile in httpContents.Files)
                {
                    // Content is a file.
                    // File retrieved from client-side.
                    var contentParameter = httpFile.Name;
                    var parameterParts = FindContentDispositionParameters(contentParameter);

                    var file = new HttpFileBase(
                        httpFile.GetFileName(),
                        await httpFile.ReadAsStreamAsync(),
                        httpFile.GetContentType());

                    await BuildRequestModelAsync(instance, parameterParts, file);
                }

                return InputFormatterResult.Success(instance);
            }
            catch (Exception e)
            {
                // TODO: Implement logger.
                //if (logger == null)
                //    throw;
                //logger.LogError(string.Empty, e);
                var defaultValue = GetDefaultValueForType(type);
                return InputFormatterResult.Success(defaultValue);
            }
        }
    }
}

#endif