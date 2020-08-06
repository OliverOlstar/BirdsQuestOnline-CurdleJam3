using UnityEngine;

public interface IPeckable
{
    Transform Pecked();
    void Released();
    IPeckable GetThis();
    int Type();
    int ID();
}