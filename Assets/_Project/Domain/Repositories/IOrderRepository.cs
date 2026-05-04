using System.Collections.Generic;
using SolarPhobia.Domain;

namespace SolarPhobia.Domain.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetActiveOrders();
        void Add(Order order);
        void Remove(string orderId);
    }
}

