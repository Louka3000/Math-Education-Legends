using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public const float scrollSpeed = 0.015f;

    private void Update() {
        transform.position = new Vector3(transform.position.x - (scrollSpeed * Time.deltaTime), 0, 0);
        if (transform.position.x < -38.4f) {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
