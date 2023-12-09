using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 60f;
    public float KBfriction = 5f;
    private float speedEffectMultiplier = 1f, rotEffectMultiplier = 1f;
    public Vector2 knockbackForce;
    public Camera cam;
    public Rigidbody2D rb;
    public Transform R_H;

    private Vector2 move, mousePOS;

    private bool inKnockback = false, isRecoiling = false, rotateLocked = false;

    private void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        mousePOS = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate() {
        if (!inKnockback)
        {
            rb.velocity = move * moveSpeed * speedEffectMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = knockbackForce * 10 * Time.fixedDeltaTime;
            if (Mathf.Abs(knockbackForce.x) - KBfriction >= 0) knockbackForce.x -= KBfriction * (knockbackForce.x / Mathf.Abs(knockbackForce.x)); else knockbackForce.x = 0;
            if (Mathf.Abs(knockbackForce.y) - KBfriction >= 0) knockbackForce.y -= KBfriction * (knockbackForce.y / Mathf.Abs(knockbackForce.y)); else knockbackForce.y = 0;

            if (knockbackForce.x == 0 && knockbackForce.y == 0) inKnockback = false;
        }

        Vector2 lookDir = mousePOS - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if (!rotateLocked) transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + 20f * rotEffectMultiplier * Vector3.Cross(transform.up, lookDir).z, new Vector3(0, 0, 1));

        Vector2 R_lookDir = mousePOS - (Vector2)R_H.position;
        float value = R_H.localEulerAngles.z + 14f * rotEffectMultiplier * Vector3.Cross(R_H.up, R_lookDir).z;
        if (value < 0) value += 360;
        if (value < 350f && value > 270) value = 350f; else if (value > 50f && value < 127f) value = 50f;
        R_H.localRotation = Quaternion.Euler(0, 0, value);

    }

    public void knockback(List<float> vctr, float armRecoilFactor = 0f, float delay = 0f)
    {
        inKnockback = true;
        if (vctr.Count == 1)
        {
            knockbackForce = (rb.position - mousePOS).normalized * vctr[0];
        }
        else if (vctr.Count == 2)
        {
            knockbackForce = new Vector2(vctr[0], vctr[1]);
        } else
        {
            Debug.LogError("Index Error in 'knockback()' @ 'player.Movement'.");
        }
        if (armRecoilFactor > 0 && !isRecoiling) StartCoroutine(recoilRightH(armRecoilFactor, delay));
    }

    private IEnumerator recoilRightH(float Factor, float delay)
    {
        isRecoiling = true;
        float ClampedFactor = Mathf.Clamp(Factor, 0.5f, 1.4f);
        Vector3 originalPos = R_H.localPosition;
        float t = 0;
        while (R_H.localPosition != originalPos - 0.1F * ClampedFactor * (Vector3)Vector2.up)
        {
            R_H.localPosition = new Vector3(originalPos.x, Mathf.Lerp(originalPos.y, originalPos.y - 0.1F * ClampedFactor, t));
            t += 60f * Time.deltaTime;
            yield return null;
        }

        rotateLocked = true;

        GetComponent<Transform>().eulerAngles = new Vector3(0, 0, GetComponent<Transform>().eulerAngles.z - ClampedFactor*5f);
        yield return new WaitForSeconds(delay);

        rotateLocked = false;

        Vector3 recoiledPos = R_H.localPosition;
        t = 0;
        while (R_H.localPosition != originalPos)
        {
            R_H.localPosition = new Vector3(originalPos.x, Mathf.Lerp(recoiledPos.y, originalPos.y, t));
            t += 40f * Time.deltaTime;
            yield return null;
        }
        isRecoiling = false;
    }

    public void setSpeedMultiplier(float sped)
    {
        speedEffectMultiplier = sped;
        rotEffectMultiplier = sped / 2f;
    }
}
