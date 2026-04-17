using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public delegate bool ConditionDelegate();
public delegate bool QualityDelegate<T>(T t);

public static class Helpers
{

    public readonly static ContactFilter2D emptyFilter = new ContactFilter2D();

    public static Collider2D getCollision(Collider2D currentCollider)
    {
        return getCollision(currentCollider, emptyFilter.NoFilter());
    }

    public static Collider2D getCollision(Collider2D currentCollider, LayerMask layerMask)
    {
        ContactFilter2D filter = new ContactFilter2D();

        filter.SetLayerMask(layerMask);
        filter.useTriggers = true;

        return getCollision(currentCollider, filter);
    }

    //returns the first collider touching the given collider. If no colliders touching, returns null
    public static Collider2D getCollision(Collider2D currentCollider, ContactFilter2D filter)
    {
        List<Collider2D> intersectingColliders = new List<Collider2D>();

        if (currentCollider.OverlapCollider(filter, intersectingColliders) > 0)
        {
            return intersectingColliders[0];
        }

        return null;
    }

    public static Collider2D[] getCollisions(Collider2D currentCollider)
    {
        return getCollisions(currentCollider, emptyFilter.NoFilter());
    }

    public static Collider2D[] getCollisions(Collider2D currentCollider, ContactFilter2D filter)
    {
        List<Collider2D> intersectingColliders = new List<Collider2D>();

        currentCollider.OverlapCollider(filter, intersectingColliders);

        return intersectingColliders.ToArray();
    }

    public static bool hasCollision(Collider2D currentCollider)
    {
        List<Collider2D> intersectingColliders = new List<Collider2D>();

        return currentCollider.OverlapCollider(emptyFilter.NoFilter(), intersectingColliders) > 0;
    }

    public static bool hasCollision(Collider2D currentCollider, LayerMask layerMask)
    {
        ContactFilter2D filter = new ContactFilter2D();

        filter.SetLayerMask(layerMask);
        filter.useTriggers = true;

        return hasCollision(currentCollider, filter);
    }

    public static bool hasCollision(Collider2D currentCollider, ContactFilter2D filter)
    {
        List<Collider2D> intersectingColliders = new List<Collider2D>();

        if (currentCollider == null)
        {
            return false;
        }

        return currentCollider.OverlapCollider(filter, intersectingColliders) > 0;
    }

    //updates colliders attached to a transform's .gameObject by setting it to inactive and then reactivating it.
    //should update the collider's position in a single frame
    public static void updateColliderPosition(Transform t)
    {
        updateColliderPosition(t.gameObject);
    }

    public static void updateColliderPosition(GameObject gObj)
    {
        gObj.SetActive(false);
        gObj.SetActive(true);
    }

    public static void updateSpritePosition(GameObject gObj)
    {
        gObj.SetActive(false);
        gObj.SetActive(true);
    }

    public static Color cloneColor(Color colorToClone)
    {
        return new Color(colorToClone.r, colorToClone.b, colorToClone.g, colorToClone.a);
    }

    public static void updateGameObjectPosition(GameObject gObj)
    {
        gObj.SetActive(false);
        gObj.SetActive(true);
    }

    public static void updateGameObjectPosition(Transform transform)
    {
        transform.gameObject.SetActive(false);
        transform.gameObject.SetActive(true);
    }

