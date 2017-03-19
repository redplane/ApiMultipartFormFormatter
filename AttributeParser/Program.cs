using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
            dictionary.Add("category[photo][absolute]", "Absolute");
            dictionary.Add("category[photo][relative]", "Relative");

            var instance = Activator.CreateInstance(typeof(Account));

            foreach (var key in dictionary.Keys)
            {
                var parameters = key.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
                Read(instance, parameters, dictionary[key]);
            }

            var a = 1;
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
            PropertyInfo propertyInfo;

            // Loop through every keys.
            for (var index = 0; index < parameters.Count - 1; index++)
            {
                // Find key.
                var key = parameters[index];

                propertyInfo =
                    pointer.GetType()
                        .GetProperties()
                        .FirstOrDefault(x => key.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

                // Property hasn't been initialized.
                if (propertyInfo == null)
                    break;

                // Initiate property.
                propertyInfo = pointer.GetType()
                    .GetProperties()
                    .FirstOrDefault(x => key.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

                // Property doesn't exist in object.
                if (propertyInfo == null)
                    return;
                
                var val = propertyInfo.GetValue(pointer);
                if (val == null)
                {
                    val = Convert.ChangeType(Activator.CreateInstance(propertyInfo.PropertyType), propertyInfo.PropertyType);
                    propertyInfo.SetValue(pointer, val);
                    pointer = val;
                    continue;
                }

                pointer = val;
            }

            propertyInfo = pointer.GetType()
                .GetProperties()
                .FirstOrDefault(x => lastKey.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo != null)
                propertyInfo.SetValue(pointer, Convert.ChangeType(value, propertyInfo.PropertyType));
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