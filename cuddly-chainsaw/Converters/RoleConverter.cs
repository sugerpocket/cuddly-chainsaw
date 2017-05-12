using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace cuddly_chainsaw.Converters
{
    /// <summary>
    /// 这个主要用在用户列表中，排除管理员信息，只需要显示普通用户的信息。
    /// 只是为了排除修改头像后，在<所有用户>这一菜单中无法同步更新管理员的头像
    /// BUG 应用在XAML中出现很奇怪的情况
    /// </summary>
    public class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            #region IValueCoverter Members
            bool? isAdmin = value as bool?;
            if (isAdmin == true)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
