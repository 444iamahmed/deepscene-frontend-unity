using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Distance
{
    CLOSE = 1,
    FURTHER = 2,
    FAR = 3
}

public enum Direction
{
    UP = 1, DOWN = 2, LEFT = 3, RIGHT = 4, BEHIND = 5, FRONT = 6
}

[Serializable]
public class Tuple
{
    public GameObject subGameObject;
    public GameObject objGameObject;
    public string sub;
    public string obj;
    public string relation;
    public Distance distance;
    public Direction direction;
}


//[Serializable]
//public class Player
//{
//    public string playerId;
//    public string playerLoc;
//    public string playerNick;
//}