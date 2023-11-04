using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
public GameObject _player;
public GameObject _mainCamera;

// Start is called before the first frame update
    void Awake()
    {
     _player = GameObject.Find("Player");
    _mainCamera = GameObject.Find("Main Camera");
    }

// Update is called once per frame
    void Update()
    {
      transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y+86,_player.transform.position.z-2);
      transform.rotation = Quaternion.Euler( 90,_mainCamera.transform.localRotation.eulerAngles.y,0); //rotacion de minimapa con la camara
      //transform.rotation = Quaternion.Euler( 90,_player.transform.localRotation.eulerAngles.y,0); //rotacion de minimapa con el personaje
    }
}
