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

            public BaseViewModel PreviousViewModel => vmStack.Count == 0 ? null : vmStack.Peek();

            public NavigationServiceImpl(Func<Type, BaseViewModel> cvm)
            {
                consViewModel = cvm;
                vmStack = new Stack<BaseViewModel>();
            }

            public async Task InitializeFirst<TViewModel>()
            {
                rootVm = nameof(TViewModel);
                var page = (Page)Routing.GetOrCreateContent(rootVm);
                var vm = consViewModel(typeof(TViewModel));
                vm.OnEntering(null);
                page.BindingContext = vm;
                await Task.CompletedTask;
            }

            public async Task NavigateBack(object parameter = null)
            {
                ((BaseViewModel)Shell.Current.BindingContext).OnDestroying();
                await Shell.Current.GoToAsync("..");
                vmStack.Pop().OnReturning(parameter);
            }

            public async Task NavigateTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
            {
                var page = (Page)Routing.GetOrCreateContent(nameof(TViewModel));
                var vm = consViewModel(typeof(TViewModel));

                vmStack.Push((BaseViewModel)Shell.Current.BindingContext);

                vm.OnEntering(null);
                page.BindingContext = vm;

                await Shell.Current.GoToAsync(nameof(TViewModel));
            }

            public async Task NavigateBackTo<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
            {
                while (vmStack.Peek() is not TViewModel)
                    vmStack.Pop().OnDestroying();
                vmStack.Peek().OnReturning(parameter);

                await Shell.Current.GoToAsync($"/{nameof(TViewModel)}");
            }

            public async Task ToRoot(object parameter = null)
            {
                ((BaseViewModel)Shell.Current.BindingContext).OnDestroying();
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
