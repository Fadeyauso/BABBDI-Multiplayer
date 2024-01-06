using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BabbdiHelper : MonoBehaviour
{
    [MenuItem("KNet/SetObjectIds")]
    public static void SetObjectsIds()
    {
        uint id = 0;
        var objects = FindObjectsOfType<KNetworkObject>();
        foreach (var obj in objects)
        {
            obj.objectId = new KNetworkId(id++);
            EditorUtility.SetDirty(obj);
        }
    }
}
