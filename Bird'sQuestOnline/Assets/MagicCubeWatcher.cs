using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MagicCubeWatcher : MonoBehaviour
{
    public UnityEvent Triggered;

    [SerializeField] private MagicCube[] cubes = new MagicCube[4];
    private bool Active = false;

    public void Update()
    {
        int count = 0;
        foreach(MagicCube cube in cubes)
        {
            if (cube.Active)
                count++;
        }

        if (count == cubes.Length)
        {
            Triggered.Invoke();

            foreach (MagicCube cube in cubes)
                cube.NeverEnd = true;

            Destroy(this);
        }
    }
}
