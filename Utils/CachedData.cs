using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CachedData
/// </summary>
namespace Utils
{
    public class CachedData<T>
    {
        public DateTime Time { get; private set; }

        T data;
        public T Data
        {
            get
            {
                return data;
            }
            set
            {
                Time = DateTime.Now;
                data = value;
            }
        }
    }
}