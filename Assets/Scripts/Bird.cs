using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5;

    Vector2 _startPosition;
    Rigidbody2D _rigidbody2d;
    SpriteRenderer _spriteRenderer;

    void Awake() 
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidbody2d.position;
        _rigidbody2d.isKinematic = true;
    }

    void OnMouseDown() 
    {
        _spriteRenderer.color = Color.red;    
    }

    void OnMouseUp() 
    {
        Vector2 currentPosition = _rigidbody2d.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidbody2d.isKinematic = false;
        _rigidbody2d.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;    
    }

    void OnMouseDrag() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;


        float distance =  Vector2.Distance(desiredPosition, _startPosition);
        if (distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;

        _rigidbody2d.position = desiredPosition;

    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidbody2d.position = _startPosition;
        _rigidbody2d.isKinematic = true;
        _rigidbody2d.velocity = Vector2.zero;
    }
}
