using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static bool isLayerInMask(int layer, LayerMask mask)
    {
        return (mask & (1 << layer)) != 0;
    }
}
