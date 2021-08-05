using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    Transform NextAnchor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToNextAnchor(Transform anchor)
    {
        transform.DOMove(anchor.position, 2.0f);
    }
}
