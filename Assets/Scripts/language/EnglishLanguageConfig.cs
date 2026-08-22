using System.Collections;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;

public static class EnglishLanguageConfig
{
    public static Dictionary<string, string> currentTexts = new Dictionary<string, string>()
    {
        {"Loading", "Loading"},
        {"Level", "Level"},
        {"LEVEL", "LEVEL"},
        {"NoThanks", "No,Thanks"},
        {"CLAIM", "CLAIM"},
        {"Claim", "Claim"},
        {"ClaimAll", "Claim All"},
        {"CONTINUE", "CONTINUE"},
        {"Continue", "Continue"},
        {"RESET", "REPLAY"},
        {"PrivacyPolicy", "Privacy Policy"},
        {"TermsofService", "Terms of Service"},
        {"SETTINGS", "SETTINGS"},
        //sceneitem
        {"LockLvTips", " Unlocks at level{0}"},
        //网络
        {"RETRY", "RETRY"},
        {"NetworkStr", "Network connection lost. Please check your internet and try again."},       
         //评分
        {"EvaluationGamePanel_title1", "Are you enjoying the game?"},
        {"EvaluationGamePanel_btn1", "Not Really"},
        {"EvaluationGamePanel_btn2", "Love it!"},
        {"EvaluationGamePanel_btn3", "LATER"},
        {"EvaluationGamePanel_btn4", "5 STARS"},
        {"EvaluationGamePanel_title2", "Your 5 stars are very important to us.please give us 5 stars if you like it."},
        //tipsPanel
        {"ITEM", "ITEM"},
        {"NoItemHintTips", "No movable cards available!"},
        {"InsufficientDiamond", "Insufficient diamond!"},
        {"AdsNotReady", "The video is not ready,please try again later."},
        {"Limit", "Limit"},
        {"Free", "Free"},
        //addgameScene
        {"AddSceneItemPanel_ex1", "Automatically taps on 3 snakes."},
        {"AddSceneItemPanel_ex2", "Shows as a hint all snakes that can exit the board in 10 seconds."},
        //Daily Mission
        {"DAILYMISSION", "DAILY MISSION"},
        {"GO", "GO!"},
        {"SubtitleEx", "Watch videos and {0} instantly"},
        {"DailyMissionEx2", "Watch {0} videos and recevie {1} rewards ({2})"},
        {"DailyMissionEx3", "Next update time {0}"},
        //gamelose
        {"gameloseEx", "No lives remaining.\nLevel failed."},
        {"gamelosetitle", "DEFEAT"},
        {"Revive", "Revive"},
        {"Restart", "Restart"},
        //otherreward
        {"Extras", "Extras"},
        {"otherrewardEx", "Watch videos to increase your rewards"},
        {"otherrewardEx2", "Watch {0} more videos today to {1} {2}"},
        //-------
        {"Special_Diamond_unit", "JA=="},//特殊钻石符号$
        {"cht", "Y2FzaCBvdXQ="},//cash out
        {"Ch", "Q2FzaA=="},//Cash 
        {"CH", "Q0FTSA=="},//CASH
        {"WD", "V0lUSERSQVc="},
        {"wd", "d2l0aGRyYXc="},
        {"Wh", "V2l0aGRyYXdhbA=="},//Withdrawal 
        {"wh", "d2l0aGRyYXdhbA=="},
        {"pp", "cGF5cGFs"},//paypal
        {"Bl", "QmFsYW5jZQ=="},//Balance    
    };
}
