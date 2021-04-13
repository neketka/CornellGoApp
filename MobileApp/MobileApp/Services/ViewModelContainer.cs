using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            NavigationService = new NavigationServiceImpl(ConstructViewModel, ConstructPage,
                pageViewModelMapping.Values.Select(pvm => pvm.ViewModel).ToArray());

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

        private Page ConstructPage(Type vmType)
        {
            string name = vmType.Name;
            return (Page)Activator.CreateInstance(pageViewModelMapping[name.Substring(0, name.Length - 9)].Page);
        }

        private class NavigationServiceImpl : INavigationService
        {
            private Func<Type, BaseViewModel> consViewModel;
            private Func<Type, Page> consPage;
            private Stack<BaseViewModel> vmStack;
            private Dictionary<Type, Page> cachedPages;
            private Type[] vmTypes;
            private string rootVm;
            private bool suppressExtraPop = false;

            private Page LastPage => Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 1];

            public BaseViewModel PreviousViewModel => vmStack.Count == 0 ? null : vmStack.Peek();

            public NavigationServiceImpl(Func<Type, BaseViewModel> cvm, Func<Type, Page> cpage, Type[] vmTypes)
            {
                consViewModel = cvm;
                consPage = cpage;
                vmStack = new Stack<BaseViewModel>();
                cachedPages = new Dictionary<Type, Page>();
                this.vmTypes = vmTypes;
            }

            private void Current_Navigated(object sender, ShellNavigatedEventArgs e)
            {
                if (e.Source == ShellNavigationSource.Pop && !suppressExtraPop)
                    vmStack.Pop();
            }

            public async Task InitializeFirst<TViewModel>()
            {
                rootVm = typeof(TViewModel).Name;
                Shell.Current.Navigated += Current_Navigated;

                foreach (Type vmType in vmTypes)
                {
                    if (vmType == typeof(TViewModel) || vmType == typeof(BaseViewModel))
                        continue;
                    try
                    {
                        cachedPages[vmType] = consPage(vmType);
                        Console.WriteLine("Initialized " + vmType.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to create page for {vmType.Name}: {e.Message}");
                    }
                }

                var vm = consViewModel(typeof(TViewModel));
                await vm.OnEntering(null);

                LastPage.BindingContext = vm;
            }

            public async Task NavigateBack(object parameter = null, bool animate = true)
            {
                Console.WriteLine("Navigating back");
                ((BaseViewModel)LastPage.BindingContext).OnDestroying();

                await Shell.Current.Navigation.PopAsync(animate);
                await vmStack.Pop().OnReturning(parameter);
            }

            public async Task NavigateTo<TViewModel>(object parameter = null, bool animate = true) where TViewModel : BaseViewModel
            {
                string vmName = typeof(TViewModel).Name;
                Console.WriteLine("Navigating to " + vmName);

                if (Shell.Current.Navigation.NavigationStack.FirstOrDefault()?.
                    BindingContext.GetType().IsAssignableFrom(typeof(TViewModel)) ?? false)
                    return;

                Page p = cachedPages[typeof(TViewModel)];
                var vm = consViewModel(typeof(TViewModel));
                p.BindingContext = vm;

                vmStack.Push(vmStack.Count == 0 ? null : LastPage.BindingContext as BaseViewModel);

                await Shell.Current.Navigation.PushAsync(p, animate);
                await vm.OnEntering(null);
            }

            public async Task NavigateBackTo<TViewModel>(object parameter = null, bool animate = true) where TViewModel : BaseViewModel
            {
                string vmName = typeof(TViewModel).Name;
                Console.WriteLine("Navigating back to " + vmName);

                if (Shell.Current.Navigation.NavigationStack.FirstOrDefault()?.
                    BindingContext.GetType().IsAssignableFrom(typeof(TViewModel)) ?? false)
                    return;

                string backRoute = "..";
                while (vmStack.Peek() is not TViewModel)
                {
                    var vm = vmStack.Pop();
                    Shell.Current.Navigation.RemovePage(cachedPages[vm.GetType()]);
                    vm.OnDestroying();
                }

                suppressExtraPop = true;
                await Shell.Current.Navigation.PopAsync(animate);
                suppressExtraPop = false;

                await vmStack.Pop().OnReturning(parameter);
            }

            public async Task NavigateToRoot(object parameter = null, bool animate = true)
            {
                Console.WriteLine("Navigating to root");

                ((BaseViewModel)LastPage.BindingContext).OnDestroying();
                while (vmStack.Count > 1)
                    vmStack.Pop().OnDestroying();
                await Shell.Current.Navigation.PopToRootAsync(animate);

                await vmStack.Pop().OnReturning(parameter);
            }
        }
    }

    public interface INavigationService
    {
        BaseViewModel PreviousViewModel { get; }

        Task InitializeFirst<TViewModel>();

        Task NavigateTo<TViewModel>(object parameter = null, bool animate = true) where TViewModel : BaseViewModel;

        Task NavigateBackTo<TViewModel>(object parameter = null, bool animate = true) where TViewModel : BaseViewModel;

        Task NavigateBack(object parameter = null, bool animate = true);

        Task NavigateToRoot(object parameter = null, bool animate = true);
    }
}