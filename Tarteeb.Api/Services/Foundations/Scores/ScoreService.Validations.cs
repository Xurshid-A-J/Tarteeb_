﻿//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using System;
using Tarteeb.Api.Models.Foundations.Scores;
using Tarteeb.Api.Models.Foundations.Scores.Exceptions;
using Tarteeb.Api.Models.Foundations.Times;

namespace Tarteeb.Api.Services.Foundations.Scores
{
    public partial class ScoreService
    {
        private void ValidateTimeOnModify(Score score)
        {
            ValidateScoreNotNull(score);
            Validate(
                (Rule: IsInvalid(score.Id), nameof(Score.Id)),
                (Rule: IsInvalid(score.Grade), nameof(Score.Grade)),
                (Rule: IsInvalid(score.Weight), nameof(Score.Weight)),
                (Rule: IsInvalid(score.EffortLink), nameof(Score.EffortLink)),
                (Rule: IsInvalid(score.TicketId), nameof(Score.TicketId)),
                (Rule: IsInvalid(score.UserId), nameof(Score.UserId)),
                (Rule: IsInvalid(score.CreatedDate), nameof(Score.CreatedDate)),
                (Rule: IsInvalid(score.UpdatedDate), nameof(Score.UpdatedDate)),
                (Rule: IsInvalid(score.UpdatedDate), nameof(Score.UpdatedDate)));
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(int grade) => new
        {
            Condition = grade == default,
            Message = "Grade is required"
        };

        private static dynamic IsInvalid(float weight) => new
        {
            Condition = weight == default,
            Message = "Weight is required"
        };

        private static dynamic IsInvalid(string effortLink) => new
        {
            Condition = String.IsNullOrWhiteSpace(effortLink),
            Message = "Text is required"
        };

        private static dynamic IsInvalid(DateTimeOffset dates) => new
        {
            Condition = dates == default,
            Message = "Date is required"
        };

        private static void ValidateStorageScoreExist(Score maybeScore, Guid scoreId)
        {
            if (maybeScore is null)
            {
                throw new NotFoundScoreException(scoreId);
            }
        }

        private void ValidateScoreId(Guid scoreId) =>
            Validate((Rule: IsInvalid(scoreId), Parameter: nameof(Score.Id)));

        private static void ValidateScoreNotNull(Score score)
        {
            if (score is null)
            {
                throw new NullScoreException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidScoreException = new InvalidScoreException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidScoreException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidScoreException.ThrowIfContainsErrors();
        }
    }
}
