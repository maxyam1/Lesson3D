using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.SerializableDict
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver// where TKey : Enum
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> pairs = new List<SerializableKeyValuePair<TKey, TValue>>();
    
        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            pairs.Clear();
            foreach (var pair in this)
            {
                pairs.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var pair in pairs)
            {
                // if (pair.Key == null)
                // {
                //     this.Add(pair.Key, pair.Value);
                //     continue;
                // }
                //
                // if (this.ContainsKey(pair.Key))
                // {
                //     this.Add(null, pair.Value);   
                // }
                // else
                // {

                
                if (this.ContainsKey(pair.Key))
                {
                    if (pair.Key is Enum)
                    {
                        TKey keyPlusOne = (TKey)(object)((int)(object)pair.Key + 1);
                        this.Add(keyPlusOne, pair.Value);
                    }
                }
                else
                {
                    this.Add(pair.Key, pair.Value);
                }

                //}
            }
    
        }
    
        [Serializable]
        public class SerializableKeyValuePair<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;
    
            public SerializableKeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
    
     // public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
     // {
     //     [SerializeField]
     //     private List<TKey> keys = new List<TKey>();
     //      
     //     [SerializeField]
     //     private List<TValue> values = new List<TValue>();
     //      
     //     // save the dictionary to lists
     //     public void OnBeforeSerialize()
     //     {
     //         keys.Clear();
     //         values.Clear();
     //         foreach(KeyValuePair<TKey, TValue> pair in this)
     //         {
     //             keys.Add(pair.Key);
     //             values.Add(pair.Value);
     //         }
     //     }
     //      
     //     // load dictionary from lists
     //     public void OnAfterDeserialize()
     //     {
     //         this.Clear();
     //  
     //         if(keys.Count != values.Count)
     //             throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
     //  
     //         for(int i = 0; i < keys.Count; i++)
     //             this.Add(keys[i], values[i]);
     //     }
     // }
    
}

