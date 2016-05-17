using System.ComponentModel;
using System.Runtime.CompilerServices;
using TeamDailyWork.Annotations;

namespace TeamDailyWork.ViewModels
{
    public class NotificationObject:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
