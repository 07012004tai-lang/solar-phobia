namespace SolarPhobia.Domain.Services
{
    public static class RouteRiskRule
    {
        public static int GetRiskScore(string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId))
            {
                return 0;
            }

            return routeId.Length;
        }
    }
}

