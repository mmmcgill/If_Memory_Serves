using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addycontroller : MonoBehaviour {

    public Animator animAddy;
    public float speedAddy = 10f;
    bool facingright=true;
    public Rigidbody2D rbAddy;

    // Use this for initialization
    void Start () {
        animAddy = GetComponent<Animator>();
        facingright = true;
        rbAddy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * speedAddy, GetComponent<Rigidbody2D>().velocity.y);

        animAddy.SetFloat("speed", Mathf.Abs(moveHorizontal));

        //rbAddy.velocity = new Vector2(moveHorizontal, moveVertical) * speedAddy;

        if (moveHorizontal > 0 && !facingright) {
           flip();
        }
        else if (moveHorizontal<0 && facingright){
           flip();
        }
    }
    void flip()
    {
        facingright = !facingright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
