using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Domain.Dtos
{
    public class UserDto : IDomain<UserDto>, INotifyPropertyChanged
    {
        private string login;
        private string password;
        private string fullName;
        private List<string> roles;
        private UserState state;
        private Guid uniqueId;
        public string Login 
        { 
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            } 
        }
        public string Password
        { 
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string FullName
        { 
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        public List<string> Roles
        {
            get => roles;
            set
            {
                roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        public UserState State 
        { 
            get => state;
            set
            {
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }
        public Guid UniqueId 
        { 
            get => uniqueId;
            set
            {
                uniqueId = value;
                OnPropertyChanged(nameof(UniqueId));
            }    
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool AreEqual(UserDto newElem)
        {
            return this.UniqueId == newElem.UniqueId;
        }

        public void ChangeValues(UserDto newElem)
        {
            this.Login = newElem.Login;
            this.Password = newElem.Password;
            this.Roles = newElem.Roles;
            this.FullName = newElem.FullName;
            this.State = newElem.State;
        }
    }
}
