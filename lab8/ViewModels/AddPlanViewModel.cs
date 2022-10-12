using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.ComponentModel;
using lab8.Models;

namespace lab8.ViewModels
{
    public class AddPlanViewModel : ViewModelBase
    {
        public AddPlanViewModel()
        {
            PlanToReturn = new Plan();
            PlanToReturn.PropertyChanged += resetEnable;
        }
        Plan planToReturn;
        public Plan PlanToReturn
        {
            get { return planToReturn; }
            set { this.RaiseAndSetIfChanged(ref planToReturn, value); }
        }
        bool buttonEnable;
        public bool ButtonEnable
        {
            get { return buttonEnable; }
            set { this.RaiseAndSetIfChanged(ref buttonEnable, value); }
        }
        public void resetEnable(object? sender, PropertyChangedEventArgs e)
        {
            ButtonEnable = !string.IsNullOrEmpty(PlanToReturn.Name);
        }
    }
}
