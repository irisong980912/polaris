using System;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private ThirdPersonPlayer _playerScript;
    private float _normalSpeed;
    private float _boostSpeed;

    private void Start()
    {
        _playerScript = GameObject.FindWithTag("|Player|").GetComponent<ThirdPersonPlayer>();
        _normalSpeed = _playerScript.speed;
        _boostSpeed = _normalSpeed * 3;
    }

    void Update()
    {
        if (_playerScript.stardust >= 3)
        {
            _playerScript.speed = Input.GetButton("Jump") ? _boostSpeed : _normalSpeed;
        }
    }
}
