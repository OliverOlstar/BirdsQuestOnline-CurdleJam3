using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCamera : MonoBehaviour
{
    private RenderTexture texture;
    private Camera camera;

    [SerializeField] private Vector2Int resolution = new Vector2Int(384, 256);

    void Start()
    {
        texture = new RenderTexture(resolution.x, resolution.y, 24);
        camera = GetComponent<Camera>();

        camera.targetTexture = texture;
    }

    private void OnPostRender()
    {
        Camera.main.targetTexture = null;
        Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }

    private void OnPreRender()
    {
        Camera.main.targetTexture = texture;
    }
}
