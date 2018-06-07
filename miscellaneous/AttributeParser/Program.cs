using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AttributeParser.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AttributeParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("id", 1);
            dictionary.Add("category[id]", 1);

            dictionary.Add("category[photos][0][absolute]", "Absolute");
            dictionary.Add("category[photos][0][relative]", "Relative");
            dictionary.Add("category[photos][1][absolute]", "Absolute");
            dictionary.Add("category[photos][1][relative]", "This is relative");

            var instance = Activator.CreateInstance(typeof(Account));

            foreach (var key in dictionary.Keys)
            {
                var parameters = key.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
                Read(instance, parameters, dictionary[key]);
            }
        }

        /// <summary>
        /// Read and construct nested properties instance base on parameters with final value.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="parameters"></param>
        /// <param name="value"></param>
        private static void Read(object model, IList<string> parameters, object value)
        {
            // Initiate model pointer.
            var pointer = model;

            // Find the last key.
            var lastKey = parameters[parameters.Count - 1];

            // Initiate property information.
            PropertyInfo propertyInfo = null;

            // Loop through every keys.
            for (var index = 0; index < parameters.Count - 1; index++)
            {
                // Find key.
                var key = parameters[index];

                // Whether key is numeric or not.
                // Collection should be used numeric index to insert to list.
                if (IsNumeric(key))
                {
                    int iCollectionIndex;
                    if (!int.TryParse(key, out iCollectionIndex))
                        break;

                    if (propertyInfo == null)
                        break;


                    var itemCount = (int)propertyInfo.PropertyType.GetProperty("Count").GetValue(pointer, null);
                    var genericArguments = propertyInfo.PropertyType.GetGenericArguments();

                    //var iGenericListArgumentTotal = propertyInfo.PropertyType.GetMethod("Count").Invoke(pointer, new[] { null });
                    if (iCollectionIndex > itemCount - 1)
                    {
                        var listItem = Activator.CreateInstance(propertyInfo.PropertyType.GetGenericArguments()[0]);
                        propertyInfo.PropertyType.GetMethod("Add").Invoke(pointer, new[] { listItem });
                        pointer = listItem;
                        continue;
                    }

                    var commandSelectItem = typeof(Enumerable)
                        .GetMethod("ElementAt")
                        .MakeGenericMethod(genericArguments[0]);

                    // Find item at specific index.
                    pointer = commandSelectItem.Invoke(pointer, new[] { pointer, iCollectionIndex });
                }

                // Find property information of pointer.
                propertyInfo = FindInstanceProperty(pointer, key);

                // Property hasn't been initialized.
                if (propertyInfo == null)
                    break;

                var val = propertyInfo.GetValue(pointer);
                if (val == null)
                {
                    val = Convert.ChangeType(Activator.CreateInstance(propertyInfo.PropertyType),
                        propertyInfo.PropertyType);
                    propertyInfo.SetValue(pointer, val);
                    pointer = val;
                    continue;
                }

                pointer = val;
            }

            // Find last property.
            propertyInfo = FindInstanceProperty(pointer, lastKey);

            if (propertyInfo != null)
                propertyInfo.SetValue(pointer, Convert.ChangeType(value, propertyInfo.PropertyType));
        }

        /// <summary>
        /// Find property information of an instance by using property name.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static PropertyInfo FindInstanceProperty(object instance, string name)
        {
            return
                    instance.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Check whether text is only numeric or not.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static bool IsNumeric(string text)
        {
            var regexNumeric = new Regex("^[0-9]*$");
            return regexNumeric.IsMatch(text);
        }

        private static void Main_1(string[] args)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("owner[name][first]", "Linh");
            dictionary.Add("owner[name][last]", "Nguyen");
            dictionary.Add("owner[age]", "1");
            dictionary.Add("power[index]", "1");
            dictionary.Add("power[range]", "5");

            var model = new Dictionary<string, object>();
            foreach (var key in dictionary.Keys)
            {
                var parameters = key.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
                Read_1(model, parameters, dictionary[key]);
            }
            //var url = "owner[name][first]";
            //var parameters = url.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();

            //var parameter = new Dictionary<string, object>();
            //object pointer = parameter;
            //string lastKey = parameters[parameters.Count - 1];
            //for (var index = 0; index < parameters.Count - 1; index++ )
            //{
            //    var key = parameters[index];
            //    var pair = new Dictionary<string, object>();
            //    ((Dictionary<string, object>)pointer).Add(key, pair);
            //    pointer = pair;
            //}

            //((Dictionary<string, object>) pointer)[lastKey] = "Linh Nguyen";
            var text = JsonConvert.SerializeObject(model);
            Console.WriteLine(text);
            Console.ReadLine();
        }

        private static void Read_1(IDictionary<string, object> model, IList<string> parameters, object value)
        {
            object pointer = model;
            var lastKey = parameters[parameters.Count - 1];
            for (var index = 0; index < parameters.Count - 1; index++)
            {
                var dictionary = (Dictionary<string, object>)pointer;
                var key = parameters[index];
                if (dictionary.ContainsKey(key))
                {
                    pointer = dictionary[key];
                    continue;
                }

                //var pair = new Dictionary<string, object>();
                var val = new Dictionary<string, object>();
                dictionary.Add(key, val);
                //((Dictionary<string, object>)pointer).Add(key, pair);
                pointer = val;
            }

            ((Dictionary<string, object>)pointer)[lastKey] = value;
        }
    }
}