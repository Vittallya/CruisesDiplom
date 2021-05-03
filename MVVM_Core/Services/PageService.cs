using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MVVM_Core
{

    public enum AnimateTo
    {
        None, Left, Rigth
    };

    public class PageService
    {
        public event Action<Page, ISliderAnimation> PageChanged;

        public void ChangePage<TPage>(ISliderAnimation anim) where TPage : Page, new()
        {
            var page = new TPage();
            OnChangePage(page, anim);

        }
        public void ChangePage<TPage>(int poolIndex, ISliderAnimation anim) where TPage: Page, new()
        {
            TPage page = AddToHistory<TPage>(poolIndex);
            OnChangePage(page, anim);
        }

        private TPage AddToHistory<TPage>(int poolIndex) where TPage : Page, new()
        {
            TPage page = null;

            bool isExist = false;
            bool isOtherPool = poolIndex != ActualPool;
            bool poolContains = _pool.ContainsKey(poolIndex);
            bool hasSame = poolContains && _pool[poolIndex].Any(x => x.GetType() == typeof(TPage));

            if (isOtherPool)
            {
                ActualPool = poolIndex;

                if (hasSame)
                {
                    page = _pool[poolIndex].FirstOrDefault(x => x.GetType() == typeof(TPage)) as TPage;
                    isExist = true;
                }
            }

            if (!_poolHistory.Contains(poolIndex))
                _poolHistory.Add(poolIndex);


            if (!isExist)
            {
                page = new TPage();
                if (!poolContains)
                {
                    _pool.Add(poolIndex, new List<Page>());
                }
                _pool[poolIndex].Add(page);

            }

            return page;
        }

        public void OnChangePage<TPage>(TPage target, ISliderAnimation anim) where TPage : Page, new()
        {
            PageChanged?.Invoke(target, anim);
        }

        private Action<int> _nextInvoker;

        public void SetupNext<TPage>(ISliderAnimation animation) where TPage : Page, new()
        {
            _nextInvoker = pool => ChangePage<TPage>(pool, animation);
        }

        public void Next(int poolIndex)
        {
            _nextInvoker?.Invoke(poolIndex);
        }

        #region ByType
        

        public void ChangePageByType<TPage, Type>(AnimateTo animateTo = AnimateTo.None, object dataCont = null) where TPage : Page, new()
        {
            var target = new TPage();
            if (dataCont != null)
            {
                target.DataContext = dataCont;
            }
            OnChangePageByType<TPage, Type>(target, animateTo);
        }

        public void ChangePageByType<TPage, Type>(int poolIndex, AnimateTo animateTo = AnimateTo.None, object dataCont = null) where TPage : Page, new()
        {
            var target = new TPage();
            if (dataCont != null)
            {
                target.DataContext = dataCont;
            }
            ChangePageByType<TPage, Type>(target, AnimateTo.None, poolIndex);            
        }

        

        public void ChangePageByType<TPage, Type>(TPage target, AnimateTo animateTo, int poolIndex) where TPage : Page, new()
        {
            OnChangePageByType<TPage, Type>(target, animateTo);

            if (!_pool.ContainsKey(poolIndex))
            {
                _pool.Add(poolIndex, new List<Page>());
            }
            _pool[poolIndex].Add(target);
        }

        public void OnChangePageByType<TPage, Type>(TPage target, AnimateTo animateTo) where TPage : Page, new()
        {
            if (_subs.ContainsKey(typeof(Type)))
            {
                _subs[typeof(Type)]?.Invoke(target, animateTo);
            }
        }

        Dictionary<Type, Action<Page, AnimateTo>> _subs = new Dictionary<Type, Action<Page, AnimateTo>>();

        public void SubscribeByType<T>(Action<Page, AnimateTo> method)
        {
            if (!_subs.ContainsKey(typeof(T)))
            {
                _subs.Add(typeof(T), method);
            }
        }
        #endregion


        List<Page> _history = new List<Page>();

        List<int> _poolHistory = new List<int>();

        public int ActualPool { get; private set; } = -1;

        Dictionary<int, List<Page>> _pool = new Dictionary<int, List<Page>>();

        public void Back(int poolIndex, ISliderAnimation anim = null)
        {
            if (_pool.ContainsKey(poolIndex))
            {                

                if (_pool[poolIndex].Count > 1)
                {
                    Page last = _pool[poolIndex].LastOrDefault();
                    Page target = _pool[poolIndex].Skip(_pool[poolIndex].Count - 2).FirstOrDefault();

                    _pool[poolIndex].Remove(last);
                    OnChangePage(target, anim);
                }
                else if(_poolHistory.IndexOf(poolIndex) > 0)
                {
                    int index = _poolHistory.IndexOf(poolIndex) - 1;
                    ActualPool = _poolHistory[index];

                    ChangeToLastByPool(ActualPool, anim);
                }
            }

        }

        public void Back<TPage>(int poolIndex, ISliderAnimation anim = null) where TPage: Page, new()
        {
            if (_pool.ContainsKey(poolIndex) &&
                _pool[poolIndex].Count > 1 &&
                _pool[poolIndex].Any(x => x.GetType() == typeof(TPage)))
            {
                var list = _pool[poolIndex];

                var target = list.Find(x => x.GetType() == typeof(TPage));
                int index = list.IndexOf(target);


                _pool[poolIndex].RemoveRange(index + 1, list.Count - index - 1);
                OnChangePage(target, anim);

            }

        }

        public void ChangeToLastByPool(int index, ISliderAnimation animation = null)
        {
            if (_pool.ContainsKey(index) && _pool[index].Count > 0)
            {
                var target = _pool[index].Last();
                OnChangePage(target, animation);
            }
        }

        public void ChangeToLastByActualPool(ISliderAnimation animation = null)
        {            
            var target = _pool[ActualPool].Last();
            OnChangePage(target, animation);            
        }

        public void AddPageToPool<TPage>(int poolId) where TPage: Page, new()
        {
            if (_pool.ContainsKey(poolId))
            {
                var target = new TPage();
                _pool[poolId].Add(target);
            }
        }

        public void ClearHistoryByPool(int index)
        {
            if (_pool.ContainsKey(index))
            {
                _pool.Remove(index);
            }
        }

        public void ClearAllPools()
        {
            _pool.Clear();
        }

        public bool HasPoolByIndex(int index)
        {
            return _pool.ContainsKey(index);
        }
        public bool HasActualPool()
        {
            return _pool.Count > 0 && _pool.ContainsKey(ActualPool);
        }
    }
}
