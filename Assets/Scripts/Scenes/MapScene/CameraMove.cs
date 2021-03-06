using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]private Transform _target = null;

    void Update()
    {
        if (!_target) return;
        transform.Translate(
            _target.transform.position.x - this.transform.position.x,
            _target.transform.position.y - this.transform.position.y,0);
    }
}
