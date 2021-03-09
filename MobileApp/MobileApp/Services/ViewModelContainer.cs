using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public class ViewModelContainer
    {
        private Dictionary<string, (Type Page, Type ViewModel)> pageViewModelMapping;
        private Dictionary<Type, object> serviceMapping;
        public INavigationService NavigationService { get; }
        public ViewModelContainer()
        {
            pageViewModelMapping = new();
            serviceMapping = new();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.Name.EndsWith("ViewModel"))
                {
                    string mapping = type.Name.Substring(0, type.Name.Length - 9);
                    pageViewModelMapping.TryGetValue(mapping, out (Type, Type) vmPair);
                    pageViewModelMapping[mapping] = (vmPair.Item1, type);
                }
                else if (type.Name.EndsWith("Page"))
                {
                    string mapping = type.Name.Substring(0, type.Name.Length - 4);
                    pageViewModelMapping.TryGetValue(mapping, out (Type, Type) vmPair);
                    pageViewModelMapping[mapping] = (type, vmPair.Item2);
                }
            }
            foreach (var pvm in pageViewModelMapping)
            {
                Routing.RegisterRoute(pvm.Value.ViewModel.Name, pvm.Value.Page);
            }
            NavigationService = new NavigationServiceImpl(ConstructViewModel);
            serviceMapping[typeof(INavigationService)] = NavigationService;
        }

        public void RegisterService<TService, T>() where T : TService, new()
        {
            serviceMapping.Add(typeof(TService), new T());
        }

        public TService GetService<TService>()
        {
            return (TService)serviceMapping[typeof(TService)];
        }

        private BaseViewModel ConstructViewModel(Type vmType)
        {
            var consParams = vmType.GetConstructors()[0].GetParameters();
            object[] args = new object[consParams.Length];
            for (int i = 0; i < args.Length; ++i)
                args[i] = serviceMapping[consParams[i].ParameterType];
            return (BaseViewModel)Activator.CreateInstance(vmType, args);
        }

        private class NavigationServiceImpl : INavigationService
        {
            private Func<Type, BaseViewModel> consViewModel;
            private Stack<BaseViewModel> vmStack;
            private string rootVm;

            private Page LastPage => Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1];

            public BaseViewModel PreviousViewModel => vmStack.Count == 0 ? null : vmStack.Peek();

            public NavigationServiceImpl(Func<Type, BaseViewModel> cvm)
            {
                consViewModel = cvm;
                vmStack = new Stack<BaseViewModel>();
            }

            private void Current_Navigated(object sender, ShellNavigatedEventArgs e)
            {
                if (e.Source == ShellNavigationSource.Pop)
                    vmStack.Pop();
            }

            public async Task InitializeFirst<TViewModel>()
            {
                rootVm = typeof(TViewModel).Name;

                var vm = consViewModel(typeof(TViewModel));
                vm.OnEntering(null);
                LastPage.BindingContext = vm;
                Shell.Current.Navigated += Current_Navigated;
                await Task.CompletedTask;
            }

            public async Task NavigateBack(object parameter = null)
            {
                Console.WriteLine("Navigating back");
                ((BaseViewModel)LastPage.BindingContext).OnDestroying();

                await Shell.Current.GoToAsync("..");
                BaseViewModel vm = vmStack.Pop();
                LastPage.BindingContext = vm;
                vm.OnReturning(parameter);
            }

            public async Task NavigateTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
            {
                string vmName = typeof(TViewModel).Name;
                Console.WriteLine("Navigating to " + vmName);

                var vm = consViewModel(typeof(TViewModel));

                vmStack.Push(vmStack.Count == 0 ? null : LastPage.BindingContext as BaseViewModel);

                vm.OnEntering(null);

                await Shell.Current.GoToAsync(vmName);
                LastPage.BindingContext = vm;
            }

            public async Task NavigateBackTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
            {
                string vmName = typeof(TViewModel).Name;
                Console.WriteLine("Navigating back to " + vmName);

                while (vmStack.Peek() is not TViewModel)
                    vmStack.Pop().OnDestroying();
                vmStack.Peek().OnReturning(parameter);

                await Shell.Current.GoToAsync($"/{vmName}");
            }

            public async Task ToRoot(object parameter = null)
            {
                Console.WriteLine("Navigating to root");

                ((BaseViewModel)LastPage.BindingContext).OnDestroying();
                while (vmStack.Count > 1)
                    vmStack.Pop().OnDestroying();
                vmStack.Pop().OnReturning(parameter);

                await Shell.Current.GoToAsync($"//{rootVm}");
            }
        }
    }

    public interface INavigationService
    {
        BaseViewModel PreviousViewModel { get; }
        Task InitializeFirst<TViewModel>();
        Task NavigateTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task NavigateBackTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;
        Task NavigateBack(object parameter = null);
        Task ToRoot(object parameter = null);
    }
}
