using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans. Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }
}
