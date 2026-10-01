using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//Temporary debugging tool: on left click, logs everything under the mouse that could affect OnMouseEnter-based hovers
//(NPCMouseHover etc.) as a single Debug.LogError, so the whole report can be copied from the console at once.
//It creates itself when play mode starts, so it doesn't need to be placed in a scene. Delete this file when finished.
public class MouseHoverBlockerDebugger : MonoBehaviour
{
    //static readonly rather than const so the early return below doesn't raise an unreachable code warning
    private static readonly bool active = true;

// #if UNITY_EDITOR
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void createInstance()
//     {
//         if(!active)
//         {
//             return;
//         }

//         GameObject debuggerObject = new GameObject(nameof(MouseHoverBlockerDebugger));
//         DontDestroyOnLoad(debuggerObject);
//         debuggerObject.AddComponent<MouseHoverBlockerDebugger>();
//     }
// #endif

//     private void Update()
//     {
//         if(Input.GetMouseButtonDown(0))
//         {
//             Debug.LogError(buildReport(Input.mousePosition));
//         }
//     }

    private static string buildReport(Vector3 mousePosition)
    {
        StringBuilder report = new StringBuilder();

        report.AppendLine("===== MOUSE HOVER BLOCKER REPORT =====");
        report.AppendLine($"frame={Time.frameCount} mouseScreen={mousePosition}");
        appendSafe(report, "activity", () => PlayerStateManager.currentActivity.ToString());
        appendSafe(report, "location", () => AreaManager.locationName);
        report.AppendLine($"loadedScenes=[{string.Join(", ", getLoadedSceneNames())}] activeScene={SceneManager.GetActiveScene().name}");
        report.AppendLine($"Physics2D.queriesHitTriggers={Physics2D.queriesHitTriggers} Physics2D.queriesStartInColliders={Physics2D.queriesStartInColliders} Physics.queriesHitTriggers={Physics.queriesHitTriggers}");
        report.AppendLine($"Camera.main={describeObject(Camera.main)}");

        appendEventSystem(report, mousePosition);

        Camera[] cameras = Camera.allCameras.OrderBy(camera => camera.depth).ToArray();
        report.AppendLine();
        report.AppendLine($"----- CAMERAS ({cameras.Length} enabled, ascending depth; OnMouseEnter uses the last camera below that gets a hit) -----");

        foreach(Camera camera in cameras)
        {
            appendCamera(report, camera, mousePosition);
        }

        appendMouseHoverColliders(report, cameras.Length > 0 ? cameras[cameras.Length - 1] : Camera.main, mousePosition);

        report.AppendLine("===== END REPORT =====");

        return report.ToString();
    }

