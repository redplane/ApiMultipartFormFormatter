using System.Collections.Generic;
using MultiPartFormData.Models;

namespace MultiPartFormData.Converters
{
    public class MultipartFormDataAnalyzer : Original.MultipartFormDataAnalyzer
    {
        /// <summary>
        ///     This function is for converting posted data into bytes stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public byte[] Analyze(object value, string boundary)
        {
            // Invalid value.
            if (value == null)
                return null;

            // Boundary is not detected.
            if (string.IsNullOrWhiteSpace(boundary))
                return null;

            // Retrieve the list of properties from posted data.
            var propertiesList = InitializeFlatPropertiesList(value);

            // Initialize a buffer from form data.
            var buffer = GetMultipartFormDataBytes(propertiesList, boundary);

            return buffer;
        }

        /// <summary>
        ///     Initialize properties list from posted data.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, object>> InitializeFlatPropertiesList(object value)
        {
            var propertiesList = new List<KeyValuePair<string, object>>();
            var file = value as MultipartFormData;

            if (file != null)
            {
                FillFlatPropertiesListFromFormData(file, propertiesList);
                return propertiesList;
            }

            FillFlatPropertiesListFromObject(value, "", propertiesList);
            return propertiesList;
        }
    }
}