    public static bool checkPositionForColliders(Vector3 position, LayerMask layerMask)
    {
        if (Physics2D.OverlapCircle(position, Constants.detectionSize, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void flipImageByXScale(Image image)
    {
        image.rectTransform.localScale = new Vector3(image.rectTransform.localScale.x * -1,
                                                     image.rectTransform.localScale.y,
                                                     image.rectTransform.localScale.z);
    }

    public static T[] appendArray<T>(T[] array, T t)
    {

        T[] newArray = new T[(array.Length + 1)];

        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        newArray[array.Length] = t;

        return newArray;
    }

    public static T[] appendArray<T>(T[] arrayOne, T[] arrayTwo)
    {
        T[] outputArray = new T[arrayOne.Length + arrayTwo.Length];

        arrayOne.CopyTo(outputArray, 0);
        arrayTwo.CopyTo(outputArray, arrayOne.Length);

        return outputArray;
    }
    /*
		public static T[] subarray<T>(T[] data, int startIndex)
		{
			int length = data.Length - startIndex;
			return subarray<T>(data, startIndex, length);
		}

		public static T[] subarray<T>(T[] data, int startIndex, int endIndex)
		{
			int length = (endIndex - startIndex) + 1;
			return subarray<T>(data, startIndex, length);
		}

		public static T[] subarray<T>(T[] data, int startIndex, int length)
		{
			T[] result = new T[length];
			Array.Copy(data, startIndex, result, 0, length);
			return result;
		}

		public static string[] arrayToString(string[] array)
		{
			string output = "";

			return array.Aggregate(output, (a,b) => a += b);
		}
	*/

    public static void debugNullCheck(string objName, object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.LogError(objName + " == null");
        }
        else
        {
            Debug.Log(objName + " != null");
        }

        if (objectToCheck is null)
        {
            Debug.LogError(objName + " is null");
        }
        else
        {
            Debug.Log(objName + " is not null");
        }
    }

    public static bool hasQuality<T>(IEnumerable enumerable, QualityDelegate<T> containsQuality)
    {
        if (enumerable == null || enumerable is null)
        {
            return false;
        }

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                if (containsQuality(t))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static T getObjectWithQuality<T>(IEnumerable enumerable, QualityDelegate<T> containsQuality)
    {
        if (enumerable == null || enumerable is null)
        {
            return default(T);
        }

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                if (containsQuality(t))
                {
                    return t;
                }
            }
        }

        return default(T);
    }

    public static List<T> getAllObjectsWithQuality<T>(IEnumerable enumerable, QualityDelegate<T> containsQuality)
    {
        List<T> list = new List<T>();

        if (enumerable == null || enumerable is null)
        {
            return list;
        }

        foreach (T t in enumerable)
        {
            if (t != null && containsQuality(t))
            {
                list.Add(t);
            }
        }

        return list;
    }

    public delegate int SumDelegateInt<T>(T t);
    public delegate double SumDelegateDouble<T>(T t);
    public delegate float SumDelegateFloat<T>(T t);

    public static int sum<T>(IEnumerable enumerable, SumDelegateInt<T> addend)
    {
        if (enumerable == null || enumerable is null)
        {
            return 0;
        }

        int sum = 0;

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                sum += addend(t);
            }
        }

