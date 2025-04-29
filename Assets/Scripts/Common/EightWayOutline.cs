using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Effects/8-Way Outline")]
public class EightWayOutline : BaseMeshEffect
{
    public Color effectColor = Color.black;
    [Range(1, 10)]
    public int thickness = 1;  // 描边厚度（像素）

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        // 先缓存原始顶点，再清空 vh
        var original = new List<UIVertex>();
        vh.GetUIVertexStream(original);
        vh.Clear();

        // 八个方向偏移
        Vector2[] dirs = {
            new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1),
            new Vector2(0, -1),                  /*skip*/  new Vector2(0, 1),
            new Vector2(1, -1),  new Vector2(1, 0),  new Vector2(1, 1)
        };

        // 对每个方向叠加描边
        foreach (var dir in dirs)
        {
            var offset = dir.normalized * thickness;
            for (int i = 0; i < original.Count; i++)
            {
                var v = original[i];
                v.position += (Vector3)offset;
                v.color = effectColor;
                vh.AddVert(v);
            }
            // 三角形索引也要复制一份
            for (int i = 0; i < original.Count; i += 6)
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
                vh.AddTriangle(i + 3, i + 4, i + 5);
            }
        }

        // 最后再把原文字绘制在最上层
        vh.AddUIVertexTriangleStream(original);
    }
}

