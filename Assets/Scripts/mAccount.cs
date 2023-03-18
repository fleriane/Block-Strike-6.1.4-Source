using System;
using UnityEngine;

public class mAccount : MonoBehaviour
{
    public CryptoString WebClientID;

    public static string Name;

#if UNITY_EDITOR
    public static string gmail;
#endif

    private void Start()
    {
        if (AccountManager.isConnect)
        {
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            TimerManager.In(0.2f, delegate
            {
                if (mPopUp.activePopup)
                {
                    mPopUp.HideAll("Menu");
                }
                AndroidGoogleSignIn.SignIn(this.WebClientID, delegate
                {
                    this.Start2();
                }, delegate (string error)
                {
                    mPopUp.ShowPopup(Localization.Get("Error") + ": " + Localization.Get("Your device is not found a Google Account"), "Account", Localization.Get("Exit"), new Action(this.Exit), Localization.Get("Retry"), new Action(this.Start));
                });
            });
        }
        else
        {
#if UNITY_EDITOR
            gmail = PlayerPrefs.GetString("EditorGmail", "tibers28h@gmail*com");
            this.Start2();
#else
            this.Start2();
#endif
        }
    }

    private void Start2()
    {
#if UNITY_EDITOR
        TimerManager.In(0.1f, delegate { mPopUp.SetActiveWait(true, Localization.Get("Connect to the account", true) + ": " + gmail.Remove(gmail.LastIndexOf("@"))); });
        TimerManager.In(0.3f, delegate { AccountManager.Login(gmail, new Action<bool>(this.Login), new Action<string>(this.LoginError)); });
        return;
#endif
#if UNITY_STANDALONE_WIN
        mPopUp.SetActiveWait(true, Localization.Get("Connect to the account", true) + ": " + "v*kozlov2010");
        TimerManager.In(0.3f, delegate { AccountManager.Login("v*kozlov2010@gmail*com", new Action<bool>(this.Login), new Action<string>(this.LoginError)); });
        return;
#endif
        string email = AndroidGoogleSignIn.Account.Email;
        email = email.Replace(".", "*");
        if (string.IsNullOrEmpty(email))
        {
            mPopUp.ShowPopup(Localization.Get("Error") + ": " + Localization.Get("Your device is not found a Google Account"), "Account", Localization.Get("Exit"), new Action(this.Exit), Localization.Get("Retry"), new Action(this.Start));
            return;
        }
        mPopUp.SetActiveWait(true, Localization.Get("Connect to the account") + ": " + email.Remove(email.LastIndexOf("@")));
        TimerManager.In(0.6f, delegate ()
        {
            AccountManager.Login(email, new Action<bool>(this.Login), new Action<string>(this.LoginError));
        });
    }

    private void Login(bool isCreated)
    {
        if (isCreated)
        {
            mPopUp.SetActiveWait(false);
            EventManager.Dispatch("AccountUpdate");
            WeaponManager.UpdateData();
            if (string.IsNullOrEmpty(AccountManager.instance.Data.AccountName) || AccountManager.instance.Data.AccountName.ToString() == " ")
            {
                this.SetPlayerName(delegate (string playerName)
                {
                    AccountManager.UpdateName(playerName, new Action<string>(this.SetPlayerNameComplete), new Action<string>(this.SetPlayerNameError));
                    mPopUp.ShowText(Localization.Get("Please wait", true) + "...");
                });
                return;
            }
        }
        else
        {
            mPopUp.SetActiveWait(false);
            mPopUp.ShowText(Localization.Get("Please wait", true) + "...");
            this.SetPlayerName(delegate (string playerName)
            {
                AccountManager.Register(playerName, new Action(this.RegisterComplete), new Action<string>(this.RegisterError));
                mPopUp.ShowText(Localization.Get("Please wait", true) + "...");
            });
        }
    }

    private void LoginError(string error)
    {
        mPopUp.ShowPopup(Localization.Get("Error", true) + ": " + error, "Account", Localization.Get("Retry", true), new Action(this.LoginErrorTry), Localization.Get("Exit", true), new Action(this.Exit));
        mPopUp.SetActiveWait(false);
    }

