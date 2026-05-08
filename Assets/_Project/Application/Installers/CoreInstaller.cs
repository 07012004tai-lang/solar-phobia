using System;
using SolarPhobia.Application.Repositories;
using SolarPhobia.Application.Services;
using VContainer;
using VContainer.Unity;

namespace SolarPhobia.Application.Installers
{
    /// <summary>
    /// VContainer 2.x LifetimeScope for core application services.
    /// Register this via VContainerSettings asset (Assets -> Create -> VContainer -> VContainer Settings).
    /// </summary>
    public class CoreLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // ── Phase State Machine ─────────────────────────────────────────
            builder.Register<PhaseStateMachine>(Lifetime.Singleton).As<IPhaseStateMachine>();
            
            // ── Soul Repository ────────────────────────────────────────────────
            builder.Register<SoulRepository>(Lifetime.Singleton).As<ISoulRepository>();
            
            // ── Day Phase Timeline ─────────────────────────────────────────────
            builder.Register<DayPhaseTimelineService>(Lifetime.Singleton).As<IDayPhaseTimelineService>();
        }
    }
}