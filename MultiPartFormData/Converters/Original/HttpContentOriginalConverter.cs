using System;
using System.IO;
using System.Net.Http.Headers;

namespace MultiPartFormData.Converters.Original
{
    public class HttpContentOriginalConverter
    {
        /// <summary>
        ///     This function is for checking whether a ContentDispositionHeaderValue instance is a file or not.
        /// </summary>
        /// <param name="contentDispositionHeaderValue"></param>
        /// <returns></returns>
        protected bool IsFile(ContentDispositionHeaderValue contentDispositionHeaderValue)
        {
            return !string.IsNullOrEmpty(contentDispositionHeaderValue.FileName);
        }

        /// <summary>
        ///     Remove bounding quotes on a token if present
        /// </summary>
        protected string RemoveQuotes(string token)
        {
            // Invalid token.
            if (string.IsNullOrWhiteSpace(token))
                return token;

            // Remove quote tokens as they're detected. .
            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) &&
                (token.Length > 1))
                return token.Substring(1, token.Length - 2);

            return token;
        }

        /// <summary>
        ///     Amend filenames to remove surrounding quotes and remove path from IE
        /// </summary>
        protected string FixFilename(string originalFileName)
        {
            // Invalid original file name.
            if (string.IsNullOrWhiteSpace(originalFileName))
                return string.Empty;

            var result = originalFileName.Trim();

            // remove leading and trailing quotes
            result = result.Trim('"');

            // remove full path versions
            if (result.Contains("\\"))
                result = Path.GetFileName(result);

            return result;
        }

        /// <summary>
        ///     Read all bytes from stream.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected byte[] ReadAllBytes(Stream input)
        {
            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream);
                return stream.ToArray();
            }
        }
    }
}