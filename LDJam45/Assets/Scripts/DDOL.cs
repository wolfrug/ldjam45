using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    // This makes the game object this component is attached to not get destroyed on load!
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
