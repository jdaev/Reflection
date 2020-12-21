using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public string PetToMake;

    void Start()
    {
        try
        {
            GameObject newPetObj = new GameObject();
            Assembly asm = typeof(Pet).Assembly;
            System.Type petType = asm.GetType(PetToMake);

            Pet pet = (Pet) newPetObj.AddComponent(petType);
            pet.PetThePet();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}