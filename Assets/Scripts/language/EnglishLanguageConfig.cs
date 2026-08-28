using System.Collections;
using System.Collections.Generic;

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
        {"OK", "OK"},
        {"PrivacyPolicy", "Privacy Policy"},
        {"TermsofService", "Terms of Service"},
        {"SETTINGS", "SETTINGS"},
        //sceneitem
        {"LockLvTips", " Unlocks at level{0}"},
        {"ItemLimit", "Maximum {0} item uses per level!"},
        //网络
        {"RETRY", "RETRY"},
        {"NetworkStr", "Network connection lost. Please check your internet and try again."},       
         //评分
        {"EvaluationGamePanel_title", "Please share your rating:"},
        {"EvaluationGamePanel_EX", "Dear player, do you enjoy our game? We would greatly appreciate it if you could give us a 5‑star rating on the store. Thank you for your support!"},
        {"EvaluationGamePanel_btn", "Rate Us"},
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
        //引导
        {"Guide1Panel_ex", "Tap to Move"},
        {"Collect", "Collect"},
        {"Guide2Panel_title", "NEW REWARD"},
        {"Guide3Panel_ex", "All received value is shown here, and the balance can be cashed out."},
        {"Guide5Panel_ex", "Number Blocker\n The number shows how many snakes must leave before this blocker opens."},
        {"Guide6Panel_ex", "Black Hole\n Entering a black hole also counts as a successful escape."},  
        //tx
        {"TxPanel_myB", "My {0}"},
        {"TxPanel_levelText", "Pass Level"},
        {"TxPanel_ex1", "<color=#431422>{0}</color> The minimum {1} amount is <color=#431422>{2}</color>."},
        {"TxPanel_ex2", "You need <color=#431422>{0}</color> more."},
        {"TxPanel_ex3", "The higher the level, the more exchange amount!"},

        {"TxTipsPanel_FAQ", "FAQ"},
        {"TxFailedPanel_ex", "You can unlock {0}s after completing Stage {1}. {2} more stages to go."},
        {"TxFailedPanel_ex2", "{0} failed. Please try again later."},
        {"TxSucceedPanel_title", "{0} SUCCESSFUL"},
        {"TxSucceedPanel_ex", "The {0} request is being processed successfully. It is currently under review."},
        
        {"TxAccountPanel_title", "{0} ACCOUNT"},
        {"TxAccountPanel_account", "Account Number"},
        {"TxAccountPanel_accountEX", "Enter your Account"},
        {"TxAccountPanel_email", "Email"},
        {"TxAccountPanel_emailEX", "abcde@gmail.com"},
        {"TxAccountPanel_name", "Name"},
        {"TxAccountPanel_nameEX", "Enter your Name"},
        {"TxAccountPanel_phone", "CPF/CNPJ"},
        {"TxAccountPanel_phoneEX", "99999999999 or 99999999999999"},
        {"TxAccountPanel_nameError", "Name must be in English, without special characters, and must include both first and last name (example: John Doe)."},
        {"TxAccountPanel_phoneError", "Nomor telepon tidak valid."},
        {"TxAccountPanel_emailError", "Email tidak valid. Masukkan alamat yang benar (contoh: user@domain.com)."},
        {"TxAccountPanel_accountError", "account Error."},

        //records
        {"TxHistoryPanel_Title", "RECORDS"},
        {"WDLStatus_REVIEWING", "Requested"},
        {"WDLStatus_PAYING", "Paying"},
        {"WDLStatus_SUCCESS", "Successful"},
        {"WDLStatus_REJECTED", "Rejected"},
        {"WDLStatus_FAILED", "Failed"},
        {"Month_1", "Jan"},
        {"Month_2", "Feb"},
        {"Month_3", "Mar"},
        {"Month_4", "Apr"},
        {"Month_5", "May"},
        {"Month_6", "Jun"},
        {"Month_7", "Jul"},
        {"Month_8", "Aug"},
        {"Month_9", "Sep"},
        {"Month_10", "Oct"},
        {"Month_11", "Nov"},
        {"Month_12", "Dec"},

        {"500", "System error. Please try again later."},
        {"1001", "Invalid request information. Please check your input."},
        {"1002", "App configuration error. Please try again later."},
        {"2001", "Player not found. Please try again later."},
        {"3001", "Insufficient balance."},
        {"3002", "Invalid payee name."},
        {"3003", "Invalid phone number."},
        {"3004", "Invalid email address."},
        {"3005", "A order is already in progress."},
        //-------
        {"Special_Diamond_unit", "JA=="},//特殊钻石符号$
        {"cht", "Y2FzaCBvdXQ="},//cash out
        {"Ch", "Q2FzaA=="},//Cash 
        {"CH", "Q0FTSA=="},//CASH
        {"WD", "V0lUSERSQVc="}, //WITHDRAW
        {"Wd", "V2l0aGRyYXc="}, //Withdraw
        {"wd", "d2l0aGRyYXc="}, //withdraw
        {"WH", "V0lUSERSQVdBTA=="},//WITHDRAWAL 
        {"Wh", "V2l0aGRyYXdhbA=="},//Withdrawal 
        {"wh", "d2l0aGRyYXdhbA=="},//withdrawal
        {"Bl", "QmFsYW5jZQ=="},//Balance    
        {"Pm", "UGF5bWF4"},//Paymax    
    };
}
