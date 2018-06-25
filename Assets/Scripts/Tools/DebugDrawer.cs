using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class DebugDrawer {

    public static void DrawCross(Vector2 point, Color color) {
        var up = Vector2.up;
        var rt = Vector2.right;
        Debug.DrawLine(point + up, point - up);
        Debug.DrawLine(point - rt, point + rt);
    }
}

