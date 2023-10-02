using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Chase : Enemy
{
    [SerializeField]
    private GameObject detect;
    public bool onDetect;

    void Start()
    {
        rigid.velocity = moveVec;
        detect.SetActive(true);
    }

    void Update()
    {
        Detect();
    }

    void Detect()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //this.transform.parent.GetComponent < 부모의 스크립트 이름> ().OnTriggerEnter(this.gameObject, collider.gameObject);
    }
}