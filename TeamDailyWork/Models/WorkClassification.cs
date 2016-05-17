using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using TeamDailyWork.Annotations;

namespace TeamDailyWork.Models
{
    public class WorkClassification:INotifyPropertyChanged
    {

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private String _type;
        public String Type
        {
            get { return _type; }
            set
            {
                if (value == null) return;
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public WorkClassification(Guid id, String type, Color color)
        {
            if(id==null) throw new ArgumentNullException(nameof(id));
            if(type==null) throw new ArgumentNullException(nameof(type));
            if (color == null) throw new ArgumentNullException(nameof(color));
            Id = id;
            Type = type;
            Color = color;
        }


        public WorkClassification()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
