﻿using Lavcode.Model;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public class PasswordItem : IconItem
    {
        public PasswordItem(Password password, Icon icon = null)
        {
            Set(password, icon);
        }

        public void Set(Password password, Icon icon = null)
        {
            Password = password;
            Title = password.Title;
            Remark = password.Remark.Replace('\n', ' ').Replace('\r', ' ');

            if (icon == null)
            {
                //后台设置图标
                SetIcon(password.Id);
            }
            else
            {
                Icon = icon;
            }
        }

        public Password Password { get; set; }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _remark = string.Empty;
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value); }
        }
    }
}
