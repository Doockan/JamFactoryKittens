using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimController : MonoBehaviour
{

    private Animator anim;


    public void Start()
    {
        anim = gameObject.GetComponent<Animator>();


    }
    // Start is called before the first frame update

    public void OnMouseDown()
    {
        anim.SetTrigger("Brrr");

    }


}
