using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FreeReignPublishing.MultiSpriteEditor
{
    public static class Docker
    {
        /// <summary>
        /// Dock-able sides of a window
        /// </summary>
        public enum DockPosition
        {
            Left,
            Top,
            Right,
            Bottom,
            None
        }

        /// <summary>
        /// </summary>
        /// <param name="thisWindow"></param>
        /// Window to dock.
        /// <param name="targetWindow"></param>
        /// Window to dock too.
        /// <param name="targetDockOffset"></param>
        /// targetWindow Offset to perform dock at.
        /// <param name="dockPosition"></param>
        /// Side of window to attempt dock.
        public static void DockTo(this EditorWindow thisWindow, EditorWindow targetWindow, Vector2 targetDockOffset, DockPosition dockPosition)
        {
            var mousePosition = GetFakeMousePosition(targetWindow, targetDockOffset, dockPosition);

            var assembly = typeof(EditorWindow).Assembly;
            var containerWindow = assembly.GetType("UnityEditor.ContainerWindow");
            var dockArea = assembly.GetType("UnityEditor.DockArea");
            var iDropArea = assembly.GetType("UnityEditor.IDropArea");

            object dropInfo = null;
            object targetView = null;

            var windows = containerWindow.GetProperty("windows", BindingFlags.Static | BindingFlags.Public).GetValue(null, null) as object[];

            if (windows != null)
            {
                foreach (var window in windows)
                {
                    var rootSplitView = window.GetType().GetProperty("rootSplitView", BindingFlags.Instance | BindingFlags.Public).GetValue(window, null);
                    if (rootSplitView != null)
                    {
                        var method = rootSplitView.GetType().GetMethod("DragOverRootView", BindingFlags.Instance | BindingFlags.Public);
                        dropInfo = method.Invoke(rootSplitView, new object[] { mousePosition });
                        targetView = rootSplitView;
                    }

                    if (dropInfo == null)
                    {
                        var rootView = window.GetType().GetProperty("rootView", BindingFlags.Instance | BindingFlags.Public).GetValue(window, null);
                        var allChildren = rootView.GetType().GetProperty("allChildren", BindingFlags.Instance | BindingFlags.Public).GetValue(rootView, null) as object[];
                        foreach (var view in allChildren)
                        {
                            if (iDropArea.IsAssignableFrom(view.GetType()))
                            {
                                var method = view.GetType().GetMethod("DragOver", BindingFlags.Instance | BindingFlags.Public);
                                dropInfo = method.Invoke(view, new object[] { targetWindow, mousePosition });
                                if (dropInfo != null)
                                {
                                    targetView = view;
                                    break;
                                }
                            }
                        }
                    }

                    if (dropInfo != null)
                    {
                        break;
                    }
                }
            }
            if (dropInfo != null && targetView != null)
            {
                var otherParent = thisWindow.GetType().GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(thisWindow);
                dockArea.GetField("s_OriginalDragSource", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, otherParent);
                var method = targetView.GetType().GetMethod("PerformDrop", BindingFlags.Instance | BindingFlags.Public);
                method.Invoke(targetView, new object[] { thisWindow, dropInfo, mousePosition });
            }
        }

        /// <summary>
        /// Gets position to attempt dock at.
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="dockOffset"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static Vector2 GetFakeMousePosition(EditorWindow wnd, Vector2 dockOffset, DockPosition position)
        {
            Vector2 mousePosition = Vector2.zero;

            var padding = 0;
            switch (position)
            {
                case DockPosition.Left:
                    mousePosition = new Vector2(padding, wnd.position.size.y / 2);
                    break;
                case DockPosition.Top:
                    mousePosition = new Vector2(wnd.position.size.x / 2, padding);
                    break;
                case DockPosition.Right:
                    mousePosition = new Vector2(wnd.position.size.x - padding, wnd.position.size.y / 2);
                    break;
                case DockPosition.Bottom:
                    mousePosition = new Vector2(wnd.position.size.x / 2, wnd.position.size.y - padding);
                    break;
            }

            return dockOffset + mousePosition;
        }
    }
}