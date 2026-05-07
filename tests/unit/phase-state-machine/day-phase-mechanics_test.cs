using NUnit.Framework;
using UnityEngine.TestTools;
using SolarPhobia.Application.Services;
using SolarPhobia.Application.Models;
using SolarPhobia.Domain.Repositories;
using Moq;

namespace Tests.Unit.PhaseStateMachine
{
    public class DayPhaseMechanicsTests
    {
        private DayPhaseMechanicsService _service;
        private Mock<ISoulRepository> _soulRepoMock;
        private Mock<IAnimationService> _animMock;
        private Mock<IAudioService> _audioMock;

        [SetUp]
        public void Setup()
        {
            _soulRepoMock = new Mock<ISoulRepository>();
            _animMock = new Mock<IAnimationService>();
            _audioMock = new Mock<IAudioService>();
            _service = new DayPhaseMechanicsService(_soulRepoMock.Object, _animMock.Object, _audioMock.Object);
        }

        [Test]
        public void Swap_ShouldReturnSuccess_WhenValidIds()
        {
            // Arrange
            var playerId = "player1";
            var soulId = "soulA";
            _soulRepoMock.Setup(r => r.IsAtShadowEdge(soulId)).Returns(true);

            // Act
            var result = _service.Swap(playerId, soulId);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(playerId, result.PlayerId);
            Assert.AreEqual(soulId, result.SoulId);
            _soulRepoMock.Verify(r => r.SwapPositions(playerId, soulId), Times.Once);
            _animMock.Verify(a => a.PlaySwapAnimation(playerId, soulId, 0.5f), Times.Once);
            _audioMock.Verify(a => a.PlaySwapSound(), Times.Once);
        }

        [Test]
        public void Shove_ShouldAbandonSoul_AndPersistId()
        {
            // Arrange
            var playerId = "player1";
            var candidateSoul = "soulB";
            _soulRepoMock.Setup(r => r.GetFirstSoulAtShadowEdge()).Returns(candidateSoul);

            // Act
            var result = _service.Shove(playerId);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(candidateSoul, result.SacrificedGhostId);
            _soulRepoMock.Verify(r => r.MarkAbandoned(candidateSoul), Times.Once);
            _soulRepoMock.Verify(r => r.SetSacrificedGhostId(candidateSoul), Times.Once);
            _animMock.Verify(a => a.PlayShoveAnimation(playerId, candidateSoul), Times.Once);
            _audioMock.Verify(a => a.PlayShoveImpact(), Times.Once);
            _audioMock.Verify(a => a.PlaySoulBurn(), Times.Once);
        }
    }
}

