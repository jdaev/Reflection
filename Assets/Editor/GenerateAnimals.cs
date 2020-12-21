using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GenerateAnimals
    {
        private static Dictionary<string, Type> typeDict;

        [MenuItem("Custom Tools/Generate Animals")]
        private static void Generate()
        {
            String prefabPath = Application.dataPath + "/Resources/AnimalPrefabs/";

            if (Directory.Exists(prefabPath))
            {
                Directory.Delete(prefabPath, true);
            }

            Directory.CreateDirectory(prefabPath);
            typeDict = new Dictionary<string, Type>();
            string[] allAnimalTypes = System.IO.Directory.GetFiles(Application.dataPath + "/Animals/", "*.cs");
            for (int i = 0; i < allAnimalTypes.Length; i++)
            {
                allAnimalTypes[i] = System.IO.Path.GetFileNameWithoutExtension(allAnimalTypes[i]);
            }

            foreach (string pet in allAnimalTypes)
            {
                typeDict.Add(pet, Type.GetType(pet));
            }

            foreach (KeyValuePair<string, Type> animalType in typeDict)
            {
                try
                {
                    Texture2D animalTexture = (Texture2D) Resources.Load("AnimalSprites/" + animalType.Key);
                    Sprite animalSprite = Sprite.Create(animalTexture, Rect.zero, Vector2.zero);
                    if (animalSprite != null)
                    {
                        CreateAnimal(animalType.Value, animalSprite);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public static void CreateAnimal(Type animalType, Sprite animalSprite)
        {
            String prefabPath = Application.dataPath + "/Resources/AnimalPrefabs";

            GameObject animal = new GameObject();
            animal.name = animal.ToString();
            SpriteRenderer renderer = animal.AddComponent<SpriteRenderer>();
            renderer.sprite = animalSprite;
            animal.AddComponent(animalType);

            PrefabUtility.SaveAsPrefabAsset(animal, prefabPath);
            animal.SetActive(false);
        }
    }
}