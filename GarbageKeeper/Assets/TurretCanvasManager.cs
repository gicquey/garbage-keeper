﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCanvasManager : MonoBehaviour
{
    public Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = initialRotation;
    }
}
