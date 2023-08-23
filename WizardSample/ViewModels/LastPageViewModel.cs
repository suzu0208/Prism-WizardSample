using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Threading.Tasks;

namespace WizardSample.ViewModels
{
    public class LastPageViewModel : PageViewModelBase
    {
        /// <summary>
        /// Page上部の説明文
        /// </summary>
        public ReactivePropertySlim<string> Detail { get; } = new(string.Empty);

        /// <summary>
        /// ログ
        /// </summary>
        public ReactivePropertySlim<string> Logs { get; } = new(string.Empty);

        /// <summary>
        /// プログレスバーの表示/非表示
        /// </summary>
        public ReactivePropertySlim<bool> IsVisibleProgress { get; private set; } = new(false);

        #region PageViewModelBase Functions

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            // 処理実行中はProgressBarを表示させたい。
            IsBusy.Subscribe(x => IsVisibleProgress.Value = x);

            _ = WorkingTask();
        }

        #endregion PageViewModelBase Functions

        private async Task WorkingTask()
        {
            try
            {
                IsBusy.Value = true;
                Detail.Value = "処理を実行中です。";
                Logs.Value = string.Empty;


                {   // ToDo: 時間のかかる処理
                    await Task.Delay(5000);
                    Logs.Value += $"ファイルを解析中...{Environment.NewLine}";
                    await Task.Delay(5000);
                    Logs.Value += $"ファイルを解析しました。{Environment.NewLine}";
                    await Task.Delay(1000);
                }

                Logs.Value += "処理が完了しました。";
                Detail.Value = "処理が完了しました。";
            }
            catch
            {
                Detail.Value = "処理が失敗しました。";
            }
            finally
            {
                IsBusy.Value = false;
            }
        }
    }
}
