using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInstance : MonoBehaviour
{
    public static Canvas instance;
    private void Awake()
    {
        instance = GetComponent<Canvas>();
    }
}
