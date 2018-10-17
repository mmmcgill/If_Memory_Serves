using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmpersandController : PlayerController {


    private StarController star;
    private Animator animAddy;
    float speedAddy;
    bool facingright;
    private Rigidbody rbAddy;

    override public void Start() {
    base.Start();
    this.Type = "R";

        star = GameObject.Find("/players/star").GetComponent<StarController>();
        levelController = GameObject.Find("/TheLevel").GetComponent<LevelController>();
        animAddy = GetComponent<Animator>();
        facingright = true;
        //rbAddy = GetComponent<Rigidbody>();

    otherPlayer = star;
  }

    override public void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        animAddy.SetFloat("speed", Mathf.Abs(moveHorizontal));

        //rbAddy.velocity = new Vector2(moveHorizontal, moveVertical) * speed;

        if (moveHorizontal > 0 && !facingright) {
            flip();
        }
        else if (moveHorizontal<0 && facingright){
            flip();
        }
    }

     void flip(){
        facingright = !facingright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    //    base.Update();
    //
    ////    // Emit feeler pointer on left-click.
    ////    if (Input.GetMouseButtonDown(0)) {
    ////      if (caster != null) {
    ////        StopCoroutine(caster);
    ////      }
    ////      caster = StartCoroutine(CastPointer());
    ////    }
    ////
    ////    // Cancel pointer on right-click.
    ////    if (Input.GetMouseButtonDown(1)) {
    ////      Depoint();
    ////    }
    //
    //
    //
    //    if (Input.GetButtonDown("Attach")) {
    //      Interact(true);
    //    } 
    //  }


    override protected void Jump (){
  }



//  IEnumerator CastPointer() {
//    lineRenderer.enabled = true;
//    leftBarbRenderer.enabled = true;
//    rightBarbRenderer.enabled = true;
//
//    Vector3 mousePixels = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
//    Vector2 to = Camera.main.ScreenToWorldPoint(mousePixels);
//    Vector2 from = transform.position;
//    float maximumLength = Vector2.Distance(from, to);
//    Vector2 diff = to - from;
//    diff.Normalize();
//
//    float startTime = Time.time;
//    float elapsedTime = 0.0f;
//    float targetTime = 0.5f;
//    bool isHit = false;
//    targetCell = null;
//
//    RaycastHit2D hit;
//    while (elapsedTime < targetTime && !isHit) {
//      float proportion = elapsedTime / targetTime;
//      float length = proportion * maximumLength;
//      hit = Physics2D.Raycast(from, diff, length, Utilities.GROUND_MASK);
//      if (hit.collider != null) {
//        PointAt(hit.point);
//        GameObject hitObject = hit.collider.gameObject;
//        if (hitObject.tag == "pointer") {
//          Transform hitParent = hitObject.transform.parent;
//          // is it part of a linked cell?
//          if (hitParent != null && hitParent.gameObject.tag == "linkedCell") {
//            targetCell = hitParent.GetComponentInChildren<CellController>();
//          }
//        }
//        else if (hitObject.tag == "cell") {
//          targetCell = hitObject.GetComponent<CellController>();
//        }
//
//        targetPosition = hit.point;
//        isHit = true;
//      } else {
//        PointAt(from + diff * length);
//      }
//      yield return null;
//      elapsedTime = Time.time - startTime;
//    }
//
//    lineRenderer.enabled = targetCell != null;
//    leftBarbRenderer.enabled = targetCell != null;
//    rightBarbRenderer.enabled = targetCell != null;
//    caster = null;
//  }

//  public CellController Target {
//    get {
//      return targetCell;
//    }
//  }

}
