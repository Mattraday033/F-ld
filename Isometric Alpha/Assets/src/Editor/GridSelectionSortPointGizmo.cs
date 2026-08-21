using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

//Draws a red horizontal line at the sort point of every tile currently held by the Tile Palette
//window's Select (grid selection) tool. Nothing needs to be added to a GameObject - the DrawGizmo
//callback below is invoked by the editor for each Tilemap that is rendered in the scene view.
#pragma warning disable UDR0001 // Editor-only: no runtime init method is required.
[InitializeOnLoad]
public static class GridSelectionSortPointGizmo
{
    private const int MaxCellsDrawn = 256;

    static GridSelectionSortPointGizmo()
    {
        //The scene view does not repaint on its own when the palette's selection changes.
        GridSelection.gridSelectionChanged -= onGridSelectionChanged;
        GridSelection.gridSelectionChanged += onGridSelectionChanged;
    }

    private static void onGridSelectionChanged()
    {
        SceneView.RepaintAll();
    }

    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Active)]
    private static void drawGridSelectionSortPoints(Tilemap tilemap, GizmoType gizmoType)
    {
        if (!GridSelection.active || tilemap == null)
        {
            return;
        }

        //Only the tilemap the selection was made on should draw; every other tilemap in the scene
        //gets this same callback.
        if (GridSelection.target != tilemap.gameObject)
        {
            return;
        }

        BoundsInt selection = normalize(GridSelection.position);
        int cellCount = selection.size.x * selection.size.y * selection.size.z;

        float halfWidth = Mathf.Max(tilemap.layoutGrid != null ? tilemap.layoutGrid.cellSize.x : 1f, 1f) / 2f;
        int drawn = 0;

        Gizmos.color = Color.red;

        foreach (Vector3Int cell in selection.allPositionsWithin)
        {
            if (drawn >= MaxCellsDrawn)
            {
                Debug.LogWarning($"GridSelectionSortPointGizmo: grid selection covers {cellCount} cells; drawing the first {MaxCellsDrawn} only.", tilemap);
                return;
            }

            Vector3 sortPoint = getSortPoint(tilemap, cell);

            Gizmos.DrawLine(new Vector3(sortPoint.x - halfWidth, sortPoint.y, sortPoint.z),
                            new Vector3(sortPoint.x + halfWidth, sortPoint.y, sortPoint.z));

            drawn++;
        }
    }

    //A selection dragged up or to the left comes back with a negative size, which no cell iterates
    //within, and a flat selection can come back with a zero depth.
    private static BoundsInt normalize(BoundsInt bounds)
    {
        Vector3Int min = Vector3Int.Min(bounds.min, bounds.max);
        Vector3Int size = new Vector3Int(Mathf.Max(Mathf.Abs(bounds.size.x), 1),
                                         Mathf.Max(Mathf.Abs(bounds.size.y), 1),
                                         Mathf.Max(Mathf.Abs(bounds.size.z), 1));

        return new BoundsInt(min, size);
    }

    //A tile is sorted from its anchor point - the cell position offset by the tilemap's tile anchor -
    //rather than from the middle of its sprite. A per tile transform matrix moves the drawn sprite,
    //so its translation is applied on top of that anchor.
    private static Vector3 getSortPoint(Tilemap tilemap, Vector3Int cell)
    {
        Vector3 anchor = tilemap.GetCellCenterWorld(cell);
        Matrix4x4 tileTransform = tilemap.GetTransformMatrix(cell);

        return anchor + tilemap.transform.rotation * Vector3.Scale(tileTransform.GetColumn(3), tilemap.transform.lossyScale);
    }
}
