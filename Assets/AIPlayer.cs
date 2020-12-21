using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

public class AIPlayer : MonoBehaviour
{
    public int movementSpeed = 10;
    public int timestep = 1;

    public Command[] commandsToExecute;

    void Start()
    {
    }


    void Update()
    {
    }

    [AIPlayerTargetAttribute]
    public void Move(Vector2 direction)
    {
    }

    [AIPlayerTargetAttribute]
    public void Shoot(Vector2 direction)
    {
        Debug.Log("Shooting...");
    }

    [AIPlayerTargetAttribute]
    public void ChangeMoveSpeed(int speed)
    {
        movementSpeed = speed;
    }

    [AIPlayerTargetAttribute]
    public void Wait()
    {
    }

    public void NotUsingAttr()
    {
    }
}

[System.Serializable]
public class Command
{
    public Command(string commandName, object argument)
    {
        this.commandName = commandName;
        this.argument = argument;
        this.uid = UniqueID();
    }

    string UniqueID()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int) (DateTime.UtcNow - epochStart).TotalSeconds;
        int z1 = UnityEngine.Random.Range(0, 1000000);
        int z2 = UnityEngine.Random.Range(0, 1000000);
        return currentEpochTime + ":" + z1 + ":" + z2;
    }

    public string commandName;
    public object argument;
    public string uid;
}