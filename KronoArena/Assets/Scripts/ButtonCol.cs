﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCol : MonoBehaviour
{

    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    private GameObject Player1;
    private GameObject Player2;
    private GameObject Player3;

    //選択した攻撃までの時間
    public static float Time;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.cameraflag == true)
        {
            if(PhotonNetwork.player.ID == 1)
            {
                Player1 = GameObject.Find("P1_Chara1");
                animator1 = Player1.GetComponent<Animator>();
                Player2 = GameObject.Find("P1_Chara2");
                animator2 = Player2.GetComponent<Animator>();
                Player3 = GameObject.Find("P1_Chara3");
                animator3 = Player3.GetComponent<Animator>();
            }
            else
            {
                Player1 = GameObject.Find("P2_Chara1");
                animator1 = Player1.GetComponent<Animator>();
                Player2 = GameObject.Find("P2_Chara2");
                animator2 = Player2.GetComponent<Animator>();
                Player3 = GameObject.Find("P2_Chara3");
                animator3 = Player3.GetComponent<Animator>();
            }
        }
    }

    public void TurnChange()
    {
        GameObject.Find("TurnCol").GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
        TurnCol.ChangeTurn();
        Debug.Log("button");
        //TimerScript.TotalTime = 5.0f;
    }

    //攻撃ボタン1
    public void Attack1()
    {
        if (ChangeChara.nowChara == 0)
        {
            animator1.SetBool("Attack", true);
            //剣コライダーをオンに
            Knight_Data.Sword.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 0.3f);
            GameManager.CharaAttackTime1.SetActive(true);
        }
        else if (ChangeChara.nowChara == 1)
        {
            animator2.SetBool("Attack", true);
            GameManager.CharaAttackTime2.SetActive(true);
        }
        else if (ChangeChara.nowChara == 2)
        {
            animator3.SetBool("Attack", true);
            GameManager.CharaAttackTime3.SetActive(true);
        }
        Debug.Log(ChangeChara.nowChara + " Attack");
    }

    //スキル1
    public void Special1()
    {
        if (ChangeChara.nowChara == 0)
        {
            if (Knight_Data.SkillFlag2 == false)
            {
                Knight_Data.SkillFlag1 = true;
                animator1.SetBool("Skill1", true);
            }
           //CharaData1.AttackFlag = true;
        }
        else if (ChangeChara.nowChara == 1)
        {
            animator2.SetBool("Skill1", true);
        }
        else if (ChangeChara.nowChara == 2)
        {
            animator3.SetBool("Skill1", true);
        }
        Debug.Log(ChangeChara.nowChara + " Special1");
    }
    //スキル2
    public void Special2()
    {
        if (ChangeChara.nowChara == 0)
        {
            if (Knight_Data.SkillFlag1 == false)
            {
                Knight_Data.SkillFlag2 = true;
                animator1.SetBool("Skill2", true);
            }

            //CharaData1.AttackFlag = true;
        }
        else if (ChangeChara.nowChara == 1)
        {
            animator2.SetBool("Skill2", true);
        }
        else if (ChangeChara.nowChara == 2)
        {
            animator3.SetBool("Skill2", true);
        }
        Debug.Log(ChangeChara.nowChara + " Special2");
    }


}
