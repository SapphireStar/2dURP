using Isekai.UI.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Isekai.UI.Views
{
    public abstract class View<TViewModel> : UIElement where TViewModel : IViewModel
    {
        public TViewModel ViewModel { get; private set; }

        public virtual void Initialize(TViewModel newViewModel)
        {
            AssignViewModel(newViewModel);
        }

        public virtual void AssignViewModel(TViewModel newViewModel)
        {
            if (newViewModel.Equals(ViewModel))
            {
                return;
            }
            if (ViewModel != null)
            {
                ViewModel.PropertyValueChanged -= HandleViewModelPropertyChanged;
                UnregisterEvents();
            }

            ViewModel = newViewModel;
            RegisterEvents();
            ViewModel.PropertyValueChanged += HandleViewModelPropertyChanged;
            RefreshAll();
        }
        /// <summary>
        /// This method is called by the base class "View" when a new ViewModel is set, and thus all fields could have changed.
        /// </summary>
        protected virtual void RefreshAll() { }

        /// <summary>
        /// This method is called by the base class "View" when AssignViewModel.
        /// </summary>
        protected virtual void RegisterEvents() { }

        /// <summary>
        /// This method is called by the base class "View" when OnDestroy.
        /// </summary>
        protected virtual void UnregisterEvents() { }

        /// <summary>
        /// This method can be overriden in order to pass (sub)ViewModels to subViews.
        /// E.g. A screen view is composing some views of smaller elements. The screen view is in charge of setting the relevent ViewModels on its components.
        /// </summary>
        public virtual void OnViewModelChanged() { }

        public virtual void HandleViewModelPropertyChanged(object sender, PropertyValueChangedEventArgs e) { }
    }

}
