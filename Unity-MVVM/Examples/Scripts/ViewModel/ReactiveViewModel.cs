using System;
using UnityMVVM.ViewModel;
using UnityEngine;
using System.Collections;
using UniRx;
using UnityMVVM.Reactive;
using BoolReactiveProperty = UnityMVVM.Reactive.BoolReactiveProperty;
using ColorReactiveProperty = UnityMVVM.Reactive.ColorReactiveProperty;
using Random = UnityEngine.Random;
using StringReactiveProperty = UnityMVVM.Reactive.StringReactiveProperty;

namespace UnityMVVM.Examples
{
    public class ReactiveViewModel : ViewModelBase
    {
        public Reactive.ReactiveProperty<ApplicationState> State { get; set; } = new Reactive.ReactiveProperty<ApplicationState>();
        public UniRx.ReactiveProperty<int> IntProp { get; set; } = new UniRx.ReactiveProperty<int>(10);
        [TwoWayBindable]
        public StringReactiveProperty Text { get; set; } = new StringReactiveProperty();
        public BoolReactiveProperty BoolProp { get; set; } = new BoolReactiveProperty();
        public ColorReactiveProperty Color { get; set; } = new ColorReactiveProperty();
        public UniRx.ReactiveCommand<int> TestCommand { get; set; }
        public Reactive.ReactiveCommand ChangeColor { get; set; }
        public BoolReactiveProperty Flagger { get; set; } = new BoolReactiveProperty();
        public Reactive.ReactiveCollection<DataModel> TestCollection { get; set; } = new Reactive.ReactiveCollection<DataModel>();

        public string TestStaticString { get; set; } = "Observable Collection Test";

        public int Result;

        public void Awake()
        {
            TestCommand = IntProp.Select(x => x > 0).ToReactiveCommand<int>();
            TestCommand.Subscribe(val =>
            {
                Debug.unityLogger.Log($"val = {val} IntProp.Value = {IntProp.Value}");
                IntProp.Value -= 1;
            });
            ChangeColor = Flagger.ToMVVMReactiveCommand();
            ChangeColor.Subscribe(_ => Color.Value = Random.ColorHSV());
        }

        public void Start()
        {
            Text.Value = DateTime.Now.ToShortTimeString();
            StartCoroutine(ChangeRoutine());
            //            TestCollection.ObserveAdd().Subscribe(e => Debug.Log(e)); ;
        }

        public void Update()
        {
            if (DateTime.Now.Second % 5 == 0)
                Text.Value = DateTime.Now.ToShortTimeString();
            BoolProp.Value = (DateTime.Now.Second % 2 == 0);
        }

        private IEnumerator ChangeRoutine()
        {
            while (true)
            {
                State.Value = (ApplicationState)((int)(State.Value + 1) % 3);
                Flagger.Value = DateTime.Now.Second % 2 == 0;
                if (TestCollection.Count > 0)
                    TestCollection.RemoveAt(Random.Range(0, TestCollection.Count - 1));
                for (var i = 0; i < 2; i++)
                    TestCollection.Add(new DataModel
                    { message = Random.Range(0, 1000).ToString(), color = Random.ColorHSV() });
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
