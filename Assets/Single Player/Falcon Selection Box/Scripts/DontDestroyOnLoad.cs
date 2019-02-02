//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;

namespace Falcon
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}