    private static void appendCamera(StringBuilder report, Camera camera, Vector3 mousePosition)
    {
        int effectiveMask = camera.cullingMask & camera.eventMask;

        report.AppendLine();
        report.AppendLine($"[Camera] {describeObject(camera)} isCameraMain={camera == Camera.main}");
        report.AppendLine($"  depth={camera.depth} clearFlags={camera.clearFlags} orthographic={camera.orthographic} near={camera.nearClipPlane} far={camera.farClipPlane} position={camera.transform.position} rotation={camera.transform.eulerAngles}");
        report.AppendLine($"  targetTexture={(camera.targetTexture != null ? camera.targetTexture.name : "none")} targetDisplay={camera.targetDisplay} pixelRect={camera.pixelRect} containsMouse={camera.pixelRect.Contains(mousePosition)}");
        report.AppendLine($"  eventMask={camera.eventMask} [{describeMask(camera.eventMask)}]");
        report.AppendLine($"  cullingMask={camera.cullingMask} [{describeMask(camera.cullingMask)}]");
        report.AppendLine($"  effectiveMask(culling&event)={effectiveMask} [{describeMask(effectiveMask)}]");

        Ray ray = camera.ScreenPointToRay(mousePosition);
        //matches the distance Unity's mouse event pass uses for its raycasts
        float distance = Mathf.Approximately(0f, ray.direction.z) ? Mathf.Infinity : Mathf.Abs((camera.farClipPlane - camera.nearClipPlane) / ray.direction.z);

        report.AppendLine($"  ray origin={ray.origin} direction={ray.direction} distance={distance}");

        RaycastHit2D winner2D = Physics2D.GetRayIntersection(ray, distance, effectiveMask);
        report.AppendLine($"  >> 2D hit OnMouseEnter would target (effective mask): {(winner2D.collider != null ? describeCollider2D(winner2D.collider) + $" hitDistance={winner2D.distance}" : "NONE")}");

        if(Physics.Raycast(ray, out RaycastHit winner3D, distance, effectiveMask))
        {
            report.AppendLine($"  >> 3D hit OnMouseEnter would ALSO target: {describeObject(winner3D.collider)} layer={layerName(winner3D.collider.gameObject.layer)} distance={winner3D.distance}");
        }

        RaycastHit2D[] all2D = Physics2D.GetRayIntersectionAll(ray, distance, Physics2D.AllLayers)
                                        .OrderBy(hit => hit.distance)
                                        .ToArray();

        report.AppendLine($"  all 2D colliders along ray, ANY layer, nearest first ({all2D.Length}):");

        for(int index = 0; index < all2D.Length; index++)
        {
            Collider2D collider = all2D[index].collider;
            bool inEffectiveMask = (effectiveMask & (1 << collider.gameObject.layer)) != 0;

            report.AppendLine($"    #{index} hitDistance={all2D[index].distance} inEffectiveMask={inEffectiveMask} {describeCollider2D(collider)}");
        }

        RaycastHit[] all3D = Physics.RaycastAll(ray, distance, Physics.AllLayers);

        if(all3D.Length > 0)
        {
            report.AppendLine($"  all 3D colliders along ray ({all3D.Length}):");

            foreach(RaycastHit hit in all3D.OrderBy(hit => hit.distance))
            {
                report.AppendLine($"    distance={hit.distance} {describeObject(hit.collider)} layer={layerName(hit.collider.gameObject.layer)} trigger={hit.collider.isTrigger}");
            }
        }
    }

    //every NPCMouseHover whose collider covers the clicked point, i.e. the hovers that are expected to fire here
    private static void appendMouseHoverColliders(StringBuilder report, Camera camera, Vector3 mousePosition)
    {
        report.AppendLine();
        report.AppendLine("----- NPCMouseHover COLLIDERS COVERING THIS POINT (ignoring z and layer) -----");

        if(camera == null)
        {
            report.AppendLine("  no camera to convert the mouse position with");
            return;
        }

        Vector2 worldPoint = camera.ScreenToWorldPoint(mousePosition);
        report.AppendLine($"  worldPoint={worldPoint} (via {camera.name})");

        NPCMouseHover[] hovers = FindObjectsByType<NPCMouseHover>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int covering = 0;

        foreach(NPCMouseHover hover in hovers)
        {
            Collider2D collider = hover.GetComponent<Collider2D>();

            if(collider == null || !collider.bounds.Contains(new Vector3(worldPoint.x, worldPoint.y, collider.bounds.center.z)))
            {
                continue;
            }

            covering++;
            report.AppendLine($"  NPCMouseHover on {hierarchyPath(hover.transform)} activeInHierarchy={hover.gameObject.activeInHierarchy} scriptEnabled={hover.enabled} overlapsPoint={collider.OverlapPoint(worldPoint)}");
            report.AppendLine($"    {describeCollider2D(collider)}");
        }

        report.AppendLine($"  {covering} of {hovers.Length} NPCMouseHovers have collider bounds over this point");
    }

