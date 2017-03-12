using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiPartFormData.Models
{
    public class MultipartFormData
    {
        #region Constructor

        /// <summary>
        ///     Initialize an instance of FormData
        /// </summary>
        public MultipartFormData()
        {
            Files = new List<KeyValuePair<string, HttpFile>>();
            Fields = new List<KeyValuePair<string, string>>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     List of primitive parameters
        /// </summary>
        public IList<KeyValuePair<string, HttpFile>> Files;

        /// <summary>
        ///     List of primitive fields.
        /// </summary>
        public List<KeyValuePair<string, string>> Fields;

        #endregion

        #region Methods

        /// <summary>
        ///     Retrieve list of every keys submitted to server.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> AllKeys()
        {
            return Files.Select(m => m.Key).Union(Files.Select(m => m.Key));
        }

        /// <summary>
        ///     Add primitive parameters to list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            Fields.Add(new KeyValuePair<string, string>(name, value));
        }

        /// <summary>
        ///     Add file parameters to list.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, HttpFile value)
        {
            Files.Add(new KeyValuePair<string, HttpFile>(name, value));
        }

        /// <summary>
        ///     Parse data and retrieve primitive data.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out string value)
        {
            var parameter =
                Fields.FirstOrDefault(m => string.Equals(m.Key, name, StringComparison.CurrentCultureIgnoreCase));

            if (parameter.Value != null)
            {
                value = parameter.Value;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        ///     Parse data and return HttpFile data.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out HttpFile value)
        {
            // Find the first condition matched key in parameters list.
            var parameter = Files.FirstOrDefault(m => string.Equals(m.Key, name, StringComparison.CurrentCultureIgnoreCase));

            // Parameter contains value.
            if (parameter.Value != null)
            {
                value = parameter.Value;
                return true;
            }

            value = null;
            return false;
        }

        #endregion
    }
}