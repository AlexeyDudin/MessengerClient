﻿using System;
using System.ComponentModel;

namespace Domain.Dtos
{
    public class MessageDto : INotifyPropertyChanged
    {
        private string message = "";
        private string from = "";
        private string toUser = "";
        private string toGroup = "";
        private DateTime timeStamp = DateTime.Now;
        private Guid uniqueId = Guid.NewGuid();

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        public string From 
        {
            get => from;
            set
            {
                from = value;
                OnPropertyChanged(nameof(From));
            } 
        }
        public string ToUser
        {
            get => ToUser;
            set
            {
                toUser = value; 
                OnPropertyChanged(nameof(ToUser));
            } 
        }
        public string ToGroup 
        { 
            get => toGroup;
            set
            {
                toGroup = value;
                OnPropertyChanged(nameof(ToGroup));
            }
        }
        public DateTime TimeStamp 
        { 
            get => timeStamp;
            set
            {
                timeStamp = value;
                OnPropertyChanged(nameof(TimeStamp));
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
    }
}