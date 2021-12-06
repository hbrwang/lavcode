﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordDetail : UserControl
    {
        public PasswordDetail()
        {
            DataContext = VM;
            this.InitializeComponent();
            Messenger.Default.Register<object>(this, "AddNewPassword", (obj) => AddNewPassword());

            VM.CalcTextBlock = CalcTextBlock; // 用于计算Key宽度
        }

        public PasswordDetailViewModel VM { get; } = SimpleIoc.Default.GetInstance<PasswordDetailViewModel>();

        ~PasswordDetail()
        {
            Messenger.Default.Unregister(this);
        }

        private async void AddNewPassword()
        {
            if (SettingHelper.Instance.AddPasswordTaught)
            {
                return;
            }
            await PopupHelper.ShowTeachingTipAsync(TitleTextBox, "密码标题（添加记录 2/6）", "标题作为密码项的标识，应输入具有代表性且便于识别的内容，在密码列表中容易查找");

            VM.Title = "测试标题";
            VM.Remark = "这条记录是用来教学的，完成后可以自行删除";
            await PopupHelper.ShowTeachingTipAsync(PasswordGeneratorBtn, "生成密码（添加记录 3/6）", "点击此按钮能随机生成复杂密码，当创建账号或修改密码时，能够使用复杂密码");
            PasswordGeneratorTip.IsOpen = true;
            await TaskExtend.SleepAsync();
            await PopupHelper.ShowTeachingTipAsync(PasswordGenerator, "生成完成（添加记录 4/6）", "配置完成后，点击 生成 按钮即可");
            PasswordGeneratorTip.IsOpen = false;
            VM.Value = "Lavcode";
            await PopupHelper.ShowTeachingTipAsync(AddKvpBtn, "关联内容（添加记录 5/6）", "可以无限制添加多条内容，每项内容都可自定义名称，便于管理与账号相关的信息");
            await PopupHelper.ShowTeachingTipAsync(SaveBtn, "编辑完成（添加记录 6/6）", "编辑完成，别忘记保存哦！（虽然有退出提醒，但手动保存是个好习惯）");
            SettingHelper.Instance.AddPasswordTaught = true;
            VM.HandleSave();
        }

        private void SelectKey_Click(object sender, RoutedEventArgs e)
        {
            ((sender as MenuFlyoutItem).DataContext as KeyValuePairItem).Key = (sender as MenuFlyoutItem).Text;
        }

        private async void CustomKey_Click(object sender, RoutedEventArgs e)
        {
            await VM.CustomKey((sender as MenuFlyoutItem).DataContext as KeyValuePairItem);
        }

        private async void DeleteKey_Click(object sender, RoutedEventArgs e)
        {
            await VM.DeleteKey((sender as MenuFlyoutItem).DataContext as KeyValuePairItem);
        }

        private void PasswordGenerator_Click(object sender, RoutedEventArgs e)
        {
            if (!PasswordGeneratorTip.IsOpen && !VM.ReadOnly)
            {
                PasswordGeneratorTip.IsOpen = true;
            }
        }

        private void PasswordGenerator_PasswordChanged(PasswordGenerator sender, string args)
        {
            VM.Value = args;
        }

        private void CopyKeyValue_Click(object sender, RoutedEventArgs e)
        {
            VM.CopyKeyValue((sender as Button).DataContext as KeyValuePairItem, sender as Button);
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);

            switch (e.Key)
            {
                case VirtualKey.S:
                    if ((ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
                    {
                        VM.HandleSave();
                    }
                    break;

                default:
                    break;
            }
        }

        private void KeyTrimmedChanged(TextBlock sender, IsTextTrimmedChangedEventArgs args)
        {
            if (sender.IsTextTrimmed)
            {
                (sender.Tag as Button).Margin = new Thickness(12, 0, -12, 0);
            }
            else
            {
                (sender.Tag as Button).Margin = new Thickness(0);
            }
        }
    }
}
