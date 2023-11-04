using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaNoche : MonoBehaviour
{
    [SerializeField] private float _velocidad = 10;
    
    void Update()
    {
        transform.Rotate(_velocidad * Time.deltaTime,0,0);
    }
}
