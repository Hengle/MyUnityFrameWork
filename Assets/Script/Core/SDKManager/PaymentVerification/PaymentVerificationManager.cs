﻿using FrameWork.SDKManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 管理支付验证
/// </summary>
public class PaymentVerificationManager
{
    public static CallBack<int, string> onVerificationResultCallBack;

    private static PaymentVerificationInterface verificationInterface;
    public static void Init(PaymentVerificationInterface verificationInterface)
    {
        PaymentVerificationManager.verificationInterface = verificationInterface;
        verificationInterface.Init();
        SDKManager.PayCallBack += PayCallBack;
    }

    private static void PayCallBack(OnPayInfo info)
    {
        if (info.isSuccess)
        {
            verificationInterface.CheckRecipe(info);
        }
        else
        {
            Debug.LogWarning("PaymentVerificationManager PayCallBack=========" + info.goodsId);
            int code = info.isSuccess ? 0 : -1;
            OnVerificationResult(code, info.goodsId,false,info.receipt);
        }
    }
    /// <summary>
    /// 验证结果调用
    /// </summary>
    /// <param name="code">是否成功</param>
    /// <param name="goodsID">物品ID</param>
    /// <param name="repeatReceipt">是否是重复的订单凭据</param>
    /// <param name="receipt">回执，商户订单号等</param>
    public static void OnVerificationResult(int code,string goodsID, bool repeatReceipt,string receipt)
    {
        if (onVerificationResultCallBack != null)
        {
            Debug.LogWarning("验证回调code=========" + code + "-------goodsID=========" + goodsID);
            onVerificationResultCallBack(code, goodsID);
        }
       
        if (code==0 || repeatReceipt)
            SDKManager.ConfirmPay(goodsID, receipt);
        //验证成功
        if (code!=0)
        {
            Debug.LogError("凭据验证失败！ goodID:" + goodsID);
        }
    }
}


