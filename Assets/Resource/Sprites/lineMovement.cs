using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineMovement : MonoBehaviour {
    public int speed;
    public GameObject line;
    public GameObject bg;

    void Start() {
        Instantiate(line, bg.transform.position);
    }
    
    void Update() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        transform.Translate(Vector2.down * 965 / 1000 * speed * Time.deltaTime);

        if (transform.position.x < -5)
            Destroy(line);
    }
}
