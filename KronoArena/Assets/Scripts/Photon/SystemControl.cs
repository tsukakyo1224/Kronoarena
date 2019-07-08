﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemControl : MonoBehaviour
{
    private PhotonView _PhotonViewControl;
    //PhotonのRPCを使うときのコントローラ。こいつを使って相手に値を送る。

    private int[] sendData = { 123, 666666, 2121212 };//送るデータ
    private int[] ReciveData = { 0, 0, 0 };//受信データ




    private void Awake()
    { //初期化

        _PhotonViewControl = GetComponent<PhotonView>();
        //Inspectorに突っ込んでおいたComponentからPhotonViewを引っ張りだす
    }

    public void Send_ToOthers()
    {//送信メソッド

        _PhotonViewControl.RPC("SendFunc", PhotonTargets.Others, sendData);
        //RPC([使うメソッドの名前],[メソッドを発動させる対象],[メソッドの引数に突っ込む値])
    }

    [PunRPC]//RPCで呼び出したいメソッドは、メソッドの前にこいつを必ず書いとく
    private void SendFunc(int[] SendedData)
    {//相手が送ってきたときに自動的に発動

        ReciveData = SendedData;
        //相手から来たデータを自分の受け皿に上書き保存

    }




}//end this class