﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ナイトのデータ一覧
public class Knight_Data : MonoBehaviour
{
    //キャラクター名
    public static string CharaName;
    //キャラクタジョブ番号
    public static int job;
    //キャラクターアイコン
    public static Sprite CharaIconImage;
    //ジョブアイコン
    public static Sprite JobIconImage;
    //ミニキャラクターアイコン
    public static Sprite MiniIcon;
    //通常攻撃アイコン
    public static Sprite AttackIcon;
    //スキル攻撃アイコン1
    public static Sprite SkillIcon1;
    //スキル攻撃アイコン2
    public static Sprite SkillIcon2;

    //攻撃までの時間
    public static float AttackTime;
    public static float SkillTime1;
    public static float SkillTime2;

    //攻撃までの時間テキスト
    public static GameObject ATText1;
    public static GameObject ATText2;
    public static GameObject ATText3;

    //攻撃したかのフラグ
    public static bool AttackFlag;
    public static bool SkillFlag1;
    public static bool SkillFlag2;

    //


    public static Collider Sword;

    public static Slider YourHP;

    //アニメーター 
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //キャラクター情報入力
        CharaName = "ナイト";
        CharaIconImage = Resources.Load<Sprite>("CharaIcon/CharaIcon1");
        MiniIcon = Resources.Load<Sprite>("MiniCharaIcon/MiniIcon1");
        JobIconImage = Resources.Load<Sprite>("JobIcon/knite");
        AttackIcon = Resources.Load<Sprite>("AttackIcon/AttackIcon1");
        SkillIcon1 = Resources.Load<Sprite>("AttackIcon/KnightSkillIcon1");
        SkillIcon2 = Resources.Load<Sprite>("AttackIcon/KnightSkillIcon2");
        SkillTime1 = 20.0f;
        SkillTime2 = 10.0f;
        AttackFlag = false;
        SkillFlag1 = false;
        SkillFlag2 = false;
        Sword = GameObject.Find("Sword_Collider").GetComponent<BoxCollider>();
        animator = this.GetComponent<Animator>();
        //剣コライダーをオンにする
        Sword.enabled = false;


        ATText2 = GameObject.Find("ATime2");
        ATText3 = GameObject.Find("ATime3");


    }

    // Update is called once per frame
    void Update()
    {

        //スキル1発動
        if (SkillFlag1 == true && SkillFlag2 == false)
        {
            //スキル1時間減少
            SkillTime1 -= Time.deltaTime;
            //スキル1時間が0になったら発動
            if (SkillTime1 <= 0)
            {
                animator.SetBool("Skill1_Trigger", true);
                SkillFlag1 = false;
                SkillTime1 = 20.0f;
            }
        }

        //スキル2発動
        if (SkillFlag2 == true && SkillFlag1 == false)
        {
            //スキル2時間減少
            SkillTime2 -= Time.deltaTime;
            //スキル2時間が0になったら発動
            if (SkillTime2 <= 0)
            {
                animator.SetBool("Skill2_Trigger", true);
                SkillFlag2 = false;
                SkillTime2 = 10.0f;
            }
        }

        if(AttackFlag == true)
        {
            Sword.enabled = true;
            //一定時間後にコライダーの機能をオフにする
            Invoke("ColliderReset", 1.0f);
        }
    }

    //ダメージ計算
    void OnTriggerExit(Collider other)
    {
        //if ((PhotonNetwork.player.ID == 1 && other.tag == "Player2") ||
        //    (PhotonNetwork.player.ID == 2 && other.tag == "Player1"))
        //{
            //other.GetComponent<Status>().hpSlider.value -=
            other.GetComponent<Status>().HP -=
                    (int)(this.GetComponent<Status>().Attack / ((1 + other.GetComponent<Status>().Defense) / 10));
            //other.GetComponent<Status>().hpSlider.value = other.GetComponent<Status>().HP;
            //Debug.Log(other + "に" + (int)(this.GetComponent<Status>().Attack /
            //    ((1 + other.GetComponent<Status>().Defense) / 10)) + "ダメージ");

        //}
    }

    public void ColliderReset()
    {
        Sword.enabled = false;
        AttackFlag = false;
    }


    //名前とtagの送受信
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //データの送信
            stream.SendNext(this.name);
            stream.SendNext(this.tag);
        }
        else
        {
            //データの受信
            this.name = (string)stream.ReceiveNext();
            this.tag = (string)stream.ReceiveNext();
        }
    }


}
