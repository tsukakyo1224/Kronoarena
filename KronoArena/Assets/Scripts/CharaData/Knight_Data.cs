
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
    public static float SkillTime1;
    public static float SkillTime2;

    //持続時間
    public static float Skill1_Limit;
    public static float Skill2_Limit;

    //攻撃までの時間テキスト
    public static GameObject ATText1;
    public static GameObject ATText2;
    public static GameObject ATText3;

    //攻撃したかのフラグ
    public static bool SkillFlag1;
    public static bool SkillFlag2;
    public static bool AttackFlag;

    //持続時間フラグ
    public static bool LimitFlag1;
    public static bool LimitFlag2;

    //エフェクト用フラグ
    public static bool EffectFlag;

    public static Slider YourHP;

    //アニメーター 
    private Animator animator;

    private PhotonView photonView;

    //ナイトのエフェクト
    [SerializeField] private static GameObject Skill1_Set;
    [SerializeField] private static GameObject Skill1;
    [SerializeField] private static GameObject Skill2_Set;
    [SerializeField] private static GameObject Skill2;


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
        Skill1_Limit = 3.0f;
        Skill2_Limit = 10.0f;
        SkillFlag1 = false;
        SkillFlag2 = false;
        LimitFlag1 = false;
        LimitFlag2 = false;

        AttackFlag = false;

        EffectFlag = false;

        animator = this.GetComponent<Animator>();

        photonView = GetComponent<PhotonView>();

        //エフェクト呼び出し
        Skill1_Set = Resources.Load<GameObject>("Knight_RollSet");
        Skill1 = Resources.Load<GameObject>("Knight_Roll");
        Skill2_Set = Resources.Load<GameObject>("Knight_BuffSet");
        Skill2 = Resources.Load<GameObject>("Knight_Buff");

    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.isMine)
        {
            //スキル1発動
            if (SkillFlag1 == true && SkillFlag2 == false)
            {
                //スキル1時間減少
                SkillTime1 -= Time.deltaTime;

                //待機エフェクト発動
                if (EffectFlag == false)
                {
                    photonView.RPC("Knight_Effect", PhotonTargets.All, 1);
                }

                //スキル1時間が0になったら発動
                if (SkillTime1 <= 0)
                {

                    //エフェクト発動
                    photonView.RPC("Knight_Effect", PhotonTargets.All, 2);

                    this.GetComponent<Status>().Attack += 100.0f;
                    //回転攻撃
                    photonView.RPC("RollDamage", PhotonTargets.All);
                    SkillFlag1 = false;
                    SkillTime1 = 20.0f;
                    //持続時間フラグをオン
                    LimitFlag1 = true;
                    Debug.Log("ナイトの回転攻撃!");
                }
            }

            //スキル1の持続時間が終わるまで
            if (LimitFlag1 == true)
            {
                Skill1_Limit -= Time.deltaTime;
                if (Skill1_Limit <= 0)
                {
                    this.GetComponent<Status>().Attack -= 100.0f;
                    LimitFlag1 = false;
                    Skill1_Limit = 3.0f;
                }

            }

            //スキル2発動
            if (SkillFlag2 == true && SkillFlag1 == false)
            {
                //スキル2時間減少
                SkillTime2 -= Time.deltaTime;

                //待機エフェクト発動
                if (EffectFlag == false)
                {
                    photonView.RPC("Knight_Effect", PhotonTargets.All, 3);
                }

                //スキル2時間が0になったら発動
                if (SkillTime2 <= 0)
                {
                    photonView.RPC("Knight", PhotonTargets.All, 4);
                    animator.SetBool("Skill2_Trigger", true);
                    SkillFlag2 = false;
                    SkillTime2 = 10.0f;
                    this.GetComponent<Status>().Attack += 300.0f;
                    LimitFlag2 = true;   //持続時間フラグをオン
                    Debug.Log("ナイトの攻撃力が300UP!");
                }
            }
            //スキル2の持続時間が終わるまで
            if (LimitFlag2 == true)
            {
                Skill2_Limit -= Time.deltaTime;
                if (Skill2_Limit <= 0)
                {
                    this.GetComponent<Status>().Attack -= 300.0f;
                    LimitFlag2 = false;
                    Skill2_Limit = 10.0f;
                    Debug.Log("ナイトの攻撃力が元に戻った");
                }

            }
        }
    }

    //ナイトのエフェクト
    [PunRPC]
    public void Knight_Effect(int num)
    {
        if (num == 1)
        {
            animator.SetBool("Skill1_Trigger", false);
            animator.SetBool("Skill1", true);
            var instantiateEffect = GameObject.Instantiate(Skill1_Set, this.transform.position, Quaternion.identity) as GameObject;
            if ((PhotonNetwork.player.ID == 1 && this.tag == "Player1") ||
                PhotonNetwork.player.ID == 2 && this.tag == "Player2")
            {
                EffectFlag = true;
            }
        }

        else if (num == 2)
        {
            animator.SetBool("Skill1", false);
            animator.SetBool("Skill1_Trigger", true);
            var instantiateEffect = GameObject.Instantiate(Skill1, this.transform.position, Quaternion.identity) as GameObject;
            if ((PhotonNetwork.player.ID == 1 && this.tag == "Player1") ||
                    PhotonNetwork.player.ID == 2 && this.tag == "Player2")
            {
                EffectFlag = false;
            }
        }
        else if (num == 3)
        {
            animator.SetBool("Skill2_Trigger", false);
            animator.SetBool("Skill2", true);
            var instantiateEffect = GameObject.Instantiate(Skill2_Set, this.transform.position, Quaternion.identity) as GameObject;
            if ((PhotonNetwork.player.ID == 1 && this.tag == "Player1") ||
                PhotonNetwork.player.ID == 2 && this.tag == "Player2")
            {
                EffectFlag = true;
            }
        }
        else if (num == 4)
        {
            animator.SetBool("Skill2", false);
            animator.SetBool("Skill2_Trigger", true);
            var instantiateEffect = GameObject.Instantiate(Skill2, this.transform.position, Quaternion.identity) as GameObject;
            if ((PhotonNetwork.player.ID == 1 && this.tag == "Player1") ||
                    PhotonNetwork.player.ID == 2 && this.tag == "Player2")
            {
                EffectFlag = false;
            }
        }
    }

    //スキル2(回転攻撃)
    [PunRPC]
    public void RollDamage()
    {
        //回転攻撃判定
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player2");
        if (this.tag == "Player2")
        {
            targets = GameObject.FindGameObjectsWithTag("Player1");
        }
        foreach (GameObject obj in targets)
        {
            // 対象となるGameObjectとの距離を調べ、近くだったら何らかの処理をする
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            //対象キャラとの距離表示
            Debug.Log(obj.name + "との距離は" + dist + "m");
            //3m以下なら体力攻撃判定
            if (dist < 3)
            {
                //ダメージを与える
                obj.GetComponent<Status>().HP -=
                (int)(this.GetComponent<Status>().Attack / ((1 + obj.GetComponent<Status>().Defense) / 10));

                Debug.Log(this.name + "が" + obj + "に" + (int)(this.GetComponent<Status>().Attack /
                ((1 + obj.GetComponent<Status>().Defense) / 10)) + "ダメージ");
            }
        }

    }

    //通常攻撃
    public void Damage()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player2");
        if(this.tag == "Player2")
        {
            targets = GameObject.FindGameObjectsWithTag("Player1");
        }
        foreach (GameObject obj in targets)
        {
            // 対象となるGameObjectとの距離を調べ、近くだったら何らかの処理をする
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            //対象キャラとの距離表示
            if (dist < 2.0 && obj.tag != this.tag)
            {
                Guardian();
                if (AttackFlag == false)
                {
                    Vector3 eyeDir = this.transform.forward; // プレイヤーの視線ベクトル。
                    Vector3 playerPos = this.transform.position; // プレイヤーの位置
                    Vector3 enemyPos = obj.transform.position; // 敵の位置

                    float angle = 30.0f;    //攻撃範囲内の角度

                    // プレイヤーと敵を結ぶ線と視線の角度差がangle以内なら当たり
                    if (Vector3.Angle((enemyPos - playerPos).normalized, eyeDir) <= angle)
                    {
                        //Debug.Log(obj.name);
                        //ダメージを与える
                        obj.GetComponent<Status>().HP -=
                        (int)(this.GetComponent<Status>().Attack / ((1 + obj.GetComponent<Status>().Defense) / 10));

                        Debug.Log(this.name + "が" + obj + "に" + (int)(this.GetComponent<Status>().Attack /
                        ((1 + obj.GetComponent<Status>().Defense) / 10)) + "ダメージ");
                    }
                }

                AttackFlag = false;
            }
        }


    }


    //周りにガーディアンがいて、ガーディアンが身代わりをしていたらガーディアンに攻撃
    void Guardian()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player2");
        if (this.tag == "Player2")
        {
            targets = GameObject.FindGameObjectsWithTag("Player1");
        }
        foreach (GameObject obj in targets)
        {
            // 対象となるGameObjectとの距離を調べ、近くだったら何らかの処理をする
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            //対象キャラとの距離表示
            if(obj.GetComponent<Status>().Name == "Guardian" && dist < 2.0)
            {
                if (obj.GetComponent<Guardian_Data>().GuardFlag == true)
                {
                    obj.GetComponent<Status>().HP -=
                    (int)(this.GetComponent<Status>().Attack / ((1 + obj.GetComponent<Status>().Defense) / 10));
                    AttackFlag = true;
                }
            }
        }
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
