using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    public bool isPlayer;
    private BoxCollider boxCollider;
    private Rigidbody rigidbody;
    public float force;
    public bool isGrounded;
    public bool isJump;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator C_Jump()
    {
        force = (float)Random.Range(850, 1350) / 100.0f;
        rigidbody.velocity = Vector3.up * force;
        isGrounded = false;
        yield return null;
        isGrounded = false;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //  rigidbody.AddForce(Vector3.up * force);
            force = (float)Random.Range(850, 1350) / 100.0f;
            rigidbody.velocity = Vector3.up * force;
        }
    }

    //private bool IsGrounded()
    //{
    //    return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (isPlayer && isJump)
        {
            force = (float)Random.Range(850, 1350) / 100.0f;
            rigidbody.velocity = Vector3.up * force;
        }

       

        isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
