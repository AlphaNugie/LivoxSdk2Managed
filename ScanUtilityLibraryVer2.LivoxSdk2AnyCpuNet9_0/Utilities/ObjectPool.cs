using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Utilities
{
    /// <summary>
    /// 对象池减少GC频率
    /// </summary>
    public class ObjectPool<T>(Func<T> createFunc) where T : class
    {
        //对象池
        private readonly Stack<T> _pool = new();
        //对象池中对象的创建函数
        private readonly Func<T> _createFunc = createFunc;

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
    }
}
