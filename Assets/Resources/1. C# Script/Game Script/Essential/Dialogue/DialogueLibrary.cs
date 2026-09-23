using UnityEngine;
using System.Collections;

public class DialogueLibrary : MonoBehaviour
{
    public static DialogueLibrary Instance { get; private set; }
    private DialogueManager dm;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        dm = DialogueManager.Instance;
    }

    public IEnumerator PlayIntroSequence(){
        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Welcome");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Silence");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_DifficultTime");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_DifficultTime");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Debt");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_HowKnow");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Business");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Silence2");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_WhyHelp");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Yes");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_BeenThere");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_SecondChance");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_So");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_LendMoney");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Catch");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_OfCourse");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_NothingFree");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_NotToday");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FamilyFirst");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_RepaymentLater");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_ThankYou");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_DontThank");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_DontDisappear");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "Cutscene_System_TimeSkip");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_WelcomeBack");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FamilyWell");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Safe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_ThanksToYou");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Glad");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Now");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_AboutLoan");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_NoMoney");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_IKnow");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_BetterSolution");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_WorkForMe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_DoingWhat");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Bartending");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Bartending");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_HonestWork");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_GoodPay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_WarmCustomers");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FreeMeals");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FreeAccommodation");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_DebtPaid");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FreeToLeave");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_Family");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_FamilySafe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Motivation");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Cutscene_MC_NoChoice");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Cutscene_Boss_Wonderful");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayTutorialSequence(){
        GameManager.Instance.allowInteraction = true;
        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Oh");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_NewBartender");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Guess");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Hmm");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Cuter");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Uh");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Relax");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Teasing");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Name");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Training");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_NiceToMeet");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_IsIt");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Kidding");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_ComeOn");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_ShowAround");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_MoveLeftRight");
        yield return dm.WaitForMoveLeft(.2f);
        yield return dm.WaitForMoveRight(.2f);

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Nice");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_GetUsed");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_SecondHome");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Quiet");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Thinking");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Family");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Yeah");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_FinishDebt");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_SeeThem");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Simple");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Hope");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Hope");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Anyway");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Fun");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_MakingDrinks");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_OpenRecipeBook");
        yield return dm.WaitForInteraction("OpenRecipeBook");
        yield return dm.WaitForInteraction("CloseRecipeBook");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_EveryDrink");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_FollowExactly");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Consistency");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_DontCreative");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Hey");
        yield return dm.WaitForSpace();
        CustomerManager.Instance.SpawnCustomer(false);

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Interrupting");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_PerfectTiming");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_MeetMarco");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Nice");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_MostDays");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Hey2");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_LuckyMe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_CustomerClick");
        yield return dm.WaitForInteraction("CustomerClick");

        dm.PlayDialogue(SpeakerType.System, "System_AddSugarCube");
        yield return dm.WaitForInteraction("AddSugarCube");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Precision");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_AddBitters");
        yield return dm.WaitForInteraction("AddBitters");

        dm.PlayDialogue(SpeakerType.System, "System_CutChunkyIce");
        yield return dm.WaitForInteraction("CutIce");

        GameManager.Instance.allowInteraction = false;
        dm.PlayDialogue(SpeakerType.System, "System_AddIce");
        GameManager.Instance.allowInteraction = true;
        yield return dm.WaitForInteraction("AddIce");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Perfect");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_PourWhiskey");
        yield return dm.WaitForInteraction("PourWhiskey");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Spill");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Expensive");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_Stir");
        yield return dm.WaitForInteraction("Stir");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Circles");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "System_CutOrange");
        yield return dm.WaitForInteraction("CutOrange");

        GameManager.Instance.allowInteraction = false;
        dm.PlayDialogue(SpeakerType.System, "System_GarnishOrange");
        GameManager.Instance.allowInteraction = true;
        yield return dm.WaitForInteraction("GarnishOrange");

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_Presentation");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_LookingGood");
        yield return dm.WaitForSpace();

        GameManager.Instance.allowInteraction = false;
        dm.PlayDialogue(SpeakerType.System, "System_ServeDrink");
        GameManager.Instance.allowInteraction = true;
        yield return dm.WaitForInteraction("ServeDrink");

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Good");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Really");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Marco, "Tutorial_Marco_Another");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_See");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_Thanks");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_KeepDoing");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_BeforeYouKnow");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_DebtDisappear");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Tutorial_MC_AllIWant");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.Giovanna, "Tutorial_Giovanna_EveryoneSays");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift1Sequence(){
        dm.PlayDialogue(SpeakerType.System, "Shift1_System_NewDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_FirstDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_CanDoThis");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_JustBartending");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_WorkHard");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_PayOffDebt");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_GoHome");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift1_MC_AllThereIs");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift2Sequence(){
        dm.PlayDialogue(SpeakerType.System, "Shift2_System_NewDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_NotSoBad");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_NicerThanExpected");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_EveryDayLikeThat");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_ReallyCanPay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_WonderFamily");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift2_MC_EatingWell");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift3Sequence(){
        GameManager.Instance.AddMoney(1500);
        dm.PlayDialogue(SpeakerType.System, "Shift3_System_NewDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift3_MC_DebtBarelyMoved");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift3_MC_JustStarted");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift3_MC_GetBetter");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift3_MC_HasTo");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift3_MC_KeepWorking");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift4Sequence(){
        GameManager.Instance.AddMoney(9000);
        dm.PlayDialogue(SpeakerType.System, "Shift4_System_NewDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_FourDays");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_NothingFromThem");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_NoLetters");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_NoPhoneCalls");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_ShouldAskBoss");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift4_MC_ThereIsReason");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift5Sequence(){
        dm.PlayDialogue(SpeakerType.System, "Shift5_System_CustomerArrived");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_GoodEvening");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_Boss");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_FavoriteDrink");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_OfCourse");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShift5EndingSequence(){
        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Delicious");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Improved");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_ThankYou");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_Actually");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_AskSomething");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Certainly");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_MyDebt");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_WorkingEveryDay");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_DoesntGetSmaller");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_AndMyFamily");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_HaventSeenThem");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_SaidTheyBeSafe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_TheyWere");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_What");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_SaidTheyWereSafe");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_PastTense");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_DontUnderstand");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_DebtConsiderable");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Unfortunately");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_PerformanceNotEnough");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_WhatTalkingAbout");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_RecoverInvestment");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_WifeCompatible");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_ChildAsWell");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_OrgansSoldWell");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_No");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_YoureLying");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_RarelyLie");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_BadForBusiness");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_No2");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_NO");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Please");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_LowerVoice");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_DisturbCustomers");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_YouPromised");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_PromisedTakenCare");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_AndTheyWere");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_EveryPartBuyer");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_Monster");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_No");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Businessman");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_DebtExceedsValue");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_Fortunately");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_YoureHealthy");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_OrgansCoverRest");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_Please");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MC, "Shift5_MC_PleaseDont");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_ThankYouService");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.MafiaBoss, "Shift5_Boss_ExcellentDrinks");
        yield return dm.WaitForSpace();

        dm.PlayDialogue(SpeakerType.System, "Shift5_System_EndOfGame");
        yield return dm.WaitForSpace();

        dm.HideDialogue();
    }

    public IEnumerator PlayShiftSequence(int shiftNumber){
        switch(shiftNumber){
            case 1: yield return StartCoroutine(PlayShift1Sequence()); break;
            case 2: yield return StartCoroutine(PlayShift2Sequence()); break;
            case 3: yield return StartCoroutine(PlayShift3Sequence()); break;
            case 4: yield return StartCoroutine(PlayShift4Sequence()); break;
            case 5: yield return StartCoroutine(PlayShift5Sequence()); break;
        }
    }
}