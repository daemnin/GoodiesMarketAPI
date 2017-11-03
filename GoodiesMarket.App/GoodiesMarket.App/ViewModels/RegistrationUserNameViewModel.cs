﻿using GoodiesMarket.App.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace GoodiesMarket.App.ViewModels
{
    public class RegistrationUserNameViewModel : ViewModelBase
    {
        public ICommand NextCommand { get; private set; }
        private INavigationService navigationService;

        private RegistrationModel model;
        public RegistrationModel Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public RegistrationUserNameViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            Model = new RegistrationModel();
            NextCommand = new DelegateCommand(NextView);
        }

        private async void NextView()
        {
            var navigationParameters = new NavigationParameters
            {
                { "model", model }
            };
            await navigationService.NavigateAsync("RegistrationEmail", navigationParameters);
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                Model = (RegistrationModel)parameters["model"];
                System.Diagnostics.Debug.WriteLine(model.IsSeller);
            }
        }
    }
}
