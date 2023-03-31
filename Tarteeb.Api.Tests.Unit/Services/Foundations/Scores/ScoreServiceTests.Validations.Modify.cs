//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Xunit;

namespace Tarteeb.Api.Tests.Unit.Services.Foundations.Scores
{
    public partial class ScoreServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfScoreIsNullAndLogItAsync()
        {
            // given
            Score nullScore = null;
            var nullScoreException = new NullScoreException();

            var expectedScoreValidationException =
                new ScoreValidationException(nullScoreException);

            // when
            ValueTask<Score> modifyScoreTask =
                this.scoreService.ModifyScoreAsync(nullScore);

            ScoreValidationException actualScoreValidationException =
                await Assert.ThrowsAsync<ScoreValidationException>(modifyScoreTask.AsTask);

            // then
            actualScoreValidationException.Should()
                .BeEquivalentTo(expectedScoreValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedScoreValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateScoreAsync(It.IsAny<Score>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}

