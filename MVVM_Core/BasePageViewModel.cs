using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_Core
{
    public abstract class BasePageViewModel: BaseViewModel
    {
        protected readonly PageService pageservice;

        public abstract int PoolIndex { get; }

        protected virtual ISliderAnimation BackSlideAnim => DisappearAnimation.Default;

        protected virtual void Back(object param)
        {
            pageservice.Back(PoolIndex, BackSlideAnim);
        }

        protected virtual void Next(object param)
        {

        }

        protected virtual async Task BackAsync(object param)
        {
            pageservice.Back(PoolIndex, BackSlideAnim);
        }

        protected virtual async Task NextAsync(object param)
        {

        }

        public ICommand BackCommand => new Command(x =>
        {
            Back(x);
        });

        public ICommand NextCommand => new Command(x =>
        {
            Next(x);
        });

        public ICommand BackCommandAsync => new CommandAsync(async x =>
        {
            await BackAsync(x);
        });

        public ICommand NextCommandAsync => new CommandAsync(async x =>
        {
            await NextAsync(x);
        });

        public BasePageViewModel(PageService pageservice)
        {
            this.pageservice = pageservice;
        }
    }
}
