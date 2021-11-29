using System;
using BetterTimerApp.Abstractions;
using BetterTimerApp.Implementations;
using BetterTimerApp.Tests.Stubs;
using Moq;
using NUnit.Framework;
using Action = BetterTimerApp.Tests.Stubs.Action;

namespace BetterTimerApp.Tests.Tests
{
    [TestFixture]
    public class PublisherTests
    {
        private TimerStub m_TimerStub;
        private Mock<IConsole> m_ConsoleMock;
        private Publisher m_Sut;

        [SetUp]
        public void SetUp()
        {
            m_TimerStub = new TimerStub();
            m_ConsoleMock = new Mock<IConsole>();
            m_Sut = new Publisher(m_TimerStub, m_ConsoleMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            m_Sut = null;
            m_ConsoleMock = null;
            m_TimerStub = null;
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.AreEqual(Action.Enabled, m_TimerStub.Log[1].Action);
            Assert.AreEqual(Action.Enabled.ToString(), m_TimerStub.Log[1].Message);
            Assert.AreEqual(Action.IntervalSet, m_TimerStub.Log[2].Action);
            Assert.AreEqual(1000.ToString("G17"), m_TimerStub.Log[2].Message);
        }

        [Test]
        public void StartPublishingTest()
        {
            // Arrange
            var expectedDateTime = DateTime.Now;

            m_ConsoleMock
                .Setup
                (
                    m => m.WriteLine
                    (
                        It.Is<DateTime>(p => p == expectedDateTime)
                    )
                )
                .Verifiable();


            // Act
            m_Sut.StartPublishing();
            m_TimerStub.TriggerTimerIntervalElapsed(expectedDateTime);


            // Assert
            ConstructorTest();

            m_ConsoleMock
                .Verify
                (
                    m => m.WriteLine(expectedDateTime)
                );

            Assert.AreEqual(Action.Start, m_TimerStub.Log[3].Action);
            Assert.AreEqual("Started", m_TimerStub.Log[3].Message);
            Assert.AreEqual(Action.Triggered, m_TimerStub.Log[4].Action);
            Assert.AreEqual(Action.Triggered.ToString(), m_TimerStub.Log[4].Message);
        }

        [Test]
        public void StopPublishingTest()
        {
            // Act
            m_Sut.StopPublishing();


            // Assert
            ConstructorTest();

            Assert.AreEqual(Action.Stop, m_TimerStub.Log[3].Action);
            Assert.AreEqual("Stopped", m_TimerStub.Log[3].Message);
        }

        [Test]
        public void FullProcessTest()
        {
            // Arrange
            var expectedDateTime1 = DateTime.Now;
            var expectedDateTime2 = expectedDateTime1 + TimeSpan.FromSeconds(1);
            var expectedDateTime3 = expectedDateTime2 + TimeSpan.FromSeconds(1);

            var sequence = new MockSequence();

            m_ConsoleMock
                .InSequence(sequence)
                .Setup
                (
                    m => m.WriteLine
                    (
                        It.Is<DateTime>(p => p == expectedDateTime1)
                    )
                )
                .Verifiable();

            m_ConsoleMock
                .InSequence(sequence)
                .Setup
                (
                    m => m.WriteLine
                    (
                        It.Is<DateTime>(p => p == expectedDateTime2)
                    )
                )
                .Verifiable();

            m_ConsoleMock
                .InSequence(sequence)
                .Setup
                (
                    m => m.WriteLine
                    (
                        It.Is<DateTime>(p => p == expectedDateTime3)
                    )
                )
                .Verifiable();


            // Act
            m_Sut.StartPublishing();
            m_TimerStub.TriggerTimerIntervalElapsed(expectedDateTime1);


            // Assert
            ConstructorTest();

            m_ConsoleMock
                .Verify
                (
                    m => m.WriteLine(expectedDateTime1)
                );

            Assert.AreEqual(Action.Start, m_TimerStub.Log[3].Action);
            Assert.AreEqual("Started", m_TimerStub.Log[3].Message);
            Assert.AreEqual(Action.Triggered, m_TimerStub.Log[4].Action);
            Assert.AreEqual(Action.Triggered.ToString(), m_TimerStub.Log[4].Message);


            // Act
            m_TimerStub.TriggerTimerIntervalElapsed(expectedDateTime2);


            // Assert
            m_ConsoleMock
                .Verify
                (
                    m => m.WriteLine(expectedDateTime2)
                );

            Assert.AreEqual(Action.Triggered, m_TimerStub.Log[5].Action);
            Assert.AreEqual(Action.Triggered.ToString(), m_TimerStub.Log[5].Message);


            // Act
            m_Sut.StopPublishing();


            // Assert
            Assert.AreEqual(Action.Stop, m_TimerStub.Log[6].Action);
            Assert.AreEqual("Stopped", m_TimerStub.Log[6].Message);
        }
    }
}