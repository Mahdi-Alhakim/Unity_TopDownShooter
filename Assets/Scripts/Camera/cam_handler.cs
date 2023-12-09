using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_handler : MonoBehaviour
{
    private class shake { public float _mag, _dur; public shake(float mag, float dur) { _mag = mag; _dur = dur; } }
    List<shake> shakes;

    public float camSize = 3f;
    public float camFollowSpeed = 13f;
    public float shootZoomSpeed = 0.5f;
    public float maxZoomIn = 0.3f;
    public float zoomOutSpeed = 0.2f;
    public Transform player;
    private float ShakeMag = 0.0f, zoom;
    private bool isZooming = false;
    private Vector2 mousePos;
    private Vector2 target;

    private void Start()
    {
        shakes = new List<shake>();
        zoom = camSize;
    }

    public void AddShake(float mag, float duration)
    {
        shake newShake = new shake(mag, duration);
        shakes.Add(newShake);
    }

    private void handleShakes()
    {
        ShakeMag = 0.0f;

        for (int S = 0; S < shakes.Count; S++)
        {
            if (shakes[S]._dur <= 0.0f)
            {
                shakes.Remove(shakes[S]);
                continue;
            }
            shakes[S]._dur -= Time.deltaTime;
            ShakeMag += shakes[S]._mag;
        }
        float x = Random.Range(-1f, 1f) * ShakeMag;
        float y = Random.Range(-1f, 1f) * ShakeMag;
        transform.position += new Vector3(x, y, 0);
    }

    private IEnumerator handleZoom()
    {
        if (isZooming) 
        {
            if (zoom - shootZoomSpeed > camSize - maxZoomIn)
                zoom -= shootZoomSpeed;
            else zoom = camSize - maxZoomIn;
        } else if (zoom != camSize)
        {
            float zoomOut = (camSize - zoom) / Mathf.Abs(camSize - zoom) * zoomOutSpeed;
            if (zoom + zoomOut <= camSize)
                zoom += zoomOut;
            else zoom = camSize;
        }
        GetComponent<Camera>().orthographicSize = zoom; //Add condition to check if update is needed before <<--
        yield return null;
    }

    public void applyShootZoom(float wpnZoomFactor, bool impulse = false)
    {
        shootZoomSpeed = wpnZoomFactor;
        isZooming = true;
        if (impulse) Invoke("resetZoom", 0.2f);
    }

    public void resetZoom()
    {
        isZooming = false;
    }

    void Update()
    {
        if (player != null)
        {
            mousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            target = (Vector2)player.position + (mousePos - (Vector2)player.position).normalized * Mathf.Clamp(Vector2.Distance(player.position, mousePos) / 4, 0f, 2.5f);
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector2 camMoveDir = (target - (Vector2)transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, target);
            transform.position = transform.position + (Vector3)camMoveDir * camFollowSpeed * distanceToPlayer * Time.fixedDeltaTime / 2.5f;
        }
        if (shakes.Count > 0)
            handleShakes();
        StartCoroutine(handleZoom());
    }

}