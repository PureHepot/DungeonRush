using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/Effects/8-Way Outline")]
public class EightWayOutline : BaseMeshEffect
{
    public Color effectColor = Color.black;
    [Range(1, 10)]
    public int thickness = 1;  // ��ߺ�ȣ����أ�

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0) return;

        // �Ȼ���ԭʼ���㣬����� vh
        var original = new List<UIVertex>();
        vh.GetUIVertexStream(original);
        vh.Clear();

        // �˸�����ƫ��
        Vector2[] dirs = {
            new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1),
            new Vector2(0, -1),                  /*skip*/  new Vector2(0, 1),
            new Vector2(1, -1),  new Vector2(1, 0),  new Vector2(1, 1)
        };

        // ��ÿ������������
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
            // ����������ҲҪ����һ��
            for (int i = 0; i < original.Count; i += 6)
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
                vh.AddTriangle(i + 3, i + 4, i + 5);
            }
        }

        // ����ٰ�ԭ���ֻ��������ϲ�
        vh.AddUIVertexTriangleStream(original);
    }
}

