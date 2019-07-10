using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WpfTools
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void OnPropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            if (PropertyChanged != null)
            {
                var PropertyName = propertyName.Body as MemberExpression;
                if (PropertyName != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(PropertyName.Member.Name));
                }
            }
        }
    }
}
