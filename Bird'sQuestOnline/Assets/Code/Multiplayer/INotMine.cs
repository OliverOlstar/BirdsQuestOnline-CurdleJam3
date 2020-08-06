using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INotMine: MonoBehaviour
{
    [SerializeField] private bool notMineGameObject = false;

    public void Remove()
    {
        if (notMineGameObject)
            Destroy(gameObject);
        else
            Destroy(this);
    }
}
