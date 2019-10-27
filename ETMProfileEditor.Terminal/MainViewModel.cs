using ETMProfileEditor.Contract;
using Reactive.Bindings;
using System;
using System.Linq;

namespace ETMProfileEditor.Terminal
{
    using Common;
    using ViewModel;
    using System.Windows.Input;

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

            (SaveCommand as ReactiveCommand).Subscribe(a =>
            {
                foreach (var profile in RepositoryViewModel.Items.Select(a => a.Value))
                {
                    profileRepository.UpSert((Profile)profile);
                }
            });
        }

        public ICommand SaveCommand { get; } = new ReactiveCommand();

        public RepositoryViewModel RepositoryViewModel { get; }

        private void CollectionChanged(CollectionChanged<SelectDeleteItem> selectDeleteItem)
        {
            if (selectDeleteItem.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                profileRepository.UpSert((Profile)(selectDeleteItem.Value.Value));
            }
            else if (selectDeleteItem.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                profileRepository.Delete((Profile)(selectDeleteItem.Value.Value));
            }
        }

        private void AddItems()
        {
            var types = TypeHelper.Filter<ViewModel.Step>().ToArray();

            foreach (var profile in profileRepository.Select())
            {
                RepositoryViewModel.Items.Add(new SelectDeleteItem(profile));
            }
        }
    }
}