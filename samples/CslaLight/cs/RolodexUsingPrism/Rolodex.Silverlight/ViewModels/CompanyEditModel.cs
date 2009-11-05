﻿using System;
using Rolodex.Business.BusinessClasses;
using Rolodex.Silverlight.Core;
using Rolodex.Silverlight.Views;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Events;
using Rolodex.Silverlight.Events;

namespace Rolodex.Silverlight.ViewModels
{
    public class CompanyEditModel : ViewModel<Company, CompanyEditView>
    {
        private int _companyID;
        private CompanyContact _selectedContact;
        private CompanyContactPhone _selectedContactPhone;
        private IEventAggregator _eventAggregator;
        public CompanyEditModel(IEventAggregator eventAggregator)
        {
            Initialize(eventAggregator);
        }

        public CompanyEditModel(int companyID, IEventAggregator eventAggregator)
        {
            _companyID = companyID;
            Initialize(eventAggregator);
        }
        protected CompanyEditModel() { }


        private void Initialize(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            CloseCommand = new DelegateCommand<object>(CloseMethod, CanCloseMethod);
            SelectContactCommand = new DelegateCommand<object>(SelectContactMethod, CanSelectContactMethod);
            RemoveContactCommand = new DelegateCommand<object>(RemoveContactMethod, CanRemoveContactMethod);
            AddContactCommand = new DelegateCommand<object>(AddContactMethod, CanAddContactMethod);
            RemoveContactPhoneCommand = new DelegateCommand<object>(RemoveContactPhoneMethod, CanRemoveContactPhoneMethod);
            AddContactPhoneCommand = new DelegateCommand<object>(AddContactPhoneMethod, CanAddContactPhoneMethod);
            SelectContactPhoneCommand = new DelegateCommand<object>(SelectContactPhoneMethod, CanSelectContactPhoneMethod);
            View = new CompanyEditView();
            SetSecondaryDataCounter(1);
            Ranks.GetRanks(GetDataLoadedHandler<Ranks>());
        }

        protected override void OnDataLoaded<T>(Csla.DataPortalResult<T> result)
        {
            if (typeof(Ranks) == typeof(T))
            {
                ((SecondaryModel)View.Resources["RanksModel"]).Model = result.Object;
            }
        }

        protected override void OnAllSecondaryDataLoaded()
        {
            if (_companyID == 0)
            {
                DoRefresh("CreateCompany");
            }
            else
            {
                DoRefresh("GetCompany", new object[] { _companyID });
            }
        }

        public DelegateCommand<object> CloseCommand { get; private set; }

        private void CloseMethod(object parameter)
        {
            _eventAggregator.GetEvent<CloseEditViewEvent>().Publish(EventArgs.Empty);
        }

        private bool CanCloseMethod(object parameter)
        {
            return !CanCancel;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (CloseCommand != null)
                CloseCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand<object> SelectContactCommand { get; private set; }

        private void SelectContactMethod(object parameter)
        {
            _selectedContact = parameter as CompanyContact;
            if (_selectedContact != null)
                View.ContactsPhonesGrid.ItemsSource = _selectedContact.ContactPhones;
            else
                View.ContactsPhonesGrid.ItemsSource = null;

            RemoveContactCommand.RaiseCanExecuteChanged();
            AddContactPhoneCommand.RaiseCanExecuteChanged();
            RemoveContactPhoneCommand.RaiseCanExecuteChanged();
        }

        private bool CanSelectContactMethod(object parameter)
        {
            return true;
        }


        public DelegateCommand<object> SelectContactPhoneCommand { get; private set; }

        private void SelectContactPhoneMethod(object parameter)
        {
            _selectedContactPhone = parameter as CompanyContactPhone;
            RemoveContactPhoneCommand.RaiseCanExecuteChanged();
        }

        private bool CanSelectContactPhoneMethod(object parameter)
        {
            return _selectedContact != null;
        }

        public DelegateCommand<object> RemoveContactCommand { get; private set; }

        private void RemoveContactMethod(object parameter)
        {
            if (_selectedContact != null)
            {
                TypedModel.Contacts.Remove(_selectedContact);
                _selectedContact = null;
                AddContactPhoneCommand.RaiseCanExecuteChanged();
                RemoveContactPhoneCommand.RaiseCanExecuteChanged();
                // needed becuase SelectionChanged event is not raised by the grid 
                // properly
                if (TypedModel.Contacts.Count > 0)
                {
                    View.ContactsGrid.SelectedItem = null;
                    View.ContactsGrid.SelectedItem = TypedModel.Contacts[0];
                }
            }
        }

        private bool CanRemoveContactMethod(object parameter)
        {
            return _selectedContact != null &&
                Csla.Security.AuthorizationRules.CanDeleteObject(typeof(CompanyContact)); ;
        }


        public DelegateCommand<object> AddContactCommand { get; private set; }

        private void AddContactMethod(object parameter)
        {
            TypedModel.Contacts.AddNew();
        }

        private bool CanAddContactMethod(object parameter)
        {
            return Csla.Security.AuthorizationRules.CanCreateObject(typeof(CompanyContact));
        }



        public DelegateCommand<object> RemoveContactPhoneCommand { get; private set; }

        private void RemoveContactPhoneMethod(object parameter)
        {
            if (_selectedContactPhone != null && _selectedContact != null)
            {
                _selectedContact.ContactPhones.Remove(_selectedContactPhone);
                _selectedContactPhone = null;
                
                // needed because SelectionChanged event is not raised by the grid 
                // properly
                if (_selectedContact != null && _selectedContact.ContactPhones.Count > 0)
                {
                    View.ContactsPhonesGrid.SelectedItem = null;
                    View.ContactsPhonesGrid.SelectedItem = _selectedContact.ContactPhones[0];
                }
                RemoveContactPhoneCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanRemoveContactPhoneMethod(object parameter)
        {
            return _selectedContactPhone != null &&
                Csla.Security.AuthorizationRules.CanDeleteObject(typeof(CompanyContactPhone)); ;
        }


        public DelegateCommand<object> AddContactPhoneCommand { get; private set; }

        private void AddContactPhoneMethod(object parameter)
        {
            _selectedContact.ContactPhones.AddNew();
        }

        private bool CanAddContactPhoneMethod(object parameter)
        {
            return _selectedContact != null &&
                Csla.Security.AuthorizationRules.CanCreateObject(typeof(CompanyContactPhone));
        }

        protected override void OnAfterDeleteSave()
        {
            base.OnAfterDeleteSave();
            CloseMethod(null);
        }

        protected override void OnAfterCancelNew()
        {
            base.OnAfterCancelNew();
            CloseMethod(null);
        }
    }
}
