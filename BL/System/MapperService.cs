using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BL
{
    public class MapperService
    {
        public T GetCopy<T>(T obj)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<T, T>()));

            var copy = mapper.Map<T>(obj);
            return copy;
        }

        public U MapTo<V, U>(V obj)
        {
            Mapper mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<V, U>()));

            var inst = mapper.Map<V, U>(obj);
            return inst;
        }
    }
}
