using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControledHeroSwaper : MonoBehaviour
{
    public bool IsControledPlayer { get; private set; } = true;
    public bool IsControledPet { get; private set;  }
    public void SwapControledHero()
    {
        IsControledPet = !IsControledPet;
        IsControledPlayer = !IsControledPlayer;
    }    
}
