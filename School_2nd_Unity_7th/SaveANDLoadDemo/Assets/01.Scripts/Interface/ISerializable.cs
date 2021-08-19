using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public interface ISerializable
{
    public JObject Serialize();

    public void DeSerialize(string jsonString);

    string GetJsonKey();
}
