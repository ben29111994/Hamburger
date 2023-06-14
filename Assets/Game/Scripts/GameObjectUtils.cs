using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectUtils
{
    public static List<GameObject> SortByDistance(this List<GameObject> objects, Vector3 mesureFrom)
    {
        return objects.OrderBy(x => Vector3.Distance(x.transform.position, mesureFrom)).ToList();
    }
}