    private void LoginErrorTry()
    {
        if (mPopUp.activePopup)
        {
            mPopUp.HideAll("Menu");
        }
        AccountManager.Login(AccountManager.AccountID, new Action<bool>(this.Login), new Action<string>(this.LoginError));
        string text = AccountManager.AccountID;
        text = text.Remove(text.LastIndexOf("@"));
        mPopUp.SetActiveWait(true, Localization.Get("Connect to the account", true) + ": " + text);
    }

    private void RegisterComplete()
    {
        EventManager.Dispatch("AccountUpdate");
        WeaponManager.UpdateData();
        mPopUp.HideAll("Menu");
    }

    private void RegisterError(string error)
    {
        mPopUp.ShowPopup(Localization.Get("Error", true) + ": " + error, "Account", Localization.Get("Retry", true), new Action(this.RegisterErrorTry), Localization.Get("Exit", true), new Action(this.Exit));
    }

    private void RegisterErrorTry()
    {
        if (mPopUp.activePopup)
        {
            mPopUp.HideAll("Menu");
        }
        this.SetPlayerName(delegate (string playerName)
        {
            AccountManager.Register(playerName, new Action(this.RegisterComplete), new Action<string>(this.RegisterError));
            mPopUp.ShowText(Localization.Get("Please wait", true) + "...");
        });
    }

    private void SetPlayerName(Action<string> action)
    {
        mPopUp.SetActiveWait(false);
        mPopUp.ShowInput(string.Empty, Localization.Get("ChangeName", true), 12, UIInput.KeyboardType.Default, new Action(this.SetPlayerNameSubmit), new Action(this.SetPlayerNameChange), "Ok", delegate
        {
            this.SetPlayerNameSave(action);
        }, Localization.Get("Back", true), null);
    }

    private void SetPlayerNameSave(Action<string> action)
    {
        string text = NGUIText.StripSymbols(mPopUp.GetInputText());
        string text2 = mChangeName.UpdateSymbols(text, true);
        if (text != text2)
        {
            mPopUp.SetInputText(text2);
            return;
        }
        if (text.Length <= 3 || text == "Null" || text[0].ToString() == " " || text[text.Length - 1].ToString() == " ")
        {
            text = "Player " + UnityEngine.Random.Range(0, 99999);
            mPopUp.SetInputText(text);
            return;
        }
        if (action != null)
        {
            action(text);
        }
    }

    private void SetPlayerNameSubmit()
    {
        string text = mPopUp.GetInputText();
        if (text.Length <= 3 || text == "Null" || text[0].ToString() == " " || text[text.Length - 1].ToString() == " ")
        {
            text = "Player " + UnityEngine.Random.Range(0, 99999);
        }
        text = NGUIText.StripSymbols(text);
        mPopUp.SetInputText(text);
        mAccount.Name = text;
    }

    private void SetPlayerNameChange()
    {
        string inputText = mPopUp.GetInputText();
        string text = mChangeName.UpdateSymbols(inputText, true);
        if (inputText != text)
        {
            mPopUp.SetInputText(text);
        }
    }

    private void SetPlayerNameComplete(string playerName)
    {
        EventManager.Dispatch("AccountUpdate");
        WeaponManager.UpdateData();
        mPopUp.HideAll("Menu");
    }

    private void SetPlayerNameError(string error)
    {
        this.SetPlayerName(delegate (string playerName)
        {
            AccountManager.UpdateName(playerName, new Action<string>(this.SetPlayerNameComplete), new Action<string>(this.SetPlayerNameError));
            mPopUp.ShowText(Localization.Get("Please wait", true) + "...");
        });
        string text = error;
        if (text == "Name already taken")
        {
            text = Localization.Get("Name already taken", true);
        }
        UIToast.Show(Localization.Get("Error", true) + ": " + text, 3f);
    }

    private void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
