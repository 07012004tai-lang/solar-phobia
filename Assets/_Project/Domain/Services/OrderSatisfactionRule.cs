
namespace SolarPhobia.Domain.Services
{
    public static class OrderSatisfactionRule
    {
        public static int GetSpiritEssenceReward(Order order)
        {
            return order != null && order.IsComplete ? 1 : 0;
        }
    }
}

