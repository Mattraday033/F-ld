using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetFromJson
{
    private const string cleanSlateSaveName = "cleanSlateSave";
    private const string saveNameElementName = "saveName";

    public static dynamic getElementFromJson(string elementName, dynamic jsonDynamic, dynamic defaultValue)
    {
        return getElementFromJson(elementName, elementName, jsonDynamic, defaultValue); 
    }

    public static dynamic getElementFromJson(string jsonName, string elementName, dynamic jsonDynamic, dynamic defaultValue)
    {
        try
        {
            dynamic element = jsonDynamic[elementName];

            if (element == null)
            {
                return defaultValue;
            }
            else
            {
                return element;
            }
        }
        catch (Exception e)
        {
            if (!jsonName.Equals(cleanSlateSaveName) && !elementName.Equals(saveNameElementName))
            {
                Debug.LogError("Caught Exception of type:" + e.GetType().Name +
                                "\nMessage:" + e.Message +
                                "\n\nSaveBlueprint(" + jsonName + ") does not have an elementNamed: " + elementName +
                                "\n\njsonDynamic: (" + jsonDynamic.ToString() + ")");
            }

            return defaultValue;
        }
    }
}
