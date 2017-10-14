using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Quanti.WPF.Utils.Base
{
    /// <summary>
    /// Base class which supports <see cref="INotifyPropertyChanged"/> interface.
    /// It can be used either by view models or modles which need to be able to notify about their changes.
    /// For example view model needs to be notified when "Status" property changes in model.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets whether model is modified.
        /// </summary>
        public virtual bool IsDirty { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets new value to property and invokes event about change of that property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <param name="dependentPropertyNames">Related properties which are alse changed as a result of change of the property. Their value is calculated using that property.</param>
        /// <param name="supportsDirtiness">When property is part of a data model then it supports <see cref="IsDirty"/> flag. Otherwise <see cref="IsDirty"/> shouldn't change state of the model.</param>
        protected void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null, IEnumerable<string> dependentPropertyNames = null, bool supportsDirtiness = true)
        {
            if (!EqualityComparer<T>.Default.Equals(member, value))
            {
                member = value;
                OnPropertyChanged(propertyName);

                if (dependentPropertyNames != null)
                {
                    foreach (var dependentPropertyName in dependentPropertyNames)
                    {
                        OnPropertyChanged(dependentPropertyName);
                    }
                }

                if (supportsDirtiness)
                    IsDirty = true;
            }
        }
    }
}
