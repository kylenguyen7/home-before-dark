using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHatHandler : MonoBehaviour
{
    public WhiteHatController[] myWhiteHats;
    public int hpToSet = 5;

    public void Start()
    {
        foreach(WhiteHatController wh in myWhiteHats)
        {
            wh.hp = hpToSet;
        }
    }

    public void masterTakeDamage(int amount)
    {
        foreach (WhiteHatController wh in myWhiteHats)
        {
            wh.takeDamage(amount);
        }
    }

    public void masterToggleOutline(bool turnOn)
    {
        foreach (WhiteHatController wh in myWhiteHats)
        {
            wh.toggleOutline(turnOn);
        }
    }
}
