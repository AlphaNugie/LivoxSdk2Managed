using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Utilities
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// 对象池，并使用给定的对象池创建函数初始化对象池（方便进行定制）
    /// </summary>
    /// <typeparam name="T">对象池内对象的类型</typeparam>
    /// <param name="createFunc">对象池创建函数</param>
    public class ObjectPool<T>(Func<T> createFunc) where T : class
#elif NET45
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : class
#endif
    {
        /// <summary>
        /// 对象池
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Stack<T> _pool = new();
#elif NET45
        private readonly Stack<T> _pool = new Stack<T>();
#endif
        /// <summary>
        /// 对象池中对象的创建函数
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Func<T> _createFunc = createFunc;
#elif NET45
        private readonly Func<T> _createFunc;
#endif

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <returns></returns>
        public T Get() => _pool.Count > 0 ? _pool.Pop() : _createFunc();

        /// <summary>
        /// 归还一个对象
        /// </summary>
        /// <param name="obj"></param>
        public void Return(T obj) => _pool.Push(obj);

#if NET45
        /// <summary>
        /// 使用给定的对象池创建函数初始化对象池（方便进行定制）
        /// </summary>
        /// <param name="createFunc">对象池创建函数</param>
        public ObjectPool(Func<T> createFunc)
        {
            _pool = new Stack<T>();
            _createFunc = createFunc;
        }
#endif
    }
}
