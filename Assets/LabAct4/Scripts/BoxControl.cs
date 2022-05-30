using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    public int point;
    // Akan dipanggil ketika objek lain ber-collider (bola) bersentuhan dengan box.
    void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        // Jika objek tersebut bernama "Ball":
        if (anotherCollider.name == "Ball" )
        {
            BallControl ball = anotherCollider.gameObject.GetComponent<BallControl>();
            Debug.Log("Box Collide with Ball "+ point);
            if(ball.player != null){
                ball.AddPlayerScore(point);
                Destroy(gameObject);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