    private static void appendEventSystem(StringBuilder report, Vector3 mousePosition)
    {
        EventSystem eventSystem = EventSystem.current;

        if(eventSystem == null)
        {
            report.AppendLine("EventSystem.current=null");
            return;
        }

        PointerEventData pointerData = new PointerEventData(eventSystem) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        report.AppendLine($"EventSystem={describeObject(eventSystem)} IsPointerOverGameObject={eventSystem.IsPointerOverGameObject()} uiRaycastHits={results.Count} (UI does not block OnMouseEnter, listed for completeness)");

        foreach(RaycastResult result in results.Take(10))
        {
            report.AppendLine($"  ui: {hierarchyPath(result.gameObject.transform)} layer={layerName(result.gameObject.layer)} raycaster={result.module?.GetType().Name} sortingLayer={result.sortingLayer} sortingOrder={result.sortingOrder} depth={result.depth}");
        }
    }

    private static string describeCollider2D(Collider2D collider)
    {
        StringBuilder description = new StringBuilder();
        GameObject gameObject = collider.gameObject;

        description.Append($"{collider.GetType().Name} on '{hierarchyPath(collider.transform)}' scene={gameObject.scene.name}");
        description.Append($" layer={layerName(gameObject.layer)}({gameObject.layer}) tag={gameObject.tag}");
        description.Append($" colliderEnabled={collider.enabled} activeInHierarchy={gameObject.activeInHierarchy} isTrigger={collider.isTrigger}");
        description.Append($" transformZ={collider.transform.position.z} boundsCenter={collider.bounds.center} boundsSize={collider.bounds.size}");

        if(collider.compositeOperation != Collider2D.CompositeOperation.None)
        {
            description.Append($" compositeOperation={collider.compositeOperation}");
        }

        if(collider.attachedRigidbody != null)
        {
            description.Append($" rigidbody='{hierarchyPath(collider.attachedRigidbody.transform)}' layer={layerName(collider.attachedRigidbody.gameObject.layer)}");
        }

        if(collider.transform.parent != null)
        {
            description.Append($" parentLayer={layerName(collider.transform.parent.gameObject.layer)}");
        }

        description.Append($" components=[{describeComponents(gameObject)}]");

        return description.ToString();
    }

    private static string describeComponents(GameObject gameObject)
    {
        return string.Join(", ", gameObject.GetComponents<Component>().Select(component =>
        {
            if(component == null)
            {
                return "MissingScript";
            }

            string name = component.GetType().Name;

            if(component is Behaviour behaviour && !behaviour.enabled)
            {
                name += "(disabled)";
            }

            if(component is MonoBehaviour && hasMouseMessage(component.GetType()))
            {
                name += "(OnMouse*)";
            }

            return name;
        }));
    }

    private static bool hasMouseMessage(Type type)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        return type.GetMethod("OnMouseEnter", flags) != null ||
               type.GetMethod("OnMouseOver", flags) != null ||
               type.GetMethod("OnMouseExit", flags) != null;
    }

    private static string describeMask(int mask)
    {
        if(mask == -1)
        {
            return "Everything";
        }

        List<string> names = new List<string>();

        for(int layer = 0; layer < 32; layer++)
        {
            if((mask & (1 << layer)) != 0)
            {
                names.Add($"{layerName(layer)}({layer})");
            }
        }

        return names.Count == 0 ? "Nothing" : string.Join(", ", names);
    }

    private static string layerName(int layer)
    {
        string name = LayerMask.LayerToName(layer);

        return string.IsNullOrEmpty(name) ? "<unnamed>" : name;
    }

    private static string describeObject(Component component)
    {
        if(component == null)
        {
            return "null";
        }

        return $"'{hierarchyPath(component.transform)}' (scene={component.gameObject.scene.name}, tag={component.gameObject.tag}, layer={layerName(component.gameObject.layer)})";
    }

    private static string hierarchyPath(Transform transform)
    {
        string path = transform.name;

        while(transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }

        return path;
    }

    private static IEnumerable<string> getLoadedSceneNames()
    {
        for(int index = 0; index < SceneManager.sceneCount; index++)
        {
            yield return SceneManager.GetSceneAt(index).name;
        }
    }

    private static void appendSafe(StringBuilder report, string label, Func<string> getValue)
    {
        try
        {
            report.AppendLine($"{label}={getValue()}");
        }
        catch(Exception exception)
        {
            report.AppendLine($"{label}=<error: {exception.GetType().Name}>");
        }
    }
}
