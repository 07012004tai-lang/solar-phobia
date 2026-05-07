using SolarPhobia.Application.Repositories;
using SolarPhobia.Application.Services;
using VContainer;

namespace SolarPhobia.Application.Installers
{
    public static class CoreInstaller
    {
        public static void Install(IContainerBuilder builder)
        {
            builder.Register<PhaseStateMachine>(Lifetime.Singleton).As<IPhaseStateMachine>();
            builder.Register<SoulRepository>(Lifetime.Singleton).As<ISoulRepository>();
        }
    }
}