using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AttributeParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("owner[name][first]", "Linh");
            dictionary.Add("owner[name][last]", "Nguyen");
            dictionary.Add("owner[age]", "Nguyen");
            dictionary.Add("power[index]", "1");
            dictionary.Add("power[range]", "5");

            var model = new Dictionary<string, object>();
            foreach (var key in dictionary.Keys)
            {
                var parameters = key.Replace("[", ",").Replace("]", ",").Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
                Read(model, parameters, dictionary[key]);
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

        private static void Read(IDictionary<string, object> model, IList<string> parameters, object value)
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