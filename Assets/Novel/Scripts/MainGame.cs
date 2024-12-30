using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace HatenoWorks.Novel
{
    public class MainGame : MonoBehaviour
    {
        public NovelScene CurrentNovelScene { get; private set; }
        public int CurrentMomentIndex { get; private set; }
        public Place CurrentPlace { get; private set; }
        public bool IsNovelEnd { get; private set; }

        public List<Actor> CurrentActors { get; private set; } = new List<Actor>();

        [Header("0以下だと不具合が起こります")]
        [SerializeField] private float messageSendSpeed = 1f;

        [SerializeField] private float fadeOutTime = 0.75f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        [SerializeField] private float fadeInTime = 0.75f;

        [SerializeField] private NovelScene startNovelScene;

        [SerializeField] private Actor actorPrefab;

        [SerializeField] private Fader fader;

        [SerializeField] private Image background;

        [SerializeField] private TextWindow placeNameWindow;

        [SerializeField] private Transform graphicsRoot;
        [SerializeField] private Transform guiRoot;

        private TextWindow currentNameWindow;
        private TextWindow currentMessageWindow;

        private TextWindow previousNameWindowPrefab;
        private TextWindow previousMessageWindowPrefab;

        private bool isStopMainSequence;

        private bool isSignCompleted;

        public void StartNovel()
        {
            CurrentNovelScene = startNovelScene;
            CurrentMomentIndex = 0;
            SetMoment();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsNovelEnd && !isStopMainSequence)
            {
                if (currentMessageWindow.CurrentState == TextWindow.TextWindowStates.Send)
                {
                    currentMessageWindow.SendToLast();
                }
                else
                {
                    ++CurrentMomentIndex;
                    isSignCompleted = false;
                    SetMoment();
                }
            }
        }

        public void SetMoment()
        {
            if (CurrentMomentIndex >= CurrentNovelScene.Moments.Length)
            {
                if (CurrentNovelScene.IsEnableChoices)
                {
                    // 選択肢を表示する
                    isStopMainSequence = true;
                    DoChoices(CurrentNovelScene.SceneChoicesFormat, CurrentNovelScene.ChoicesDescription, CurrentNovelScene.Choices, this.GetCancellationTokenOnDestroy()).Forget();
                }
                else
                {
                    if (CurrentNovelScene.NextSceneIfNotChoices == null)
                    {
                        IsNovelEnd = true;
                        currentNameWindow.gameObject.SetActive(false);
                        currentMessageWindow.gameObject.SetActive(false);
                        return;
                    }
                    CurrentNovelScene = CurrentNovelScene.NextSceneIfNotChoices;
                    CurrentMomentIndex = 0;
                    SetMoment();
                }
                return;
            }

            Moment moment = CurrentNovelScene.Moments[CurrentMomentIndex];

            if (CurrentPlace != moment.CurrentPlace || (moment.Sign != "" && !isSignCompleted))
            {
                isStopMainSequence = true;
                MovePlace(moment.CurrentPlace, moment.IsShowPlaceName, moment.Sign).Forget();
                return;
            }

            // 名前ウィンドウのセットアップ
            TextWindow nameWindowPrefab = moment.IsUseDefaultTextWindows ? CurrentNovelScene.DefaultNameWindowPrefab : moment.NameWindowPrefab;
            if (nameWindowPrefab != previousNameWindowPrefab)
            {
                if (previousNameWindowPrefab != null) Destroy(currentNameWindow.gameObject);
                currentNameWindow = Instantiate(nameWindowPrefab, guiRoot);
                previousNameWindowPrefab = nameWindowPrefab;
                currentNameWindow.SetUp();
            }

            // メッセージウィンドウのセットアップ
            TextWindow messageWindowPrefab = moment.IsUseDefaultTextWindows ? CurrentNovelScene.DefaultMessageWindowPrefab : moment.MessageWindowPrefab;
            if (messageWindowPrefab != previousMessageWindowPrefab)
            {
                if (previousMessageWindowPrefab != null) Destroy(currentMessageWindow.gameObject);
                currentMessageWindow = Instantiate(messageWindowPrefab, guiRoot);
                previousMessageWindowPrefab = messageWindowPrefab;
                currentMessageWindow.SetUp(messageSendSpeed);
            }

            // 役者削除
            List<Actor> destroyedActors = new List<Actor>();
            foreach (Actor currentActor in CurrentActors)
            {
                bool isExist = false;
                foreach (DisplayedActorInfo actorInfo in moment.DisplayedActorInfoArray)
                {
                    if (actorInfo.DisplayedActorData == currentActor.Data)
                    {
                        isExist = true;
                    }
                }
                if (!isExist)
                {
                    destroyedActors.Add(currentActor);
                }
            }
            foreach (Actor destroyedActor in destroyedActors)
            {
                CurrentActors.Remove(destroyedActor);
                Destroy(destroyedActor.gameObject);
            }

            // 効果音再生
            if (moment.Se != "")
            {
                AudioManager.Instance.PlaySE(moment.Se, isOverride: true);
            }

            // BGM再生または停止
            if (moment.IsStopBgm)
            {
                AudioManager.Instance.StopBgm();
            }
            if (moment.Bgm != "")
            {
                AudioManager.Instance.PlayBgm(moment.Bgm);
            }

            // 役者追加
            foreach (DisplayedActorInfo actorInfo in moment.DisplayedActorInfoArray)
            {
                Actor actor = null;
                foreach (Actor currentActor in CurrentActors)
                {
                    if (actorInfo.DisplayedActorData == currentActor.Data)
                    {
                        actor = currentActor;
                        break;
                    }
                }
                if (actor == null)
                {
                    actor = Instantiate(actorPrefab, graphicsRoot);
                    actor.Data = actorInfo.DisplayedActorData;
                    CurrentActors.Add(actor);
                }
                actor.IsSpeaking = actorInfo.IsSpeaking;
                actor.IsShow = actorInfo.IsShow;
                actor.Status = actorInfo.Status;
                actor.GraphicPosition = actorInfo.GraphicPosition;
            }

            // 名前表示
            if (Array.Exists(moment.DisplayedActorInfoArray, dai => dai.IsSpeaking))
            {
                DisplayedActorInfo speaker = Array.Find(moment.DisplayedActorInfoArray, dai => dai.IsSpeaking);
                currentNameWindow.StartSending(speaker.DisplayedActorData.Name, true);
            }
            else
            {
                currentNameWindow.StartSending("???", true);
            }


            // メッセージ表示
            string message = "";
            if (moment.MessageType == Moment.MessageTypes.Think)
            {
                message = "( " + moment.Message + " )";
            }
            else
            {
                message = moment.Message;
            }
            currentMessageWindow.StartSending(message, false);

            // 効果音を再生する
            // BGMを再生する
        }

        private async UniTask MovePlace(Place nextPlace, bool isShowPlaceName, string sign)
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();

            fader.SetColor(Color.black);

            if (CurrentPlace != null)
            {
                foreach (Actor actor in CurrentActors)
                {
                    actor.gameObject.SetActive(false);
                }
                currentNameWindow.gameObject.SetActive(false);
                currentMessageWindow.gameObject.SetActive(false);
                await fader.FadeOut(fadeOutTime, token);
                await UniTask.Delay((int)(fadeWaitTime * 1000f), cancellationToken: token);
            }

            background.sprite = nextPlace.BgSprite;
            CurrentPlace = nextPlace;

            await fader.FadeIn(fadeInTime, token);
            token.ThrowIfCancellationRequested();

            if (sign != "")
            {
                placeNameWindow.gameObject.SetActive(true);
                placeNameWindow.StartSending(sign, true);
                await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);
                token.ThrowIfCancellationRequested();
                placeNameWindow.gameObject.SetActive(false);
                isSignCompleted = true;
            }

            // 場所名を表示
            if (isShowPlaceName)
            {
                placeNameWindow.gameObject.SetActive(true);
                placeNameWindow.StartSending(nextPlace.Name, true);
                await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: token);
                token.ThrowIfCancellationRequested();
                placeNameWindow.gameObject.SetActive(false);
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            isStopMainSequence = false;
            foreach (Actor actor in CurrentActors)
            {
                if (actor.IsShow)
                {
                    actor.gameObject.SetActive(true);
                }
            }
            if (currentNameWindow != null) currentNameWindow.gameObject.SetActive(true);
            if (currentMessageWindow != null) currentMessageWindow.gameObject.SetActive(true);

            SetMoment();
        }

        private async UniTask DoChoices(ChoicesFormat format, string description, NovelScene.Choice[] choices, CancellationToken token)
        {
            currentNameWindow.gameObject.SetActive(false);
            currentMessageWindow.gameObject.SetActive(false);
            TextWindow descriptionWindow = Instantiate(format.DescriptionWindowPrefab, guiRoot);
            descriptionWindow.SetUp(messageSendSpeed);
            descriptionWindow.StartSending(description, true);

            ChoiceButton[] choiceButtons = new ChoiceButton[choices.Length];
            int clicked = -1;
            for (int i = 0; i < choices.Length; ++i)
            {
                NovelScene.Choice choice = choices[i];

                ChoiceButton choiceButton = Instantiate(format.ChoiceWindowPrefab, guiRoot).GetComponent<ChoiceButton>();
                choiceButton.GetComponent<RectTransform>().anchoredPosition = format.ChoicesOffset + format.ChoiceDisplacemnet * i;
                choiceButton.Index = i;
                choiceButton.OnClicked += index => clicked = index;
                choiceButton.ChoiceWindow.StartSending(choices[i].Message, true);
                choiceButtons[i] = choiceButton;
            }
            await UniTask.WaitWhile(() => clicked == -1, cancellationToken: token);
            token.ThrowIfCancellationRequested();
            CurrentNovelScene = choices[clicked].NextScene;
            CurrentMomentIndex = 0;

            Destroy(descriptionWindow.gameObject);
            foreach (ChoiceButton choiceButton in choiceButtons)
            {
                Destroy(choiceButton.gameObject);
            }

            await UniTask.Yield(token);
            isStopMainSequence = false;
            currentNameWindow.gameObject.SetActive(true);
            currentMessageWindow.gameObject.SetActive(true);
            SetMoment();
        }
    }
}
