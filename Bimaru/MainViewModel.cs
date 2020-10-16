using System;
using System.Collections.Generic;
using System.Text;
using Bimaru.Logic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bimaru
{
    public class MainViewModel : ViewModelBase
    {
        private IPitchProvider _provider;
        
        public Pitch _pitch;
        public Pitch Pitch
        {
            get => _pitch;
            set => Set(ref _pitch, value, nameof(Pitch));
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => Set(ref _message, value, nameof(Message));
        }

        public RelayCommand<string> ToggleCommand { get; set; }

        public MainViewModel()
        {
            _provider = new RawPitchProvider();
            Pitch = new Pitch(_provider.GetNextPitchRaw());
            ToggleCommand = new RelayCommand<string>(index =>
            {
                Pitch.Toggle(int.Parse(index));
                Message = $"Field at index {index} set";
                RaisePropertyChanged(nameof(Pitch));
                if (Pitch.IsSolved())
                {
                    Message = Message + Environment.NewLine + "congratulation you won!";
                }
            });
        }
    }
}
