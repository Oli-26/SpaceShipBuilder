using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public string orientation = "forward";
    public Vector3 target = new Vector3(0, 0 ,0);

    public float cooldown = 0.1f;
    private float _cooldown = 0f;

    Transform _transform;
    Camera mainCamera;
    void Start()
    {
        _transform = transform;
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space) && _cooldown <= 0){
            Shoot();
            _cooldown = cooldown;
        }

        _cooldown -= Time.deltaTime;
 
 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mousePos.x = mousePos.x - _transform.position.x;
        mousePos.y = mousePos.y - _transform.position.y;
        Debug.Log(mousePos);
 
         float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
        _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, _transform.position, Quaternion.identity);
        Destroy(bullet, 4f);
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        bullet.GetComponent<Rigidbody2D>().AddForce(15 * Vector3.Normalize(targetPosition - _transform.position), ForceMode2D.Force);
        bullet.GetComponent<Rigidbody2D>().AddForce(_transform.parent.parent.GetComponent<Rigidbody2D>().velocity, ForceMode2D.Force);
    }
}
