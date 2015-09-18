using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Michal.Project.Models
{
   // Summary:
    //     Defines a key/value pair that can be set or retrieved.
    //
    // Type parameters:
    //   TKey:
    //     The type of the key.
    //
    //   TValue:
    //     The type of the value.
    [Serializable]
    [Newtonsoft.Json.JsonObject]
    public class KeyValuePairUI
    {

        public KeyValuePairUI(string key, string value)
        {
            Key = key; Value = value;
        }
        [Newtonsoft.Json.JsonProperty("id")]
        public string Key { get; set; }
        //
        // Summary:
        //     Gets the value in the key/value pair.
        //
        // Returns:
        //     A TValue that is the value of the System.Collections.Generic.KeyValuePair<TKey,TValue>.
        [Newtonsoft.Json.JsonProperty("value")]
        public string Value { get; set; }

    } 
    
}