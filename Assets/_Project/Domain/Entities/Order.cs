using ObservableCollections;

namespace SolarPhobia.Domain {
    public class Order {
        public string Id { get; set; }
        public string GhostId { get; set; }
        public ObservableDictionary<OrderType, int> ItemsNeeded { get; } = new ObservableDictionary<OrderType, int>();
        public float TimeLimitSeconds { get; set; } = 30f;
        public bool IsComplete { get; private set; }

        public Order(string id, string ghostId) {
            Id = id;
            GhostId = ghostId;
        }

        public void AddItem(OrderType type) {
            if (!ItemsNeeded.ContainsKey(type)) return;
            ItemsNeeded[type] -= 1;
            if (ItemsNeeded[type] <= 0) {
                ItemsNeeded.Remove(type);
            }

            if (ItemsNeeded.Count == 0) CompleteOrder();
        }

        private void CompleteOrder() {
            IsComplete = true;
        }

        public override string ToString()
        {
            return $"{Id}::{GhostId}::{IsComplete}";
        }
    }
}