using ETMProfileEditor.BLL;
using ETMProfileEditor.Contract;
using ETMProfileEditor.Model;
using System.Windows;
using Unity;

namespace ETMProfileEditor.Terminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UnityContainer container = new UnityContainer();

            container.RegisterType<ISelect<Limit>, DAL.LimitRepository>();
            container.RegisterType<IRepository<ViewModel.Profile, string>, DAL.ProfileRepository>();
            container.RegisterType<IFactory<ViewModel.Profile>, ProfileFactory>();
            container.RegisterInstance<IDispatcher>(new View.Dispatcher(this.Dispatcher));

            container.Resolve<MainWindow>().Show();
        }
    }
}