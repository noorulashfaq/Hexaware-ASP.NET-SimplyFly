﻿using SimplyFlyServer.Models;

namespace SimplyFlyServer.Interface
{
    public interface IRepository<K,T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(K key);
        Task<T> Add(T entity);
        Task<T> Update(K key, T entity);
        Task<T> Delete(K key);
      
    }
}