        return sum;
    }

    public static double sum<T>(IEnumerable enumerable, SumDelegateDouble<T> addend)
    {
        if (enumerable == null || enumerable is null)
        {
            return 0;
        }

        double sum = 0.0;

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                sum += addend(t);
            }
        }

        return sum;
    }

    public static float sum<T>(IEnumerable enumerable, SumDelegateFloat<T> addend)
    {
        if (enumerable == null || enumerable is null)
        {
            return 0.0f;
        }

        float sum = 0.0f;

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                sum += addend(t);
            }
        }

        return sum;
    }

    public static int castAndSum<T>(IEnumerable enumerable, SumDelegateInt<T> addend)
    {
        if (enumerable == null || enumerable is null)
        {
            return 0;
        }

        int sum = 0;

        foreach (T t in enumerable)
        {
            if (t != null)
            {
                sum += addend(t);
            }
        }

        return sum;
    }

    private delegate bool CandidacyDelegate<T>(T t);

    public static void setInteractability(Button[] buttons, int index)
    {
        for (int currentIndex = 0; currentIndex < buttons.Length; currentIndex++)
        {
            if (currentIndex == index)
            {
                buttons[currentIndex].interactable = false;
            }
            else
            {
                buttons[currentIndex].interactable = true;
            }
        }
    }

    public static void setInteractability(Button[] buttons, bool newInteractability)
    {
        for (int currentIndex = 0; currentIndex < buttons.Length; currentIndex++)
        {
            if (buttons[currentIndex] == null)
            {
                continue;
            }

            buttons[currentIndex].interactable = newInteractability;
        }
    }

    public static Sprite loadSpriteFromResources(string spriteName)
    {
        if(spriteName == null)
        {
            return null;
        }

        if (spriteName.Equals(IconList.actionTypeIconName) ||
            spriteName.Equals(IconList.traitTypeIconName) ||
            spriteName.Equals(IconList.armorTypeIconName))
        {
            spriteName = IconList.typeIconName;
        }

        // if(spriteName.Contains("Door"))
        // {
        //     Debug.LogError("spriteName = " + spriteName);
        // }

        Sprite sprite = Resources.Load<Sprite>(spriteName);

        if (sprite != null)
        {
            return sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>(spriteName.Replace(" ", ""));

            return sprite;
        }
    }

    public static Dictionary<T, int> addToDictionary<T>(Dictionary<T, int> dictionary, T key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public static Dictionary<T, bool> addToDictionary<T>(Dictionary<T, bool> dictionary, T key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = true;
        }
        else
        {
            dictionary.Add(key, true);
        }

        return dictionary;
    }

    public static void nullSafeSetInteractable(Button button, bool active)
    {
        if (button != null && !(button is null))
        {
            button.interactable = active;
        }
    }

    public static void nullSafeSetActive(MonoBehaviour component, bool active)
    {
        if (component != null && !(component is null))
        {
            component.gameObject.SetActive(active);
        }
    }

    public static void nullSafeSetActive(GameObject gameObject, bool active)
    {
        if (gameObject != null && !(gameObject is null))
        {
            gameObject.SetActive(active);
        }
    }

    public static Vector2 findMidPoint(Vector2 point1, Vector2 point2)
    {
        return (point1 + point2) / 2;
    }

    public static Vector2 findQuarterPoint(Vector2 closerPoint, Vector2 fartherPoint)
    {
        return findMidPoint(closerPoint, findMidPoint(closerPoint, fartherPoint));
    }

    public static Vector2 findEighthPoint(Vector2 closerPoint, Vector2 fartherPoint)
    {
        return findMidPoint(closerPoint, findQuarterPoint(closerPoint, fartherPoint));
    }

    public static bool tagMatchesCriteria(GameObject combatSprite, string[] tagCriteria)
    {
        foreach (string tag in tagCriteria)
        {
            if (combatSprite.gameObject.tag.Equals(tag))
            {
                return true;
            }
        }

        return false;
    }

    public static string removeSpacesOnEnds(string stringWithSpaces)
    {
        int startIndex = 0;
        for(int i = 0; i < stringWithSpaces.Length; i++)
        {
            if(stringWithSpaces[i] == ' ')
            {
                continue;
            }

            startIndex = i;
            break;
        }

        int endIndex = stringWithSpaces.Length;
        for(int i = stringWithSpaces.Length-1; i >= 0; i--)
        {
            if(stringWithSpaces[i] == ' ')
            {
                continue;
            }

            endIndex = i+1;
            break;
        }

        return stringWithSpaces.Substring(startIndex,endIndex - startIndex);
    }

    public static void updatePolygonCollider(SpriteRenderer spriteRenderer, PolygonCollider2D polygonCollider2D)
    {
        if(spriteRenderer.sprite == null)
        {
            return;
        }

        List<Vector2> pointsList = new List<Vector2>();

        spriteRenderer.sprite.GetPhysicsShape(0, pointsList); 

        polygonCollider2D.points = pointsList.ToArray();
    }


    public static Vector3 getAveragePosition(List<Vector3> positions)
    {
        if(positions.Count <= 0)
        {
            return Vector3.zero;
        }

        Vector3 averagePosition = new Vector3(0f, 0f, 0f);

        foreach(Vector3 position in positions)
        {
            averagePosition += position;
        }

        return new Vector3(averagePosition.x / positions.Count, averagePosition.y / positions.Count, averagePosition.z / positions.Count);
    }

    private const float zPosMultiplier  = .0001f;
    public static float calculateColliderZPosition(Vector3Int cellCoords)
    {
        return -1f + ((( zPosMultiplier * (float) cellCoords.x) + ( zPosMultiplier * (float) cellCoords.y))/2f);
    }

} 