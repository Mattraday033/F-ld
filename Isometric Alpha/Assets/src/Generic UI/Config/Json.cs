using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class Json 
{

    public static bool fileIsJson(string filePath)
    {
        return String.Equals(filePath.Split(".")[1], Constants.jsonFileExtensionWithoutPeriod, StringComparison.OrdinalIgnoreCase);
    }

	public static T getObjectFromJSON<T>(string jsonPath)
	{
        T output = default(T);

		if (File.Exists(jsonPath))
		{
			string jsonString = File.ReadAllText(jsonPath);

            try
            {
                output = JsonConvert.DeserializeObject<T>(jsonString);
            } catch(Exception e)
            {
                
            }
		}

        return output;
	}

	public static void writeObjectToJSON<T>(string jsonPath, T origin)
	{
        string[] jsonPathParts = jsonPath.Split("/");
        string jsonName = jsonPathParts[jsonPathParts.Length - 1];
        string destinationFolder = jsonPath.Split("/" + jsonName)[0];

		if (!Directory.Exists(destinationFolder))
		{
            Directory.CreateDirectory(destinationFolder);
		}

		if (File.Exists(jsonPath))
		{
            File.Delete(jsonPath);
		}

        string json = JsonConvert.SerializeObject(origin);

		File.WriteAllText(jsonPath, json);

	}

}
