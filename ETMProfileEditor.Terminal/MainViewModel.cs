using ETMProfileEditor.Contract;
using Reactive.Bindings;
using System;
using System.Linq;

namespace ETMProfileEditor.Terminal
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using ViewModel;

    public class MainViewModel
    {
        private readonly IRepository<Profile, string> profileRepository;

        public MainViewModel(ISelect<Model.Limit> limits,
            IRepository<Profile, string> profileRepository,
            IDispatcher dispatcher,
            IFactory<ViewModel.Profile> factory)
        {
            RepositoryViewModel = new RepositoryViewModel(dispatcher, factory);
            this.profileRepository = profileRepository;

            this.AddItems();

            RepositoryViewModel.Items.ToCollectionChanged().Subscribe(CollectionChanged);

            (SaveCommand as ReactiveCommand).Subscribe(async a =>
            {
                await Task.Run(() =>
                {
                    foreach (var profile in RepositoryViewModel.Items.Select(a => a.Value))
                    {
                        profileRepository.UpSert((Profile)profile);
                    }
                });
            });
        }

        public RepositoryViewModel RepositoryViewModel { get; }

        public ICommand SaveCommand { get; } = new ReactiveCommand();

        private void AddItems()
        {
            foreach (var profile in profileRepository.Select())
            {
                RepositoryViewModel.Items.Add(new SelectDeleteItem(profile));
            }
        }

        private async void CollectionChanged(CollectionChanged<SelectDeleteItem> selectDeleteItem)
        {
            await Task.Run(() =>
            {
                if (selectDeleteItem.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    profileRepository.UpSert((Profile)(selectDeleteItem.Value.Value));
                }
                else if (selectDeleteItem.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    profileRepository.Delete((Profile)(selectDeleteItem.Value.Value));
                }
            });
        }
    }
}